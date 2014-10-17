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
using VirtualGuide.Services;
using VirtualGuide.Services.Repository;

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
        public HttpResponseMessage GetTravels()
        {         
            try
            {
                IList<BasicTravelViewModel> travels = tr.GetApprovedTravelList();
                return Request.CreateResponse<IList<BasicTravelViewModel>>(HttpStatusCode.OK, travels);
            } 
            catch
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
        public HttpResponseMessage GetOwnedTravels()
        {
            try
            {
                string userName = User.Identity.Name;
                IList<CustomerTravelViewModel> travels = tr.GetOwnedTravelList(userName);
    
                return Request.CreateResponse<IList<CustomerTravelViewModel>>(HttpStatusCode.OK, travels);
            } 
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Listr of travels created by user
        /// Use: WebApp
        /// </summary>
        /// <returns></returns>
        [Route("CreatorTravel")]
        [HttpGet]
        public HttpResponseMessage GetCreatedTravels()
        {
            try
            {
                string userName = User.Identity.Name;
                IList<BasicTravelViewModel> travels = tr.GetCreatedTravelList(userName);

                return Request.CreateResponse<IList<BasicTravelViewModel>>(HttpStatusCode.OK, travels);
            }
            catch
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
        public HttpResponseMessage GetTravel(int id)
        {
            try
            {
                string userName = User.Identity.Name;
                CreatorTravelViewModel travel = tr.GetTravelDetailsForCreator(id, userName);

                return Request.CreateResponse<CreatorTravelViewModel>(HttpStatusCode.OK, travel);
            }
            catch (ObjectNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }


        [Route("CreatorTravel")]
        [HttpPost]
        public HttpResponseMessage PostTravel(CreatorTravelViewModel travel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                CreatorTravelViewModel item = tr.Add(travel);
                return Request.CreateResponse<CreatorTravelViewModel>(HttpStatusCode.Created, item);
            } 
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }


        //TODO
        //[Route("CreatorTravel")]
        //public HttpResponseMessage PutTravel(CreatorTravelViewModel travel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    try
        //    {
        //        tr.Add(travel);
        //        return Request.CreateResponse<CreatorTravelViewModel>(HttpStatusCode.Created, travel);
        //    }
        //    catch
        //    {
        //        throw new HttpResponseException(HttpStatusCode.BadRequest);
        //    }
        //}

        // PUT: api/Travels/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutTravel(int id, Travel travel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != travel.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(travel).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TravelExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //POST: api/Travels
        //[ResponseType(typeof(Travel))]
        //public IHttpActionResult PostTravel(Travel travel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Travels.Add(travel);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = travel.Id }, travel);
        //}

        //// DELETE: api/Travels/5
        //[ResponseType(typeof(Travel))]
        //public IHttpActionResult DeleteTravel(int id)
        //{
        //    Travel travel = db.Travels.Find(id);
        //    if (travel == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Travels.Remove(travel);
        //    db.SaveChanges();

        //    return Ok(travel);
        //}


    }
}
