using System;
using System.Diagnostics;
using AutoPoco;
using AutoPoco.DataSources;
using AutoPoco.Engine;
using QuestionYourFriendsDataAccessPoco;
using QuestionYourFriendsDataGen.DataSources;

namespace QuestionYourFriendsDataGen
{
    public class Program
    {
        private static QuestionYourFriendsPocoEntities _qyfe;
        private static IGenerationSession _session;

        static void Main()
        {
            _qyfe = new QuestionYourFriendsPocoEntities();

            // Perform factory set up (once for entire test run)
            IGenerationSessionFactory factory = AutoPocoContainer.Configure(x =>
            {
                x.Conventions(c => c.UseDefaultConventions());

                x.AddFromAssemblyContainingType<Question>();
                x.Include<Question>()
                    .Setup(q => q.anom_price).Use<RandomIntegerSource>(250)
                    .Setup(q => q.date_pub).Use<DateOfBirthSource>()
                    .Setup(q => q.Owner).Use<RandomSqlRowDataSource<User>>(_qyfe.Users)
                    .Setup(q => q.Receiver).Use<RandomSqlRowDataSource<User>>(_qyfe.Users)
                    .Setup(q => q.private_price).Use<RandomIntegerSource>(250)
                    .Setup(q => q.text).Use<RandomStringSource>(120, 240)
                    .Setup(q => q.undesirable).Use<RandomBooleanSource>();
             
                x.AddFromAssemblyContainingType<Transac>();
                x.Include<Transac>()
                    .Setup(t => t.fid).Use<RandomLongSource>()
                    .Setup(t => t.amount).Use<RandomIntegerSource>(25000)
                    .Setup(t => t.User).Use<RandomSqlRowDataSource<User>>(_qyfe.Users)
                    .Setup(t => t.status).Use<RandomStringSource>(6, 8);

                x.AddFromAssemblyContainingType<User>();
                x.Include<User>()
                    .Setup(u => u.fid).Use<RandomLongSource>()
                    .Setup(u => u.activated).Use<RandomBooleanSource>()
                    .Setup(u => u.credit_amount).Use<RandomIntegerSource>(25000);
            });

            // Generate one of these per test (factory will be a static variable most likely)
            _session = factory.CreateSession();

            Console.WriteLine(@"Cleaning database:");
            CleanDb();
            Console.WriteLine();

            Console.WriteLine(@"Adding data:");
            AddData();
            Console.WriteLine();

            Console.Write(@"Generation done, press enter to exit...");
            Console.ReadLine();
        }

        public static void CleanDb()
        {
            Console.WriteLine(@"Cleaning questions...");
            var questions = _qyfe.Questions;
            foreach (var question in questions)
                _qyfe.Questions.DeleteObject(question);

            Console.WriteLine(@"Cleaning transactions...");
            var transactions = _qyfe.Transacs;
            foreach (var transaction in transactions)
                _qyfe.Transacs.DeleteObject(transaction);

            Console.WriteLine(@"Cleaning users...");
            var users = _qyfe.Users;
            foreach (var user in users)
                _qyfe.Users.DeleteObject(user);

            _qyfe.SaveChanges();
        }

        public static void AddData()
        {
            Console.WriteLine(@"Adding users...");
            var users = _session.List<User>(100).Get();
            foreach (var user in users)
            {
                Debug.WriteLine(string.Format("{0} - {1} - {2} - {3}", user.id, user.credit_amount, user.activated, user.fid));
                _qyfe.Users.AddObject(user);
            }
            _qyfe.SaveChanges();

            Console.WriteLine(@"Adding transacs...");
            var transacs = _session.List<Transac>(100).Get();
            foreach (var transac in transacs)
            {
                Debug.WriteLine(string.Format("{0} - {1} - {2} - {3} - {4}", transac.id, transac.fid, transac.amount, transac.status, transac.userId));
                _qyfe.Transacs.AddObject(transac);
            }
            _qyfe.SaveChanges();

            Console.WriteLine(@"Adding questions...");
            var questions = _session.List<Question>(100).Get();
            foreach (var question in questions)
            {
                Debug.WriteLine(string.Format("{0} - {1} - {2} - {3} - {4} - {5} - {6} - {7} - {8} - {9}", question.id, question.anom_price, question.answer, question.date_answer, question.date_pub,
                    question.id_owner, question.id_receiver, question.private_price, question.text, question.undesirable)); 
                _qyfe.Questions.AddObject(question);
            }
            _qyfe.SaveChanges();
        }
    }
}
