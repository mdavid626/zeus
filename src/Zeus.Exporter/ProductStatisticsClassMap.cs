using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Zeus.Trackers;

namespace Zeus.Exporter
{
    public class ProductStatisticsClassMap : CsvClassMap<ProductStatistics>
    {
        public ProductStatisticsClassMap()
        {
            Map(m => m.Id).Name("ID");
            Map(m => m.ListImpressions).Name("# List Impressions");
            Map(m => m.DetailsViews).Name("# Details Views");
            Map(m => m.Conversions).Name("# Conversions");
            Map(m => m.ClickRate7Days).Name("Click Rate 7 Days");
            Map(m => m.ConversionRate7Days).Name("Conversion Rate 7 Days");
            Map(m => m.ConversionRate14Days).Name("Conversion Rate 14 Days");
        }
    }
}
