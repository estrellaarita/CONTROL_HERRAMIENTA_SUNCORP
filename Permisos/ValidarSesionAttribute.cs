using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CONTROL_HERRAMIENTA_SUNCORP.Permisos
{
    public class ValidarSesionAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (HttpContext.Current.Session["USUARIO"] == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/login");
            }
            base.OnActionExecuted(filterContext);
        }
    }
}