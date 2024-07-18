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
    public class Serie_herramientaController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Serie> oserie = new List<Serie>();
        // GET: Serie_herramienta
        public ActionResult serie()
        {
            oserie = new List<Serie>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT rsh.*, s.*, h.*, eh.*, nf.* FROM REGISTRO_SERIE_HERRAMIENTA rsh INNER JOIN SUCURSAL s ON rsh.ID_SUCURSAL = s.ID_SUCURSAL INNER JOIN HERRAMIENTA h ON rsh.ID_HERRAMIENTA = h.ID_HERRAMIENTA INNER JOIN ESTADO_HERRAMIENTA eh ON rsh.ID_ESTADO_HERRAMIENTA = eh.ID_ESTADO_HERRAMIENTA INNER JOIN FACTURA nf ON rsh.NUMERO_FACTURA = nf.NUMERO_FACTURA", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Serie serie = new Serie();

                        serie.ID_REGISTRO_SERIE_HERRAMIENTA= Convert.ToInt32(dr["ID_REGISTRO_SERIE_HERRAMIENTA"]);
                        serie.ID_SUCURSAL = Convert.ToInt32(dr["ID_SUCURSAL"]);
                        serie.ID_HERRAMIENTA = Convert.ToInt32(dr["ID_HERRAMIENTA"]);
                        serie.UBICACION_FISICA = dr["UBICACION_FISICA"].ToString();
                        serie.NUMERO_SERIE = dr["NUMERO_SERIE"].ToString();
                        serie.PRECIO = dr["PRECIO"].ToString();
                        serie.ID_ESTADO_HERRAMIENTA = Convert.ToInt32(dr["ID_ESTADO_HERRAMIENTA"]);
                        serie.NUMERO_FACTURA = Convert.ToInt32(dr["NUMERO_FACTURA"]);
                        serie.COMENTARIO = dr["COMENTARIO"].ToString();
                        serie.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //SUCURSAL
                        Sucursal asucursal = new Sucursal();

                        asucursal.ID_SUCURSAL = Convert.ToInt32(dr["ID_SUCURSAL"]);
                        asucursal.NOMBRE_SUCURSAL = dr["NOMBRE_SUCURSAL"].ToString();
                        asucursal.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        serie.SUCURSAL = asucursal;

                        //HERRAMIENTA
                        Herramienta aherramienta = new Herramienta();

                        aherramienta.ID_HERRAMIENTA = Convert.ToInt32(dr["ID_HERRAMIENTA"]);
                        aherramienta.MODELO = dr["MODELO"].ToString();
                        aherramienta.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        serie.HERRAMIENTA = aherramienta;

                        //ESTADO HERRAMIENTA
                        Estado_herramienta aestadoherramienta = new Estado_herramienta();

                        aestadoherramienta.ID_ESTADO_HERRAMIENTA = Convert.ToInt32(dr["ID_ESTADO_HERRAMIENTA"]);
                        aestadoherramienta.DECRIPCION_ESTADO_HERRAMIENTA = dr["DECRIPCION_ESTADO_HERRAMIENTA"].ToString();
                        aherramienta.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        serie.ESTADO_HERRAMIENTA = aestadoherramienta;

                        //NUMERO FACTURA
                         aestadoherramienta = new Estado_herramienta();

                        aestadoherramienta.ID_ESTADO_HERRAMIENTA = Convert.ToInt32(dr["ID_ESTADO_HERRAMIENTA"]);
                        aestadoherramienta.DECRIPCION_ESTADO_HERRAMIENTA = dr["DECRIPCION_ESTADO_HERRAMIENTA"].ToString();
                        aherramienta.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        serie.ESTADO_HERRAMIENTA = aestadoherramienta;

                        oserie.Add(serie);

                    }
                }
            }
            return View(oserie);
        }
    }
}