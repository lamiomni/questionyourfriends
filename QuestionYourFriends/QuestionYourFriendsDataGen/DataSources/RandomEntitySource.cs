using System;
using System.Collections.Generic;
using System.Linq;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    /// <summary>
    /// Random entity source for AutoPoco
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RandomEntitySource<T> : DatasourceBase<T> where T : class
    {
        private readonly IEnumerable<T> _set;
        private readonly Func<T, bool> _where;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="set">Entity's ObjectSet</param>
        public RandomEntitySource(IEnumerable<T> set)
            : this(set, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="set">Entity's ObjectSet</param>
        /// <param name="where">Filter</param>
        public RandomEntitySource(IEnumerable<T> set, Func<T, bool> where)
        {
            _set = set;
            _where = where;
        }

        /// <summary>
        /// Get a random entity
        /// </summary>
        /// <param name="context">AutoPoco context</param>
        /// <returns>Return an entity</returns>
        public override T Next(IGenerationContext context)
        {
            return _where != null ? _set.AsParallel().Where(_where).Random() : _set.Random();
        }
    }
}