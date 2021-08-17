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
        [TestCase]
        public void Constructor_ComponentsOfColumnName()
        {
            //Arrange
            String targetTag = "test";
            List<string> listOfColumnNameComponents = new List<string> { "ColNamePart1", "ColNamePart2", "ColNamePart3" };
            ComponentsOfColumnName componentsOfColumnName = new ComponentsOfColumnName(listOfColumnNameComponents);
            List<ComponentsOfColumnName> ListOfColumnLevelComponentsToInsert = new List<ComponentsOfColumnName> { componentsOfColumnName };
            List<string> Expected = listOfColumnNameComponents;
            int ExpectedColumnCount = 1;

            //Act
            LineProcessorConfig sut = new LineProcessorConfig(targetTag, ListOfColumnLevelComponentsToInsert);
            int ActualColumnCount = sut.ListOfColumnsToInsert.Count;

            //Assert
            Assert.AreEqual(ExpectedColumnCount, ActualColumnCount);
            Assert.AreEqual(Expected[0], sut.ListOfColumnsToInsert[0].ListOfColumnNameComponents[0]);
            Assert.AreEqual(Expected[1], sut.ListOfColumnsToInsert[0].ListOfColumnNameComponents[1]);
            Assert.AreEqual(Expected[2], sut.ListOfColumnsToInsert[0].ListOfColumnNameComponents[2]);
        }
        [TestCase]
        public void Constructor_ComponentsOfColumnName_And_ExpectedElementCountSpecified()
        {
            //Arrange
            int Expected = 4;
            String targetTag = "test";
            List<string> listOfColumnNameComponents = new List<string> { "ColNamePart1", "ColNamePart2", "ColNamePart3" };
            ComponentsOfColumnName componentsOfColumnName = new ComponentsOfColumnName( listOfColumnNameComponents );
            List<ComponentsOfColumnName> ListOfColumnsToInsert = new List<ComponentsOfColumnName> { componentsOfColumnName };
            LineProcessorConfig sut = new LineProcessorConfig(targetTag, ListOfColumnsToInsert, Expected);

            //Act
            int Actual = sut.ExpectedElementCount;

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
        [TestCase]
        public void ConvertListOfOnePartColumnNamesToListOfComponentsOfColumnName()
        {
            //Arrange
            List<string> Expected = new List<string> { "ColName1", "ColName2", "ColName3" };
            LineProcessorConfig sut = new LineProcessorConfig();

            //Act
            List<ComponentsOfColumnName> Actual = sut.ConvertListOfOnePartColumnNamesToListOfComponentsOfColumnName(Expected);

            //Assert
            Assert.AreEqual(Expected[0], Actual[0].ListOfColumnNameComponents[0]);
            Assert.AreEqual(Expected[1], Actual[1].ListOfColumnNameComponents[0]);
            Assert.AreEqual(Expected[2], Actual[2].ListOfColumnNameComponents[0]);
        }
        [TestCase]
        public void ConvertListOfOnePartColumnNamesToListOfComponentsOfColumnName_ErrorIfEmptyList()
        {
            //Arrange
            List<string> Input = new List<string> { };
            LineProcessorConfig sut = new LineProcessorConfig();

            //Act
            
            //Assert
            Assert.Throws<ConvertEmptyListOfOnePartColumnNamesException>(delegate { sut.ConvertListOfOnePartColumnNamesToListOfComponentsOfColumnName(Input); });
        }
    }
}
