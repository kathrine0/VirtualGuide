using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
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
        [HttpGet]
        public IList<PlaceCategoryViewModel> GetPlaceCategories(string language)
        {
            return pr.GetPlaceCategories(language);

        }

        /// <summary>
        /// get places for travel
        /// Use: WebApp 
        /// </summary>
        /// <param name="travelId"></param>
        /// <returns></returns>
        [Route("Places/{travelId}")]
        [HttpGet]
        public IList<BasicPlaceViewModel> GetPlaces(int travelId)
        {
            //todo: authorize creator
            try
            {
                return pr.GetAllForTravel(travelId);
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
        [ResponseType(typeof(void))]
        public IHttpActionResult PostPlaces(IList<BasicPlaceViewModel> places)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                pr.AddMany(places);
                return StatusCode(HttpStatusCode.Created);
            }
            catch
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// update places 
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Places")]
        [HttpPut]
        [ResponseType(typeof(void))]

        public IHttpActionResult PutPlaces(IList<BasicPlaceViewModel> places)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                pr.UpdateMany(places);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// create place for travel
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Place")]
        [HttpPost]
        [ResponseType(typeof(void))]        
        public IHttpActionResult PostPlace(BasicPlaceViewModel place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var item = pr.Add(place);
                return StatusCode(HttpStatusCode.Created);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// update place
        /// Use: WebApp 
        /// </summary>
        /// <returns></returns>
        [Route("Place/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlace(int id, BasicPlaceViewModel place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                pr.Update(id, place);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch
            {
                return BadRequest();
            }
        }
    }

}
