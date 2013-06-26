using System;
using System.Collections.Generic;

namespace RedBit.CCAdmin
{
    /// <summary>
    /// Defines a collection of results that get returned
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityResultsArgs<T> : ResultArgs
    {
        public EntityResultsArgs(Exception ex)
            : base(ex)
        {
        }

        public EntityResultsArgs(IEnumerable<T> results)
            : base(null)
        {
            _results = results;
        }

        private IEnumerable<T> _results;

        public IEnumerable<T> Results
        {
            get { return _results; }
        }

    }

    /// <summary>
    /// Defines only a single item that gets returned
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityResultArgs<T> : ResultArgs
    {
         public EntityResultArgs(Exception ex)
            : base(ex)
        {
        }

         public EntityResultArgs(T result)
            : base(null)
        {
            _result = result;
        }

         private T _result;

         public T Result
         {
             get { return _result; }
         }
    }
}
