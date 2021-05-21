using STI_Online_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STI_Online_Monitoring.Controllers
{
    public class MonitoringController : Controller
    {
        // GET: Monitoring
        public ActionResult RequestMonitoring()
        {
            GuestDBHandle dbhandle = new GuestDBHandle();
            ModelState.Clear();
            return View(dbhandle.GetRequest());
        }

        // GET: Monitoring/Details/5
        public ActionResult Cashier()
        {
            GuestDBHandle dbhandle = new GuestDBHandle();
            ModelState.Clear();
            return View(dbhandle.GetCashier());
        }

        // GET: Monitoring/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Monitoring/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Monitoring/Edit/5
        public JsonResult EditRequest(int ID)
        {
            GuestDBHandle guestDB = new GuestDBHandle();
            var Guest = guestDB.GetRequest().Find(x => x.VisitLogID.Equals(ID));
            return Json(Guest, JsonRequestBehavior.AllowGet);
        }
        // GET: Monitoring/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Monitoring/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
