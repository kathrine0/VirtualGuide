using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtualGuide.Models;
using VirtualGuide.Services;
using VirtualGuide.Services.Repository;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class TravelController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TravelRepository tr = new TravelRepository();

        /// <summary>
        /// All Travels in the system
        /// Use: WebApp & Mobile
        /// GET: api/Travels
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Travels")]
        public HttpResponseMessage GetTravels()
        {
            var travels = tr.GetApprovedTravelList();

            if (travels == null) 
                Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse<IList<BasicTravelViewModel>>(HttpStatusCode.OK, travels);

        }

        /// <summary>
        /// Travels owned by user
        /// Use: WebApp & Mobile
        /// </summary>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("OwnedTravels")]
        public HttpResponseMessage GetOwnedTravels()
        {
            var userName = User.Identity.Name;
            var travels = tr.GetOwnedTravelList(userName);

            if (travels == null) 
                Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse<IList<CustomerTravelViewModel>>(HttpStatusCode.OK, travels);
        }

        /// <summary>
        /// Travels created by user
        /// Use: WebApp
        /// </summary>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("CreatorTravel")]
        public HttpResponseMessage GetCreatedTravels()
        {
            var userName = User.Identity.Name;
            var travels = tr.GetCreatedTravelList(userName);

            if (travels == null) 
                Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse<IList<BasicTravelViewModel>>(HttpStatusCode.OK, travels);
        }


        /// <summary>
        /// Get travel detail for Creator
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("CreatorTravel/{id}")]
        // GET: api/Travels/5
        //[ResponseType(typeof(Travel))]
        public HttpResponseMessage GetTravel(int id)
        {
            var userName = User.Identity.Name;

            var travel = tr.GetTravelDetailsForCreator(id, userName);

            if (travel == null) 
                Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse<CreatorTravelViewModel>(HttpStatusCode.OK, travel);
        }

        //POST: api/Travels
        [ResponseType(typeof(Travel))]
        [Route("CreatorTravel")]
        public HttpResponseMessage PostTravel(Travel travel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Travels.Add(travel);
            db.SaveChanges();

            return Request.CreateResponse<BasicTravelViewModel>(HttpStatusCode.OK, null);

        }

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TravelExists(int id)
        {
            return db.Travels.Count(e => e.Id == id) > 0;
        }
    }
}
