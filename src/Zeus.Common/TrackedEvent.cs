﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Trackers
{
    public class TrackedEvent
    {
        public int[] Ids { get; set; }

        public TrackedEventType Type { get; set; }
    }
}
