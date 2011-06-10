//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace QuestionYourFriendsDataAccess
{
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(Question))]
    [KnownType(typeof(Transac))]
    public partial class User: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'id' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _id = value;
                    OnPropertyChanged("id");
                }
            }
        }
        private int _id;
    
        [DataMember]
        public long fid
        {
            get { return _fid; }
            set
            {
                if (_fid != value)
                {
                    _fid = value;
                    OnPropertyChanged("fid");
                }
            }
        }
        private long _fid;
    
        [DataMember]
        public int credit_amount
        {
            get { return _credit_amount; }
            set
            {
                if (_credit_amount != value)
                {
                    _credit_amount = value;
                    OnPropertyChanged("credit_amount");
                }
            }
        }
        private int _credit_amount;
    
        [DataMember]
        public bool activated
        {
            get { return _activated; }
            set
            {
                if (_activated != value)
                {
                    _activated = value;
                    OnPropertyChanged("activated");
                }
            }
        }
        private bool _activated;
    
        [DataMember]
        public string login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged("login");
                }
            }
        }
        private string _login;
    
        [DataMember]
        public string passwd
        {
            get { return _passwd; }
            set
            {
                if (_passwd != value)
                {
                    _passwd = value;
                    OnPropertyChanged("passwd");
                }
            }
        }
        private string _passwd;

        #endregion
        #region Navigation Properties
    
    	[XmlIgnore]
        [DataMember]
        public TrackableCollection<Question> MyQuestions
        {
            get
            {
                if (_myQuestions == null)
                {
                    _myQuestions = new TrackableCollection<Question>();
                    _myQuestions.CollectionChanged += FixupMyQuestions;
                }
                return _myQuestions;
            }
            set
            {
                if (!ReferenceEquals(_myQuestions, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_myQuestions != null)
                    {
                        _myQuestions.CollectionChanged -= FixupMyQuestions;
                    }
                    _myQuestions = value;
                    if (_myQuestions != null)
                    {
                        _myQuestions.CollectionChanged += FixupMyQuestions;
                    }
                    OnNavigationPropertyChanged("MyQuestions");
                }
            }
        }
        private TrackableCollection<Question> _myQuestions;
    
    	[XmlIgnore]
        [DataMember]
        public TrackableCollection<Question> QuestionsToMe
        {
            get
            {
                if (_questionsToMe == null)
                {
                    _questionsToMe = new TrackableCollection<Question>();
                    _questionsToMe.CollectionChanged += FixupQuestionsToMe;
                }
                return _questionsToMe;
            }
            set
            {
                if (!ReferenceEquals(_questionsToMe, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_questionsToMe != null)
                    {
                        _questionsToMe.CollectionChanged -= FixupQuestionsToMe;
                    }
                    _questionsToMe = value;
                    if (_questionsToMe != null)
                    {
                        _questionsToMe.CollectionChanged += FixupQuestionsToMe;
                    }
                    OnNavigationPropertyChanged("QuestionsToMe");
                }
            }
        }
        private TrackableCollection<Question> _questionsToMe;
    
    	[XmlIgnore]
        [DataMember]
        public TrackableCollection<Transac> Transacs
        {
            get
            {
                if (_transacs == null)
                {
                    _transacs = new TrackableCollection<Transac>();
                    _transacs.CollectionChanged += FixupTransacs;
                }
                return _transacs;
            }
            set
            {
                if (!ReferenceEquals(_transacs, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_transacs != null)
                    {
                        _transacs.CollectionChanged -= FixupTransacs;
                    }
                    _transacs = value;
                    if (_transacs != null)
                    {
                        _transacs.CollectionChanged += FixupTransacs;
                    }
                    OnNavigationPropertyChanged("Transacs");
                }
            }
        }
        private TrackableCollection<Transac> _transacs;

        #endregion
        #region ChangeTracking
    
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
        private ObjectChangeTracker _changeTracker;
    
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }
    
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }
    
        protected bool IsDeserializing { get; private set; }
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }
    
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
            ChangeTracker.ChangeTrackingEnabled = true;
        }
    
        protected virtual void ClearNavigationProperties()
        {
            MyQuestions.Clear();
            QuestionsToMe.Clear();
            Transacs.Clear();
        }

        #endregion
        #region Association Fixup
    
        private void FixupMyQuestions(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (Question item in e.NewItems)
                {
                    item.Owner = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("MyQuestions", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Question item in e.OldItems)
                {
                    if (ReferenceEquals(item.Owner, this))
                    {
                        item.Owner = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("MyQuestions", item);
                    }
                }
            }
        }
    
        private void FixupQuestionsToMe(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (Question item in e.NewItems)
                {
                    item.Receiver = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("QuestionsToMe", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Question item in e.OldItems)
                {
                    if (ReferenceEquals(item.Receiver, this))
                    {
                        item.Receiver = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("QuestionsToMe", item);
                    }
                }
            }
        }
    
        private void FixupTransacs(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (Transac item in e.NewItems)
                {
                    item.User = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("Transacs", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Transac item in e.OldItems)
                {
                    if (ReferenceEquals(item.User, this))
                    {
                        item.User = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("Transacs", item);
                    }
                }
            }
        }

        #endregion
    }
}
