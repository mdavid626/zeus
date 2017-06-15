using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Common
{
    public class ProductStatistics
    {
        public int Id { get; set; }

        public int ListImpressions { get; set; }

        public int DetailsViews { get; set; }

        public float ClickRate7Days { get; set; }

        public float ConversionRate7Days { get; set; }

        public float ConversionRate14Days { get; set; }
    }
}
