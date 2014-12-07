using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtualGuide.Repository;
using VirtualGuide.BindingModels;

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
        public IList<BasicPropertyBindingModel> GetProperties(int travelId)
        {
            var properties = pr.GetPropertiesList(travelId);
            return properties;
        }

        /// <summary>
        /// Properties of travel
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Properties")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult PostProperties(IList<BasicPropertyBindingModel> properties)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                pr.AddMany(properties);
                return StatusCode(HttpStatusCode.Created);
            }
            catch
            {
                return BadRequest();
            }

        }

        [Route("Property")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult PostProperty(BasicPropertyBindingModel property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var item = pr.Add(property);
                return StatusCode(HttpStatusCode.Created);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Route("Property/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProperty(int id, BasicPropertyBindingModel property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                pr.Update(id, property);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch
            {
                return BadRequest();
            }
        }
    }

}
