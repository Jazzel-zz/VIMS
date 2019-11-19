using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VIMS.Models;
using WebApplication19.Models;

namespace VIMS.Controllers
{
    [Authorize]
    public class VehicleInformationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VehicleInformations
        public ActionResult Index()
        {
            if (User.IsInRole("Employee"))
            {
                var vehicleInformations = db.VehicleInformations.ToList();
                return View(vehicleInformations);
            }
            else if (User.IsInRole("Customer"))
            {
                string userId = User.Identity.GetUserId();
                var vehicleInformations = db.VehicleInformations.Include(v => v.ApplicationUser);
                return View(vehicleInformations.Where(find => find.ApplicationUserId == userId));
            }
            else
            {
                return HttpNotFound();

            }

          
        }

        [HttpGet]
        public ActionResult UserIndex()
        {
            return View(db.VehicleInformations.ToList());
        }
        [HttpPost]
        public ActionResult UserIndex(string search)
        {
            if (search != "")
            {
                ViewBag.Search = search;
                return View(db.VehicleInformations.Where(find => find.VehicleName.Contains(search) || find.VehicleModel == search || find.VehicleColor.Contains(search)));
            }
            else
            {
                return View(db.VehicleInformations.ToList());
            }

        }

        // GET: VehicleInformations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleInformation vehicleInformation = db.VehicleInformations.Find(id);
            ViewBag.Images = vehicleInformation.VehicleImages;
            if (vehicleInformation == null)
            {
                return HttpNotFound();
            }
            return View(vehicleInformation);
        }

        // GET: VehicleInformations/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: VehicleInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VehicleInformationId,VehicleCompany,ApplicationUserId,VehicleName,VehicleColor,VehicleModel,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,VehicleNumber,VehicleWarranty,VehicleDescription,VehicleStatus")] VehicleInformation vehicleInformation)
        {
            if (ModelState.IsValid)
            {
                vehicleInformation.RegistrationDate = DateTime.Now.Date;
                List<VehicleImages> fileDetails = new List<VehicleImages>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        VehicleImages fileDetail = new VehicleImages()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName),
                            Id = Guid.NewGuid()
                        };
                        fileDetails.Add(fileDetail);

                        var path = Path.Combine(Server.MapPath("~/Data/"), fileDetail.Id + fileDetail.Extension);
                        file.SaveAs(path);
                    }
                }

                vehicleInformation.VehicleImages = fileDetails;
                vehicleInformation.ApplicationUserId = User.Identity.GetUserId();
                db.VehicleInformations.Add(vehicleInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", vehicleInformation.ApplicationUserId);
            return View(vehicleInformation);
        }

        // GET: VehicleInformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleInformation vehicleInformation = db.VehicleInformations.Include(v => v.VehicleImages).SingleOrDefault(x => x.VehicleInformationId == id);
            ViewBag.Images = vehicleInformation.VehicleImages;
            if (vehicleInformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", vehicleInformation.ApplicationUserId);
            return View(vehicleInformation);
        }

        // POST: VehicleInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehicleInformationId,VehicleCompany,VehicleName,ApplicationUserId,VehicleColor,VehicleModel,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,VehicleWarranty,VehicleNumber,VehicleDescription,VehicleImages,VehicleStatus")] VehicleInformation vehicleInformation)
        {
            if (ModelState.IsValid)
            {
                vehicleInformation.RegistrationDate = DateTime.Now;
                //New Files
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        VehicleImages fileDetail = new VehicleImages()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName),
                            Id = Guid.NewGuid(),
                            VehicleInformationId = vehicleInformation.VehicleInformationId
                        };
                        var path = Path.Combine(Server.MapPath("~/Data/"), fileDetail.Id + fileDetail.Extension);
                        file.SaveAs(path);

                        db.Entry(fileDetail).State = EntityState.Added;
                    }
                }

                db.Entry(vehicleInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", vehicleInformation.ApplicationUserId);
            return View(vehicleInformation);

        }

        public FileResult Download(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/Data/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }

        [HttpPost]
        public JsonResult DeleteFile(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                Guid guid = new Guid(id);
                VehicleImages fileDetail = db.VehicleImages.Find(guid);
                if (fileDetail == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                //Remove from database
                db.VehicleImages.Remove(fileDetail);
                db.SaveChanges();

                //Delete file from the file system
                var path = Path.Combine(Server.MapPath("~/Data/"), fileDetail.Id + fileDetail.Extension);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // GET: VehicleInformations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleInformation vehicleInformation = db.VehicleInformations.Find(id);
            if (vehicleInformation == null)
            {
                return HttpNotFound();
            }
            return View(vehicleInformation);
        }

        // POST: VehicleInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleInformation vehicleInformation = db.VehicleInformations.Find(id);
            ViewBag.Images = vehicleInformation.VehicleImages;
            db.VehicleInformations.Remove(vehicleInformation);
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
