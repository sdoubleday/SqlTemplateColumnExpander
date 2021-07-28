using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    public class FileInFlightTransformer
    {
        public FileInFlightTransformer() { }
        public FileInFlightTransformer(List<FilePathPair> filePathPairs, List<StringReplacementPair> replacementPairs, List<LineProcessorConfig> lineProcessorConfigs)
        {
            this.FilePathPairs = filePathPairs;
            this.LineProcessorConfigs = lineProcessorConfigs;
            this.ReplacementPairs = replacementPairs;
        }

        #region Properties
        public List<FilePathPair> FilePathPairs { get; set; }
        public List<StringReplacementPair> ReplacementPairs { get; set; }
        public List<LineProcessorConfig> LineProcessorConfigs { get; set; }
        #endregion Properties

        public void ReadTransformWrite()
        {
            if (this.FilePathPairs != null)
            {
                foreach (FilePathPair filePathPair in this.FilePathPairs)
                {
                    using (StreamReader reader = new StreamReader(filePathPair.source))
                    using (StreamWriter writer = new StreamWriter(filePathPair.destination, false, Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (!line.Contains("/*Sample*/"))
                            {
                                line = this.StringReplacementsTransformations(line);
                                line = this.LineProcessorTransformations(line);

                                writer.WriteLine(line);
                            }
                        }
                    }
                }
            }
            else
            {
                throw new NoFilesFoundToTransformException();
            }
        }

        public string LineProcessorTransformations(string line)
        {
            if (this.LineProcessorConfigs != null)
            {
                foreach (LineProcessorConfig lineProcessorConfig in this.LineProcessorConfigs)
                {
                    LineProcessor lineProcessor = new LineProcessor(line, lineProcessorConfig);
                    line = lineProcessor.GetLine();
                }
            }
            return line;
        }

        public string StringReplacementsTransformations(string line)
        {
            if (this.ReplacementPairs != null)
            {
                foreach (StringReplacementPair stringReplacementPair in this.ReplacementPairs)
                {
                    line = stringReplacementPair.Replace(line);
                }
            }
            return line;
        }
    }
}
