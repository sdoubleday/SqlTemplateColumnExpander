using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    [Serializable]
    public class ComponentsOfColumnNameListLengthException : Exception
    {
        #region Properties
        public int ExpectedElementCount { get; set; }
        #endregion Properties
        public ComponentsOfColumnNameListLengthException()
        {
        }

        public ComponentsOfColumnNameListLengthException(string message) : base(message)
        {
        }

        public ComponentsOfColumnNameListLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ComponentsOfColumnNameListLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
