using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    public class FileListerReplacerPairer
    {
        public FileListerReplacerPairer() { }
        public FileListerReplacerPairer(string sourcePath, string destinationPath, List<StringReplacementPair> replacementPairs)
        {
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.ReplacementPairs = replacementPairs;
            this.PopulateSourceFilePaths();
            this.PopulateFilePairs();
        }

        #region Properties
        public String sourcePath { get; set; }
        public String destinationPath { get; set; }
        public List<StringReplacementPair> ReplacementPairs { get; set; }
        public List<String> sourceFilePaths { get; set; }
        public List<FilePathPair> sourceAndDestinationFilePaths { get; set; }
        #endregion Properties

        public void PopulateSourceFilePaths()
        {
            this.sourceFilePaths = Directory.GetFiles(sourcePath,"*",SearchOption.AllDirectories).ToList<String>();
        }

        public void PopulateFilePairs()
        {
            List<FilePathPair> filePathPairs = new List<FilePathPair>();
            foreach (String sourceFilePath in this.sourceFilePaths)
            {
                String destinationFilePath = FormatDestinationPath(sourceFilePath);

                FilePathPair filePathPair = new FilePathPair(sourceFilePath, destinationFilePath);
                filePathPairs.Add(filePathPair);
            }
            this.sourceAndDestinationFilePaths = filePathPairs;
        }

        public String FormatDestinationPath(String sourceFilePath)
        {
            String destinationFilePath = sourceFilePath.Replace(this.sourcePath, ""); //Directory.GetFile() prepends the path you provide. So this should be an exact match and simple to replace.
            destinationFilePath = ConductStringReplacement(destinationFilePath);
            destinationFilePath = this.destinationPath + destinationFilePath;
            return destinationFilePath;
        }

        private string ConductStringReplacement(string destinationFilePath)
        {
            if (this.ReplacementPairs != null)
            {
                foreach (StringReplacementPair srp in this.ReplacementPairs)
                {
                    destinationFilePath = srp.Replace(destinationFilePath);
                }
            }
            return destinationFilePath;
        }
    }
}
