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
    public class TipoController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Tipoherramienta> otipo = new List<Tipoherramienta>();
        // GET: Tipo
        public ActionResult Tipo()
        {
            otipo = new List<Tipoherramienta>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TIPO_HERRAMIENTA", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Tipoherramienta tipo = new Tipoherramienta();

                        tipo.ID_TIPO_HERRAMIENTA= Convert.ToInt32(dr["ID_TIPO_HERRAMIENTA"]);
                        tipo.DECRIPCION_TIPO_HERRAMIENTA = dr["DECRIPCION_TIPO_HERRAMIENTA"].ToString();
                        tipo.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        otipo.Add(tipo);

                    }
                }
            }
            return View(otipo);
        }

        //REGISTRAR TIPO HERRAMIENTA

        [HttpGet]
        public ActionResult registrartipo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registrartipo(Tipoherramienta etipoherramienta)
        {
            bool registrado;
            string mensaje;

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_TIPO_HERRAMIENTA", oconexion);

                cmd.Parameters.AddWithValue("DESCRIPCION_TIPO_HERRAMIENTA", etipoherramienta.DECRIPCION_TIPO_HERRAMIENTA);
                cmd.Parameters.Add("REGISTRADOTIPO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MENSAJETIPO", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["REGISTRADOTIPO"].Value);
                mensaje = cmd.Parameters["MENSAJETIPO"].Value.ToString();
            }
            ViewData["MENSAJETIPO"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Tipo", "Tipo");
            }
            else
            {
                return View();
            }

        }

        //ACTUALIZAR TIPO HERRAMIENTA

        [HttpGet]
        public ActionResult Editartipo(int? Idtipo)
        {
            Tipoherramienta atipo = otipo.Where(c => c.ID_TIPO_HERRAMIENTA == Idtipo).FirstOrDefault();
            return View(atipo);
        }

        [HttpPost]
        public ActionResult Editartipo(Tipoherramienta etipoherramienta)
        {
            bool registrado;
            string mensaje;

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_TIPO_HERRAMIENTA", oconexion);

                cmd.Parameters.AddWithValue("ID_TIPO_HERRAMIENTA", etipoherramienta.ID_TIPO_HERRAMIENTA);
                cmd.Parameters.AddWithValue("DESCRIPCION_TIPO_HERRAMIENTA", etipoherramienta.DECRIPCION_TIPO_HERRAMIENTA);
                cmd.Parameters.Add("REGISTRADOTIPO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MENSAJETIPO", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["REGISTRADOTIPO"].Value);
                mensaje = cmd.Parameters["MENSAJETIPO"].Value.ToString();
            }
            ViewData["MENSAJETIPO"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Tipo", "Tipo");
            }
            else
            {
                return View();
            }

        }

        //ELIMINAR TIPO HERRAMIENTA

        [HttpGet]
            public ActionResult Eliminartipo(int? Idtipo)
            {
                Tipoherramienta tipoherramienta = otipo.Where(c => c.ID_TIPO_HERRAMIENTA == Idtipo).FirstOrDefault();
            return View(tipoherramienta);
        }

        [HttpPost]
        public ActionResult Eliminartipo(string Idtipo)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_TIPO_HERRAMIENTA", oconexion);
                cmd.Parameters.AddWithValue("ID_TIPO_HERRAMIENTA", Idtipo);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
                }
                // Si la eliminación es exitosa, redirigir a la vista deseada
                return RedirectToAction("Tipo", "Tipo");
            }
            catch (SqlException ex)
            {
                // En caso de un conflicto, retornar un mensaje de error a la vista
                ViewBag.ErrorMessage = "No puede eliminar el tipo de herramienta porque hay registros relacionados";
                // Aquí podrías registrar el error en un log si es necesario

                // Redirigir a la vista actual con el mensaje de error
                return View();  // Asegúrate de que "Marca" sea la vista correcta
            }
        }


    }
}