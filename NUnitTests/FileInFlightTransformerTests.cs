using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTemplateColumnExpander.Tests
{
    [TestFixture]
    public class FileInFlightTransformerTests
    {
        [TestCase]
        public void StringReplacementsTransformations()
        {
            //Arguably I could have just counted passes through the loop, and maybe should have, but this was simple enough to test. Unlike LineProcessorTransformations, which are awkward.
            //Arrange
            String Input = "inputtest1inputtest2input";
            String Expected = "output1output2";
            StringReplacementPair stringReplacementPair1 = new StringReplacementPair("input","");
            StringReplacementPair stringReplacementPair2 = new StringReplacementPair("test","output");
            List<StringReplacementPair> stringReplacementPairs = new List<StringReplacementPair> { stringReplacementPair1, stringReplacementPair2 };

            FileInFlightTransformer fileInFlightTransformer = new FileInFlightTransformer();
            fileInFlightTransformer.ReplacementPairs = stringReplacementPairs;

            //Act
            String Actual = fileInFlightTransformer.StringReplacementsTransformations(Input);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
        [TestCase]
        public void CheckForCommentInTagFormatAfterTransformations_Present()
        {
            //Arrange
            String Input = "test /*tag|Pattern|join*/ ";
            bool Expected = true;
            
            FileInFlightTransformer fileInFlightTransformer = new FileInFlightTransformer();

            //Act
            bool Actual = fileInFlightTransformer.CheckForCommentInTagFormatAfterTransformations(Input);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
        [TestCase]
        public void CheckForCommentInTagFormatAfterTransformations_Absent()
        {
            //Arrange
            String Input = "test ";
            bool Expected = false;

            FileInFlightTransformer fileInFlightTransformer = new FileInFlightTransformer();

            //Act
            bool Actual = fileInFlightTransformer.CheckForCommentInTagFormatAfterTransformations(Input);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
        [TestCase]
        public void CheckForCommentInTagFormatAfterTransformations_CommentInDifferentFormat()
        {
            //Arrange
            String Input = "test /*blahblah*/";
            bool Expected = false;

            FileInFlightTransformer fileInFlightTransformer = new FileInFlightTransformer();

            //Act
            bool Actual = fileInFlightTransformer.CheckForCommentInTagFormatAfterTransformations(Input);

            //Assert
            Assert.AreEqual(Expected, Actual);
        }
    }
}
