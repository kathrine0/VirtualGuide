using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtualGuide.Services;
using VirtualGuide.Services.Repository;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class IconController : ApiController
    {
        private IconRepository ir = new IconRepository();

        [Route("Icons")]
        [HttpGet]
        public HttpResponseMessage GetIcons()
        {
            IList<IconViewModel> icons = ir.GetIcons();

            return Request.CreateResponse<IList<IconViewModel>>(HttpStatusCode.OK, icons);

        }
    }
}
