using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTemplateColumnExpander.Tests
{
    [TestFixture]
    public class CommentTagElementsTests
    {
        [TestCase]
        public void Constructor_ThreeElements()
        {
            //Arrange
            List<string> expectedListOfStrings = new List<string> { "input", "input2", "input3" };

            //Act
            CommentTagElements sut = new CommentTagElements(expectedListOfStrings);

            //Assert
            Assert.AreEqual(expectedListOfStrings[0], sut.Tag);
            Assert.AreEqual(expectedListOfStrings[1], sut.PatternList[0]);
            Assert.AreEqual(expectedListOfStrings[2], sut.JoinString);
        }
    }
}
