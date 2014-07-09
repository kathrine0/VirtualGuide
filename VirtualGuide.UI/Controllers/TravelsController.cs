using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VirtualGuide.Models;
using VirtualGuide.Services;

namespace VirtualGuide.UI.Controllers
{
    public partial class TravelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Travels
        public virtual ActionResult Index()
        {
            var travels = db.Travels.Include(t => t.Approver).Include(t => t.Creator);
            return View(travels.ToList());
        }

        // GET: Travels/Details/5
        public virtual ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Travel travel = db.Travels.Find(id);
            if (travel == null)
            {
                return HttpNotFound();
            }
            return View(travel);
        }

        // GET: Travels/Create
        public virtual ActionResult Create()
        {
            //ViewBag.ApproverId = new SelectList(db.Users, "Id", "Firstname");
            //ViewBag.CreatorId = new SelectList(db.Users, "Id", "Firstname");
            return View();
        }

        // POST: Travels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create([Bind(Include = "Id,Name,Description,Price,Language")] BasicTravelViewModel model)
        {
            if (ModelState.IsValid)
            {
                Travel travel = new Travel()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Language = model.Language
                };


                db.Travels.Add(travel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Travels/Edit/5
        public virtual ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Travel travel = db.Travels.Find(id);
            if (travel == null)
            {
                return HttpNotFound();
            }

            BasicTravelViewModel travelView = new BasicTravelViewModel(travel);

            return View(travelView);
        }

        // POST: Travels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit([Bind(Include = "Id,Name,Description,Price,Language")] BasicTravelViewModel model)
        {
            if (ModelState.IsValid)
            {
                Travel travel = db.Travels.Find(model.Id);
                if (travel == null)
                {
                    return HttpNotFound();
                }

                travel.Name = model.Name;
                travel.Description = model.Description;
                travel.Price = model.Price;
                travel.Language = model.Language;
               

                db.Entry(travel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Travels/Delete/5
        public virtual ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Travel travel = db.Travels.Find(id);
            if (travel == null)
            {
                return HttpNotFound();
            }
            return View(travel);
        }

        // POST: Travels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            Travel travel = db.Travels.Find(id);
            db.Travels.Remove(travel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
