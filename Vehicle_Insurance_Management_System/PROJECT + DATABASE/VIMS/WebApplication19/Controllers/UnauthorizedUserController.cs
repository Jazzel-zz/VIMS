using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication19.Controllers
{
    public class UnauthorizedUserController : Controller
    {
        // GET: UnauthorizedUser
        public ActionResult Index()
        {
            return View();
        }
    }
}