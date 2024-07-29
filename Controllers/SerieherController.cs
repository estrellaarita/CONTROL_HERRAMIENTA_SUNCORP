using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Serie_empleadoController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Serieempleado> oserieempleado = new List<Serieempleado>();

        // GET: Serie_empleado
        public ActionResult serie_empleado()
        {
            oserieempleado = new List<Serieempleado>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT ersh.*, e.*, sr.* FROM EMPLEADO_REGISTRO_SERIE_HERRAMIENTA ersh INNER JOIN EMPLEADO e ON ersh.ID_EMPLEADO = e.ID_EMPLEADO INNER JOIN REGISTRO_SERIE_HERRAMIENTA sr ON ersh.ID_REGISTRO_SERIE_HERRAMIENTA = sr.ID_REGISTRO_SERIE_HERRAMIENTA", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Serieempleado serieempleadoS = new Serieempleado();

                        serieempleadoS.ID_EMPLEADO_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(dr["ID_EMPLEADO_REGISTRO_SERIE_HERRAMIENTA"]);
                        serieempleadoS.ID_EMPLEADO = Convert.ToInt32(dr["ID_EMPLEADO"]);
                        serieempleadoS.ID_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(dr["ID_REGISTRO_SERIE_HERRAMIENTA"]);
                        serieempleadoS.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        Empleado uempleado = new Empleado();

                        uempleado.ID_EMPLEADO = Convert.ToInt32(dr["ID_EMPLEADO"]);
                        uempleado.PRIMER_NOMBRE = dr["PRIMER_NOMBRE"].ToString();
                        uempleado.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        serieempleadoS.EMPLEADO = uempleado;

                        Serie userie = new Serie();

                        userie.ID_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(dr["ID_REGISTRO_SERIE_HERRAMIENTA"]);
                        userie.NUMERO_SERIE = dr["NUMERO_SERIE"].ToString();
                        userie.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        serieempleadoS.SERIE = userie;

                        oserieempleado.Add(serieempleadoS);

                    }
                }
            }

            return View();
        }


    }
}