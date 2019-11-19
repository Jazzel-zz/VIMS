using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VIMS.Models;
using WebApplication19.Models;

namespace VIMS.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var data = from item in db.PolicyTypes
                       where
                       item.PolicyTypeId == 5 ||
                       item.PolicyTypeId == 6 ||
                       item.PolicyTypeId == 7
                       select item;
            dynamic HOMEMODEL = new ExpandoObject();
            HOMEMODEL.PolicySetOne = db.PolicyTypes.Take(4);
            HOMEMODEL.PolicySetTwo = data.ToList();
            HOMEMODEL.PolicySetThree = db.PolicyTypes.OrderByDescending(x => x.PolicyTypeId).Take(4);
            return View(HOMEMODEL);
        }
        public ActionResult UserIndex()
        {
            return View();
        }
        [Authorize(Roles = "Employee")]
        public ActionResult Dashboard()
        {
            DateTime today = DateTime.Today;
            int countPolicyApplications = (from item in db.CustomerPolicyRecords
                                           select item).Count();
            ViewBag.CountPolicy = countPolicyApplications;
            var FetchMonthlyPolicies = (from item in db.CustomerPolicyRecords
                                        group item by new { month = item.PolicyDate.Month } into g
                                        select g).ToArray()
                        .Select(p => new
                        {
                            January = p.Where(x => x.PolicyDate.Month == 1).Count(),
                            February = p.Where(x => x.PolicyDate.Month == 2).Count(),
                            March = p.Where(x => x.PolicyDate.Month == 3).Count(),
                            April = p.Where(x => x.PolicyDate.Month == 4).Count(),
                            May = p.Where(x => x.PolicyDate.Month == 5).Count(),
                            June = p.Where(x => x.PolicyDate.Month == 6).Count(),
                            July = p.Where(x => x.PolicyDate.Month == 7).Count(),
                            August = p.Where(x => x.PolicyDate.Month == 8).Count(),
                            September = p.Where(x => x.PolicyDate.Month == 9).Count(),
                            October = p.Where(x => x.PolicyDate.Month == 10).Count(),
                            November = p.Where(x => x.PolicyDate.Month == 11).Count(),
                            December = p.Where(x => x.PolicyDate.Month == 12).Count(),
                        });
            var FetchMonthlyVehicles = (from item in db.VehicleInformations
                                        group item by new { month = item.RegistrationDate.Month } into g
                                        select g).ToArray()
                        .Select(p => new
                        {
                            January = p.Where(x => x.RegistrationDate.Month == 1).Count(),
                            February = p.Where(x => x.RegistrationDate.Month == 2).Count(),
                            March = p.Where(x => x.RegistrationDate.Month == 3).Count(),
                            April = p.Where(x => x.RegistrationDate.Month == 4).Count(),
                            May = p.Where(x => x.RegistrationDate.Month == 5).Count(),
                            June = p.Where(x => x.RegistrationDate.Month == 6).Count(),
                            July = p.Where(x => x.RegistrationDate.Month == 7).Count(),
                            August = p.Where(x => x.RegistrationDate.Month == 8).Count(),
                            September = p.Where(x => x.RegistrationDate.Month == 9).Count(),
                            October = p.Where(x => x.RegistrationDate.Month == 10).Count(),
                            November = p.Where(x => x.RegistrationDate.Month == 11).Count(),
                            December = p.Where(x => x.RegistrationDate.Month == 12).Count(),
                        });
            string data = "";
            string VehicleData = "";
            foreach (var item in FetchMonthlyPolicies)
            {
                data += item;
            }
            foreach (var item in FetchMonthlyVehicles)
            {
                VehicleData += item;
            }
            data = data.Replace("{", "");
            data = data.Replace("}", "");
            data = data.Replace(" ", "");
            data = data.Replace(",", ":");
            data = data.Replace("=", ":");
            VehicleData = VehicleData.Replace("{", "");
            VehicleData = VehicleData.Replace("}", "");
            VehicleData = VehicleData.Replace(" ", "");
            VehicleData = VehicleData.Replace(",", ":");
            VehicleData = VehicleData.Replace("=", ":");
            string[] valuesArray = data.Split(':');
            string[] valuesVehicleArray = VehicleData.Split(':');
            string values = "";
            string VehicleValues = "";
            foreach (var item in valuesArray)
            {
                if (IsNumeric(item))
                {
                    if (values == "")
                    {
                        values = item;
                    }
                    else
                    {
                        values += ":" + item;
                    }
                }
            }
            foreach (var item in valuesVehicleArray)
            {
                if (IsNumeric(item))
                {
                    if (VehicleValues == "")
                    {
                        VehicleValues = item;
                    }
                    else
                    {
                        VehicleValues += ":" + item;
                    }
                }
            }
            ViewBag.MonthlyData = values;
            ViewBag.MonthlyVehicleData = VehicleValues;

            ViewBag.CustomersCount = db.Users.Count();
            ViewBag.CustomerPoliciesCount = db.CustomerPolicyRecords.Count();
            ViewBag.ExpensesCount = (from item in db.Expenses select item.AmountOfExpense).Sum();
            ViewBag.VehiclesCount = db.VehicleInformations.Count();

            dynamic VIMSINFOMODEL = new ExpandoObject();
            VIMSINFOMODEL.Vehicles = db.VehicleInformations.OrderByDescending(find => find.VehicleInformationId).Take(5);
            VIMSINFOMODEL.Policies = db.PolicyTypes.OrderByDescending(find => find.PolicyTypeId).Take(3);
            VIMSINFOMODEL.Applications = db.CustomerPolicyRecords.OrderByDescending(find => find.Id).Take(5);
            VIMSINFOMODEL.Expenses = db.Expenses.OrderByDescending(find => find.ExpenseId).Take(5);
            VIMSINFOMODEL.Claims = db.ClaimDetails.OrderByDescending(find => find.ClaimDetailId).Take(5);
            return View(VIMSINFOMODEL);

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.CustomersCount = db.Users.Count();
            ViewBag.CustomerPoliciesCount = db.CustomerPolicyRecords.Count();
            if(db.Expenses.Count() != 0)
            {
                ViewBag.ExpensesCount = (from item in db.Expenses select item.AmountOfExpense).Sum();

            }
            else
            {
                ViewBag.ExpensesCount = 0;

            }
            ViewBag.VehiclesCount = db.VehicleInformations.Count();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Success = "";
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                ViewBag.Success = "Done";
            }
            return View(contact);
        }
        public ActionResult Testimonial()
        {

            return View();
        }
        public ActionResult SiteMap()
        {

            return View();
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}