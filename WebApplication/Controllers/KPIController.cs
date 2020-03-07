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
            var model = db.KPIs.Where(kpi => kpi.idKPI == kpi.id);
            return View(model.ToList());
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
            ViewBag.Names = db.KpiUsers.First().Names;
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
                model.Email = model.Email ?? User.Identity.Name.Split('@')[0];
                db.KPIs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = -model.idKPI });
            }

            ViewBag.KPI = db.KPIs.Find(model.idKPI);
            ViewBag.Names = db.KpiUsers.First().Names;
            return View(model);
        }

        // GET: KPI/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Names = db.KpiUsers.First().Names;
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
                KPI.Email = model.Email ?? User.Identity.Name.Split('@')[0];

                db.Entry(KPI).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = -KPI.idKPI });
            }

            ViewBag.Names = db.KpiUsers.First().Names;
            return View(db.KPIs.Find(model.id));
        }

        // GET: KPI/Delete/5
        public ActionResult Delete(int id)
        {
            return View(db.KPIs.Find(-id));
        }

        // POST: KPI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var model = db.KPIs.Find(-id);
            db.KPIs.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = -model.idKPI });
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
