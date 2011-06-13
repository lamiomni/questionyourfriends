using System;
using System.Collections.Generic;
using System.Reflection;
using AutoPoco.Configuration;
using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    // Woah
    public class FlexibleEnumerableSource<TSource, TCollectionType, TCollectionElement> : IDatasource<TCollectionType>
        where TCollectionType : IEnumerable<TCollectionElement>
        where TSource : DatasourceBase<TCollectionElement>

    {
        private readonly EnumerableSource<TSource, TCollectionElement> mInnerSource;

        public FlexibleEnumerableSource(int count)
            : this(count, count, new object[] {})
        {
        }

        public FlexibleEnumerableSource(int min, int max)
            : this(min, max, new object[] {})
        {
        }

        public FlexibleEnumerableSource(int minCount, int maxCount, params object[] args)
        {
            mInnerSource = new EnumerableSource<TSource, TCollectionElement>(minCount, maxCount, args);
        }

        #region IDatasource<TCollectionType> Members

        object IDatasource.Next(IGenerationContext context)
        {
            ConstructorInfo ctor = typeof (TCollectionType).GetConstructor(Type.EmptyTypes);
            var propertyCollection =
                (ICollection<TCollectionElement>) Activator.CreateInstance(typeof (TCollectionType));
            IEnumerable<TCollectionElement> collectionContents = mInnerSource.Next(context);

            foreach (TCollectionElement item in collectionContents) propertyCollection.Add(item);

            return propertyCollection;
        }

        #endregion
    }


    /// <summary>
    /// Allows you to use another Source to generate an enumerable collection
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="T"></typeparam>
    public class EnumerableSource<TSource, T> : DatasourceBase<IEnumerable<T>>
        where TSource : IDatasource<T>
    {
        private readonly object[] mArgs;
        private readonly int mMaxCount;
        private readonly int mMinCount;
        private readonly Random mRandom = new Random(1337);
        private readonly IDatasource<T> mSource;

        public EnumerableSource(int count)
            : this(count, count, new object[] {})
        {
        }

        public EnumerableSource(int count, object[] args)
            : this(count, count, args)
        {
        }

        public EnumerableSource(int min, int max)
            : this(min, max, new object[] {})
        {
        }

        public EnumerableSource(int minCount, int maxCount, object[] args)
        {
            mMinCount = minCount;
            mMaxCount = maxCount;
            mArgs = args;

            var factory = new DatasourceFactory(typeof (TSource));
            factory.SetParams(mArgs);
            mSource = (IDatasource<T>) factory.Build();
        }

        public override IEnumerable<T> Next(IGenerationContext context)
        {
            int count = mRandom.Next(mMinCount, mMaxCount + 1);
            for (int i = 0; i < count; i++)
                yield return (T) mSource.Next(context);
        }
    }
}