using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    public class LineProcessorConfig
    {
        public LineProcessorConfig() {
            this.ExpectedElementCount = 3;
        }
        public LineProcessorConfig(string targetTag, List<string> perLineSubstitutions) : this()
        {
            this.targetTag = targetTag;
            this.perLineSubstitutions = perLineSubstitutions;
        }
        public LineProcessorConfig(string targetTag, List<string> perLineSubstitutions, int ExpectedElementCount) : this(targetTag, perLineSubstitutions)
        {
            this.ExpectedElementCount = ExpectedElementCount;
        }

        #region Properties
        public String targetTag { get; set; }
        public List<String> perLineSubstitutions { get; set; }
        public int ExpectedElementCount { get; set; }
        #endregion Properties


    }
}
