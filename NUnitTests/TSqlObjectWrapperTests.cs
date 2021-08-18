using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTemplateColumnExpander.Tests
{
    [TestFixture]
    public class TSqlObjectWrapperTests
    {
        [TestCase]
        public void GetMetaObjectName_NoObjectInNameDelimiter()
        {
            //Arrange
            String Expected = "TestString_RP_Test";
            String Input = "TestString_RP_Test_SUFFIX";
            String SrcObjectSearchSuffix = "_SUFFIX";
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            generatorSpecification.SrcObjectSearchSuffix = SrcObjectSearchSuffix;

            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ObjectName = Input;
            
            //Act
            String Actual = sqlObjectWrapper.GetMetaObjectName();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void GetMetaObjectName_InObjectNameDelimiter()
        {
            //Arrange
            String Expected = "TestString";
            String Input = "TestString_RP_Test_SUFFIX";
            String InObjectNameDelimiter = "_RP_";
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            generatorSpecification.InObjectNameDelimiter = InObjectNameDelimiter;
            generatorSpecification.Flag_InObjectNameDelimiter_IsPresent = true;

            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ObjectName = Input;

            //Act
            String Actual = sqlObjectWrapper.GetMetaObjectName();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void GetMetaObjectAlias_NoObjectInNameDelimiter()
        {
            //Arrange
            String Expected = null;
            String Input = "TestString_RP_Test_SUFFIX";
            String SrcObjectSearchSuffix = "_SUFFIX";
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            generatorSpecification.SrcObjectSearchSuffix = SrcObjectSearchSuffix;

            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ObjectName = Input;

            //Act
            String Actual = sqlObjectWrapper.GetMetaObjectAlias();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void GetMetaObjectAlias_InObjectNameDelimiter()
        {
            //Arrange
            String Expected = "Test";
            String Input = "TestString_RP_Test_SUFFIX";
            String InObjectNameDelimiter = "_RP_";
            String SrcObjectSearchSuffix = "_SUFFIX";
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            generatorSpecification.SrcObjectSearchSuffix = SrcObjectSearchSuffix;
            generatorSpecification.InObjectNameDelimiter = InObjectNameDelimiter;
            generatorSpecification.Flag_InObjectNameDelimiter_IsPresent = true;

            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ObjectName = Input;

            //Act
            String Actual = sqlObjectWrapper.GetMetaObjectAlias();

            //Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestCase]
        public void GetListOfDimColumns()
        {
            //Arrange
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            List<String> Expected = new List<string> {
                 "IAmADimension"
                ,"ImAlsoADimension"
            };
            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ListOfColumnNames = GenerateColumnList();
            int ExpectedCount = 2;

            //Act
            List<String> Actual = sqlObjectWrapper.GetListOfDimColumns();

            //Assert
            Assert.AreEqual(Expected[0], Actual[0]);
            Assert.AreEqual(Expected[1], Actual[1]);
            Assert.AreEqual(ExpectedCount, Actual.Count);
        }

        [TestCase]
        public void GetListOfSkColumns()
        {
            //Arrange
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            List<String> Expected = new List<string> {
                 "Person"
                ,"Product"
            };
            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ListOfColumnNames = GenerateColumnList();
            int ExpectedCount = 2;

            //Act
            List<String> Actual = sqlObjectWrapper.GetListOfSkColumns();

            //Assert
            Assert.AreEqual(Expected[0], Actual[0]);
            Assert.AreEqual(Expected[1], Actual[1]);
            Assert.AreEqual(ExpectedCount, Actual.Count);
        }
        [TestCase]
        public void GetListOfSkRPColumns()
        {
            //Arrange
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            List<ComponentsOfColumnName> Expected = new List<ComponentsOfColumnName> {
                 new ComponentsOfColumnName ( new List<String> { "Person", "Employee" } )
                ,new ComponentsOfColumnName ( new List<String> { "Person", "Customer" } )
                ,new ComponentsOfColumnName ( new List<String> { "Product", "Moose" } )
            };
            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ListOfColumnNames = GenerateColumnList();
            int ExpectedCount = 3;

            //Act
            List<ComponentsOfColumnName> Actual = sqlObjectWrapper.GetListOfSkRPColumns();

            //Assert
            Assert.AreEqual(Expected[0].ListOfColumnNameComponents[0], Actual[0].ListOfColumnNameComponents[0]);
            Assert.AreEqual(Expected[0].ListOfColumnNameComponents[1], Actual[0].ListOfColumnNameComponents[1]);
            Assert.AreEqual(Expected[1].ListOfColumnNameComponents[0], Actual[1].ListOfColumnNameComponents[0]);
            Assert.AreEqual(Expected[1].ListOfColumnNameComponents[1], Actual[1].ListOfColumnNameComponents[1]);
            Assert.AreEqual(Expected[2].ListOfColumnNameComponents[0], Actual[2].ListOfColumnNameComponents[0]);
            Assert.AreEqual(Expected[2].ListOfColumnNameComponents[1], Actual[2].ListOfColumnNameComponents[1]);
            Assert.AreEqual(ExpectedCount, Actual.Count);
        }
        [TestCase]
        public void GetListOfNkColumns()
        {
            //Arrange
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            List<String> Expected = new List<string> {
                 "NK_SomeGUID"
                ,"NK_SnowflakeSK_DateDim"
            };
            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ListOfColumnNames = GenerateColumnList();
            int ExpectedCount = 2;

            //Act
            List<String> Actual = sqlObjectWrapper.GetListOfNkColumns();

            //Assert
            Assert.AreEqual(Expected[0], Actual[0]);
            Assert.AreEqual(Expected[1], Actual[1]);
            Assert.AreEqual(ExpectedCount, Actual.Count);
        }
        [TestCase]
        public void GetListOfDegenDimColumns()
        {
            //Arrange
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            List<String> Expected = new List<string> {
                 "IAmADimension_OnFactDim"
                ,"ImAlsoADimension_OnFactDim"
            };
            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ListOfColumnNames = GenerateColumnList();
            int ExpectedCount = 2;

            //Act
            List<String> Actual = sqlObjectWrapper.GetListOfDegenDimColumns();

            //Assert
            Assert.AreEqual(Expected[0], Actual[0]);
            Assert.AreEqual(Expected[1], Actual[1]);
            Assert.AreEqual(ExpectedCount, Actual.Count);
        }
        [TestCase]
        public void GetListOfCtlColumns()
        {
            //Arrange
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            List<String> Expected = new List<string> {
                 "Ctl_ImNotADimension"
                ,"Ctl_EffectiveDate"
            };
            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ListOfColumnNames = GenerateColumnList();
            int ExpectedCount = 2;

            //Act
            List<String> Actual = sqlObjectWrapper.GetListOfCtlColumns();

            //Assert
            Assert.AreEqual(Expected[0], Actual[0]);
            Assert.AreEqual(Expected[1], Actual[1]);
            Assert.AreEqual(ExpectedCount, Actual.Count);
        }
        [TestCase]
        public void GetLineProcessorConfigs()
        {
            //Arrange
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            
            LineProcessorConfig lineProcessorConfigSK = new LineProcessorConfig();
            lineProcessorConfigSK.targetTag = "SurrogateKey_ReplacementPoint";
            LineProcessorConfig lineProcessorConfigNK = new LineProcessorConfig();
            lineProcessorConfigNK.targetTag = "NaturalKey_ReplacementPoint";
            LineProcessorConfig lineProcessorConfigCtl = new LineProcessorConfig();
            lineProcessorConfigCtl.targetTag = "ControlColumn_ReplacementPoint";
            LineProcessorConfig lineProcessorConfigDim = new LineProcessorConfig();
            lineProcessorConfigDim.targetTag = "DimensionAttribute_ReplacementPoint";
            LineProcessorConfig lineProcessorConfigDegenDim = new LineProcessorConfig();
            lineProcessorConfigDegenDim.targetTag = "DegenerateDimensionAttribute_ReplacementPoint";
            LineProcessorConfig lineProcessorConfigSkRP = new LineProcessorConfig();
            lineProcessorConfigSkRP.targetTag = "SurrogateKey_RolePlay_ReplacementPoint";

            List<LineProcessorConfig> Expected = new List<LineProcessorConfig>
            {
                lineProcessorConfigSK
                ,lineProcessorConfigNK
                ,lineProcessorConfigCtl
                ,lineProcessorConfigDim
                ,lineProcessorConfigDegenDim
                ,lineProcessorConfigSkRP
            };

            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ListOfColumnNames = GenerateColumnList();

            //Act
            List<LineProcessorConfig> Actual = sqlObjectWrapper.GetLineProcessorConfigs();

            //Assert
            Assert.AreEqual(Expected[0].targetTag, Actual[0].targetTag);
            Assert.AreEqual(Expected[1].targetTag, Actual[1].targetTag);
            Assert.AreEqual(Expected[2].targetTag, Actual[2].targetTag);
            Assert.AreEqual(Expected[3].targetTag, Actual[3].targetTag);
            Assert.AreEqual(Expected[4].targetTag, Actual[4].targetTag);
            Assert.AreEqual(Expected[5].targetTag, Actual[5].targetTag);
        }
        [TestCase]
        public void GetLineProcessorConfigs_ColumnCounts()
        {
            //Arrange
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();
            List<Int32> Expected = new List<Int32>
            {
                2
                ,2
                ,2
                ,2
                ,2
                ,3
            };

            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ListOfColumnNames = GenerateColumnList();

            //Act
            List<LineProcessorConfig> Actual = sqlObjectWrapper.GetLineProcessorConfigs();

            //Assert
            Assert.AreEqual(Expected[0], Actual[0].ListOfColumnsToInsert.Count);
            Assert.AreEqual(Expected[1], Actual[1].ListOfColumnsToInsert.Count);
            Assert.AreEqual(Expected[2], Actual[2].ListOfColumnsToInsert.Count);
            Assert.AreEqual(Expected[3], Actual[3].ListOfColumnsToInsert.Count);
            Assert.AreEqual(Expected[4], Actual[4].ListOfColumnsToInsert.Count);
            Assert.AreEqual(Expected[5], Actual[5].ListOfColumnsToInsert.Count);
        }
        [TestCase]
        public void GetLineProcessorConfigs_SkipIfNoColumns()
        {
            //Arrange
            GeneratorSpecification generatorSpecification = new GeneratorSpecification();

            LineProcessorConfig lineProcessorConfigNK = new LineProcessorConfig();
            lineProcessorConfigNK.targetTag = "NaturalKey_ReplacementPoint";
            LineProcessorConfig lineProcessorConfigCtl = new LineProcessorConfig();
            lineProcessorConfigCtl.targetTag = "ControlColumn_ReplacementPoint";
            LineProcessorConfig lineProcessorConfigDim = new LineProcessorConfig();
            lineProcessorConfigDim.targetTag = "DimensionAttribute_ReplacementPoint";

            List<LineProcessorConfig> Expected = new List<LineProcessorConfig>
            {
                 lineProcessorConfigNK
                ,lineProcessorConfigCtl
                ,lineProcessorConfigDim
            };

            TSqlObjectWrapper sqlObjectWrapper = new TSqlObjectWrapper(generatorSpecification);
            sqlObjectWrapper.ListOfColumnNames = GenerateColumnList_NotAllTypes();

            int ExpectedLineProcessorConfigCount = 3;

            //Act
            List<LineProcessorConfig> Actual = sqlObjectWrapper.GetLineProcessorConfigs();

            //Assert
            Assert.AreEqual(ExpectedLineProcessorConfigCount, Actual.Count);
            Assert.AreEqual(Expected[0].targetTag, Actual[0].targetTag);
            Assert.AreEqual(Expected[1].targetTag, Actual[1].targetTag);
            Assert.AreEqual(Expected[2].targetTag, Actual[2].targetTag);
        }
        private List<String> GenerateColumnList()
        {
            List<String> returnable = new List<String> { 
                 "IAmADimension"
                ,"ImAlsoADimension"
                ,"Ctl_ImNotADimension"
                ,"Ctl_EffectiveDate"
                ,"NK_SomeGUID"
                ,"NK_SnowflakeSK_DateDim" //Will I support Snowflaking? Only time will tell.
                ,"SK_Person"
                ,"SK_Product"
                ,"SK_Person_RP_Employee"
                ,"SK_Person_RP_Customer"
                ,"SK_Product_RP_Moose"
                ,"IAmADimension_OnFactDim"
                ,"ImAlsoADimension_OnFactDim"
            };
            return returnable;
        }
        private List<String> GenerateColumnList_NotAllTypes()
        {
            List<String> returnable = new List<String> {
                 "IAmADimension"
                ,"ImAlsoADimension"
                ,"Ctl_EffectiveDate"
                ,"NK_SomeGUID"
            };
            return returnable;
        }

    }
}
