using System;
using System.Collections.Generic;
using System.Linq;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    public class RandomSqlDataSource<T> : DatasourceBase<T> where T : class
    {
        private readonly IEnumerable<T> _set;
        private readonly Func<T, bool> _where;

        public RandomSqlDataSource(IEnumerable<T> set)
            : this(set, null)
        {
        }

        public RandomSqlDataSource(IEnumerable<T> set, Func<T, bool> where)
        {
            _set = set;
            _where = where;
        }

        public override T Next(IGenerationContext context)
        {
            return _where != null ? _set.AsParallel().Where(_where).Random() : _set.Random();
        }
    }
}