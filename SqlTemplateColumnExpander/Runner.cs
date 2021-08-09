using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    public class Runner
    {
        #region Properties
        public GeneratorSpecification generatorSpecification { get; set; }
        #endregion Properties

        #region Constructors
        public Runner(GeneratorSpecification generatorSpecification)
        {
            this.generatorSpecification = generatorSpecification;
        }
        #endregion Constructors
        public void Run()
        {
            DacPacSrcObjectFinder objFinder = new DacPacSrcObjectFinder(generatorSpecification);

            foreach (TSqlObjectWrapper myWrapper in objFinder.GetSourceObjects())
            {
                FileInFlightTransformer fileInFlightTransformer = new FileInFlightTransformer(myWrapper.GetFilePathPairs(), myWrapper.GetStringReplacementPairs(), myWrapper.GetLineProcessorConfigs());
                fileInFlightTransformer.ReadTransformWrite();
            }
        }
    }
}
