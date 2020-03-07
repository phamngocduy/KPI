using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    public class KPIController : Controller
    {
        private KPIEntities db = new KPIEntities();

        // GET: KPI
        public ActionResult Index()
        {
            var kPIs = db.KPIs.Include(k => k.KP1);
            return View(kPIs.ToList());
        }

        // GET: KPI/Details/5
        public ActionResult Details(int id)
        {
            var model = db.KPIs.Find(-id);
            if (model == null)
                return HttpNotFound();
            ViewBag.KPI = model;
            return View(model.KPIs.Except(new KPI[] { model }).ToList());
        }

        // GET: KPI/Create
        public ActionResult Create(int id)
        {
            ViewBag.KPI = db.KPIs.Find(-id);
            return View(new KPI { idKPI = -id });
        }

        // POST: KPI/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KPI model)
        {
            if (ModelState.IsValid)
            {
                model.Email = User.Identity.Name;
                db.KPIs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = -model.idKPI });
            }

            ViewBag.KPI = db.KPIs.Find(model.idKPI);
            return View(model);
        }

        // GET: KPI/Edit/5
        public ActionResult Edit(int id)
        {
            return View(db.KPIs.Find(-id));
        }

        // POST: KPI/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KPI model)
        {
            model.id = model.id > 0 ? -model.id : model.id;
            if (ModelState.IsValid)
            {
                var KPI = db.KPIs.Find(model.id);
                KPI.MucTieu = model.MucTieu;
                KPI.TyTrong = model.TyTrong;
                KPI.ChiTieu = model.ChiTieu;
                KPI.DonViTinh = model.DonViTinh;

                db.Entry(KPI).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = -KPI.idKPI });
            }
            return View(db.KPIs.Find(model.id));
        }

        // GET: KPI/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KPI kPI = db.KPIs.Find(id);
            if (kPI == null)
            {
                return HttpNotFound();
            }
            return View(kPI);
        }

        // POST: KPI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KPI kPI = db.KPIs.Find(id);
            db.KPIs.Remove(kPI);
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
