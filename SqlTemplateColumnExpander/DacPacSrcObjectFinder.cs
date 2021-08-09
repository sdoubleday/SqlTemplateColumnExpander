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
    public class DacPacSrcObjectFinder
    {
        public DacPacSrcObjectFinder() { }

        public DacPacSrcObjectFinder(
             GeneratorSpecification generatorSpecification
        ) {
            this.generatorSpecification = generatorSpecification;
        }

        #region Properties
        GeneratorSpecification generatorSpecification { get; set; }
        #endregion Properties
        public string GetSrcObjectSearchSuffixPlusSquareBracket()
        {
            String returnable = this.generatorSpecification.SrcObjectSearchSuffix.TrimEnd(']') + ']';
            return returnable;
        }

        //Review, at minimum. Maybe test
        public List<TSqlObjectWrapper> GetSourceObjects()
        {

            Microsoft.SqlServer.Dac.Model.TSqlModel sqlModel = new Microsoft.SqlServer.Dac.Model.TSqlModel(this.generatorSpecification.DacpacFilePath);

            //Where takes a predicate thing. No, I haven't figured out how to do that without lambda stuff yet. But this is tolerably readable for main control flow, I think.
            var views = sqlModel.GetObjects(DacQueryScopes.Default, View.TypeClass).ToList().Where(view => view.Name.ToString().EndsWith(this.GetSrcObjectSearchSuffixPlusSquareBracket()));
            var tables = sqlModel.GetObjects(DacQueryScopes.Default, Table.TypeClass).ToList().Where(table => table.Name.ToString().EndsWith(this.GetSrcObjectSearchSuffixPlusSquareBracket()));

            List<TSqlObjectWrapper> sqlObjectWrappers = new List<TSqlObjectWrapper>();
            foreach (TSqlObject sqlObject in views)
            {
                sqlObjectWrappers.Add(new TSqlObjectWrapper(sqlObject, this.generatorSpecification));
            }
            foreach (TSqlObject sqlObject in tables)
            {
                sqlObjectWrappers.Add(new TSqlObjectWrapper(sqlObject, this.generatorSpecification));
            }

            return sqlObjectWrappers;
        }

    }
}
