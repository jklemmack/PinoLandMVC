using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PinoLandMVC4.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/

        public ActionResult Index()
        {
            //Lookup & return 
            return RedirectToAction("Details", "Student", new { id = 5 });

            // or show game list (if user in more than one)
            // or show "no available games"
        }

        public ActionResult Details(int id)
        {
            return View("Index");
        }

    }
}
