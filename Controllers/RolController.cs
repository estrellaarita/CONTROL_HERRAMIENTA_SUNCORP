using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    public class RolController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Rol> orol = new List<Rol>();
        // GET: Rol
        public ActionResult Rol()
        {
            orol = new List<Rol>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ROL", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Rol rol = new Rol();

                        rol.ID_ROL= Convert.ToInt32(dr["ID_ROL"]);
                        rol.DECRIPCION_ROL = dr["DECRIPCION_ROL"].ToString();
                        rol.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        orol.Add(rol);

                    }
                }
            }
            return View(orol);
        }

        //REGISTRAR
        [HttpGet]
        public ActionResult registrarrol()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registrarrol(Rol orol)
        {

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREAR_ROL", oconexion);

                cmd.Parameters.AddWithValue("DESCRIPCION_ROL", orol.DECRIPCION_ROL);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Rol", "Rol");
        }

    }
}