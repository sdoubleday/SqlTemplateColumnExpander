using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    public class LineProcessorConfig
    {
        public LineProcessorConfig() { }
        public LineProcessorConfig(string targetTag, List<string> perLineSubstitutions)
        {
            this.targetTag = targetTag;
            this.perLineSubstitutions = perLineSubstitutions;
        }

        public String targetTag { get; set; }
        public List<String> perLineSubstitutions { get; set; }



    }
}
