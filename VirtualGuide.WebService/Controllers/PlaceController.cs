using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using VirtualGuide.Services;
using VirtualGuide.Services.Repository;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class PlaceController : ApiController
    {
        private PlaceRepository pr = new PlaceRepository();

        [Route("Places/Categories/{language}")]
        public HttpResponseMessage GetPlaceCategories(string language)
        {
            IList<PlaceCategoryViewModel> categories = pr.GetPlaceCategories(language);

            return Request.CreateResponse<IList<PlaceCategoryViewModel>>(HttpStatusCode.OK, categories);

        }

        /// <summary>
        /// place of travel
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Places/{travelId}")]
        [HttpPost]
        public HttpResponseMessage PostPlace(int travelId, IList<BasicPlaceViewModel> places)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
            }


            try
            {
                pr.AddMany(places, travelId);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }

        [Route("Place/{id}")]
        [HttpPut]
        public HttpResponseMessage PutPlace(int id, BasicPlaceViewModel place)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                pr.Update(id, place);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }

}
