using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTemplateColumnExpander.Tests
{
    [TestFixture]
    public class FileListerReplacerPairerTests
    {
        [TestCase]
        public void FormatDestinationPath()
        {
            //Arrange
            String sourcePath = "S:\\SourcePath";
            String Input = sourcePath + "\\ReplaceOnlyInSourceFileName.txt";
            String destinationPath = "c:\\DestPath\\ReplaceOnlyInSourceFileName";
            String Expected = destinationPath + "\\replaced.sql";

            List<StringReplacementPair> replacementPairs = new List<StringReplacementPair>();
            replacementPairs.Add(new StringReplacementPair("ReplaceOnlyInSourceFileName", "replaced"));
            replacementPairs.Add(new StringReplacementPair(".txt", ".sql"));

            FileListerReplacerPairer fileListerReplacerPairer = new FileListerReplacerPairer();

            fileListerReplacerPairer.ReplacementPairs = replacementPairs;
            fileListerReplacerPairer.sourcePath = sourcePath;
            fileListerReplacerPairer.destinationPath = destinationPath;

            //Act
            String Actual = fileListerReplacerPairer.FormatDestinationPath(Input);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void PopulateFilePairs()
        {
            //Arrange
            String sourcePath = "S:\\SourcePath";
            String fileName = "fileName.txt";
            String sourceFilePath = sourcePath + "\\" + fileName;
            String destinationPath = "c:\\DestPath";
            FilePathPair Expected = new FilePathPair( sourceFilePath , destinationPath + "\\" + fileName);


            FileListerReplacerPairer fileListerReplacerPairer = new FileListerReplacerPairer();

            fileListerReplacerPairer.sourcePath = sourcePath;
            fileListerReplacerPairer.destinationPath = destinationPath;
            fileListerReplacerPairer.sourceFilePaths = new List<String> { sourceFilePath };

            //Act
            fileListerReplacerPairer.PopulateFilePairs();
            FilePathPair Actual = fileListerReplacerPairer.sourceAndDestinationFilePaths[0];

            //Assert
            Assert.AreEqual(Expected.source, Actual.source);
            Assert.AreEqual(Expected.destination, Actual.destination);
        }

        [TestCase]
        public void StripSourcePath_LeaveRelativeIfFalse()
        {
            //Arrange
            String sourcePath = "S:\\SourcePath";
            String fileName = "\\relativepath\\fileName.txt";
            String sourceFilePath = sourcePath + fileName;
            String Expected = fileName;

            FileListerReplacerPairer fileListerReplacerPairer = new FileListerReplacerPairer();

            //Act
            String Actual = fileListerReplacerPairer.StripSourcePath(sourcePath,sourceFilePath,false);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void StripSourcePath_StripRelativeIfTrue()
        {
            //Arrange
            String sourcePath = "S:\\SourcePath";
            String fileName = "\\relativepath\\fileName.txt";
            String sourceFilePath = sourcePath + fileName;
            String Expected = "\\fileName.txt";

            FileListerReplacerPairer fileListerReplacerPairer = new FileListerReplacerPairer();

            //Act
            String Actual = fileListerReplacerPairer.StripSourcePath(sourcePath, sourceFilePath, true);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void CleanupSourcePath_SlashOnEnd()
        {
            //Arrange
            String sourcePath = "S:\\Source\\Path\\";
            String Expected = "S:\\Source\\Path";

            FileListerReplacerPairer fileListerReplacerPairer = new FileListerReplacerPairer();

            //Act
            String Actual = fileListerReplacerPairer.CleanupSourcePath(sourcePath);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void CleanupSourcePath_NoSlashOnEnd()
        {
            //Arrange
            String sourcePath = "S:\\Source\\Path";
            String Expected = "S:\\Source\\Path";

            FileListerReplacerPairer fileListerReplacerPairer = new FileListerReplacerPairer();

            //Act
            String Actual = fileListerReplacerPairer.CleanupSourcePath(sourcePath);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
    }
}
