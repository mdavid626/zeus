using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeus.Common;

namespace Zeus.Trackers
{
    public class SqlTracker : ITracker
    {
        public void Track(TrackedEvent trackedEvent)
        {
            
        }

        public IEnumerable<ProductStatistics> GetStatistics(DateTime maxDate)
        {
            return null;
        }
    }
}
