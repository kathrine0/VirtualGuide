using System.Collections.Generic;
using System.Web.Http;
using VirtualGuide.Repository;
using VirtualGuide.BindingModels;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class IconController : ApiController
    {
        private IconRepository ir = new IconRepository();

        [Route("Icon")]
        [HttpGet]
        public IList<IconBindingModel> GetIcons()
        {
            return ir.GetIcons();

        }
    }
}
