using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    public class FacturaController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Factura> ofactura = new List<Factura>();
  
        // GET: Factura
        public ActionResult Factura()
        {

            ofactura = new List<Factura>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT f.*, u.*, p.* FROM FACTURA f INNER JOIN USUARIO_BD u ON f.ID_USUARIO_BD = u.ID_USUARIO_BD INNER JOIN PROVEEDOR p ON f.ID_PROVEEDOR = p.ID_PROVEEDOR", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Factura factura = new Factura();

                        factura.ID_FACTURA = Convert.ToInt32(dr["ID_FACTURA"]);
                        factura.NUMERO_FACTURA = dr["NUMERO_FACTURA"].ToString();
                        factura.ID_USUARIO_BD = Convert.ToInt32(dr["ID_USUARIO_BD"]);
                        factura.FECHA_COMPRA = Convert.ToDateTime(dr["FECHA_COMPRA"]);
                        factura.ID_PROVEEDOR = Convert.ToInt32(dr["ID_PROVEEDOR"]);
                        factura.COMENTARIO = dr["COMENTARIO"].ToString();


                        if (dr["FOTO"] != DBNull.Value)
                        {
                            factura.FOTO = (byte[])dr["FOTO"];
                        }
                        else
                        {
                            // Aquí decides qué hacer si el valor es DBNull.Value (es decir, NULL en la base de datos)
                            // Por ejemplo, podrías asignar un valor predeterminado o dejar serie.ID_FACTURA como 0 (dependiendo de tu lógica)
                            factura.FOTO = null; // Asignando un valor predeterminado, en este caso 0
                        }

                        
                        factura.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //USUARIO BD
                        usuario eusuario = new usuario();

                        eusuario.ID_USUARIO_BD = Convert.ToInt32(dr["ID_USUARIO_BD"]);
                        eusuario.USUARIO = dr["USUARIO"].ToString();
                        eusuario.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        factura.USUARIO_BD = eusuario;

                        //PROVEEDOR
                       Proveedor aproveedor = new Proveedor();

                        aproveedor.ID_PROVEEDOR = Convert.ToInt32(dr["ID_PROVEEDOR"]);
                        aproveedor.NOMBRE_PROVEEDOR = dr["NOMBRE_PROVEEDOR"].ToString();
                        aproveedor.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        factura.PROVEEDOR = aproveedor;

                        ofactura.Add(factura);

                    }
                }
            }
            return View();
        }

        //CREATE FACTURA

        [HttpGet]
        public ActionResult Registrarfactura()
        {

            List<usuario> ousuario = new List<usuario>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM USUARIO_BD", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuario eusuario = new usuario();
                    eusuario.ID_USUARIO_BD = Convert.ToInt32(reader["ID_USUARIO_BD"]);
                    eusuario.USUARIO = reader["USUARIO"].ToString();
                    ousuario.Add(eusuario);
                }
            }
            ViewBag.Usuario = new SelectList(ousuario, "ID_USUARIO_BD", "USUARIO");


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

    }
}