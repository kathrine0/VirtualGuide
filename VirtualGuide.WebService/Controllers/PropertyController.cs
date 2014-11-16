using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using VirtualGuide.Repository;
using VirtualGuide.ViewModels;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class PropertyController : ApiController
    {
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

            return Request.CreateResponse<IList<BasicPropertyViewModel>>(HttpStatusCode.OK, properties);
        }

        /// <summary>
        /// Properties of travel
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Properties")]
        [HttpPost]
        public HttpResponseMessage PostProperties(IList<BasicPropertyViewModel> properties)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
            }


            try
            {
                pr.AddMany(properties);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }

        [Route("Property")]
        [HttpPost]
        public HttpResponseMessage PostProperty(BasicPropertyViewModel property)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                var item = pr.Add(property);
                return Request.CreateResponse(HttpStatusCode.Created, item);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("Property/{id}")]
        [HttpPut]
        public HttpResponseMessage PutProperty(int id, BasicPropertyViewModel property)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                pr.Update(id, property);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }

}
