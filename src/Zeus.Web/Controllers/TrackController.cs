using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Zeus.Trackers;
using Zeus.Web.Binders;

namespace Zeus.Web.Controllers
{
    
    public class TrackController : ApiController
    {
        private readonly ITracker tracker;

        public TrackController(ITracker tracker)
        {
            this.tracker = tracker;
        }

        [Route("track")]
        public IHttpActionResult Get(
            [ModelBinder(typeof(IntArrayModelBinder))] int[] ids,
            [ModelBinder(typeof(EnumModelBinder<TrackedEventType>))] TrackedEventType? eventType = null)
        {
            if (ModelState.IsValid)
            {
                if (ids != null && eventType != null)
                {
                    var trackedEvent = new TrackedEvent();
                    trackedEvent.Ids = ids;
                    trackedEvent.Type = eventType.Value;
                    tracker.Track(trackedEvent);
                    return Ok();
                }
            }
            return BadRequest();
        }
    }
}
