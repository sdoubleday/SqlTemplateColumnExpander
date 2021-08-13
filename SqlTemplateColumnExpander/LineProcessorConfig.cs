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
        public LineProcessorConfig(string targetTag, List<string> ListOfColumnsToInsert) : this()
        {
            this.targetTag = targetTag;
            this.ListOfColumnsToInsert = ListOfColumnsToInsert;
        }
        public LineProcessorConfig(string targetTag, List<string> ListOfColumnsToInsert, int ExpectedElementCount) : this(targetTag, ListOfColumnsToInsert)
        {
            this.ExpectedElementCount = ExpectedElementCount;
        }

        #region Properties
        public String targetTag { get; set; }
        public List<String> ListOfColumnsToInsert { get; set; }
        public int ExpectedElementCount { get; set; }
        #endregion Properties


    }
}
