using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STI_Online_Monitoring.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Guestform()
        {
            return View();
        }
        public ActionResult Studentform()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Approval()
        {
            return View();
        }
        public ActionResult Nrequest()
        {
            return View();
        }
        public ActionResult Napproved()
        {
            return View();
        }
        public ActionResult Grequest()
        {
            return View();
        }
        public ActionResult Guard()
        {
            return View();
        }
    }
}