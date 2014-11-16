using System;
using System.Collections.Generic;
using System.Linq;
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
        /// get places for travel
        /// Use: WebApp 
        /// </summary>
        /// <param name="travelId"></param>
        /// <returns></returns>
        [Route("Places/{travelId}")]
        [HttpGet]
        public HttpResponseMessage GetPlaces(int travelId)
        {
            //todo: authorize creator
            try
            {
                IList<BasicPlaceViewModel> places = pr.GetAllForTravel(travelId);
                return Request.CreateResponse(HttpStatusCode.OK, places);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// create places for travel
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Places")]
        [HttpPost]
        public HttpResponseMessage PostPlaces(IList<BasicPlaceViewModel> places)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
            }


            try
            {
                pr.AddMany(places);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }

        /// <summary>
        /// update places 
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Places")]
        [HttpPut]
        public HttpResponseMessage PutPlaces(IList<BasicPlaceViewModel> places)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
            }


            try
            {
                pr.UpdateMany(places);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }

        /// <summary>
        /// create place for travel
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Place")]
        [HttpPost]
        public HttpResponseMessage PostPlace(BasicPlaceViewModel place)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                var item = pr.Add(place);
                return Request.CreateResponse(HttpStatusCode.Created, item);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// update place
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
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
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }

}
