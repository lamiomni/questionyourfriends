using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionYourFriendsDataGen
{
    /// <summary>
    /// Extensions for IEnumerable
    /// </summary>
    public static class QyfExtensions
    {
        private static readonly Random Rnd = new Random();

        /// <summary>
        /// Get a value randomly.
        /// </summary>
        /// <typeparam name="T">Type of the IEnumerable</typeparam>
        /// <param name="ienum">IEnumerable container</param>
        /// <returns>A random item from the container</returns>
        public static T Random<T>(this IEnumerable<T> ienum)
        {
            return ienum.Skip(Rnd.Next(ienum.Count())).FirstOrDefault();
        }
    }
}