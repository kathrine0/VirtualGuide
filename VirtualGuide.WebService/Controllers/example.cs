//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Description;
//using VirtualGuide.Models;
//using VirtualGuide.BindingModels;

//namespace VirtualGuide.WebService.Controllers
//{
//    public class example : ApiController
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();


//        // GET: api/example/5
//        [ResponseType(typeof(BasicPropertyBindingModel))]
//        public IHttpActionResult GetBasicPropertyViewModel(int id)
//        {
//            BasicPropertyBindingModel basicPropertyViewModel = db.BasicPropertyViewModels.Find(id);
//            if (basicPropertyViewModel == null)
//            {
//                return NotFound();
//            }

//            return Ok(basicPropertyViewModel);
//        }

//        // PUT: api/example/5
//        [ResponseType(typeof(void))]
//        public IHttpActionResult PutBasicPropertyViewModel(int id, BasicPropertyBindingModel basicPropertyViewModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            if (id != basicPropertyViewModel.Id)
//            {
//                return BadRequest();
//            }

//            db.Entry(basicPropertyViewModel).State = EntityState.Modified;

//            try
//            {
//                db.SaveChanges();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!BasicPropertyViewModelExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return StatusCode(HttpStatusCode.NoContent);
//        }

//        // POST: api/example
//        [ResponseType(typeof(BasicPropertyBindingModel))]
//        public IHttpActionResult PostBasicPropertyViewModel(BasicPropertyBindingModel basicPropertyViewModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            db.BasicPropertyViewModels.Add(basicPropertyViewModel);
//            db.SaveChanges();

//            return CreatedAtRoute("DefaultApi", new { id = basicPropertyViewModel.Id }, basicPropertyViewModel);
//        }

//        // DELETE: api/example/5
//        [ResponseType(typeof(BasicPropertyBindingModel))]
//        public IHttpActionResult DeleteBasicPropertyViewModel(int id)
//        {
//            BasicPropertyBindingModel basicPropertyViewModel = db.BasicPropertyViewModels.Find(id);
//            if (basicPropertyViewModel == null)
//            {
//                return NotFound();
//            }

//            db.BasicPropertyViewModels.Remove(basicPropertyViewModel);
//            db.SaveChanges();

//            return Ok(basicPropertyViewModel);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private bool BasicPropertyViewModelExists(int id)
//        {
//            return db.BasicPropertyViewModels.Count(e => e.Id == id) > 0;
//        }
//    }
//}