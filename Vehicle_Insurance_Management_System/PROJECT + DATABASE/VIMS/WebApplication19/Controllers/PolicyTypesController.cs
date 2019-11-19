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
    public class PolicyTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: PolicyTypes
        [Authorize]
        public ActionResult Index()
        {
            var policyTypes = db.PolicyTypes.Include(p => p.ApplicationUser);
            return View(policyTypes.ToList());
        }
        [Authorize(Roles = "Employee")]
        // GET: PolicyTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PolicyType policyType = db.PolicyTypes.Find(id);
            if (policyType == null)
            {
                return HttpNotFound();
            }
            return View(policyType);
        }
        [Authorize(Roles = "Employee")]
        // GET: PolicyTypes/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: PolicyTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PolicyTypeId,Type,Description,Rate,Offer,Tracker,ApplicationUserId")] PolicyType policyType)
        {
            if (ModelState.IsValid)
            {
                db.PolicyTypes.Add(policyType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Name", policyType.ApplicationUserId);
            return View(policyType);
        }

        [Authorize(Roles = "Employee")]
        // GET: PolicyTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PolicyType policyType = db.PolicyTypes.Find(id);
            if (policyType == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Name", policyType.ApplicationUserId);
            return View(policyType);
        }

        // POST: PolicyTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PolicyTypeId,Type,Description,Rate,Offer,Tracker,ApplicationUserId")] PolicyType policyType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(policyType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Name", policyType.ApplicationUserId);
            return View(policyType);
        }

        [Authorize(Roles = "Employee")]
        // GET: PolicyTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PolicyType policyType = db.PolicyTypes.Find(id);
            if (policyType == null)
            {
                return HttpNotFound();
            }
            return View(policyType);
        }

        // POST: PolicyTypes/Delete/5
        [Authorize(Roles = "Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PolicyType policyType = db.PolicyTypes.Find(id);
            db.PolicyTypes.Remove(policyType);
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
