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
        DacPacSrcObjectFinder() { }

        DacPacSrcObjectFinder(
             String DacpacFilePath
            ,String SrcObjectSearchSuffix
        ) {
            this.DacpacFilePath = DacpacFilePath;
            this.SrcObjectSearchSuffix = SrcObjectSearchSuffix;
        }

        #region Properties
        String DacpacFilePath { get; set; }
        String SrcObjectSearchSuffix { get; set; }
        #endregion Properties

        //Review, at minimum. Maybe test
        public List<TSqlObjectWrapper> GetSourceObjects()
        {

            Microsoft.SqlServer.Dac.Model.TSqlModel sqlModel = new Microsoft.SqlServer.Dac.Model.TSqlModel(this.DacpacFilePath);

            //Where takes a predicate thing. No, I haven't figured out how to do that without lambda stuff yet. But this is tolerably readable for main control flow, I think.
            var views = sqlModel.GetObjects(DacQueryScopes.Default, View.TypeClass).ToList().Where(view => view.Name.ToString().EndsWith(this.SrcObjectSearchSuffix));
            var tables = sqlModel.GetObjects(DacQueryScopes.Default, Table.TypeClass).ToList().Where(table => table.Name.ToString().EndsWith(this.SrcObjectSearchSuffix));

            List<TSqlObjectWrapper> sqlObjectWrappers = new List<TSqlObjectWrapper>();
            foreach (TSqlObject sqlObject in views)
            {
                sqlObjectWrappers.Add(new TSqlObjectWrapper(sqlObject));
            }
            foreach (TSqlObject sqlObject in tables)
            {
                sqlObjectWrappers.Add(new TSqlObjectWrapper(sqlObject));
            }

            return sqlObjectWrappers;
        }

    }
}
