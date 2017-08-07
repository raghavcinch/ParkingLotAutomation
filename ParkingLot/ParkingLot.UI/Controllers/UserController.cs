using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkingLot.UI.Controllers
{
    public class UserController : Controller
    {
        // GET: Use
        public ActionResult Index()
        {
            return View("User");
        }
    }
}