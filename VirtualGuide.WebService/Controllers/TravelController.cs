using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtualGuide.Common.Helpers;
using VirtualGuide.Repository;
using VirtualGuide.ViewModels;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class TravelController : ApiController
    {
        private TravelRepository tr = new TravelRepository();

        #region consumed by mobile app

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
                Logger.Instance.LogException(e, LogLevel.error);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("BuyTravel/{id}")]
        [HttpPost]
        public IHttpActionResult BuyTravel(int id)
        {
            try
            {
                string userName = User.Identity.Name;
                CustomerTravelViewModel travel = tr.BuyTravel(id, userName);

                return Ok(travel);
            }
            catch (ObjectNotFoundException e)
            {
                Logger.Instance.LogException(e, LogLevel.error);
                return NotFound();
            }
            catch (Exception e)
            {
                Logger.Instance.LogException(e, LogLevel.error);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [Route("BuyTravelAnonymous/{id}")]
        [HttpPost]
        public IHttpActionResult BuyTravelAnonymous(int id)
        {
            try
            {
                CustomerTravelViewModel travel = tr.BuyTravelAnonymous(id);

                return Ok(travel);
            }
            catch (ObjectNotFoundException e)
            {
                Logger.Instance.LogException(e, LogLevel.error);
                return NotFound();
            }
            catch (Exception e)
            {
                Logger.Instance.LogException(e, LogLevel.error);
                return BadRequest();
            }
        }

        #endregion

        #region consumed by web app

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
                Logger.Instance.LogException(e, LogLevel.error);
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
            catch (ObjectNotFoundException e)
            {
                Logger.Instance.LogException(e, LogLevel.error);
                return NotFound();
            }
            catch (Exception e)
            {
                Logger.Instance.LogException(e, LogLevel.error);
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
                Logger.Instance.LogException(e, LogLevel.error);
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
                Logger.Instance.LogException(new Exception("Invalid Model State"), LogLevel.error);
                return BadRequest(ModelState);
            }
            if (id != travel.Id)
            {
                Logger.Instance.LogException(new Exception("id != travel.Id" + id + " " + travel.Id), LogLevel.error);
                return BadRequest();
            }

            try
            {
                tr.Update(id, travel);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                Logger.Instance.LogException(e, LogLevel.error);
                return BadRequest();
            }
        }
        
        [Route("ApproveTravel/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult ApproveTravel(int id)
        {
            try
            {
                string userName = User.Identity.Name;
                tr.Approve(id, userName);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                Logger.Instance.LogException(e, LogLevel.error);
                return BadRequest();
            }
        }

        #endregion

        #region universal methods

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
                Logger.Instance.LogException(e, LogLevel.error);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
        
        #endregion
    }
}
