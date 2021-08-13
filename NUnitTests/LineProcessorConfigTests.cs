using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTemplateColumnExpander.Tests
{
    [TestFixture]
    public class LineProcessorConfigTests
    {
        [TestCase]
        public void Constructor_ExpectedElementCountOmitted()
        {
            //Arrange
            int Expected = 3;
            String targetTag = "test";
            List<string> ListOfColumnsToInsert = new List<string> { "input", "input2" };
            LineProcessorConfig sut = new LineProcessorConfig(targetTag, ListOfColumnsToInsert);
            
            //Act
            int Actual = sut.ExpectedElementCount;

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
        [TestCase]
        public void Constructor_ExpectedElementCountSpecified()
        {
            //Arrange
            int Expected = 4;
            String targetTag = "test";
            List<string> ListOfColumnsToInsert = new List<string> { "input", "input2" };
            LineProcessorConfig sut = new LineProcessorConfig(targetTag, ListOfColumnsToInsert, Expected);

            //Act
            int Actual = sut.ExpectedElementCount;

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
    }
}
