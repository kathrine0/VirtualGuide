using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
    public class TravelController : ApiController
    {
        private TravelRepository tr = new TravelRepository();

        /// <summary>
        /// All Travels in the system
        /// Use: WebApp & Mobile
        /// GET: api/Travels
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Travels")]
        [HttpGet]
        public IList<BasicTravelViewModel> GetTravels()
        {         
            try
            {
                return tr.GetApprovedTravelList();
            } 
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Travels owned by user
        /// Use: WebApp & Mobile
        /// </summary>
        /// <returns></returns>
        [Route("OwnedTravels")]
        [HttpGet]
        public IList<CustomerTravelViewModel> GetOwnedTravels()
        {
            try
            {
                string userName = User.Identity.Name;
                var travels = tr.GetOwnedTravelList(userName);
                return travels;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// TODO: secure this
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("BuyTravel")]
        [HttpPost]
        public IHttpActionResult BuyTravel(int id)
        {
            try
            {
                string userName = User.Identity.Name;
                CustomerTravelViewModel travel = tr.BuyTravel(id, userName);

                return Ok(travel);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Listr of travels created by user
        /// Use: WebApp
        /// </summary>
        /// <returns></returns>
        [Route("CreatorTravel")]
        [HttpGet]
        public IList<BasicTravelViewModel> GetCreatedTravels()
        {
            try
            {
                string userName = User.Identity.Name;
                return tr.GetCreatedTravelList(userName);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }


        /// <summary>
        /// Get travel detail for Creator
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("CreatorTravel/{id}")]
        [HttpGet]
        [ResponseType(typeof(CreatorTravelViewModel))]
        public IHttpActionResult GetTravel(int id)
        {
            try
            {
                string userName = User.Identity.Name;
                CreatorTravelViewModel travel = tr.GetTravelDetailsForCreator(id, userName);

                return Ok(travel);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [Route("CreatorTravel")]
        [ResponseType(typeof(CreatorTravelViewModel))]
        [HttpPost]
        public IHttpActionResult PostTravel(CreatorTravelViewModel travel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                CreatorTravelViewModel item = tr.Add(travel);
                return Created("CreatorTravel/{id}", item);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [Route("CreatorTravel/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTravel(int id, SimpleCreatorTravelViewModel travel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != travel.Id)
            {
                return BadRequest();
            }

            try
            {
                tr.Update(id, travel);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


    }
}
