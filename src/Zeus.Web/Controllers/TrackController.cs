using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Zeus.Web.Controllers
{
    public class TrackController : ApiController
    {
        [Route("track")]
        public string Get()
        {
            return String.Empty;
        }
    }
}
