using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Zeus.Trackers;

namespace Zeus.Exporter
{
    public class CsvExporter : IExporter
    {
        private ITracker tracker;
        private IStreamProvider streamProvider;

        public CsvExporter(ITracker tracker, IStreamProvider streamProvider)
        {
            this.tracker = tracker;
            this.streamProvider = streamProvider;
        }

        public void Export(IExporterContext context)
        {
            var stats = tracker.GetStatistics(context.UpperBound);

            using (var stream = streamProvider.Create(context.UpperBound))
            {
                var csv = new CsvWriter(stream);
                csv.WriteRecords(stats);
            }
        }
    }
}
