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
    public class ProveedorController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Proveedor> oproveedor = new List<Proveedor>();
        // GET: Departamento
        public ActionResult Proveedor()
        {

            oproveedor = new List<Proveedor>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM PROVEEDOR", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Proveedor proveedor = new Proveedor();

                        proveedor.ID_PROVEEDOR = Convert.ToInt32(dr["ID_PROVEEDOR"]);
                        proveedor.RTN = dr["RTN"].ToString();
                        proveedor.NOMBRE_PROVEEDOR = dr["NOMBRE_PROVEEDOR"].ToString();
                        proveedor.CORREO_ELECTRONICO = dr["CORREO_ELECTRONICO"].ToString();
                        proveedor.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        oproveedor.Add(proveedor);

                    }
                }
            }
            return View(oproveedor);
        }

        //REGISTRAR PROVEEDOR

        [HttpGet]
        public ActionResult registrarproveedor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registrarproveedor(Proveedor oproveedor)
        {
            bool registrado;
            string mensaje;

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_PROVEEDOR", oconexion);

                cmd.Parameters.AddWithValue("RTN", oproveedor.RTN);
                cmd.Parameters.AddWithValue("NOMBRE_PROVEEDOR", oproveedor.NOMBRE_PROVEEDOR);
                cmd.Parameters.AddWithValue("CORREO_ELECTRONICO", oproveedor.CORREO_ELECTRONICO);
                cmd.Parameters.Add("REGISTRADOPROVEEDOR", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MENSAJEPROVEEDOR", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["REGISTRADOPROVEEDOR"].Value);
                mensaje = cmd.Parameters["MENSAJEPROVEEDOR"].Value.ToString();
            }
            ViewData["MENSAJEPROVEEDOR"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Proveedor", "Proveedor");
            }
            else
            {
                return View();
            }
        }

        //ACTUALIZAR PROVEEDOR

        [HttpGet]
        public ActionResult Editarproveedor(int? Idproveedor)
        {
            Proveedor iproveedor = oproveedor.Where(c => c.ID_PROVEEDOR == Idproveedor).FirstOrDefault();
            return View(iproveedor);
        }

        [HttpPost]
        public ActionResult Editarproveedor(Proveedor oproveedor)
        {
            bool registrado;
            string mensaje;

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_PROVEEDOR", oconexion);

                cmd.Parameters.AddWithValue("ID_PROVEEDOR", oproveedor.ID_PROVEEDOR);
                cmd.Parameters.AddWithValue("RTN", oproveedor.RTN);
                cmd.Parameters.AddWithValue("NOMBRE_PROVEEDOR", oproveedor.NOMBRE_PROVEEDOR);
                cmd.Parameters.AddWithValue("CORREO_ELECTRONICO", oproveedor.CORREO_ELECTRONICO);
                cmd.Parameters.Add("REGISTRADOPROVEEDOR", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MENSAJEPROVEEDOR", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["REGISTRADOPROVEEDOR"].Value);
                mensaje = cmd.Parameters["MENSAJEPROVEEDOR"].Value.ToString();
            }
            ViewData["MENSAJEPROVEEDOR"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Proveedor", "Proveedor");
            }
            else
            {
                return View();
            }

        }

        //ELIMINAR PROVEEDOR

        [HttpGet]
        public ActionResult Eliminarproveedor(int? Idproveedor)
        {
            Proveedor iproveedor = oproveedor.Where(c => c.ID_PROVEEDOR == Idproveedor).FirstOrDefault();
            return View(iproveedor);
        }

        [HttpPost]
        public ActionResult Eliminarproveedor(string Idproveedor)
        {
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_PROVEEDOR", oconexion);
                cmd.Parameters.AddWithValue("ID_PROVEEDOR", Idproveedor);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Proveedor", "Proveedor");
        }
    }

}