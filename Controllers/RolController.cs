using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    [ValidarSesion]
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

        //REGISTRAR ROL

        [HttpGet]
        public ActionResult registrarrol()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registrarrol(Rol orol)
        {
            bool registrado;
            string mensaje;


            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREAR_ROL", oconexion);

                cmd.Parameters.AddWithValue("DESCRIPCION_ROL", orol.DECRIPCION_ROL);
                cmd.Parameters.Add("REGISTRADOROL", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MENSAJEROL", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["REGISTRADOROL"].Value);
                mensaje = cmd.Parameters["MENSAJEROL"].Value.ToString();
            }
            ViewData["MENSAJEROL"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Rol", "Rol");
            }
            else
            {
                return View();
            }

        }

        //ELIMINAR ROL

        [HttpGet]
        public ActionResult Eliminarrol(int? Idrol)
        {
            Rol arol = orol.Where(c => c.ID_ROL == Idrol).FirstOrDefault();
            return View(arol);
        }

        [HttpPost]
        public ActionResult Eliminarrol(string Idrol)
        {
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_ROL", oconexion);
                cmd.Parameters.AddWithValue("ID_ROL", Idrol);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Rol", "Rol");
        }

        //ACTUALIZAR ROL

        [HttpGet]
        public ActionResult Editarrol(int? Idrol)
        {
            Rol arol = orol.Where(c => c.ID_ROL == Idrol).FirstOrDefault();
            return View(arol);
        }

        [HttpPost]
        public ActionResult Editarrol(Rol orol)
        {
            bool registrado;
            string mensaje;

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_ROL", oconexion);

                cmd.Parameters.AddWithValue("ID_ROL", orol.ID_ROL);
                cmd.Parameters.AddWithValue("DESCRIPCION_ROL", orol.DECRIPCION_ROL);
                cmd.Parameters.Add("REGISTRADOROL", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MENSAJEROL", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["REGISTRADOROL"].Value);
                mensaje = cmd.Parameters["MENSAJEROL"].Value.ToString();
            }
            ViewData["MENSAJEROL"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Rol", "Rol");
            }
            else
            {
                return View();
            }

        }

    }
}