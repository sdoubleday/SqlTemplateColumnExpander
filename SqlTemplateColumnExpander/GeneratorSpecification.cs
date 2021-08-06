using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    public class GeneratorSpecification
    {
        #region Constructors
        public GeneratorSpecification(string dacpacFilePath, string srcObjectSearchSuffix, List<FilePathPair> templateToOutputDirectoryPairs, List<StringReplacementPair> replacementPairs)
        {
            this.DacpacFilePath = dacpacFilePath;
            this.SrcObjectSearchSuffix = this.CleanUpFinalSquareBracket(srcObjectSearchSuffix);
            this.TemplateToOutputDirectoryPairs = templateToOutputDirectoryPairs;
            this.replacementPairs = replacementPairs;
            this.Flag_InObjectNameDelimiter_IsPresent = false;
        }

        public GeneratorSpecification() {
            this.Flag_InObjectNameDelimiter_IsPresent = false;
        }

        public GeneratorSpecification(string dacpacFilePath, string srcObjectSearchSuffix, string inObjectNameDelimiter, List<FilePathPair> templateToOutputDirectoryPairs, List<StringReplacementPair> replacementPairs)
        {
            this.DacpacFilePath = dacpacFilePath;
            this.SrcObjectSearchSuffix = this.CleanUpFinalSquareBracket(srcObjectSearchSuffix);
            this.InObjectNameDelimiter = inObjectNameDelimiter;
            this.TemplateToOutputDirectoryPairs = templateToOutputDirectoryPairs;
            this.replacementPairs = replacementPairs;
            this.Flag_InObjectNameDelimiter_IsPresent = true;
        }
        #endregion Constructors

        #region Properties
        public String DacpacFilePath { get; set; }
        public String SrcObjectSearchSuffix { get; set; }
        public String InObjectNameDelimiter { get; set; }
        public List<FilePathPair> TemplateToOutputDirectoryPairs { get; set; }
        public List<StringReplacementPair> replacementPairs { get; set; }
        public Boolean Flag_InObjectNameDelimiter_IsPresent { get; set; }
        #endregion Properties

        public string CleanUpFinalSquareBracket(String input)
        {
            String returnable = input.TrimEnd(']');
            return returnable;
        }
    }
}
