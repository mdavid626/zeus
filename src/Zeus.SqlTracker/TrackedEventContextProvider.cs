using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Trackers
{
    public class TrackedEventContextProvider : ITrackedEventContextProvider
    {
        public TrackedEventContext Provide()
        {
            return new TrackedEventContext();
        }
    }
}
