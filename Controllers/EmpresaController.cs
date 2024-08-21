using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Services.Description;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    [ValidarSesion]
    public class EmpresaController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Empresa> oempresa = new List<Empresa>();
        public ActionResult empresa()
        {
            oempresa = new List<Empresa>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT " +
                                   "epr.ID_EMPRESA, " +
                                   "epr.RTN, " +
                                   "epr.NOMBRE_EMPRESA, " +
                                   "epr.CORREO_ELECTRONICO, " +
                                   "epr.ID_CORPORACION, " +
                                   "c.NOMBRE_CORPORACION, " +
                                   "epr.FECHA_REGISTRO, " +
                                   "COUNT(DISTINCT s.ID_SUCURSAL) AS CANTIDAD_DE_SUCURSALES " +
                                   
                                   "FROM EMPRESA epr " +
                                   "INNER JOIN CORPORACION c ON epr.ID_CORPORACION = c.ID_CORPORACION " +
                                   "LEFT JOIN SUCURSAL s ON epr.ID_EMPRESA = s.ID_EMPRESA " +
                                   "GROUP BY epr.ID_EMPRESA, epr.RTN, epr.NOMBRE_EMPRESA, epr.CORREO_ELECTRONICO, epr.ID_CORPORACION, c.NOMBRE_CORPORACION, epr.FECHA_REGISTRO", oconexion);

                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Empresa empresa = new Empresa();

                        empresa.ID_EMPRESA = Convert.ToInt32(dr["ID_EMPRESA"]);
                        empresa.RTN = dr["RTN"].ToString();
                        empresa.NOMBRE_EMPRESA = dr["NOMBRE_EMPRESA"].ToString();
                        empresa.CORREO_ELECTRONICO = dr["CORREO_ELECTRONICO"].ToString();
                        empresa.ID_CORPORACION = Convert.ToInt32(dr["ID_CORPORACION"]);
                        empresa.CANTIDAD_DE_SUCURSALES = Convert.ToInt32(dr["CANTIDAD_DE_SUCURSALES"]);
                        empresa.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        Corporacion corporacion = new Corporacion();

                      corporacion.ID_CORPORACION = Convert.ToInt32(dr["ID_CORPORACION"]);
                      corporacion.NOMBRE_CORPORACION = dr["NOMBRE_CORPORACION"].ToString();
                      corporacion.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empresa.CORPORACION = corporacion;

                   
                        oempresa.Add(empresa);

                    }
                }
            }
            return View(oempresa);
        }

        //CREATE EMPRESA




        [HttpGet]
        public ActionResult Registrar()
        {

          List<Corporacion> ocorporacion = new List<Corporacion>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CORPORACION", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Corporacion corporacion = new Corporacion();
                    corporacion.ID_CORPORACION = Convert.ToInt32(reader["ID_CORPORACION"]);
                    corporacion.NOMBRE_CORPORACION = reader["NOMBRE_CORPORACION"].ToString();
                    ocorporacion.Add(corporacion);
                }
            }
            ViewBag.Corporaciones = new SelectList(ocorporacion, "ID_CORPORACION", "NOMBRE_CORPORACION");
          
             
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Empresa oempresa)
        {

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_EMPRESA", oconexion);
                
                cmd.Parameters.AddWithValue("RTN", oempresa.RTN);
                cmd.Parameters.AddWithValue("NOMBRE_EMPRESA", oempresa.NOMBRE_EMPRESA);
                cmd.Parameters.AddWithValue("CORREO_ELECTRONICO", oempresa.CORREO_ELECTRONICO);
                cmd.Parameters.AddWithValue("ID_CORPORACION", oempresa.ID_CORPORACION);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("empresa", "Empresa");
        }


        // ELIMINAR EMPRESA

        [HttpGet]
        public ActionResult Eliminar(int? Idempresa)
        {
            Empresa aempresa = oempresa.Where(c => c.ID_EMPRESA == Idempresa).FirstOrDefault();
            return View(aempresa);
        }

        [HttpPost]
        public ActionResult Eliminar(string Idempresa)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_EMPRESA", oconexion);
                cmd.Parameters.AddWithValue("ID_EMPRESA", Idempresa);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
                }
                // Si la eliminación es exitosa, redirigir a la vista deseada
                return RedirectToAction("empresa", "Empresa");
            }
            catch (SqlException ex)
            {
                // En caso de un conflicto, retornar un mensaje de error a la vista
                ViewBag.ErrorMessage = "No puede eliminar la empresa porque hay registros relacionados";
                // Aquí podrías registrar el error en un log si es necesario

                // Redirigir a la vista actual con el mensaje de error
                return View();  // Asegúrate de que "Marca" sea la vista correcta
            }
        }

        //EDITAR EMPRESA

        [HttpGet]
        public ActionResult Editar(int? Idempresa)
        {
            List<Corporacion> ocorporacion = new List<Corporacion>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CORPORACION", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Corporacion corporacion = new Corporacion();
                    corporacion.ID_CORPORACION = Convert.ToInt32(reader["ID_CORPORACION"]);
                    corporacion.NOMBRE_CORPORACION = reader["NOMBRE_CORPORACION"].ToString();
                    ocorporacion.Add(corporacion);
                }
            }
            ViewBag.Corporacione = new SelectList(ocorporacion, "ID_CORPORACION", "NOMBRE_CORPORACION");

            Empresa aempresa = oempresa.Where(c => c.ID_EMPRESA == Idempresa).FirstOrDefault();
            
            return View(aempresa);
        }

        [HttpPost]
        public ActionResult Editar(Empresa oempresa)
        {

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_EMPRESA", oconexion);

                cmd.Parameters.AddWithValue("ID_EMPRESA", oempresa.ID_EMPRESA);
                cmd.Parameters.AddWithValue("RTN", oempresa.RTN);
                cmd.Parameters.AddWithValue("NOMBRE_EMPRESA", oempresa.NOMBRE_EMPRESA);
                cmd.Parameters.AddWithValue("CORREO_ELECTRONICO", oempresa.CORREO_ELECTRONICO);
                cmd.Parameters.AddWithValue("ID_CORPORACION", oempresa.ID_CORPORACION);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("empresa", "Empresa");
        }

    }
}
