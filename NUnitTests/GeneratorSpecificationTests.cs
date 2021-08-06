using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTemplateColumnExpander.Tests
{
    [TestFixture]
    public class GeneratorSpecificationTests
    {

        [TestCase]
        public void CleanUpFinalSquareBracket_BracketOnEnd()
        {
            //Arrange
            String input = "_blah_]";
            String Expected = "_blah_";

            GeneratorSpecification generatorSpecification = new GeneratorSpecification();

            //Act
            String Actual = generatorSpecification.CleanUpFinalSquareBracket(input);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void CleanUpFinalSquareBracket_NoBracketOnEnd()
        {
            //Arrange
            String input = "_blah_";
            String Expected = "_blah_";

            GeneratorSpecification generatorSpecification = new GeneratorSpecification();

            //Act
            String Actual = generatorSpecification.CleanUpFinalSquareBracket(input);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
    }
}
