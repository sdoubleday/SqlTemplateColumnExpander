using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    public class StringReplacementPair
    {
        public StringReplacementPair() { }
        public StringReplacementPair(string pattern, string replacement)
        {
            this.pattern = pattern;
            this.replacement = replacement;
        }

        public String pattern { get; set; }
        public String replacement { get; set; }

        public String Replace (string Input)
        {
            String returnable = "";
            returnable = Input.Replace(this.pattern, this.replacement, StringComparison.OrdinalIgnoreCase);
            return returnable;
        }
    }
}
