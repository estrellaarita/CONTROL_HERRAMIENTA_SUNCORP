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
       
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_MARCA", oconexion);

                cmd.Parameters.AddWithValue("DECRIPCION_MARCA", omarca.DECRIPCION_MARCA);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction("Marca", "Marca");

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
           

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_MARCA", oconexion);

                cmd.Parameters.AddWithValue("ID_MARCA", omarca.ID_MARCA);
                cmd.Parameters.AddWithValue("DECRIPCION_MARCA", omarca.DECRIPCION_MARCA);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

            
            }
            return RedirectToAction("Marca", "Marca");
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
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_DELETE_MARCA", oconexion);
                    cmd.Parameters.AddWithValue("ID_MARCA", Idmarca);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
                // Si la eliminación es exitosa, redirigir a la vista deseada
                return RedirectToAction("Marca", "Marca");
            }
            catch (SqlException ex)
            {
                // En caso de un conflicto, retornar un mensaje de error a la vista
                ViewBag.ErrorMessage = "No puede eliminar la marca porque hay registros relacionados";
                // Aquí podrías registrar el error en un log si es necesario

                // Redirigir a la vista actual con el mensaje de error
                return View();  // Asegúrate de que "Marca" sea la vista correcta
            }
        }


    }
}