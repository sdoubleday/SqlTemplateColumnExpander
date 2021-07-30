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
            this.sourcePath = this.CleanupSourcePath(sourcePath);
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

        public String CleanupSourcePath(String sourcePath) {
            String returnable = sourcePath.TrimEnd('\\');
            return returnable;
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
            string destinationFilePath = StripSourcePath(this.sourcePath, sourceFilePath, true);
            destinationFilePath = ConductStringReplacement(destinationFilePath);
            destinationFilePath = this.destinationPath + destinationFilePath;
            return destinationFilePath;
        }

        public string StripSourcePath(string sourcePath, string sourceFilePath, bool alsoStripRelativePath)
        {
            string returnable = "";
            if (alsoStripRelativePath) {
                String[] pathParts = sourceFilePath.Split("\\");
                returnable = "\\" + pathParts[pathParts.Length - 1];
            }
            else
            {
                returnable = sourceFilePath.Replace(sourcePath, "");
            }
            return returnable;
            //Directory.GetFile() prepends the path you provide. So this should be an exact match and simple to replace.
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
