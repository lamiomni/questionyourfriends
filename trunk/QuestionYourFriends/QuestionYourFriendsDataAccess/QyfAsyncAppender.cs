using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Util;

namespace QuestionYourFriendsDataAccess
{
    /// <summary>
    /// This appender forwards logging events asynchronously to attached appenders.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The forwarding appender can be used to specify different thresholds
    /// and filters for the same appender at different locations within the hierarchy.
    /// </para>
    /// </remarks>
    public sealed class QyfAsyncAppender : IBulkAppender, IAppenderAttachable, IOptionHandler, IDisposable
    {
        #region Fields

        private const string ThreadName = "QyfAsyncAppender Thread";
        private static readonly List<WeakReference> _instances = new List<WeakReference>();
        private static readonly object _synchro = new object();
        private readonly object _appenderSynchro = new object();

        /// <summary>
        /// Thread dispatching events
        /// </summary>
        private readonly Thread _asyncForward;

        private readonly object _flushSynchro = new object();
        private readonly object _queueSynchro = new object();

        /// <summary>
        /// Implementation of the <see cref="IAppenderAttachable"/> interface
        /// </summary>
        private AppenderAttachedImpl _appenderAttachedImpl;

        private bool _disposed;

        /// <summary>
        /// Pending events to log
        /// </summary>
        private List<LoggingEvent> _eventQueue;

        private FixFlags _fixedFields;

        private bool _flushPending;

        private bool _discardLogEvents;

        /// <summary>
        /// Log Events counter which helps in deciding whether to discard or enable logging.
        /// </summary>
        private int _logEventsCounter;

        /// <summary>
        /// It is assumed and enforced that errorHandler is never null.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is assumed and enforced that errorHandler is never null.
        /// </para>
        /// <para>
        /// See <see cref="ErrorHandler"/> for more information.
        /// </para>
        /// </remarks>
        private IErrorHandler _errorHandler;

        /// <summary>
        /// Sets to true to stop the forward thread
        /// </summary>
        private bool _stopForwarding;

        #endregion Private Instance Fields

        #region Properties

        /// <summary>
        /// Gets/Sets the priority
        /// </summary>
        /// <remarks>see<see cref="ThreadPriority"/></remarks>
        public ThreadPriority Priority
        {
            get { return _asyncForward.Priority; }
            set { _asyncForward.Priority = value; }
        }

        /// <summary>
        /// Gets/Sets the fixed fields in the <see cref="LoggingEvent"/> object
        /// </summary>
        /// <remarks>
        /// <para>
        /// The fixed fields must be represented as a comma (',') seperated list
        /// of <see cref="FixFlags"/> entries.
        /// </para>
        /// <para>
        /// Default is 'Partial',<see cref="FixFlags"/> for other values
        /// </para>
        /// <para>
        /// <see cref="FixFlags.ThreadName"/> is always appended to the list of fix fields.
        /// </para>
        /// </remarks>
        public FixFlags FixedFields
        {
            get { return _fixedFields; }
            set { _fixedFields = value | FixFlags.ThreadName; }
        }

