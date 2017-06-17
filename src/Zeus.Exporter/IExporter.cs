using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Exporter
{
    public interface IExporter
    {
        void Export(IExporterContext context);
    }
}
