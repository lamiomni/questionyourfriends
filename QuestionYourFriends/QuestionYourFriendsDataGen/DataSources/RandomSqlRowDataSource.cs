using System;
using System.Data.Objects;
using System.Linq;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    public class RandomSqlRowDataSource<T> : DatasourceBase<T> where T : class
    {
        private readonly ObjectSet<T> _set;
        private readonly Func<T, bool> _where;

        public RandomSqlRowDataSource(ObjectSet<T> set) : this(set, null) { }

        public RandomSqlRowDataSource(ObjectSet<T> set, Func<T, bool> where)
        {
            _set = set;
            _where = where;
        }

        public override T Next(IGenerationContext context)
        {
            if (_where != null)
                return _set.AsParallel().Where(_where).Random();
            return _set.Random();
        }
    }

}
