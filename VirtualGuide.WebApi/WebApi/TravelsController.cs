using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtualGuide.Models;
using VirtualGuide.Services;
using VirtualGuide.Services.Repository;

namespace VirtualGuide.WebApi.WebApi
{
    [Authorize]
    [RoutePrefix("api")]
    public class TravelsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TravelRepository tr = new TravelRepository();

        // GET: api/Travels
        [AllowAnonymous]
        [Route("Travels")]
        public HttpResponseMessage GetTravels()
        {
            var travels = tr.GetApprovedTravelList();

            if (travels == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return Request.CreateResponse<IList<BasicTravelViewModel>>(HttpStatusCode.OK, travels);

        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("OwnedTravels")]
        public HttpResponseMessage GetOwnedTravels()
        {
            var userName = User.Identity.Name;
            var travels = tr.GetOwnedTravelList(userName);

            if (travels == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return Request.CreateResponse<IList<ComplexReadTravelViewModel>>(HttpStatusCode.OK, travels);
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("CreatedTravels")]
        public HttpResponseMessage GetCreatedTravels()
        {
            var userName = User.Identity.Name;
            var travels = tr.GetCreatedTravelList(userName);

            if (travels == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return Request.CreateResponse<IList<BasicTravelViewModel>>(HttpStatusCode.OK, travels);
        }
        


        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("Travel/{id}")]
        // GET: api/Travels/5
        //[ResponseType(typeof(Travel))]
        public HttpResponseMessage GetTravel(int id)
        {

            var userName = User.Identity.Name;
            var travel = tr.GetOwnedTravelDetails(id, userName);

            if (travel == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return Request.CreateResponse<BasicTravelViewModel>(HttpStatusCode.OK, travel);
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

        // POST: api/Travels
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