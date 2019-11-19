using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VIMS.Models;

namespace WebApplication19.Controllers
{
    [Authorize]
    public class ClaimDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClaimDetails
        public ActionResult Index()
        {
            if (User.IsInRole("Employee"))
            {
                var claimDetails = db.ClaimDetails.ToList();
                return View(claimDetails);
            }
            else if (User.IsInRole("Customer"))
            {
                string userId = User.Identity.GetUserId();
                var claimDetails = db.ClaimDetails.Include(c => c.CustomerPolicyRecord);

                return View(claimDetails.Where(find => find.CustomerPolicyRecord.ApplicationUserId == userId));
            }
            else
            {
                return HttpNotFound();

            }
        }

        // GET: ClaimDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimDetail claimDetail = db.ClaimDetails.Find(id);
            if (claimDetail == null)
            {
                return HttpNotFound();
            }
            return View(claimDetail);
        }

        // GET: ClaimDetails/Create
        public ActionResult Create()
        {
            ViewBag.SameVehiclePolicyError = "";
            ViewBag.CustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "PolicyNumber");
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }



        // POST: ClaimDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClaimDetailId,ClaimNumber,CustomerPolicyRecordId,PlaceOfAccident,DateOfAccident,InsuredAmount,ClaimableAmount,ApplicationUserId")] ClaimDetail claimDetail)
        {
            var searchData = db.ClaimDetails.Where(find => find.CustomerPolicyRecordId == claimDetail.CustomerPolicyRecordId).Count();
            if (searchData == 0)
            {
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

                if (ModelState.IsValid)
                {
                    claimDetail.ApplicationUserId = User.Identity.GetUserId();
                    claimDetail.ClaimNumber = generated_code;
                    db.ClaimDetails.Add(claimDetail);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.SameVehiclePolicyError = "Already claimed with current policy";
            }
            ViewBag.CustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "PolicyNumber", claimDetail.CustomerPolicyRecordId);
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", claimDetail.ApplicationUserId);
            return View(claimDetail);
        }

        // GET: ClaimDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimDetail claimDetail = db.ClaimDetails.Find(id);
            if (claimDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "ApplicationUserId", claimDetail.CustomerPolicyRecordId);
            return View(claimDetail);
        }

        // POST: ClaimDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClaimDetailId,ClaimNumber,CustomerPolicyRecordId,PlaceOfAccident,DateOfAccident,InsuredAmount,ClaimableAmount")] ClaimDetail claimDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claimDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "ApplicationUserId", claimDetail.CustomerPolicyRecordId);
            return View(claimDetail);
        }

        // GET: ClaimDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimDetail claimDetail = db.ClaimDetails.Find(id);
            if (claimDetail == null)
            {
                return HttpNotFound();
            }
            return View(claimDetail);
        }

        // POST: ClaimDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClaimDetail claimDetail = db.ClaimDetails.Find(id);
            db.ClaimDetails.Remove(claimDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: ClaimDetails/GetPolicyDetail/1
        public JsonResult GetPolicyDetail(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                int p_id = int.Parse(id);
                var customerPolicyRecords = db.CustomerPolicyRecords.Include(c => c.ApplicationUser).Include(c => c.PolicyType).Include(c => c.VehicleInformation).Include(c => c.CustomerBillingInformation);

                var data = (from item in db.CustomerPolicyRecords
                            where item.Id == p_id
                            select new
                            {
                                Number = item.PolicyNumber,
                                Date = (DateTime)item.PolicyDate,
                                Type = item.PolicyType.Type,
                                Vehicle = item.VehicleInformation.VehicleName,
                                Description = item.PolicyType.Description,
                            }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
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
