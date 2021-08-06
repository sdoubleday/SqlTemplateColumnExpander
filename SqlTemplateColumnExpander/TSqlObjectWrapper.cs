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
        public TSqlObjectWrapper() { }
        public TSqlObjectWrapper(TSqlObject sqlObject) {
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
            return this.ListOfColumnNames.Where(mystring => !mystring.StartsWith("Ctl_")).Where(mystring => !mystring.StartsWith("SK_")).Where(mystring => !mystring.StartsWith("NK_")).ToList<String>();
        }
        public List<String> GetListOfSkColumns() {
            return this.ListOfColumnNames.Where(mystring => mystring.StartsWith("SK_")).ToList<String>();
        }
        public List<String> GetListOfNkColumns()
        {
            return this.ListOfColumnNames.Where(mystring => mystring.StartsWith("NK_")).ToList<String>();
        }
        public List<String> GetListOfCtlColumns()
        {
            return this.ListOfColumnNames.Where(mystring => mystring.StartsWith("Ctl_")).ToList<String>();
        }
        #endregion Internal

        public List<LineProcessorConfig> GetLineProcessorConfigs()
        {
            LineProcessorConfig lineProcessorConfigSK = new LineProcessorConfig("SurrogateKey_ReplacementPoint", this.GetListOfSkColumns());
            LineProcessorConfig lineProcessorConfigNK = new LineProcessorConfig("NaturalKey_ReplacementPoint", this.GetListOfNkColumns());
            LineProcessorConfig lineProcessorConfigCtl = new LineProcessorConfig("ControlColumn_ReplacementPoint", this.GetListOfCtlColumns());
            LineProcessorConfig lineProcessorConfigDim = new LineProcessorConfig("DimensionAttribute_ReplacementPoint", this.GetListOfDimColumns());

            List<LineProcessorConfig> returnable = new List<LineProcessorConfig>
            {
                lineProcessorConfigSK
                ,lineProcessorConfigNK
                ,lineProcessorConfigCtl
                ,lineProcessorConfigDim
            };
            
            return returnable;
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
