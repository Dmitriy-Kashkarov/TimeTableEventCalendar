using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeTableEventCalendar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetEvents()
        {
            using (EventsDatabaseEntities db = new EventsDatabaseEntities())
            {
                var events = db.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;

            using (EventsDatabaseEntities db = new EventsDatabaseEntities())
            {
                if (e.EventID > 0)
                {
                    var v = db.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                    db.Events.Add(e);

                db.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }



        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;

            using (EventsDatabaseEntities db = new EventsDatabaseEntities())
            {
                var v = db.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    db.Events.Remove(v);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }




    }
}