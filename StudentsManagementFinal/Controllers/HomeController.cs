using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using StudentsManagementFinal.Models;

namespace StudentsManagementFinal.Controllers
{
    public class HomeController : Controller
    {
        StudentsSchoolsEntities1 db = new StudentsSchoolsEntities1();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult InsertSchool()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertSchool(School school)
        {
            if (ModelState.IsValid)
            {
                db.Schools.Add(school);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public PartialViewResult PrintSchool(School school)
        {
            var listClassRoom = db.Schools.ToList();
            return PartialView(listClassRoom);
        }

        public ActionResult DeleteSchool(int id)
        {
            try
            {
                var DeleteTBLClassRoom = db.Schools.Where(x => x.SchoolId == id).FirstOrDefault();
                db.Schools.Remove(DeleteTBLClassRoom);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult AddStudent(int id)
        {
            ViewBag.Test = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddStudent(Student student, int id)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Test = id;
            return View();
        }

        public ActionResult ListStudent(int id)
        {
            ViewBag.Test = id;
            var listStudent = (from cr in db.Schools
                               from st in db.Students
                               where cr.SchoolId == st.SchoolId && id == st.SchoolId
                               select st).ToList();
            return View(listStudent);
        }
        public ActionResult DeleteStudent(int id)
        {
            try
            {
                var DeleteStudent = db.Students.Where(x => x.StudentlId == id).FirstOrDefault();
                db.Students.Remove(DeleteStudent);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult EditStudent(int id)
        {
            var student = db.Students.Where(x => x.StudentlId == id).FirstOrDefault();
            return View(student);
        }
        [HttpPost]
        public ActionResult EditStudent(Student student)
        {
            var std = db.Students.Where(x=>x.StudentlId == student.StudentlId).FirstOrDefault();
            db.Students.Remove(std);
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public ActionResult ShowAllStudent()
        {
            var ShowAllStudent = db.Students.ToList();
            return View(ShowAllStudent);
        }

        public ActionResult OrderByAscId()
        {
            var OrderByAscId = db.Students.OrderBy(x => x.StudentlId).ToList();
            return View(OrderByAscId);
        }

        public ActionResult OrderByDscId()
        {
            var OrderByDscId = db.Students.OrderByDescending(x => x.StudentlId).ToList();
            return View(OrderByDscId);
        }

        public ActionResult IdMax()
        {
            var IdMax = db.Students.OrderByDescending(x => x.StudentlId).Take(1).ToList();
            return View(IdMax);
        }

        public ActionResult IdMin()
        {
            var IdMin = db.Students.OrderBy(x => x.StudentlId).Take(1).ToList();
            return View(IdMin);
        }

        [HttpPost]
        public ActionResult SearchByName(FormCollection f)
        {
            String KeySearchName = f["SearchValue"].ToString();
            List<Student> ListSearch = db.Students.Where(x => x.StudentFirstName.Contains(KeySearchName)).ToList();
            if (ListSearch.Count == 0)
            {
                ViewBag.Notification = " No Result found!";
            }
            else
            {
                ViewBag.Notification = "Found " + ListSearch.Count + " results";
            }
            return View(ListSearch.OrderBy(x => x.StudentFirstName));
        }


    }


    //[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    //public class RateLimitAttribute : ActionFilterAttribute
    //{
    //    public int Seconds { get; set; }

    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        // Using the IP Address here as part of the key but you could modify
    //        // and use the username if you are going to limit only authenticated users
    //        // filterContext.HttpContext.User.Identity.Name
    //        var key = string.Format("{0}-{1}-{2}",
    //            filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
    //            filterContext.ActionDescriptor.ActionName,
    //            filterContext.HttpContext.Request.UserHostAddress
    //        );
    //        var allowExecute = false;

    //        if (HttpRuntime.Cache[key] == null)
    //        {
    //            HttpRuntime.Cache.Add(key,
    //                true,
    //                null,
    //                DateTime.Now.AddSeconds(Seconds),
    //                Cache.NoSlidingExpiration,
    //                CacheItemPriority.Low,
    //                null);
    //            allowExecute = true;
    //        }

    //        if (!allowExecute)
    //        {
    //            filterContext.Result = new ContentResult
    //            {
    //                Content = string.Format("You can call this every {0} seconds", Seconds)
    //            };
    //            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
    //        }
    //    }
    //}
}