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

namespace VirtualGuide.UI.WebApi
{
    [RoutePrefix("api")]
    public class TravelsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TravelRepository tr = new TravelRepository();

        // GET: api/Travels
        [Route("Travels")]
        public IList<BasicTravelViewModel> GetTravels()
        {
            return tr.GetApprovedTravelList();
        }

        [Authorize]
        [Route("OwnedTravels")]
        public IList<BasicTravelViewModel> GetOwnedTravels()
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null
                && HttpContext.Current.User.Identity.Name != null)
            {
                var userName = HttpContext.Current.User.Identity.Name;
                return tr.GetOwnedTravelList(userName);
            }
            else
            {
                //TODO: Throw proper exception
                throw new Exception("Not Authorized");
            }

        }

        [Route("Travels/{id}")]
        // GET: api/Travels/5
        [ResponseType(typeof(Travel))]
        public IHttpActionResult GetTravel(int id)
        {
            Travel travel = db.Travels.Find(id);
            if (travel == null)
            {
                return NotFound();
            }

            return Ok(travel);
        }

        // PUT: api/Travels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTravel(int id, Travel travel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != travel.Id)
            {
                return BadRequest();
            }

            db.Entry(travel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Travels
        [ResponseType(typeof(Travel))]
        public IHttpActionResult PostTravel(Travel travel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Travels.Add(travel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = travel.Id }, travel);
        }

        // DELETE: api/Travels/5
        [ResponseType(typeof(Travel))]
        public IHttpActionResult DeleteTravel(int id)
        {
            Travel travel = db.Travels.Find(id);
            if (travel == null)
            {
                return NotFound();
            }

            db.Travels.Remove(travel);
            db.SaveChanges();

            return Ok(travel);
        }

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