using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Reflection;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    [ValidarSesion]
    public class SerieempleController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        // GET: Serie_empleado
       public ActionResult Serie_empleado(int? idseries)
       {
            List<Serieempleado> aserieempleado = new List<Serieempleado>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT " +
                    "er.ID_EMPLEADO_REGISTRO_SERIE_HERRAMIENTA, " +
                    "s.NOMBRE_SUCURSAL, " +
                    "h.MODELO, " +
                    "sr.NUMERO_SERIE, " +
                    "e.PRIMER_NOMBRE, " +
                    "e.SEGUNDO_NOMBRE, " +
                    "e.PRIMER_APELLIDO, " +
                    "er.FECHA_REGISTRO " +
                    "FROM " +
                    "EMPLEADO_REGISTRO_SERIE_HERRAMIENTA er " +
                    "INNER JOIN REGISTRO_SERIE_HERRAMIENTA sr ON er.ID_REGISTRO_SERIE_HERRAMIENTA = sr.ID_REGISTRO_SERIE_HERRAMIENTA " +
                    "INNER JOIN HERRAMIENTA h ON sr.ID_HERRAMIENTA = h.ID_HERRAMIENTA " +
                    "INNER JOIN EMPLEADO e ON er.ID_EMPLEADO = e.ID_EMPLEADO " +
                    "INNER JOIN SUCURSAL s ON e.ID_SUCURSAL = s.ID_SUCURSAL", oconexion);
                oconexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                            {

                                 Serieempleado serieempleadoS = new Serieempleado();

                                 serieempleadoS.ID_EMPLEADO_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(reader["ID_EMPLEADO_REGISTRO_SERIE_HERRAMIENTA"]);
                                 serieempleadoS.FECHA_REGISTRO = Convert.ToDateTime(reader["FECHA_REGISTRO"]);

                                Sucursal usucursal = new Sucursal();
                                usucursal.NOMBRE_SUCURSAL = reader["NOMBRE_SUCURSAL"].ToString();
                                serieempleadoS.SUCURSAL = usucursal;

                                Herramienta uherramienta = new Herramienta();
                                uherramienta.MODELO = reader["MODELO"].ToString();
                                serieempleadoS.HERRAMIENTA = uherramienta;

                                Serie userie = new Serie();
                                userie.NUMERO_SERIE = reader["NUMERO_SERIE"].ToString();
                                serieempleadoS.SERIE = userie;

                                Empleado uempleado = new Empleado();
                                uempleado.PRIMER_NOMBRE = reader["PRIMER_NOMBRE"].ToString();
                                uempleado.PRIMER_APELLIDO = reader["PRIMER_APELLIDO"].ToString();
                                serieempleadoS.EMPLEADO = uempleado;

                               aserieempleado.Add(serieempleadoS);
                    }
            }
            Serieempleado oserieempleado = aserieempleado.Where(c => c.ID_REGISTRO_SERIE_HERRAMIENTA == idseries).FirstOrDefault();

            return View(oserieempleado);
        }
        

    }
}