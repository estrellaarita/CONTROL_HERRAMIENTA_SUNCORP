using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System.Data;
using System.Data.SqlClient;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;


namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    [ValidarSesion]
    public class HerramientaController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Herramienta> oherramienta = new List<Herramienta>();
        // GET: Herramienta
        public ActionResult Herramienta()
        {
            oherramienta = new List<Herramienta>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT h.*, th.*, m.* FROM HERRAMIENTA h INNER JOIN TIPO_HERRAMIENTA th ON h.ID_TIPO_HERRAMIENTA = th.ID_TIPO_HERRAMIENTA INNER JOIN MARCA m ON h.ID_MARCA = m.ID_MARCA;", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Herramienta herramienta = new Herramienta();

                        herramienta.ID_HERRAMIENTA = Convert.ToInt32(dr["ID_HERRAMIENTA"]);
                        herramienta.ID_TIPO_HERRAMIENTA = Convert.ToInt32(dr["ID_TIPO_HERRAMIENTA"]);
                        herramienta.ID_MARCA = Convert.ToInt32(dr["ID_MARCA"]);
                        herramienta.MODELO = dr["MODELO"].ToString();
                        herramienta.COMENTARIO = dr["COMENTARIO"].ToString();
                        herramienta.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //TIPO HERRAMIENTA
                        Tipoherramienta etipoherramienta = new Tipoherramienta();

                        etipoherramienta.ID_TIPO_HERRAMIENTA = Convert.ToInt32(dr["ID_TIPO_HERRAMIENTA"]);
                        etipoherramienta.DECRIPCION_TIPO_HERRAMIENTA = dr["DECRIPCION_TIPO_HERRAMIENTA"].ToString();
                        etipoherramienta.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        herramienta.TIPO = etipoherramienta;

                        //MARCA
                        Marca amarca = new Marca();

                        amarca.ID_MARCA = Convert.ToInt32(dr["ID_MARCA"]);
                        amarca.DECRIPCION_MARCA = dr["DECRIPCION_MARCA"].ToString();
                        amarca.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        herramienta.MARCA = amarca;

                        oherramienta.Add(herramienta);

                    }
                }
            }
            return View(oherramienta);
        }


        [HttpGet]
        public JsonResult ListaHerramienta()
        {

            oherramienta = new List<Herramienta>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT h.*, th.*, m.* FROM HERRAMIENTA h INNER JOIN TIPO_HERRAMIENTA th ON h.ID_TIPO_HERRAMIENTA = th.ID_TIPO_HERRAMIENTA INNER JOIN MARCA m ON h.ID_MARCA = m.ID_MARCA;", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Herramienta herramienta = new Herramienta();

                        herramienta.ID_HERRAMIENTA = Convert.ToInt32(dr["ID_HERRAMIENTA"]);
                        herramienta.ID_TIPO_HERRAMIENTA = Convert.ToInt32(dr["ID_TIPO_HERRAMIENTA"]);
                        herramienta.ID_MARCA = Convert.ToInt32(dr["ID_MARCA"]);
                        herramienta.MODELO = dr["MODELO"].ToString();
                        herramienta.COMENTARIO = dr["COMENTARIO"].ToString();
                        herramienta.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //TIPO HERRAMIENTA
                        Tipoherramienta etipoherramienta = new Tipoherramienta();

                        etipoherramienta.ID_TIPO_HERRAMIENTA = Convert.ToInt32(dr["ID_TIPO_HERRAMIENTA"]);
                        etipoherramienta.DECRIPCION_TIPO_HERRAMIENTA = dr["DECRIPCION_TIPO_HERRAMIENTA"].ToString();
                        etipoherramienta.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        herramienta.TIPO = etipoherramienta;

                        //MARCA
                        Marca amarca = new Marca();

                        amarca.ID_MARCA = Convert.ToInt32(dr["ID_MARCA"]);
                        amarca.DECRIPCION_MARCA = dr["DECRIPCION_MARCA"].ToString();
                        amarca.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        herramienta.MARCA = amarca;

                        oherramienta.Add(herramienta);

                    }
                }
            }
            return Json(new { data = oherramienta }, JsonRequestBehavior.AllowGet);
        }



        //CREATE HERRAMIENTA

        [HttpGet]
        public ActionResult Registrarherramienta()
        {
            //LSTA DE TIPO HERRAMIENTA

            List<Tipoherramienta> otipoherramienta = new List<Tipoherramienta>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TIPO_HERRAMIENTA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Tipoherramienta tipo = new Tipoherramienta();
                    tipo.ID_TIPO_HERRAMIENTA = Convert.ToInt32(reader["ID_TIPO_HERRAMIENTA"]);
                    tipo.DECRIPCION_TIPO_HERRAMIENTA = reader["DECRIPCION_TIPO_HERRAMIENTA"].ToString();
                    otipoherramienta.Add(tipo);
                }
            }
            ViewBag.Tipo = new SelectList(otipoherramienta, "ID_TIPO_HERRAMIENTA", "DECRIPCION_TIPO_HERRAMIENTA");

            //LISTA MARCA

            List<Marca> omarca = new List<Marca>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM MARCA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Marca marca = new Marca();
                    marca.ID_MARCA = Convert.ToInt32(reader["ID_MARCA"]);
                    marca.DECRIPCION_MARCA= reader["DECRIPCION_MARCA"].ToString();
                    omarca.Add(marca);
                }
            }
            ViewBag.Marca = new SelectList(omarca, "ID_MARCA", "DECRIPCION_MARCA");

            return View();
        }

        [HttpPost]
        public ActionResult Registrarherramienta(Herramienta oherramienta)
        {

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_HERAMIENTA", oconexion);
                
                cmd.Parameters.AddWithValue("ID_TIPO_HERRAMIENTA", oherramienta.ID_TIPO_HERRAMIENTA);
                cmd.Parameters.AddWithValue("ID_MARCA", oherramienta.ID_MARCA);
                cmd.Parameters.AddWithValue("MODELO", oherramienta.MODELO);
                cmd.Parameters.AddWithValue("COMENTARIO", oherramienta.COMENTARIO);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Herramienta", "Herramienta");
        }

        // ELIMINAR HERRAMIENTA
        [HttpGet]
        public ActionResult Eliminarherramienta(int? Idherramienta)
        {
            Herramienta aherramienta = oherramienta.Where(c => c.ID_HERRAMIENTA == Idherramienta).FirstOrDefault();
            return View(aherramienta);
        }

        [HttpPost]
        public ActionResult Eliminarherramienta(string Idherramienta)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_HERRAMIENTA", oconexion);
                cmd.Parameters.AddWithValue("ID_HERRAMIENTA", Idherramienta);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
                // Si la eliminación es exitosa, redirigir a la vista deseada
                return RedirectToAction("Herramienta", "Herramienta");
            }
            catch (SqlException ex)
            {
                // En caso de un conflicto, retornar un mensaje de error a la vista
                ViewBag.ErrorMessage = "No puede eliminar la herramienta porque hay registros relacionados";
                // Aquí podrías registrar el error en un log si es necesario

                // Redirigir a la vista actual con el mensaje de error
                return View();  // Asegúrate de que "Marca" sea la vista correcta
            }
        }

        //EDITAR HERRAMIENTA

        [HttpGet]
        public ActionResult Editarherramienta(int? Idherramienta)
        {
            //LISTA DE TIPO HERRAMIENTA

            List<Tipoherramienta> otipoherramienta = new List<Tipoherramienta>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TIPO_HERRAMIENTA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Tipoherramienta tipo = new Tipoherramienta();
                    tipo.ID_TIPO_HERRAMIENTA = Convert.ToInt32(reader["ID_TIPO_HERRAMIENTA"]);
                    tipo.DECRIPCION_TIPO_HERRAMIENTA = reader["DECRIPCION_TIPO_HERRAMIENTA"].ToString();
                    otipoherramienta.Add(tipo);
                }
            }
            ViewBag.Tipo = new SelectList(otipoherramienta, "ID_TIPO_HERRAMIENTA", "DECRIPCION_TIPO_HERRAMIENTA");

            //LISTA MARCA
            List<Marca> omarca = new List<Marca>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM MARCA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Marca marca = new Marca();
                    marca.ID_MARCA = Convert.ToInt32(reader["ID_MARCA"]);
                    marca.DECRIPCION_MARCA = reader["DECRIPCION_MARCA"].ToString();
                    omarca.Add(marca);
                }
            }
            ViewBag.Marca = new SelectList(omarca, "ID_MARCA", "DECRIPCION_MARCA");

            Herramienta aherramienta = oherramienta.Where(c => c.ID_HERRAMIENTA == Idherramienta).FirstOrDefault();
           
            return View(aherramienta);
        }

        [HttpPost]
        public ActionResult Editarherramienta(Herramienta oherramienta)
        {

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_HERRAMIENTA", oconexion);

                cmd.Parameters.AddWithValue("ID_HERRAMIENTA", oherramienta.ID_HERRAMIENTA);
                cmd.Parameters.AddWithValue("ID_TIPO_HERRAMIENTA", oherramienta.ID_TIPO_HERRAMIENTA);
                cmd.Parameters.AddWithValue("ID_MARCA", oherramienta.ID_MARCA);
                cmd.Parameters.AddWithValue("MODELO", oherramienta.MODELO);
                cmd.Parameters.AddWithValue("COMENTARIO", oherramienta.COMENTARIO);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Herramienta", "Herramienta");
        }
    }
}