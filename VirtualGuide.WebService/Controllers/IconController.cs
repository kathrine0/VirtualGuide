using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtualGuide.Repository;
using VirtualGuide.ViewModels;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class IconController : ApiController
    {
        private IconRepository ir = new IconRepository();

        [Route("Icon")]
        [HttpGet]
        public IList<IconViewModel> GetIcons()
        {
            return ir.GetIcons();

        }
    }
}
