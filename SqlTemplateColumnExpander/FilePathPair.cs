using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTemplateColumnExpander
{
    public class FilePathPair
    {
        public FilePathPair() { }
        public FilePathPair(string source, string destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public String source { get; set; }
        public String destination { get; set; }

    }
}
