using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    [Serializable]
    public class NoFilesFoundToTransformException : Exception
    {
        #region Properties
        public int ExpectedElementCount { get; set; }
        #endregion Properties
        public NoFilesFoundToTransformException()
        {
        }

        public NoFilesFoundToTransformException(string message) : base(message)
        {
        }

        public NoFilesFoundToTransformException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoFilesFoundToTransformException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
