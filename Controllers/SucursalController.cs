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

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    public class SucursalController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Sucursal> osucursal = new List<Sucursal>();
        public ActionResult sucursal()
        {
            osucursal = new List<Sucursal>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT e.*, c.* FROM SUCURSAL e INNER JOIN EMPRESA c ON e.ID_EMPRESA = c.ID_EMRPESA", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Sucursal sucursal = new Sucursal();

                        sucursal.ID_EMPRESA = Convert.ToInt32(dr["ID_SUCURSAL"]);
                        sucursal.NOMBRE_SUCURSAL= dr["NOMBRE_SUCRSAL"].ToString();
                        sucursal.CORREO_ELECTRONICO = dr["CORREO_ELECTRONICO"].ToString();
                        sucursal.DIRECCION = dr["DIRECCION"].ToString();
                        sucursal.ID_SUCURSAL = Convert.ToInt32(dr["ID_EMPRESA"]);
                        sucursal.COMENTARIO = dr["COMENTARIO"].ToString();
                        sucursal.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        Empresa empresa = new Empresa();

                        empresa.ID_EMPRESA = Convert.ToInt32(dr["ID_EMPRESA"]);
                        empresa.NOMBRE_EMPRESA = dr["NOMBRE_EMPRESA"].ToString();
                        empresa.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        sucursal.EMPRESA = empresa;

                        osucursal.Add(sucursal);

                    }
                }
            }
            return View(osucursal);
        }


    }
}



