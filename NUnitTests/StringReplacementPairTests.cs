using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTemplateColumnExpander.Tests
{
    [TestFixture]
    public class StringReplacementPairTests
    {
        [TestCase]
        public void Replace_IsCaseInsensitive()
        {
            //Arrange
            String Expected = "TestStringTest";
            String Input = "inputStringINPUT";
            String Pattern = "input";
            String Replacement = "Test";
            StringReplacementPair stringReplacementPair = new StringReplacementPair(Pattern, Replacement);
            
            //Act
            String Actual = stringReplacementPair.Replace(Input);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
    }
}
