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
    }
}