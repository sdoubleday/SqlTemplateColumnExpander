using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTemplateColumnExpander.Test
{
    [TestFixture]
    public class ComponentsOfColumnNameTests
    {
        [TestCase]
        public void ConfirmListLengthMatch_CountMatchReturnsTrue()
        {
            //Arrange
            bool Expected = true;
            int Input = 3;
            List<string> expectedListOfStrings = new List<string> { "input", "input2", "input3" };

            //Act
            ComponentsOfColumnName sut = new ComponentsOfColumnName(expectedListOfStrings);
            bool Actual = sut.ConfirmListLengthMatch(Input);

            //Assert
            Assert.AreEqual(Expected,Actual);
        }


        [TestCase]
        public void ConfirmListLengthMatch_ComponentsOfColumnNameListLengthException()
        {
            //Arrange
            int Input = 2;
            List<string> expectedListOfStrings = new List<string> { "input", "input2", "input3" };

            //Act
            ComponentsOfColumnName sut = new ComponentsOfColumnName(expectedListOfStrings);

            //Assert
            Assert.Throws<ComponentsOfColumnNameListLengthException>(delegate { sut.ConfirmListLengthMatch(Input); });
        }
    }
}
