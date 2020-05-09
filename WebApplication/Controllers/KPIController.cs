using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    public class KPIController : Controller
    {
        private KPIEntities db = new KPIEntities();

        // GET: KPI
        public ActionResult Index(bool check = false)
        {
            var model = db.KPIs.Where(kpi => kpi.idKPI == kpi.id);
            if (check == true)
                model = db.KPIs.Where(kpi => kpi.KPIs.Count() > 0 &&
                kpi.KPIs.Sum(i => i.TyTrong) > 0 && kpi.KPIs.Sum(i => i.TyTrong) != 100
                    && kpi.idKPI != kpi.id);
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
        public ActionResult Create(KPI model, string[] emails)
        {
            var user = User.Identity.Name.Split('@')[0];
            if (db.KPIs.Find(model.idKPI).Email != user)
                return HttpNotFound();
            emails = emails ?? new string[] { user };
            if (ModelState.IsValid)
            using (var scope = new TransactionScope())
            {
                foreach (var email in emails)
                    db.KPIs.Add(new KPI
                    {
                        Email = email,
                        idKPI = model.idKPI,
                        MucTieu = model.MucTieu,
                        TyTrong = model.TyTrong,
                        ChiTieu = model.ChiTieu,
                        DonViTinh = model.DonViTinh,
                        GhiChu = model.GhiChu
                    });
                db.SaveChanges();
                scope.Complete();
                return RedirectToAction("Details", new { id = -model.idKPI });
            }

            ViewBag.KPI = db.KPIs.Find(model.idKPI);
            ViewBag.Names = db.KpiUsers.First().Names;
            return View(model);
        }

        // GET: KPI/Edit/5
        public ActionResult Update(int id)
        {
            ViewBag.Names = db.KpiUsers.First().Names;
            return View(db.KPIs.Find(-id));
        }

        // POST: KPI/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(KPI model)
        {
            model.id = model.id > 0 ? -model.id : model.id;
            if (ModelState.IsValid)
            {
                var user = User.Identity.Name.Split('@')[0];
                var KPI = db.KPIs.Find(model.id);
                if (KPI.Email != user && KPI.KP1.Email != user)
                    return HttpNotFound();
                KPI.MucTieu = model.MucTieu;
                KPI.TyTrong = model.TyTrong;
                KPI.ChiTieu = model.ChiTieu;
                KPI.DonViTinh = model.DonViTinh;
                KPI.GhiChu = model.GhiChu;
                KPI.Email = model.Email ?? user;

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
            var user = User.Identity.Name.Split('@')[0];
            var model = db.KPIs.Find(-id);
            if (model.KP1.Email != user)
                return HttpNotFound();
            db.KPIs.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = -model.idKPI });
        }

        public ActionResult Statistics()
        {
            ViewBag.Names = db.KpiUsers.First().Names;
            return View(db.KPIs.ToList());
        }

        public ActionResult Index2(string email)
        {
            var model = db.KPIs.FirstOrDefault(kpi => kpi.id == kpi.idKPI);
            if (!String.IsNullOrEmpty(email))
            {
                var KPIs = new List<KPI>();
                foreach (var item in db.KPIs.Where(kpi => kpi.Email == email))
                {
                    var temp = item;
                    while (temp.id != temp.idKPI)
                    {
                        KPIs.Add(temp);
                        temp = temp.KP1;
                    }
                }
                KPIs.Add(model);
                ViewBag.KPIs = KPIs.Distinct();
            }
            else
                ViewBag.KPIs = db.KPIs.ToList();
            return View(model);
        }

        public ActionResult Finish(int id)
        {
            ViewBag.Names = db.KpiUsers.First().Names;
            return View(db.KPIs.Find(-id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finish(KPI model)
        {
            model.id = model.id > 0 ? -model.id : model.id;
            if (ModelState.IsValid)
            {
                var user = User.Identity.Name.Split('@')[0];
                var KPI = db.KPIs.Find(model.id);
                if (KPI.Email != user && KPI.KP1.Email != user)
                    return HttpNotFound();
                KPI.KetQua = model.KetQua;
                KPI.GhiChu2 = model.GhiChu2;

                db.Entry(KPI).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = -KPI.idKPI });
            }

            ViewBag.Names = db.KpiUsers.First().Names;
            return View(db.KPIs.Find(model.id));
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
