using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoPoco;
using AutoPoco.DataSources;
using AutoPoco.Engine;
using log4net;
using log4net.Config;
using QuestionYourFriendsDataAccess;
using QuestionYourFriendsDataGen.DataSources;

namespace QuestionYourFriendsDataGen
{
    /// <summary>
    /// Data generation class
    /// </summary>
    public static class Program
    {
        private const long FidJr = 100002175177092;
        private const long FidTony = 1203739558;
        private const long FidVictor = 577788285;
        private const long FidAntony = 645810475;
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static QuestionYourFriendsEntities _qyfe;
        private static IGenerationSession _session;
        private const float Version = 1.0f;

        /// <summary>
        /// Main function, generates the database information
        /// </summary>
        public static void Main()
        {
            // log4net initialization.
            string targetFileName = Assembly.GetExecutingAssembly().Location;
            var fi = new FileInfo(string.Concat(targetFileName, ".config"));
            XmlConfigurator.ConfigureAndWatch(fi);

            _logger.InfoFormat(new CultureInfo("en-US"), "=== Launching Qyf DataGen v.{0:0.0}... ===", Version);
            Console.WriteLine(string.Format(new CultureInfo("en-US"), "  === Qyf DataGen v.{0:0.0} ===", Version));
            Console.WriteLine();
            _qyfe = new QuestionYourFriendsEntities();

            // Perform factory set up (once for entire test run)
            IGenerationSessionFactory factory = AutoPocoContainer.Configure(x =>
            {
                x.Conventions(c => c.UseDefaultConventions());

                // Impose question informations
                x.AddFromAssemblyContainingType<Question>();
                x.Include<Question>()
                    .Setup(q => q.date_pub).Use<DateOfBirthSource>()
                    .Setup(q => q.Owner).Use<RandomListSource<User>>(_qyfe.Users)
                    .Setup(q => q.Receiver).Use<RandomListSource<User>>(_qyfe.Users)
                    .Setup(q => q.private_price).Use<RandomIntegerSource>(250)
                    .Setup(q => q.anom_price).Use<RandomIntegerSource>(250)
                    .Setup(q => q.undesirable).Use<ValueSource<bool>>(false)
                    .Setup(q => q.deleted).Use<ValueSource<bool>>(false)
                    .Setup(q => q.text).Use<RandomListSource<string>>(Data.DataGen.Questions)
                    .Setup(q => q.answer).Use<RandomListSource<string>>(Data.DataGen.Answers);

                // Impose transaction informations
                x.AddFromAssemblyContainingType<Transac>();
                x.Include<Transac>()
                    .Setup(t => t.amount).Use<RandomIntegerSource>(250)
                    .Setup(t => t.User).Use<RandomListSource<User>>(_qyfe.Users)
                    .Setup(t => t.Question).Use<RandomListSource<Question>>(_qyfe.Questions)
                    .Setup(t => t.status).Use<RandomEnumSource<TransacStatus>>()
                    .Setup(t => t.type).Use<RandomEnumSource<TransacType>>();

                // Impose user informations
                x.AddFromAssemblyContainingType<User>();
                x.Include<User>()
                    .Setup(u => u.activated).Use<ValueSource<bool>>(true)
                    .Setup(u => u.credit_amount).Use<ValueSource<int>>(2500);
            });

            // Generate one of these per test (factory will be a static variable most likely)
            _session = factory.CreateSession();

            _logger.InfoFormat("Cleaning database:");
            Console.WriteLine(@"    Cleaning database:");
            CleanDb();
            Console.WriteLine();

            _logger.InfoFormat("Generating data:");
            Console.WriteLine(@"    Generating data:");
            AddData();
            Console.WriteLine();

            _logger.InfoFormat("Generation done.");
            Console.Write(@"  Generation done, press enter to exit...");
            Console.ReadLine();
        }

        private static void CleanDb()
        {
            // Clean transactions
            Console.Write(@"      - Transactions.");
            var transactions = _qyfe.Transacs;
            int i = 0;
            foreach (var transaction in transactions)
            {
                _qyfe.Transacs.DeleteObject(transaction);
                i++;
            }
            Console.Write(@".");
            _qyfe.SaveChanges();
            _logger.InfoFormat(string.Format("  - {0} transactions deleted.", i));
            Console.WriteLine(string.Format(". {0} transactions deleted.", i));


            // Clean questions
            Console.Write(@"      - Questions.");
            var questions = _qyfe.Questions;
            i = 0;
            foreach (var question in questions)
            {
                _qyfe.Questions.DeleteObject(question);
                i++;
            }
            Console.Write(@".");
            _qyfe.SaveChanges();
            _logger.InfoFormat(string.Format("  - {0} questions deleted.", i));
            Console.WriteLine(string.Format(". {0} questions deleted.", i));


            // Clean users
            Console.Write(@"      - Users..");
            var users = _qyfe.Users;
            i = 0;
            foreach (var user in users)
            {
                _qyfe.Users.DeleteObject(user);
                i++;
            }
            Console.Write(@".");
            _qyfe.SaveChanges();
            _logger.InfoFormat(string.Format("  - {0} users deleted.", i));
            Console.WriteLine(string.Format(". {0} users deleted.", i));
        }

