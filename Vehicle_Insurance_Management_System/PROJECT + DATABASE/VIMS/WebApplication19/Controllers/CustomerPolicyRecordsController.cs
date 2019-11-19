using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VIMS.Models;

namespace WebApplication19.Controllers
{
    [Authorize]
    public class CustomerPolicyRecordsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CustomerPolicyRecords
        public ActionResult Index()
        {
            if (User.IsInRole("Employee"))
            {
                var customerPolicyRecords = db.CustomerPolicyRecords.ToList();
                return View(customerPolicyRecords);
            }
            else if (User.IsInRole("Customer"))
            {
                string userId = User.Identity.GetUserId();
                var customerPolicyRecords = db.CustomerPolicyRecords.Include(c => c.ApplicationUser).Include(c => c.PolicyType).Include(c => c.VehicleInformation);
                return View(customerPolicyRecords.Where(find => find.ApplicationUserId == userId));
            }
            else
            {
                return HttpNotFound();

            }
            
        }

        // GET: CustomerPolicyRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerPolicyRecord customerPolicyRecord = db.CustomerPolicyRecords.Find(id);
            if (customerPolicyRecord == null)
            {
                return HttpNotFound();
            }
            return View(customerPolicyRecord);
        }

        // GET: CustomerPolicyRecords/Create
        public ActionResult Create()
        {
            ViewBag.SameVehiclePolicyError = "";
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name");
            ViewBag.PolicyTypeId = new SelectList(db.PolicyTypes, "PolicyTypeId", "Type");
            ViewBag.VehicleInformationId = new SelectList(db.VehicleInformations, "VehicleInformationId", "VehicleCompany");
            
            return View();
        }

        // POST: CustomerPolicyRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ApplicationUserId,PolicyNumber,VehicleInformationId,PolicyTypeId,PolicyDate,Tracker")] CustomerPolicyRecord customerPolicyRecord)
        {
            var searchData = db.CustomerPolicyRecords.Where(find => find.VehicleInformationId == customerPolicyRecord.VehicleInformationId).Count();
            if (searchData == 0)
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated == true)
                    {
                        customerPolicyRecord.PolicyDate = DateTime.Now;
                        var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                        var userManager = new UserManager<ApplicationUser>(store);
                        ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
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
                        customerPolicyRecord.PolicyNumber = generated_code;
                        customerPolicyRecord.ApplicationUserId = user.Id;
                        db.CustomerPolicyRecords.Add(customerPolicyRecord);
                        db.SaveChanges();
                        int bill_id = db.CustomerPolicyRecords.Count() + 1;
                        bill_id -= 1;
                        generated_code = generator.Next(0, 999999).ToString("D6");
                        CustomerBillingInformation billingInformation = new CustomerBillingInformation()
                        {
                            CustomerPolicyRecordId = bill_id,
                            BillNumber = "B-" + generated_code,
                        };
                        db.CustomerBillingInformations.Add(billingInformation);
                        db.SaveChanges();
                        TempData["id"] = customerPolicyRecord.Id;
                        return Redirect("/CustomerPolicyRecords/Details/" + customerPolicyRecord.Id);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");

                    }
                }
            }
            else
            {
                ViewBag.SameVehiclePolicyError = "Already applied with current vehicle";
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", customerPolicyRecord.ApplicationUserId);
            ViewBag.PolicyTypeId = new SelectList(db.PolicyTypes, "PolicyTypeId", "Type", customerPolicyRecord.PolicyTypeId);
            ViewBag.VehicleInformationId = new SelectList(db.VehicleInformations, "VehicleInformationId", "VehicleCompany", customerPolicyRecord.VehicleInformationId);
            return View(customerPolicyRecord);
        }

        // GET: CustomerPolicyRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerPolicyRecord customerPolicyRecord = db.CustomerPolicyRecords.Find(id);
            if (customerPolicyRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Name", customerPolicyRecord.ApplicationUserId);
            ViewBag.PolicyTypeId = new SelectList(db.PolicyTypes, "PolicyTypeId", "Type", customerPolicyRecord.PolicyTypeId);
            ViewBag.VehicleInformationId = new SelectList(db.VehicleInformations, "VehicleInformationId", "VehicleCompany", customerPolicyRecord.VehicleInformationId);
            return View(customerPolicyRecord);
        }

        // POST: CustomerPolicyRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ApplicationUserId,PolicyNumber,VehicleInformationId,PolicyTypeId,PolicyDate,Tracker")] CustomerPolicyRecord customerPolicyRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerPolicyRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Name", customerPolicyRecord.ApplicationUserId);
            ViewBag.PolicyTypeId = new SelectList(db.PolicyTypes, "PolicyTypeId", "Type", customerPolicyRecord.PolicyTypeId);
            ViewBag.VehicleInformationId = new SelectList(db.VehicleInformations, "VehicleInformationId", "VehicleCompany", customerPolicyRecord.VehicleInformationId);
            return View(customerPolicyRecord);
        }

        // GET: CustomerPolicyRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerPolicyRecord customerPolicyRecord = db.CustomerPolicyRecords.Find(id);
            if (customerPolicyRecord == null)
            {
                return HttpNotFound();
            }
            return View(customerPolicyRecord);
        }

        // POST: CustomerPolicyRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerPolicyRecord customerPolicyRecord = db.CustomerPolicyRecords.Find(id);
            db.CustomerPolicyRecords.Remove(customerPolicyRecord);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: CustomerPolicyRecords/GetPolicy/1
        public JsonResult GetPolicy(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                int p_id = int.Parse(id);
                return Json(db.PolicyTypes.Find(p_id), JsonRequestBehavior.AllowGet);
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
