using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    [Serializable]
    public class ExpectedNumberOfElementsNotFoundException : Exception
    {
        #region Properties
        public int ExpectedElementCount { get; set; }
        #endregion Properties
        public ExpectedNumberOfElementsNotFoundException()
        {
        }

        public ExpectedNumberOfElementsNotFoundException(string message) : base(message)
        {
        }

        public ExpectedNumberOfElementsNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExpectedNumberOfElementsNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ExpectedNumberOfElementsNotFoundException(string message, int ExpectedElementCount) : base(message)
        {
            this.ExpectedElementCount = ExpectedElementCount;
        }

    }
}
