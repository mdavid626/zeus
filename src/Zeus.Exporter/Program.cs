using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Zeus.Trackers;

namespace Zeus.Exporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            container.RegisterType<ITracker, SqlTracker>();
            container.RegisterType<IStreamProvider, FileStreamProvider>();
            container.RegisterType<IExporter, CsvExporter>();

            var exporter = container.Resolve<IExporter>();
            exporter.Export(new ExporterContextFromArgs(args));
        }
    }
}
