namespace AutoPoco.Engine
{
    public abstract class DatasourceBase<T> : IDatasource<T>
    {
        #region IDatasource<T> Members

        object IDatasource.Next(IGenerationContext context)
        {
            return Next(context);
        }

        #endregion

        /// <summary>
        /// Gets the next object from this data source
        /// </summary>
        /// <returns></returns>
        public abstract T Next(IGenerationContext context);
    }
}