using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Trackers
{
    public class ProductStatistics
    {
        public int Id { get; set; }

        public int ListImpressions { get; set; }

        public int DetailsViews { get; set; }

        public int Conversions { get; set; }

        public double ClickRate7Days { get; set; }

        public double ConversionRate7Days { get; set; }

        public double ConversionRate14Days { get; set; }
    }
}
