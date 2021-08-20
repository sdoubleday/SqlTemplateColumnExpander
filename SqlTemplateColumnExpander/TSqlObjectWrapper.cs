using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlTemplateColumnExpander
{
    public class TSqlObjectWrapper
    {
        public TSqlObject sqlObject { get; set; }
        public String ObjectName { get; set; }
        public String SchemaName { get; set; }
        public List<String> ListOfColumnNames { get; set; }
        public GeneratorSpecification generatorSpecification { get; set; }

        #region Constructors
        public TSqlObjectWrapper(GeneratorSpecification generatorSpecification) {
            this.generatorSpecification = generatorSpecification;
        }
        public TSqlObjectWrapper(TSqlObject sqlObject, GeneratorSpecification generatorSpecification) : this(generatorSpecification) {
            this.sqlObject = sqlObject;
            this.ObjectName = this.GetObjectName();
            this.SchemaName = this.GetSchemaName();
            this.PopulateListOfColumns();
        }
        #endregion Constructors

        #region Internal
        public string GetObjectName()
        {
            return this.sqlObject.Name.ToString().Split('.')[1].Replace("[", "").Replace("]", "");
            //This parses the [schema].[object] format that we get back
            //Tested manually.
        }

        public string GetSchemaName()
        {
            string returnable = this.sqlObject.Name.ToString().Split('.')[0].Replace("[", "").Replace("]", "");
            return returnable;
            //This parses the [schema].[object] format that we get back
            //Tested manually.
        }

        public string GetColumnName(TSqlObject col)
        {
            return col.Name.ToString().Split('.')[2].Replace("[", "").Replace("]", "");
            //This parses the [schema].[object].[column] format that we get back
            //Tested manually.
        }

        public void PopulateListOfColumns()
        {
            ModelRelationshipClass relationshipTypeColumns = View.Columns; //This SHOULD work for all TSqlObject objects with a columns relationship. Not tested for Table objects.
            List<String> listOfColumns = new List<String>();
            foreach (var col in this.sqlObject.GetReferenced(relationshipTypeColumns, DacQueryScopes.UserDefined))
            {
                String column = GetColumnName(col);
                listOfColumns.Add(column);
            }
            this.ListOfColumnNames = listOfColumns;
            //Tested manually.
        }

        public string GetMetaObjectName()
        {
            String returnable = null;
            if (this.generatorSpecification.Flag_InObjectNameDelimiter_IsPresent == true)
            {
                returnable = this.ObjectName.Split(this.generatorSpecification.InObjectNameDelimiter)[0];
            }
            else
            {
                returnable = this.ObjectName.Replace(this.generatorSpecification.SrcObjectSearchSuffix,"");
            }
            return returnable;
        }
        public string GetMetaObjectAlias()
        {
            String returnable = null;
            if (this.generatorSpecification.Flag_InObjectNameDelimiter_IsPresent == true)
            {
                returnable = this.ObjectName.Split(this.generatorSpecification.InObjectNameDelimiter)[1].Replace(this.generatorSpecification.SrcObjectSearchSuffix, "");
            }
            return returnable;
        }
        public List<String> GetListOfDimColumns() {
            return this.ListOfColumnNames.Where(mystring => !mystring.StartsWith("Ctl_")).Where(mystring => !mystring.StartsWith("SK_")).Where(mystring => !mystring.StartsWith("NK_")).Where(mystring => !mystring.EndsWith("_OnFactDim")).ToList<String>();
        }
        public List<String> GetListOfSkColumns() {
            List<String> listOfSkColumns = this.ListOfColumnNames.Where(mystring => mystring.StartsWith("SK_")).Where(mystring => !mystring.Contains("_RP_")).ToList<String>();
            List<String> returnable = new List<String>();
            foreach (String theString in listOfSkColumns)
            {
                returnable.Add(theString.Replace("SK_", ""));
            }
            return returnable;
        }
        public List<String> GetListOfNkColumns()
        {
            return this.ListOfColumnNames.Where(mystring => mystring.StartsWith("NK_")).ToList<String>();
        }
        public List<String> GetListOfCtlColumns()
        {
            return this.ListOfColumnNames.Where(mystring => mystring.StartsWith("Ctl_")).ToList<String>();
        }
        public List<ComponentsOfColumnName> GetListOfSkRPColumns()
        {
            List<String> listOfSkRPColumns = this.ListOfColumnNames.Where(mystring => mystring.StartsWith("SK_")).Where(mystring => mystring.Contains("_RP_")).ToList<String>();
            List<ComponentsOfColumnName> returnable = new List<ComponentsOfColumnName>();
            foreach (String textOfColumnName in listOfSkRPColumns )
            {
                String MetaColumnName = textOfColumnName.Split("_RP_")[0].Replace("SK_", "");
                String MetaColumnAlias = textOfColumnName.Split("_RP_")[1];
                List<String> columnComponents = new List<String>
                { 
                     MetaColumnName
                    ,MetaColumnAlias
                };
                returnable.Add(new ComponentsOfColumnName(columnComponents));
            }
            return returnable;
        }

        public List<String> GetListOfDegenDimColumns()
        {
            return this.ListOfColumnNames.Where(mystring => mystring.EndsWith("_OnFactDim")).ToList<String>();
        }
        #endregion Internal

        public List<LineProcessorConfig> GetLineProcessorConfigs()
        {
            List<LineProcessorConfig> returnable = new List<LineProcessorConfig>();

            this.AddLineProcessorConfigToReturnableIfColumnCountGreaterThanZero(returnable, "SurrogateKey_ReplacementPoint", this.GetListOfSkColumns());
            this.AddLineProcessorConfigToReturnableIfColumnCountGreaterThanZero(returnable, "NaturalKey_ReplacementPoint", this.GetListOfNkColumns());
            this.AddLineProcessorConfigToReturnableIfColumnCountGreaterThanZero(returnable, "ControlColumn_ReplacementPoint", this.GetListOfCtlColumns());
            this.AddLineProcessorConfigToReturnableIfColumnCountGreaterThanZero(returnable, "DimensionAttribute_ReplacementPoint", this.GetListOfDimColumns());
            this.AddLineProcessorConfigToReturnableIfColumnCountGreaterThanZero(returnable, "DegenerateDimensionAttribute_ReplacementPoint", this.GetListOfDegenDimColumns());
            
            //This is a List<ComponentsOfColumnName>, not a List<String>.
            if (this.GetListOfSkRPColumns().Count > 0)
            {
                LineProcessorConfig lineProcessorConfigSkRP = new LineProcessorConfig("SurrogateKey_RolePlay_ReplacementPoint", this.GetListOfSkRPColumns(), 4);
                returnable.Add(lineProcessorConfigSkRP);
            }
            return returnable;
        }

        public void AddLineProcessorConfigToReturnableIfColumnCountGreaterThanZero(List<LineProcessorConfig> returnable, String targetTag, List<String> listOfColumns)
        {
            if (listOfColumns.Count > 0)
            {
                LineProcessorConfig lineProcessorConfig = new LineProcessorConfig(targetTag, listOfColumns);
                returnable.Add(lineProcessorConfig);
            }
        }

        public List<StringReplacementPair> GetStringReplacementPairs()
        {
            //Note: I will probably want to standardize the patterns here across projects
            List<StringReplacementPair> returnable = new List<StringReplacementPair> {
                new StringReplacementPair("StagingSchema", this.SchemaName)        //SourceSchema
                ,new StringReplacementPair("templateDimCoreName", this.GetMetaObjectName())     //MetaObjectName
                ,new StringReplacementPair("dimRpName", this.GetMetaObjectAlias())      //MetaObjectAlias
            };
            returnable.AddRange(this.generatorSpecification.replacementPairs);      //Plus the mappings of my template schema names to the output schemas chosen by the user.
            return returnable;
        }
        public List<FilePathPair> GetFilePathPairs()
        {
            List<FilePathPair> returnable = new List<FilePathPair>();
            foreach (FilePathPair pathPair in this.generatorSpecification.TemplateToOutputDirectoryPairs)
            {
                FileListerReplacerPairer fileListerReplacerPairer = new FileListerReplacerPairer(pathPair.source, pathPair.destination, this.GetStringReplacementPairs());
                returnable.AddRange(fileListerReplacerPairer.sourceAndDestinationFilePaths);
            }
            return returnable;
        }
    }
}
