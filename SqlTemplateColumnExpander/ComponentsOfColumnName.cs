using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    public class ComponentsOfColumnName
    {

        #region Properties
        public List<String> ListOfColumnNameComponents { get; set; }
        #endregion Properties
        #region Constructors
        public ComponentsOfColumnName(List<string> listOfColumnNameComponents)
        {
            this.ListOfColumnNameComponents = listOfColumnNameComponents;
        }
        public ComponentsOfColumnName(string OneColumnNameComponent)
        {
            List<string> listOfColumnNameComponents = new List<string> { OneColumnNameComponent };
            this.ListOfColumnNameComponents = listOfColumnNameComponents;
        }
        #endregion Constructors
        #region Methods
        public bool ConfirmListLengthMatch(int otherListLength)
        {
            bool returnable = false;
            if (this.ListOfColumnNameComponents.Count == otherListLength)
            {
                returnable = true;
            }
            else
            {
                throw new ComponentsOfColumnNameListLengthException();
            }
            return returnable;
        }
        #endregion Methods
    }
}
