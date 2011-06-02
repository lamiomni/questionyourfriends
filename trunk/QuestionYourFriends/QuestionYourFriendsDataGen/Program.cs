﻿using System;
using System.Globalization;
using AutoPoco;
using AutoPoco.DataSources;
using AutoPoco.Engine;
using QuestionYourFriendsDataAccess;
using QuestionYourFriendsDataGen.DataSources;

namespace QuestionYourFriendsDataGen
{
    /// <summary>
    /// Data generation class
    /// </summary>
    public static class Program
    {
        private static QuestionYourFriendsEntities _qyfe;
        private static IGenerationSession _session;
        private const float Version = 1.0f;

        /// <summary>
        /// Main function, generates the database information
        /// </summary>
        public static void Main()
        {
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
                    .Setup(q => q.anom_price).Use<RandomIntegerSource>(250)
                    .Setup(q => q.date_pub).Use<DateOfBirthSource>()
                    .Setup(q => q.Owner).Use<RandomEntitySource<User>>(_qyfe.Users)
                    .Setup(q => q.Receiver).Use<RandomEntitySource<User>>(_qyfe.Users)
                    .Setup(q => q.private_price).Use<RandomIntegerSource>(250)
                    .Setup(q => q.undesirable).Use<RandomBooleanSource>()
                    .Setup(q => q.deleted).Use<RandomBooleanSource>();

                // Impose transaction informations
                x.AddFromAssemblyContainingType<Transac>();
                x.Include<Transac>()
                    .Setup(t => t.amount).Use<RandomIntegerSource>(25000)
                    .Setup(t => t.User).Use<RandomEntitySource<User>>(_qyfe.Users)
                    .Setup(t => t.status).Use<RandomEnumSource<TransacStatus>>()
                    .Setup(t => t.type).Use<RandomEnumSource<TransacType>>()
                    .Setup(t => t.Question).Use<RandomEntitySource<Question>>(_qyfe.Questions);

                // Impose user informations
                x.AddFromAssemblyContainingType<User>();
                x.Include<User>()
                    .Setup(u => u.fid).Use<RandomLongSource>()
                    .Setup(u => u.activated).Use<RandomBooleanSource>()
                    .Setup(u => u.credit_amount).Use<RandomIntegerSource>(25000);
            });

            // Generate one of these per test (factory will be a static variable most likely)
            _session = factory.CreateSession();

            Console.WriteLine(@"    Cleaning database:");
            CleanDb();
            Console.WriteLine();

            Console.WriteLine(@"    Generating data:");
            AddData();
            Console.WriteLine();

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
            Console.WriteLine(string.Format(". {0} users deleted.", i));
        }

        private static void AddData()
        {
            const int nbUser = 10;
            const int nbTransac = 200;
            const int nbQuestion = 100;


            // Add users
            Console.Write(@"      - Users");
            var users = _session.List<User>(nbUser)
                // jr
                .First(1)
                    .Impose(u => u.fid, 100002175177092)
                // Victor
                .Next(1)
                    .Impose(u => u.fid, 577788285)
                // Antony
                .Next(1)
                    .Impose(u => u.fid, 645810475)
                // Tony
                .Next(1)
                    .Impose(u => u.fid, 1203739558)
                .All()
                    .Get();
            Console.Write(@".");
            int i = 0;
            foreach (var user in users)
            {
                _qyfe.Users.AddObject(user);
                i++;
            }
            Console.Write(@".");
            _qyfe.SaveChanges();
            Console.WriteLine(string.Format(". {0} users generated.", i));


            // Add questions
            Console.Write(@"      - Questions");
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
            Console.Write(@".");
            i = 0;
            foreach (var question in questions)
            {
                _qyfe.Questions.AddObject(question);
                i++;
            }
            Console.Write(@".");
            _qyfe.SaveChanges();
            Console.WriteLine(string.Format(". {0} questions generated.", i));


            // Add transactions
            Console.Write(@"      - Transacs");
            var transacs = _session.List<Transac>(nbTransac).Get();
            Console.Write(@".");
            i = 0;
            foreach (var transac in transacs)
            {
                _qyfe.Transacs.AddObject(transac);
                i++;
            }
            Console.Write(@".");
            _qyfe.SaveChanges();
            Console.WriteLine(string.Format(". {0} transactions generated.", i));
        }
    }
}