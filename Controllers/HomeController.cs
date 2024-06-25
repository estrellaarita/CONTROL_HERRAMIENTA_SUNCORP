using CONTROL_HERRAMIENTA_SUNCORP.Models;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    [ValidarSesion]
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

        public ActionResult CerrarSesion()
        {
            Session["USUARIO"] = null;
            return RedirectToAction("login", "Acceso");
        }


        public ActionResult crear()
        {

            return View();
        }
    }
}