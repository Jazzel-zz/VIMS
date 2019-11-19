using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VIMS.Models;

namespace VIMS.Controllers
{
    public class EstimatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Estimates
        public ActionResult Index()
        {
            var estimates = db.Estimates.Include(e => e.ApplicationUser);
            return View(estimates.ToList());
        }

        // GET: Estimates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estimate estimate = db.Estimates.Find(id);
            if (estimate == null)
            {
                return HttpNotFound();
            }
            return View(estimate);
        }

        // GET: Estimates/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name");
            ViewBag.VehicleId = new SelectList(db.VehicleInformations, "VehicleInformationId", "VehicleName");
            ViewBag.PolicyId = new SelectList(db.PolicyTypes, "PolicyTypeId", "Type");
            Random generator = new Random();
            String generated_code = "";
            var estimateCodes = from item in db.Estimates
                                select item;
            bool quit = false;
            while (quit != true)
            {
                generated_code = generator.Next(0, 999999).ToString("D6");

                if (estimateCodes.Count() == 0)
                {
                    quit = true;
                    ViewBag.Code = "E-" + generated_code;

                }
                else
                {
                    foreach (var item in estimateCodes)
                    {
                        if (generated_code != item.EstimateNumber)
                        {
                            ViewBag.Code = "E-" + generated_code;
                            quit = true;

                        }
                    }

                }
            }
            return View();

        }

        // POST: Estimates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ApplicationUserId,EstimateNumber,VehicleInformationId,PolicyTypeId")] Estimate estimate)
        {
            if (ModelState.IsValid)
            {
                db.Estimates.Add(estimate);
                db.SaveChanges();
                TempData["id"] = estimate.Id;
                return RedirectToAction("GenerateBill", "CustomerBillingInformations");
            }

            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Name", estimate.ApplicationUserId);
            ViewBag.VehicleId = new SelectList(db.VehicleInformations, "Id", "VehicleName", estimate.VehicleInformationId);
            ViewBag.PolicyId = new SelectList(db.PolicyTypes, "Id", "Type", estimate.PolicyTypeId);
            return View(estimate);
        }

        // GET: Estimates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estimate estimate = db.Estimates.Find(id);
            if (estimate == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", estimate.ApplicationUserId);
            ViewBag.VehicleId = new SelectList(db.VehicleInformations, "VehicleInformationId", "VehicleName", estimate.VehicleInformationId);
            ViewBag.PolicyId = new SelectList(db.PolicyTypes, "PolicyTypeId", "Type", estimate.PolicyTypeId);
            return View(estimate);
        }

        // POST: Estimates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VehicleInformationId,PolicyTypeId")] Estimate estimate)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(estimate).State = EntityState.Modified;
                Estimate estimate_data = db.Estimates.Find(estimate.Id);
                estimate_data.VehicleInformationId = estimate.VehicleInformationId;
                estimate_data.PolicyTypeId = estimate.PolicyTypeId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Name", estimate.ApplicationUserId);
            return View(estimate);
        }

        // GET: Estimates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estimate estimate = db.Estimates.Find(id);
            if (estimate == null)
            {
                return HttpNotFound();
            }
            return View(estimate);
        }

        // POST: Estimates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estimate estimate = db.Estimates.Find(id);
            db.Estimates.Remove(estimate);
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
