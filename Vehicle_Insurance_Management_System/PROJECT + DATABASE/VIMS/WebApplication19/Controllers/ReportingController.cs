using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIMS.Models;

namespace WebApplication19.Controllers
{
    [Authorize(Roles = "Employee")]
    public class ReportingController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Reporting
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Vehicle()
        {
            return View();
        }
        public ActionResult Claim()
        {
            return View();
        }
        public ActionResult Policy()
        {
            return View();
        }

        public JsonResult PolicyResults(string type, int month)
        {
            var policyRecord = (dynamic)null;
            DateTime dateTime = DateTime.Now.Date;
            var date = dateTime.Date;
            // Show All
            if (type == "All" && month == 0)
            {
                policyRecord = (from item in db.CustomerPolicyRecords
                                orderby item.Id descending
                                select new
                                {
                                    Id = item.Id,
                                    Policy = item.PolicyNumber,
                                    Date = item.PolicyDate,
                                    VehicleName = item.VehicleInformation.VehicleName,
                                    VehicleRate = item.VehicleInformation.VehicleRate,
                                    VehicleOwner = item.VehicleInformation.ApplicationUser.Name,
                                    Tracker = item.Tracker,
                                }).ToList();

            }
            // Show Today's Record
            else if (type == "Today" && month == 0)
            {
                policyRecord = (from item in db.CustomerPolicyRecords
                               where EntityFunctions.TruncateTime(item.PolicyDate) == date
                               select new
                               {
                                   Id =item.Id,
                                   Policy = item.PolicyNumber,
                                   Date = item.PolicyDate,
                                   VehicleName = item.VehicleInformation.VehicleName,
                                   VehicleRate = item.VehicleInformation.VehicleRate,
                                   VehicleOwner = item.VehicleInformation.ApplicationUser.Name,
                                   Tracker = item.Tracker,
                               }).ToList();
            }
            // Fixing clash between month and today
            else if (type == "t" && month != 0)
            {
                return Json(new { Result = "ERROR" });
            }
            // Show Monthly Record
            else if (type == "Monthly" && month != 0)
            {
                try
                {
                    if (month != 0)
                    {
                        policyRecord = (from item in db.CustomerPolicyRecords
                                        where item.PolicyDate.Month == month
                                        select new
                                        {
                                            Id = item.Id,
                                            Policy = item.PolicyNumber,
                                            Date = item.PolicyDate,
                                            VehicleName = item.VehicleInformation.VehicleName,
                                            VehicleRate = item.VehicleInformation.VehicleRate,
                                            VehicleOwner = item.VehicleInformation.ApplicationUser.Name,
                                            Tracker = item.Tracker,
                                        }).ToList();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR" });
                    }
                }
                catch (Exception error)
                {
                    policyRecord = db.CustomerPolicyRecords.OrderByDescending(x => x.PolicyDate).ToList();
                }


            }
            // Fixing clash between month(0) and type
            else if (type == "m" && month == 0)
            {
                return Json(new { Result = "ERROR" });
            }
            // Just to Complete the Rule
            else
            {
                policyRecord = db.CustomerPolicyRecords.OrderByDescending(x => x.Id).ToList();

            }
            return Json(policyRecord, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VehicleResults(string type, int month)
        {
            var vehicleRecord = (dynamic)null;
            DateTime dateTime = DateTime.Now.Date;
            var date = dateTime.Date;
            // Show All
            if (type == "All" && month == 0)
            {
                vehicleRecord = (from item in db.VehicleInformations
                                orderby item.VehicleInformationId descending
                                select new
                                {
                                    Id = item.VehicleInformationId,
                                    Company = item.VehicleCompany,
                                    VehicleName = item.VehicleName,
                                    VehicleRate = item.VehicleRate,
                                    VehicleOwner = item.ApplicationUser.Name,
                                    Date = item.RegistrationDate,
                                    VehicleWarranty = item.VehicleWarranty,
                                }).ToList();

            }
            // Show Today's Record
            else if (type == "Today" && month == 0)
            {
                vehicleRecord = (from item in db.VehicleInformations
                                where EntityFunctions.TruncateTime(item.RegistrationDate) == date
                                select new
                                {
                                    Id = item.VehicleInformationId,
                                    Company = item.VehicleCompany,
                                    VehicleName = item.VehicleName,
                                    VehicleRate = item.VehicleRate,
                                    VehicleOwner = item.ApplicationUser.Name,
                                    Date = item.RegistrationDate,
                                    VehicleWarranty = item.VehicleWarranty,
                                }).ToList();
            }
            // Fixing clash between month and today
            else if (type == "t" && month != 0)
            {
                return Json(new { Result = "ERROR" });
            }
            // Show Monthly Record
            else if (type == "Monthly" && month != 0)
            {
                try
                {
                    if (month != 0)
                    {
                        vehicleRecord = (from item in db.VehicleInformations
                                        where item.RegistrationDate.Month == month
                                        select new
                                        {
                                            Id = item.VehicleInformationId,
                                            Company = item.VehicleCompany,
                                            VehicleName = item.VehicleName,
                                            VehicleRate = item.VehicleRate,
                                            VehicleOwner = item.ApplicationUser.Name,
                                            Date = item.RegistrationDate,
                                            VehicleWarranty = item.VehicleWarranty,
                                        }).ToList();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR" });
                    }
                }
                catch (Exception error)
                {
                    vehicleRecord = db.CustomerPolicyRecords.OrderByDescending(x => x.PolicyDate).ToList();
                }


            }
            // Fixing clash between month(0) and type
            else if (type == "m" && month == 0)
            {
                return Json(new { Result = "ERROR" });
            }
            // Just to Complete the Rule
            else
            {
                vehicleRecord = db.CustomerPolicyRecords.OrderByDescending(x => x.Id).ToList();

            }
            return Json(vehicleRecord, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClaimResults(string type, int month)
        {
            var claimRecord = (dynamic)null;
            DateTime dateTime = DateTime.Now.Date;
            var date = dateTime.Date;
            // Show All
            if (type == "All" && month == 0)
            {
                claimRecord = (from item in db.ClaimDetails
                                orderby item.ClaimDetailId descending
                                select new
                                {
                                    Id = item.ClaimDetailId,
                                    Claim = item.ClaimNumber,
                                    Date = item.DateOfAccident,
                                    Policy = item.CustomerPolicyRecord.PolicyNumber,
                                    VehicleName = item.CustomerPolicyRecord.VehicleInformation.VehicleRate,
                                    VehicleOwner = item.ApplicationUser.Name,
                                }).ToList();

            }
            // Show Today's Record
            else if (type == "Today" && month == 0)
            {
                claimRecord = (from item in db.ClaimDetails
                                where EntityFunctions.TruncateTime(item.DateOfAccident) == date
                                select new
                                {
                                    Id = item.ClaimDetailId,
                                    Claim = item.ClaimNumber,
                                    Date = item.DateOfAccident,
                                    Policy = item.CustomerPolicyRecord.PolicyNumber,
                                    VehicleName = item.CustomerPolicyRecord.VehicleInformation.VehicleRate,
                                    VehicleOwner = item.ApplicationUser.Name,
                                }).ToList();
            }
            // Fixing clash between month and today
            else if (type == "t" && month != 0)
            {
                return Json(new { Result = "ERROR" });
            }
            // Show Monthly Record
            else if (type == "Monthly" && month != 0)
            {
                try
                {
                    if (month != 0)
                    {
                        claimRecord = (from item in db.ClaimDetails
                                        where item.DateOfAccident.Month == month
                                        select new
                                        {
                                            Id = item.ClaimDetailId,
                                            Claim = item.ClaimNumber,
                                            Date = item.DateOfAccident,
                                            Policy = item.CustomerPolicyRecord.PolicyNumber,
                                            VehicleName = item.CustomerPolicyRecord.VehicleInformation.VehicleRate,
                                            VehicleOwner = item.ApplicationUser.Name,
                                        }).ToList();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR" });
                    }
                }
                catch (Exception error)
                {
                    claimRecord = db.CustomerPolicyRecords.OrderByDescending(x => x.PolicyDate).ToList();
                }


            }
            // Fixing clash between month(0) and type
            else if (type == "m" && month == 0)
            {
                return Json(new { Result = "ERROR" });
            }
            // Just to Complete the Rule
            else
            {
                claimRecord = db.CustomerPolicyRecords.OrderByDescending(x => x.Id).ToList();

            }
            return Json(claimRecord, JsonRequestBehavior.AllowGet);
        }

    }

}