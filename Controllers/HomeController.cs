using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Mostrar()
        {
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session["USUARIO"] = null;
            return RedirectToAction("login", "Acceso");
        }

    }
}