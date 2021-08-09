using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTemplateColumnExpander.Tests
{
    [TestFixture]
    public class DacPacSrcObjectFinderTests
    {

        [TestCase]
        public void GetSrcObjectSearchSuffixPlusSquareBracket()
        {
            //Arrange
            String path = "placeholder";
            String suffix = "_blah_";
            String Expected = "_blah_]";

            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            generatorSpecification.DacpacFilePath = path;
            generatorSpecification.SrcObjectSearchSuffix = suffix;

            DacPacSrcObjectFinder dacPacSrcObjectFinder = new DacPacSrcObjectFinder(generatorSpecification);
            //Act
            String Actual = dacPacSrcObjectFinder.GetSrcObjectSearchSuffixPlusSquareBracket();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
        [TestCase]
        public void GetSrcObjectSearchSuffixPlusSquareBracket_DoNotDuplicateBracket()
        {
            //Arrange
            String path = "placeholder";
            String suffix = "_blah_]";
            String Expected = "_blah_]";

            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            generatorSpecification.DacpacFilePath = path;
            generatorSpecification.SrcObjectSearchSuffix = suffix;

            DacPacSrcObjectFinder dacPacSrcObjectFinder = new DacPacSrcObjectFinder(generatorSpecification);

            //Act
            String Actual = dacPacSrcObjectFinder.GetSrcObjectSearchSuffixPlusSquareBracket();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
    }
}
