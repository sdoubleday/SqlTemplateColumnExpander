using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTemplateColumnExpander.Tests
{
    public class FakeLineProcessor : LineProcessor { 
        public FakeLineProcessor (String Input) : base(Input)
        { }
        public override char GetSplitterChar()
        {
            char returnable = ',';
            return returnable;
        }
    }

    [TestFixture]
    public class LineProcessorTests
    {
        [TestCase]
        public void ConfirmHasComment_TrueIfHasSQLBlockComment()
        {
            //Arrange
            Boolean Expected = true;
            String Input = "A String /*SqlComment*/ asdf";
            LineProcessor LineProcessor = new LineProcessor(Input);
            
            //Act
            Boolean Actual = LineProcessor.CheckHasComment();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void ConfirmHasComment_FalseIfNoSQLBlockComment()
        {
            //Arrange
            Boolean Expected = false;
            String Input = "A String //NotASqlComment";
            LineProcessor LineProcessor = new LineProcessor(Input);
            //Act
            Boolean Actual = LineProcessor.CheckHasComment();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void GetComment()
        {
            //Arrange
            String Expected = "SqlComm*en/t";
            String Input = "A String /*SqlComm*en/t*/ asdf";
            LineProcessor LineProcessor = new LineProcessor(Input);
            //Act
            String Actual = LineProcessor.GetComment();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void SplitCommentToList_ReturnsListSplitOnPipe()
        {
            //Arrange
            List<String> Expected = new List<String> { "a", "bbb", "cccc" };
            String Input = "A String /*a|bbb|cccc*/";
            LineProcessor LineProcessor = new LineProcessor(Input);

            //Act
            List<String> Actual = LineProcessor.SplitCommentToList();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void GetInputWithoutComment_RemovesCommentAndTrims()
        {
            //Arrange
            String Expected = "A String";
            String Input = "A String /*a|bbb|cccc*/";
            LineProcessor LineProcessor = new LineProcessor(Input);

            //Act
            String Actual = LineProcessor.GetInputWithoutComment();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void SplitCommentToList_Uses_GetSplitterChar()
        {
            //Arrange
            List<String> Expected = new List<String> { "a", "bbb", "cccc" };
            String Input = "/*a,bbb,cccc*/";
            FakeLineProcessor LineProcessor = new FakeLineProcessor(Input);

            //Act
            List<String> Actual = LineProcessor.SplitCommentToList();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void ValidateSplitList_NothingIfValid()
        {
            //Arrange
            String Input = "/*a|bbb|ccc*/";
            LineProcessor LineProcessor = new LineProcessor(Input);
            //Act

            //Assert
            Assert.That(delegate { LineProcessor.ValidateSplitList(); }, Throws.Nothing);
        }

        [TestCase]
        public void ValidateSplitList_ExceptionIfNotValid()
        {
            //Arrange
            String Input = "/*a|bbb|ccc|ddd*/";
            LineProcessor LineProcessor = new LineProcessor(Input);
            //Act

            //Assert
            Assert.Throws<ExpectedNumberOfElementsNotFoundException>(delegate { LineProcessor.ValidateSplitList(); });
        }

        [TestCase]
        public void ValidateSplitList_Uses_GetExpectedElementCount()
        {
            //Arrange
            String Input = "/*a|bbb*/";
            int Expected = 4;
            LineProcessor lineProcessor = new LineProcessor(Input);
            lineProcessor.lineProcessorConfig.ExpectedElementCount = Expected; 
            //Act

            //Assert
            ExpectedNumberOfElementsNotFoundException ex = Assert.Throws<ExpectedNumberOfElementsNotFoundException>(delegate { lineProcessor.ValidateSplitList(); });
            int Actual = ex.ExpectedElementCount;
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void FormatJoinString_First()
        {
            //Arrange
            LineProcessor lineProcessor = new LineProcessor();
            Boolean isFirst = true;
            String joinString = ",";
            String Expected = "";

            //Act
            String Actual = lineProcessor.FormatJoinString(joinString, isFirst);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void FormatJoinString_NotFirst()
        {
            //Arrange
            LineProcessor lineProcessor = new LineProcessor();
            Boolean isFirst = false;
            String joinString = ",";
            String Expected = ", ";

            //Act
            String Actual = lineProcessor.FormatJoinString(joinString, isFirst);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        public void DetermineControlFlow_JustReturnLine__TrueIfNoSQLBlockComment()
        {
            //Arrange
            Boolean Expected = true;
            String Input = "A String asdf";
            LineProcessor LineProcessor = new LineProcessor(Input);

            //Act
            Boolean Actual = LineProcessor.DetermineControlFlow_JustReturnLine();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void DetermineControlFlow_JustReturnLine__ExceptionIfHasSQLBlockCommentButNoSplitChar()
        {
            //Arrange
            String Input = "A String /*SqlComment*/ asdf";
            LineProcessor LineProcessor = new LineProcessor(Input);
            //Act

            //Assert
            Assert.Throws<ExpectedNumberOfElementsNotFoundException>(delegate { LineProcessor.DetermineControlFlow_JustReturnLine(); });
        }

        [TestCase]
        public void DetermineControlFlow_JustReturnLine__FalseIfPassesValidation()
        {
            //Arrange
            Boolean Expected = false;
            String Input = "A String /*Sql|Com|ment*/ asdf";
            LineProcessor LineProcessor = new LineProcessor(Input);
            //Act
            Boolean Actual = LineProcessor.DetermineControlFlow_JustReturnLine();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void GetLine_Default()
        {
            //Arrange
            String Input = "A String asdf";
            String Expected = Input;
            LineProcessor LineProcessor = new LineProcessor(Input);
            //Act
            String Actual = LineProcessor.GetLine();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void GetLine_ReplaceWithMultipleLines()
        {
            //Arrange
            String Input = "a.SampleString = b.SampleString /*SampleTag|SampleString|AND*/";
            String Expected =
                "a.Test1 = b.Test1" +
                "\r\nAND a.Test2 = b.Test2" +
                "\r\nAND a.Test3 = b.Test3" +
                "\r\n";
            String Tag = "SampleTag";
            List<String> ListOfColumnsToInsert = new List<String> { "Test1", "Test2", "Test3" };
            LineProcessor LineProcessor = new LineProcessor(Input, Tag, ListOfColumnsToInsert);
            //Act
            String Actual = LineProcessor.GetLine();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void GetLine_IgnoreIfDifferentTag()
        {
            //Arrange
            String Input = "a.SampleString = b.SampleString /*SampleTag|SampleString|AND*/";
            String Expected = Input;
            String Tag = "NonSampleTag";
            List<String> ListOfColumnsToInsert = new List<String> { "Test1", "Test2", "Test3" };
            LineProcessor LineProcessor = new LineProcessor(Input, Tag, ListOfColumnsToInsert);
            //Act
            String Actual = LineProcessor.GetLine();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void UpdateLineWithColumnToInsert()
        {
            //Arrange
            LineProcessor sut = new LineProcessor();
            List<String> listOfReplacementPatterns = new List<string> { "PatternList1", "PatternList2" };
            List<String> listOfColumnNameComponents = new List<string> { "ComponentA", "ComponentB" };
            ComponentsOfColumnName componentsOfColumnName = new ComponentsOfColumnName( listOfColumnNameComponents );
            String Input = "prefixPatternList1_separator_PatternList2_suffix";
            String Expected = "prefixComponentA_separator_ComponentB_suffix";

            //Act
            String Actual = sut.UpdateLineWithColumnToInsert(listOfReplacementPatterns, componentsOfColumnName, Input);

            //Assert
            Assert.AreEqual(Expected, Actual);

        }

        [TestCase]
        public void UpdateLineWithColumnToInsert_ErrorIfMisMatchInListLengths()
        {
            //Arrange
            LineProcessor sut = new LineProcessor();
            List<String> listOfReplacementPatterns = new List<string> { "PatternList1", "PatternList2" };
            ComponentsOfColumnName componentsOfColumnName = new ComponentsOfColumnName( new List<string> { "OneComponent" } );
            String Input = "test";

            //Act
            
            //Assert
            Assert.Throws<ComponentsOfColumnNameListLengthException>(delegate { sut.UpdateLineWithColumnToInsert(listOfReplacementPatterns, componentsOfColumnName, Input); });

        }

    }
}
