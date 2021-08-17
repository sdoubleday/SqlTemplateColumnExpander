using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    [Serializable]
    public class ConvertEmptyListOfOnePartColumnNamesException : Exception
    {
        #region Properties
        public int ExpectedElementCount { get; set; }
        #endregion Properties
        public ConvertEmptyListOfOnePartColumnNamesException()
        {
        }

        public ConvertEmptyListOfOnePartColumnNamesException(string message) : base(message)
        {
        }

        public ConvertEmptyListOfOnePartColumnNamesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConvertEmptyListOfOnePartColumnNamesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
