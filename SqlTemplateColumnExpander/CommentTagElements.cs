using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    public class CommentTagElements
    {
        #region Properties
        public String Tag { get; set; }
        public String Pattern { get; set; }
        public String JoinString { get; set; }
        #endregion Properties
        #region Constructors
        public CommentTagElements (List<String> listOfStrings)
        {
            this.Tag = listOfStrings[0];
            this.Pattern = listOfStrings[1];
            this.JoinString = listOfStrings[2];
        }
        #endregion Constructors
    }
}
