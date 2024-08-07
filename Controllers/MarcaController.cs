using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    [ValidarSesion]
    public class MarcaController : Controller
    {

        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Marca> omarca = new List<Marca>();
        // GET: Marca
        public ActionResult Marca()
        {
            omarca = new List<Marca>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM MARCA", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Marca marca = new Marca();

                        marca.ID_MARCA = Convert.ToInt32(dr["ID_MARCA"]);
                        marca.DECRIPCION_MARCA = dr["DECRIPCION_MARCA"].ToString();
                        marca.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        omarca.Add(marca);

                    }
                }
            }
            return View(omarca);
        }

        //REGISTRAR MARCA
        [HttpGet]
        public ActionResult registrarmarca()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registrarmarca(Marca omarca)
        {
            bool registrado;
            string mensaje;

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_MARCA", oconexion);

                cmd.Parameters.AddWithValue("DESCRIPCION_MARCA", omarca.DECRIPCION_MARCA);
                cmd.Parameters.Add("REGISTRADOMARCA", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MENSAJEMARCA", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["REGISTRADOMARCA"].Value);
                mensaje = cmd.Parameters["MENSAJEMARCA"].Value.ToString();
            }
            ViewData["MENSAJEMARCA"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Marca", "Marca");
            }
            else
            {
                return View();
            }

        }

        //ACTUALIZAR MARCA

        [HttpGet]
        public ActionResult Editarmarca(int? Idmarca)
        {
            Marca imarca = omarca.Where(c => c.ID_MARCA == Idmarca).FirstOrDefault();
            return View(imarca);
        }

        [HttpPost]
        public ActionResult Editarmarca(Marca omarca)
        {
            bool registrado;
            string mensaje;

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_MARCA", oconexion);

                cmd.Parameters.AddWithValue("ID_MARCA", omarca.ID_MARCA);
                cmd.Parameters.AddWithValue("DESCRIPCION_MARCA", omarca.DECRIPCION_MARCA);
                cmd.Parameters.Add("REGISTRADOMARCA", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MENSAJEMARCA", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["REGISTRADOMARCA"].Value);
                mensaje = cmd.Parameters["MENSAJEMARCA"].Value.ToString();
            }
            ViewData["MENSAJEMARCA"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Marca", "Marca");
            }
            else
            {
                return View();
            }

        }

        //ELIMINAR MARCA

        [HttpGet]
        public ActionResult Eliminarmarca(int? Idmarca)
        {
            Marca amarca = omarca.Where(c => c.ID_MARCA == Idmarca).FirstOrDefault();
            return View(amarca);
        }

        [HttpPost]
        public ActionResult Eliminarmarca(string Idmarca)
        {
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_MARCA", oconexion);
                cmd.Parameters.AddWithValue("ID_MARCA", Idmarca);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Marca", "Marca");
        }


    }
}