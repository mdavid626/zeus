using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Trackers
{
    public interface ITracker
    {
        void Track(TrackedEvent trackedEvent);

        IEnumerable<ProductStatistics> GetStatistics(DateTime upperBound);
    }
}
