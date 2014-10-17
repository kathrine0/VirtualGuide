using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
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
        /// Properties of travel
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Properties")]
        [HttpGet]
        public HttpResponseMessage GetProperties(int travelId)
        {
            var properties = pr.GetPropertiesList(travelId);

            if (properties == null)
                Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse<IList<BasicPropertyViewModel>>(HttpStatusCode.OK, properties);

        }


        /// <summary>
        /// Properties of travel
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Properties/{travelId}")]
        [HttpPost]
        public HttpResponseMessage PostProperties(int travelId, IList<BasicPropertyViewModel> properties)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
            }


            try
            {
                pr.AddMany(properties, travelId);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }
    }

}