        private static void AddData()
        {
            const int nbUser = 5;
            const int nbTransac = 0;
            const int nbQuestion = 30;
            int i = 0;


            // Add users
            if (nbUser > 0)
            {
                Console.Write(@"      - Users");
                var users = _session.List<User>(nbUser)
                    .First(1)
                        .Impose(u => u.fid, 0)
                        .Impose(u => u.login, "Question Your Friends")
                    .Next(1)
                        .Impose(u => u.fid, FidJr)
                        .Impose(u => u.login, "jr")
                        .Impose(u => u.passwd, "jr")
                    .Next(1)
                        .Impose(u => u.fid, FidVictor)
                        .Impose(u => u.login, "vic")
                        .Impose(u => u.passwd, "vic")
                    .Next(1)
                        .Impose(u => u.fid, FidAntony)
                        .Impose(u => u.login, "pricebuzz")
                        .Impose(u => u.passwd, "pricebuzz")
                    .Next(1)
                        .Impose(u => u.fid, FidTony)
                        .Impose(u => u.login, "lamiomni")
                        .Impose(u => u.passwd, "lamiomni")
                    .All()
                        .Get();
                Console.Write(@".");
                foreach (var user in users)
                {
                    _qyfe.Users.AddObject(user);
                    i++;
                }
                Console.Write(@".");
                _qyfe.SaveChanges();
                _logger.InfoFormat(string.Format("  - {0} users generated.", i));
                Console.WriteLine(string.Format(". {0} users generated.", i));
            }

            // Add questions
            if (nbQuestion > 0)
            {
                int jrid = BusinessManagement.User.Get(FidJr).id;
                int tonyid = BusinessManagement.User.Get(FidTony).id;
                int antonyid = BusinessManagement.User.Get(FidAntony).id;
                int victorid = BusinessManagement.User.Get(FidVictor).id;

                Console.Write(@"      - Questions");
                var questions = _session.List<Question>(nbQuestion)
                    .First(nbQuestion/2)
                        .Impose(q => q.date_answer, DateTime.Now)
                    .Next(nbQuestion/2 - 6)
                        .Impose(q => q.answer, null)
                    .Next(1)
                        .Impose(q => q.id_owner, jrid)
                        .Impose(q => q.id_receiver, antonyid)
                        .Impose(q => q.answer, null)
                    .Next(1)
                        .Impose(q => q.id_owner, jrid)
                        .Impose(q => q.id_receiver, tonyid)
                        .Impose(q => q.answer, null)
                    .Next(1)
                        .Impose(q => q.id_owner, jrid)
                        .Impose(q => q.id_receiver, victorid)
                    .Next(1)
                        .Impose(q => q.id_owner, antonyid)
                        .Impose(q => q.id_receiver, jrid)
                        .Impose(q => q.answer, null)
                    .Next(1)
                        .Impose(q => q.id_owner, tonyid)
                        .Impose(q => q.id_receiver, jrid)
                        .Impose(q => q.answer, null)
                    .Next(1)
                        .Impose(q => q.id_owner, victorid)
                        .Impose(q => q.id_receiver, jrid)
                        .Impose(q => q.answer, null)
                    .All()
                        .Get();
                Console.Write(@".");
                i = 0;
                foreach (var question in questions)
                {
                    _qyfe.Questions.AddObject(question);
                    i++;
                }
                Console.Write(@".");
                _qyfe.SaveChanges();
                _logger.InfoFormat(string.Format("  - {0} questions generated.", i));
                Console.WriteLine(string.Format(". {0} questions generated.", i));

                // Anti-schizophrenia feature
                Console.Write(@"      - Anti-schizophrenia check.");
                var qs = _qyfe.Questions;
                i = 0;
                foreach (var question in qs.Where(question => question.id_owner == question.id_receiver))
                {
                    _qyfe.Questions.DeleteObject(question);
                    i++;
                }
                Console.Write(@".");
                _qyfe.SaveChanges();
                _logger.InfoFormat(string.Format("  - {0} questions deleted.", i));
                Console.WriteLine(string.Format(". {0} questions deleted.", i));
            }

            // Add transactions
            if (nbTransac > 0)
            {
                Console.Write(@"      - Transacs");
                var transacs = _session.List<Transac>(nbTransac)
                    .First(nbTransac/2)
                    .Impose(t => t.questionId, null)
                    .Impose(t => t.Question, null)
                    .All()
                    .Get();
                Console.Write(@".");
                i = 0;
                foreach (var transac in transacs)
                {
                    _qyfe.Transacs.AddObject(transac);
                    i++;
                }
                Console.Write(@".");
                _qyfe.SaveChanges();
                _logger.InfoFormat(string.Format("  - {0} transactions generated.", i));
                Console.WriteLine(string.Format(". {0} transactions generated.", i));
            }
        }
    }
}
