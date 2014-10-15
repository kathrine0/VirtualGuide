using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtualGuide.Services;
using VirtualGuide.Services.Repository;
using VirtualGuide.WebService.Models;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class PropertyController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private PropertyRepository pr = new PropertyRepository();
  
        
        /// <summary>
        /// All Travels in the system
        /// Use: WebApp & Mobile
        /// GET: api/Travels
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Properties")]
        public HttpResponseMessage GetProperties(int travelId)
        {
            var properties = pr.GetPropertiesList(travelId);

            if (properties == null)
                Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse<IList<BasicPropertyViewModel>>(HttpStatusCode.OK, properties);

        }
    }

}
