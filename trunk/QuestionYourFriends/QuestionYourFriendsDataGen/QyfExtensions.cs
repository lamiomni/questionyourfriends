using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionYourFriendsDataGen
{
    public static class QyfExtensions
    {
        private static readonly Random Rnd = new Random();

        public static T Random<T>(this IEnumerable<T> ienum)
        {
            return ienum.Skip(Rnd.Next(ienum.Count())).FirstOrDefault();
        }
    }
}