        /// <summary>
        /// Gets or sets the threshold <see cref="Level"/> of this appender.
        /// </summary>
        /// <value>
        /// The threshold <see cref="Level"/> of the appender. 
        /// </value>
        /// <remarks>
        /// <para>
        /// All log events with lower level than the threshold level are ignored 
        /// by the appender.
        /// </para>
        /// <para>
        /// In configuration files this option is specified by setting the
        /// value of the <see cref="Threshold"/> option to a level
        /// string, such as "DEBUG", "INFO" and so on.
        /// </para>
        /// </remarks>
        public Level Threshold { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IErrorHandler"/> for this appender.
        /// </summary>
        /// <value>The <see cref="IErrorHandler"/> of the appender</value>
        /// <remarks>
        /// <para>
        /// The <see cref="AppenderSkeleton"/> provides a default 
        /// implementation for the <see cref="ErrorHandler"/> property. 
        /// </para>
        /// </remarks>
        public IErrorHandler ErrorHandler
        {
            get { return _errorHandler; }
            set
            {
                if (value == null)
                {
                    // We do not throw exception here since the cause is probably a
                    // bad config file.
                    LogLog.Warn("AppenderSkeleton: You have tried to set a null error-handler.");
                }
                else
                {
                    _errorHandler = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of this appender.
        /// </summary>
        /// <value>The name of the appender.</value>
        /// <remarks>
        /// <para>
        /// The name uniquely identifies the appender.
        /// </para>
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Checks if the message level is below this appender's threshold.
        /// </summary>
        /// <param name="level"><see cref="Level"/> to test against.</param>
        /// <remarks>
        /// <para>
        /// If there is no threshold set, then the return value is always <c>true</c>.
        /// </para>
        /// </remarks>
        /// <returns>
        /// <c>true</c> if the <paramref name="level"/> meets the <see cref="Threshold"/> 
        /// requirements of this appender.
        /// </returns>
        private bool IsAsSevereAsThreshold(Level level)
        {
            return ((Threshold == null) || level >= Threshold);
        }

        /// <summary>
        /// Gets or sets the Threshold value to stop enquening the log events.
        /// </summary>
        public int StopEnqueuingLogEventsThreshold { get; set; }

        /// <summary>
        /// Gets or sets the Threshold value to resume enquening the log events.
        /// </summary>
        public int ResumeEnqueuingLogEventsThreshold { get; set; }

        /// <summary>
        /// Gets or sets the Threshold value to log the events with this defined size.
        /// </summary>
        public int LogEventsChunkSize { get; set; }

        #endregion

        #region Public Instance Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QyfAsyncAppender" /> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Default constructor.
        /// </para>
        /// </remarks>
        public QyfAsyncAppender()
        {
            _errorHandler = new OnlyOnceErrorHandler(GetType().Name);
            _eventQueue = new List<LoggingEvent>();
            _fixedFields = FixFlags.Partial;
            _asyncForward = new Thread(AsyncForwarder) { IsBackground = true };

            _asyncForward.Start();
            DeclareInstance(this);
        }
        #endregion Public Instance Constructors

        #region Loop

        /// <summary>
        /// Configures log4net with a default async console appender
        /// </summary>
        public static void ConfigureBasicLog()
        {
            var layout =
                new PatternLayout
                    {
                        ConversionPattern = "%timestamp [%thread] %level %logger - %message%newline"
                    };
            layout.ActivateOptions();
            var appender = new ConsoleAppender { Layout = layout };
            appender.ActivateOptions();
            ConfigureBasicLog(appender);
        }

        /// <summary>
        /// Configures log4net with an async appender forwarding
        /// </summary>
        /// <param name="appender">The output appender.</param>
        public static void ConfigureBasicLog(IAppender appender)
        {
            var final = new QyfAsyncAppender();
            final.AddAppender(appender);
            final.ActivateOptions();
            BasicConfigurator.Configure(final);
        }

        /// <summary>
        /// Forwards events to the appenders asynchronously
        /// </summary>
        private void AsyncForwarder()
        {
            _asyncForward.Name = ThreadName;
            var localQueue = new List<LoggingEvent>();
            for (; ; )
            {
                // we grab the queue
                bool flush;
                lock (_queueSynchro)
                {
                    if (_eventQueue.Count == 0)
                    {
                        if (_stopForwarding)
                            break;

                        if (!_flushPending)
                            Monitor.Wait(_queueSynchro);
                    }

                    List<LoggingEvent> temp = _eventQueue;
                    _eventQueue = localQueue;
                    localQueue = temp;
                    // Has a flush been requested
                    flush = _flushPending;
                }
                // Shall we forward?
                lock (_appenderSynchro)
                {
                    if (_appenderAttachedImpl != null && localQueue.Count > 0)
                    {
                        // Pass the logging event on the the attached appenders
                        // this.appenderAttachedImpl.AppendLoopOnAppenders(localQueue.ToArray());
                        for (int i = 0; i < localQueue.Count; )
                        {
                            // Compute num of events to  be fetched from localQueue.
                            // If the remaining events in the queue is more than the chunksize, num of events to be fetched will be chunksize otherwise, fetch the remainging num of logevents from the queue
                            int count = localQueue.Count - i;
                            if (LogEventsChunkSize > 0)
                                count = count > LogEventsChunkSize ? LogEventsChunkSize : count;

                            // Fetch the defined num of events from the queue and log them.
                            _appenderAttachedImpl.AppendLoopOnAppenders(localQueue.GetRange(i, count).ToArray());

                            // If logging was disabled due to queue full and the logEvents counter is now less
                            // than the Resume threshold value, then enable logging 
                            int nbQueuedEvents = Interlocked.Add(ref _logEventsCounter, -count);
                            if ((nbQueuedEvents <= ResumeEnqueuingLogEventsThreshold) &&
                                _discardLogEvents)
                            {
                                _discardLogEvents = false;
                                LogLog.Warn("Event queue consumed, " + nbQueuedEvents + " events pending, considering new log events.");
                            }
                            i += count;
                        }
                    }
                }
                localQueue.Clear();

                if (flush)
                    // Flush requested, we notify that flush is done
                    lock (_flushSynchro)
                    {
                        _flushPending = false;
                        Monitor.Pulse(_flushSynchro);
                    }
            }
        }

        private void QueryFlush()
        {
            lock (_queueSynchro)
            {
                _flushPending = true;
                if (LogLog.IsDebugEnabled)
                    LogLog.Debug(string.Concat(this, ": Flush requested"));
                Monitor.Pulse(_queueSynchro);
            }
        }

        private bool WaitFlush(int timeout)
        {
            lock (_flushSynchro)
            {
                if (_flushPending)
                {
                    if (LogLog.IsDebugEnabled)
                        LogLog.Debug(string.Concat(this, ": Waiting for flush"));
                    if (!Monitor.Wait(_flushSynchro, timeout))
                    {
                        LogLog.Error(string.Format(CultureInfo.InvariantCulture,
                                                   "{0}: Flush not performed in time ({1}ms)",
                                                   this, timeout));
                        return false;
                    }
                }
                if (LogLog.IsDebugEnabled)
                    LogLog.Debug(string.Concat(this, ": Flushed"));
                return true;
            }
        }

        private static void DeclareInstance(QyfAsyncAppender instance)
        {
            lock (_synchro)
                _instances.Add(new WeakReference(instance));
        }

        /// <summary>
        /// Flushes all instances
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>true if all flush succeeded, false if any failed.</returns>
        public static bool FlushAll(int timeout)
        {
            bool ret = true;
            LogLog.Debug("QyfAsyncAppender: FlushAll requested");
            lock (_synchro)
            {
                // Query all instances
                for (int i = 0; i != _instances.Count; ++i)
                {
                    WeakReference instance = _instances[i];
                    if (!instance.IsAlive)
                    {
                        _instances.RemoveAt(i--);
                        continue;
                    }
                    ((QyfAsyncAppender)instance.Target).QueryFlush();
                }

                // Query all instances
                for (int i = 0; i != _instances.Count; ++i)
                {
                    WeakReference instance = _instances[i];
                    if (!instance.IsAlive)
                    {
                        _instances.RemoveAt(i--);
                        continue;
                    }
                    var appender = (QyfAsyncAppender)instance.Target;
                    ret = appender.WaitFlush(timeout) && ret;
                }
            }
            LogLog.Debug("QyfAsyncAppender: FlushAll done");
            return ret;
        }

        /// <summary>
        /// Flushes the appender
        /// </summary>
        /// <param name="timeout">The time allocated for the flush</param>
        public bool Flush(int timeout)
        {
            QueryFlush();
            return WaitFlush(timeout);
        }

        #endregion

        #region Override implementation of AppenderSkeleton

        /// <summary>
        /// Closes the appender and releases resources.
        /// </summary>
        /// <remarks>
        /// 	<para>
        /// Releases any resources allocated within the appender such as file handles,
        /// network connections, etc.
        /// </para>
        /// 	<para>
        /// It is a programming error to append to a closed appender.
        /// </para>
        /// </remarks>
        public void Close()
        {
            // Remove all the attached appenders
            if (_asyncForward != null)
            {
                lock (_queueSynchro)
                {
                    _stopForwarding = true;
                    Monitor.Pulse(_queueSynchro);
                }
                if (_asyncForward.Join(10000))
                    _eventQueue = null;
            }
            RemoveAllAppenders();
        }

        /// <summary>
        /// Does the append.
        /// </summary>
        /// <param name="loggingEvent">The logging event.</param>
        public void DoAppend(LoggingEvent loggingEvent)
        {
            // This lock is absolutely critical for correct formatting
            // of the message in a multi-threaded environment.  Without
            // this, the message may be broken up into elements from
            // multiple thread contexts (like get the wrong thread ID).
            if (_stopForwarding)
            {
                ErrorHandler.Error(string.Concat("Attempted to append to closed appender named [", Name, "]."));
                return;
            }

            try
            {
                if (!FilterEvent(loggingEvent) || _discardLogEvents)
                    return;

                // Fix what has to be fixed:
                loggingEvent.Fix = _fixedFields;

                // Enqueue the event;
                lock (_queueSynchro)
                {
                    if (_eventQueue.Count == 0)
                        Monitor.Pulse(_queueSynchro);

                    _eventQueue.Add(loggingEvent);

                    int nbEnqueuedEvents = Interlocked.Increment(ref _logEventsCounter);
                    if ((nbEnqueuedEvents >= StopEnqueuingLogEventsThreshold) &&
                        StopEnqueuingLogEventsThreshold != 0)
                    {
                        _discardLogEvents = true;
                        LogLog.Warn(string.Concat("Queue is full, ", nbEnqueuedEvents, " events pending, discarding new events."));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("Failed in DoAppend", ex);
            }
        }

        /// <summary>
        /// Does the append.
        /// </summary>
        /// <param name="loggingEvents">The logging events.</param>
        public void DoAppend(LoggingEvent[] loggingEvents)
        {
            if (_stopForwarding)
            {
                ErrorHandler.Error(string.Concat("Attempted to append to closed appender named [", Name, "]."));
                return;
            }

            try
            {
                var filteredEvents = new List<LoggingEvent>(loggingEvents.Length);

                foreach (LoggingEvent loggingEvent in loggingEvents.Where(FilterEvent))
                {
                    loggingEvent.Fix = _fixedFields;
                    filteredEvents.Add(loggingEvent);
                }

                if (filteredEvents.Count == 0)
                    return;

                lock (_queueSynchro)
                {
                    if (_eventQueue.Count == 0)
                        Monitor.Pulse(_queueSynchro);
                    _eventQueue.AddRange(filteredEvents);
                    if (Interlocked.Add(ref _logEventsCounter, filteredEvents.Count) >=
                        StopEnqueuingLogEventsThreshold &&
                        StopEnqueuingLogEventsThreshold != 0)
                    {
                        _discardLogEvents = true;
                        LogLog.Warn("Queue is full, Events logged after this will be discarded.");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("Failed in Bulk DoAppend", ex);
            }
        }

        ///// <summary>
        ///// Activates the options.
        ///// </summary>
        //public void ActivateOptions()
        //{
        //    if (ResumeEnqueuingLogEventsThreshold != 0 && ResumeEnqueuingLogEventsThreshold < StopEnqueuingLogEventsThreshold)
        //    {
        //        ResumeEnqueuingLogEventsThreshold = default(int)
        //    }
        //    // flush to guarantee existing events are treated according to previous settings
        //    Flush(1000);
        //}

        private bool FilterEvent(LoggingEvent loggingEvent)
        {
            return IsAsSevereAsThreshold(loggingEvent.Level);
        }

        #endregion Override implementation of AppenderSkeleton

        #region Implementation of IAppenderAttachable

        /// <summary>
        /// Adds an <see cref="IAppender" /> to the list of appenders of this
        /// instance.
        /// </summary>
        /// <param name="appender">The <see cref="IAppender" /> to add to this appender.</param>
        /// <remarks>
        /// <para>
        /// If the specified <see cref="IAppender" /> is already in the list of
        /// appenders, then it won't be added again.
        /// </para>
        /// </remarks>
        public void AddAppender(IAppender appender)
        {
            if (appender == null)
                throw new ArgumentNullException("appender");
            if (appender is QyfAsyncAppender)
                LogLog.Error(
                    string.Format(CultureInfo.InvariantCulture,
                                  "It is a design error to have an asyncappender referenced in an asyncappender. Check {0} and {1} relations",
                                  Name,
                                  appender.Name));
            lock (_appenderSynchro)
            {
                if (_appenderAttachedImpl == null)
                    _appenderAttachedImpl = new AppenderAttachedImpl();
                _appenderAttachedImpl.AddAppender(appender);
            }
        }

        /// <summary>
        /// Gets the appenders contained in this appender as an 
        /// <see cref="System.Collections.ICollection"/>.
        /// </summary>
        /// <remarks>
        /// If no appenders can be found, then an <see cref="EmptyCollection"/> 
        /// is returned.
        /// </remarks>
        /// <returns>
        /// A collection of the appenders in this appender.
        /// </returns>
        public AppenderCollection Appenders
        {
            get
            {
                lock (_appenderSynchro)
                    return _appenderAttachedImpl == null
                               ? AppenderCollection.EmptyCollection
                               : _appenderAttachedImpl.Appenders;
            }
        }

        /// <summary>
        /// Looks for the appender with the specified name.
        /// </summary>
        /// <param name="name">The name of the appender to lookup.</param>
        /// <returns>
        /// The appender with the specified name, or <c>null</c>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Get the named appender attached to this appender.
        /// </para>
        /// </remarks>
        public IAppender GetAppender(string name)
        {
            lock (_appenderSynchro)
                return _appenderAttachedImpl == null || name == null
                    ? null
                    : _appenderAttachedImpl.GetAppender(name);
        }

        /// <summary>
        /// Removes all previously added appenders from this appender.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is useful when re-reading configuration information.
        /// </para>
        /// </remarks>
        public void RemoveAllAppenders()
        {
            lock (_appenderSynchro)
                if (_appenderAttachedImpl != null)
                {
                    _appenderAttachedImpl.RemoveAllAppenders();
                    _appenderAttachedImpl = null;
                }
        }

        /// <summary>
        /// Removes the specified appender from the list of appenders.
        /// </summary>
        /// <param name="appender">The appender to remove.</param>
        /// <returns>The appender removed from the list</returns>
        /// <remarks>
        /// The appender removed is not closed.
        /// If you are discarding the appender you must call
        /// <see cref="IAppender.Close"/> on the appender removed.
        /// </remarks>
        public IAppender RemoveAppender(IAppender appender)
        {
            lock (_appenderSynchro)
                if (appender != null && _appenderAttachedImpl != null)
                    return _appenderAttachedImpl.RemoveAppender(appender);
            return null;
        }

        /// <summary>
        /// Removes the appender with the specified name from the list of appenders.
        /// </summary>
        /// <param name="name">The name of the appender to remove.</param>
        /// <returns>The appender removed from the list</returns>
        /// <remarks>
        /// The appender removed is not closed.
        /// If you are discarding the appender you must call
        /// <see cref="IAppender.Close"/> on the appender removed.
        /// </remarks>
        public IAppender RemoveAppender(string name)
        {
            lock (_appenderSynchro)
                if (name != null && _appenderAttachedImpl != null)
                    return _appenderAttachedImpl.RemoveAppender(name);
            return null;
        }

        #endregion Implementation of IAppenderAttachable

        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            Close();
            lock (_synchro)
                for (int i = 0; i != _instances.Count; ++i)
                {
                    if (!_instances[i].IsAlive)
                    {
                        _instances.RemoveAt(i--);
                        continue;
                    }
                    if (ReferenceEquals(this, _instances[i].Target))
                    {
                        _instances.RemoveAt(i);
                        break;
                    }
                }
        }

        #endregion

        #region IOptionHandler Members

        /// <summary>
        /// Activates the options.
        /// </summary>
        public void ActivateOptions()
        {
            if (StopEnqueuingLogEventsThreshold != 0)
            {
                if (ResumeEnqueuingLogEventsThreshold > StopEnqueuingLogEventsThreshold)
                {
                    LogLog.Warn("Invalid QyfAsyncAppender threshold values, ResumeEnqueingLogEventsThreshold should be lower or equal to StopEnqueueingLogEventsThreshold.");
                    ResumeEnqueuingLogEventsThreshold = 0;
                }

                if (LogEventsChunkSize == 0)
                    // This is a default value:
                    LogEventsChunkSize = 100;
            }

            LogLog.Debug(string.Format(CultureInfo.InvariantCulture,
                                       "QyfAsyncAppender config: Stop enqueing at {0}, resume at {1}, chunk size: {2}",
                                       StopEnqueuingLogEventsThreshold,
                                       ResumeEnqueuingLogEventsThreshold,
                                       LogEventsChunkSize));

            // Flush to guarantee existing events are treated according to previous settings
            Flush(1000);
        }

        #endregion
    }
}