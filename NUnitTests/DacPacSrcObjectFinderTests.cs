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

            DacPacSrcObjectFinder dacPacSrcObjectFinder = new DacPacSrcObjectFinder(path,suffix);

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

            DacPacSrcObjectFinder dacPacSrcObjectFinder = new DacPacSrcObjectFinder(path, suffix);

            //Act
            String Actual = dacPacSrcObjectFinder.GetSrcObjectSearchSuffixPlusSquareBracket();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
    }
}
