using System;
//using System.Diagnostics;
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
                    .Setup(q => q.Owner).Use<RandomSqlDataSource<User>>(_qyfe.Users)
                    .Setup(q => q.Receiver).Use<RandomSqlDataSource<User>>(_qyfe.Users)
                    .Setup(q => q.private_price).Use<RandomIntegerSource>(250)
                    .Setup(q => q.undesirable).Use<RandomBooleanSource>()
                    .Setup(q => q.deleted).Use<RandomBooleanSource>();
             
                x.AddFromAssemblyContainingType<Transac>();
                x.Include<Transac>()
                    .Setup(t => t.fid).Use<RandomLongSource>()
                    .Setup(t => t.amount).Use<RandomIntegerSource>(25000)
                    .Setup(t => t.User).Use<RandomSqlDataSource<User>>(_qyfe.Users)
                    .Setup(t => t.status).Use<RandomEnumSource<TransacStatus>>()
                    .Setup(t => t.type).Use<RandomEnumSource<TransacType>>()
                    .Setup(t => t.Question).Use<RandomSqlDataSource<Question>>(_qyfe.Questions);

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
            Console.Write(@"- Transactions.");
            var transactions = _qyfe.Transacs;
            foreach (var transaction in transactions)
                _qyfe.Transacs.DeleteObject(transaction);
            Console.WriteLine(@"..");

            Console.Write(@"- Questions.");
            var questions = _qyfe.Questions;
            foreach (var question in questions)
                _qyfe.Questions.DeleteObject(question);
            Console.WriteLine(@"..");

            Console.Write(@"- Users.");
            var users = _qyfe.Users;
            foreach (var user in users)
                _qyfe.Users.DeleteObject(user);
            Console.Write(@".");

            _qyfe.SaveChanges();
            Console.WriteLine(@".");
        }

        public static void AddData()
        {
            const int nbUser = 100;
            const int nbTransac = 300;
            const int nbQuestion = 200;

            Console.Write(@"- Users.");
            var users = _session.List<User>(nbUser).Get();
            foreach (var user in users)
            {
                //Debug.WriteLine(string.Format("{0} - {1} - {2} - {3}", user.id, user.credit_amount, user.activated, user.fid));
                _qyfe.Users.AddObject(user);
            }
            Console.Write(@".");
            _qyfe.SaveChanges();
            Console.WriteLine(@".");

            Console.Write(@"- Questions.");
            var lis = new LoremIpsumSource();
            var rnd = new Random(1337);
            var questions = _session.List<Question>(nbQuestion)
                .First(nbQuestion / 2)
                    .Impose(q => q.date_answer, DateTime.Now)
                    .Impose(q => q.answer, lis.Next(null).Substring(0, rnd.Next(240, 480)) + ".")
                    .Impose(q => q.text, lis.Next(null).Substring(0, rnd.Next(120, 240)) + "?")
                .Next(nbQuestion / 2)
                    .Impose(q => q.text, lis.Next(null).Substring(0, rnd.Next(120, 240)) + "?")
                .All()
                    .Get();
            foreach (var question in questions)
            {
                //Debug.WriteLine(string.Format("{0} - {1} - {2} - {3} - {4} - {5} - {6} - {7} - {8} - {9}", question.id, question.anom_price, question.answer, question.date_answer, question.date_pub,
                //    question.id_owner, question.id_receiver, question.private_price, question.text, question.undesirable)); 
                _qyfe.Questions.AddObject(question);
            }
            Console.Write(@".");
            _qyfe.SaveChanges();
            Console.WriteLine(@".");

            Console.Write(@"- Transacs.");
            var transacs = _session.List<Transac>(nbTransac).Get();
            foreach (var transac in transacs)
            {
                //Debug.WriteLine(string.Format("{0} - {1} - {2} - {3} - {4}", transac.id, transac.fid, transac.amount, transac.status, transac.userId));
                _qyfe.Transacs.AddObject(transac);
            }
            Console.Write(@".");
            _qyfe.SaveChanges();
            Console.WriteLine(@".");
        }
    }
}
