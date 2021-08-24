using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SqlTemplateColumnExpander
{
    public class LineProcessor
    {
        #region Properties
        public String input { get; set; }
        public LineProcessorConfig lineProcessorConfig { get; set; }
        public CommentTagElements commentTagElements { get; set; }
        #endregion Properties

        #region Constructors
        public LineProcessor () {}
        public LineProcessor (String input)
        {
            this.input = input;
            LineProcessorConfig lineProcessorConfig = new LineProcessorConfig();
            this.lineProcessorConfig = lineProcessorConfig;
        }
        public LineProcessor(String input, String targetTag) : this(input)
        {
            LineProcessorConfig lineProcessorConfig = new LineProcessorConfig();
            lineProcessorConfig.targetTag = targetTag;
            this.lineProcessorConfig = lineProcessorConfig;
        }
        public LineProcessor (String input, String targetTag, List<String> ListOfColumnsToInsert) : this(input, targetTag)
        {
            this.lineProcessorConfig.ListOfColumnsToInsert = this.lineProcessorConfig.ConvertListOfOnePartColumnNamesToListOfComponentsOfColumnName(ListOfColumnsToInsert);
        }
        public LineProcessor (String input, LineProcessorConfig lineProcessorConfig) : this(input)
        {
            this.lineProcessorConfig = lineProcessorConfig;
        }
        #endregion Constructors

        #region Methods
        public Boolean CheckHasComment()
        {
            Regex regex = new Regex("/\\*.*\\*/");
            Boolean returnable = regex.Match(input, 0).Success;
            return returnable;
        }

        public String GetComment()
        {
            Regex regex = new Regex("(?<=/\\*).*(?=\\*/)");
            String returnable = regex.Match(input, 0).Value;
            return returnable;
        }

        public virtual char GetSplitterChar()
        {
            char returnable = '|';
            return returnable;
        }

        public List<String> SplitCommentToList()
        {
            List<String> returnable = this.GetComment().Split(this.GetSplitterChar()).ToList<String>();
            return returnable;
        }
        
        public int GetExpectedElementCount()
        {
            int returnable = this.lineProcessorConfig.ExpectedElementCount;
            return returnable;
        }

        public void ValidateSplitList()
        {
            List<String> list = this.SplitCommentToList();
            int ExpectedElementCount = this.GetExpectedElementCount();
            if (list.Count != ExpectedElementCount)
            {
                throw new ExpectedNumberOfElementsNotFoundException("Input", ExpectedElementCount);
            }
        }

        public void PopulateCommentTagElements()
        {
            //No branching logic that I haven't already tested. Move on.
            List<String> list = this.SplitCommentToList();
            CommentTagElements elements = new CommentTagElements(list);
            this.commentTagElements = elements;
        }

        public Boolean DetermineControlFlow_JustReturnLine()
        {
            Boolean returnable = false;
            if (!this.CheckHasComment())
            {
                returnable = true;
            }
            else if (!this.CheckTag( this.SplitCommentToList()[0] ) )
            { 
                returnable = true;
            }
            else
            {
                this.ValidateSplitList();
            }
            return returnable;
        }

        public string GetLine()
        {
            String returnable = "";
            if (this.DetermineControlFlow_JustReturnLine())
            {
                returnable = this.input;
            }
            else
            {
                this.PopulateCommentTagElements();

                if (CheckTag(this.commentTagElements.Tag))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    Boolean isFirst = true;

                    foreach (ComponentsOfColumnName columnToInsert in this.lineProcessorConfig.ListOfColumnsToInsert)
                    {
                        string intermediate = this.GetInputWithoutComment();
                        intermediate = UpdateLineWithColumnToInsert(this.commentTagElements.PatternList, columnToInsert, intermediate);

                        stringBuilder.Append(this.FormatJoinString(this.commentTagElements.JoinString, isFirst));
                        stringBuilder.AppendLine(intermediate);

                        isFirst = false;
                    }

                    returnable = stringBuilder.ToString();
                }
                else
                {
                    returnable = this.input;
                }
            }
            return returnable;
        }

        private bool CheckTag(string tagFromComment)
        {
            return this.lineProcessorConfig.targetTag == tagFromComment;
        }

        public string UpdateLineWithColumnToInsert(List<String> listOfReplacementPatterns, ComponentsOfColumnName columnToInsert, string intermediate)
        {
            columnToInsert.ConfirmListLengthMatch(listOfReplacementPatterns.Count);
            int theIndex = 0;
            foreach(String theString in listOfReplacementPatterns)
            {
                intermediate = intermediate.Replace(listOfReplacementPatterns[theIndex], columnToInsert.ListOfColumnNameComponents[theIndex]);
                theIndex = theIndex + 1;
            }
            return intermediate;
        }

        public string FormatJoinString(String joinString, bool isFirst)
        {
            string prependable = null;
            if (isFirst)
            {
                prependable = "";
            }
            else
            {
                prependable = joinString + " ";
            }
            return prependable;
        }

        public string GetInputWithoutComment()
        {
            String returnable = this.input.Replace(this.GetComment(), "");
            returnable = returnable.Replace("/**/", "").Trim();
            return returnable;
        }
        #endregion Methods

    }
}
