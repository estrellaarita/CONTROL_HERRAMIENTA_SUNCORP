using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Common;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;
using System.Diagnostics;
using System.Drawing;
using System.Web.UI.WebControls;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    [ValidarSesion]
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
                SqlCommand cmd = new SqlCommand("SP_VERserieherramienta", oconexion);

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
                        //serie.ID_FACTURA = Convert.ToInt32(dr["ID_FACTURA"]);
                        // Asignación de la propiedad ID_FACTURA con manejo de DBNull.Value
                        serie.ID_FACTURA = dr["ID_FACTURA"] != DBNull.Value ? Convert.ToInt32(dr["ID_FACTURA"]) : default(int);


                        serie.COMENTARIO = dr["COMENTARIO"].ToString();
                        serie.CANTIDAD_DE_EMPLEADOS = Convert.ToInt32(dr["CANTIDAD_DE_EMPLEADOS"]);
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
                        
                        Factura afactura = new Factura();
                        if (dr["ID_FACTURA"] != DBNull.Value)
                        {
                            serie.ID_FACTURA = Convert.ToInt32(dr["ID_FACTURA"]);
                        }
                        else
                        {
                            // Aquí se deside qué hacer si el valor es DBNull.Value (es decir, NULL en la base de datos)

                            serie.ID_FACTURA = 0; // Asignando un valor predeterminado, en este caso 0
                        }

                        afactura.NUMERO_FACTURA = dr["NUMERO_FACTURA"].ToString();
                       afactura.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);
                        serie.FACTURA = afactura;

                        oserie.Add(serie);

                    }
                }
            }
            return View(oserie);
        }

        [HttpGet]
      
        public JsonResult listaserieherramienta()
        {
            oserie = new List<Serie>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT " +
                                               "sr.ID_REGISTRO_SERIE_HERRAMIENTA, " +
                                               "sr.ID_SUCURSAL, " +
                                               "s.NOMBRE_SUCURSAL, " +
                                               "sr.ID_HERRAMIENTA, " +
                                               "h.MODELO, " +
                                               "sr.UBICACION_FISICA, " +
                                               "sr.NUMERO_SERIE, " +
                                               "sr.PRECIO, " +
                                               "sr.ID_ESTADO_HERRAMIENTA, " +
                                               "e.DECRIPCION_ESTADO_HERRAMIENTA," +
                                               "f.ID_FACTURA," +
                                               "f.NUMERO_FACTURA, " +
                                               "sr.COMENTARIO, " +

                                               "COUNT(DISTINCT emp.DNI) AS CANTIDAD_DE_EMPLEADOS, " +
                                               "MAX(sr.FECHA_REGISTRO) AS FECHA_REGISTRO " +
                                               "FROM REGISTRO_SERIE_HERRAMIENTA sr " +
                                               "INNER JOIN SUCURSAL s ON sr.ID_SUCURSAL = s.ID_SUCURSAL " +
                                               "INNER JOIN HERRAMIENTA h ON sr.ID_HERRAMIENTA = h.ID_HERRAMIENTA " +
                                               "INNER JOIN ESTADO_HERRAMIENTA e ON sr.ID_ESTADO_HERRAMIENTA = e.ID_ESTADO_HERRAMIENTA " +
                                               "LEFT JOIN FACTURA f ON sr.ID_FACTURA = f.ID_FACTURA " +
                                               "LEFT JOIN EMPLEADO_REGISTRO_SERIE_HERRAMIENTA ersh ON sr.ID_REGISTRO_SERIE_HERRAMIENTA = ersh.ID_REGISTRO_SERIE_HERRAMIENTA " +
                                               "LEFT JOIN EMPLEADO emp ON ersh.ID_EMPLEADO = emp.ID_EMPLEADO " +
                                               "GROUP BY " +
                                               "sr.ID_REGISTRO_SERIE_HERRAMIENTA, " +
                                               "sr.ID_SUCURSAL, " +
                                               "s.NOMBRE_SUCURSAL, " +
                                               "sr.ID_HERRAMIENTA, " +
                                               "h.MODELO, " +
                                               "sr.UBICACION_FISICA, " +
                                               "sr.NUMERO_SERIE, " +
                                               "sr.PRECIO, " +
                                               "sr.ID_ESTADO_HERRAMIENTA, " +
                                               "e.DECRIPCION_ESTADO_HERRAMIENTA," +
                                               "f.ID_FACTURA," +
                                               "f.NUMERO_FACTURA, " +
                                               "sr.COMENTARIO " +
                                               "ORDER BY FECHA_REGISTRO DESC;", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
            

            using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Serie serie = new Serie();

                        serie.ID_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(dr["ID_REGISTRO_SERIE_HERRAMIENTA"]);

                        serie.ID_SUCURSAL = Convert.ToInt32(dr["ID_SUCURSAL"]);
                        serie.ID_HERRAMIENTA = Convert.ToInt32(dr["ID_HERRAMIENTA"]);
                        serie.UBICACION_FISICA = dr["UBICACION_FISICA"].ToString();
                        serie.NUMERO_SERIE = dr["NUMERO_SERIE"].ToString();
                        serie.PRECIO = dr["PRECIO"].ToString();
                        serie.COMENTARIO = dr["COMENTARIO"].ToString();
                        serie.CANTIDAD_DE_EMPLEADOS = Convert.ToInt32(dr["CANTIDAD_DE_EMPLEADOS"]);
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
                        Factura afactura = new Factura();
                        if (dr["ID_FACTURA"] != DBNull.Value)
                        {
                            serie.ID_FACTURA = Convert.ToInt32(dr["ID_FACTURA"]);
                        }
                        else
                        {
                            // Aquí se deside qué hacer si el valor es DBNull.Value (es decir, NULL en la base de datos)

                            serie.ID_FACTURA = 0; // Asignando un valor predeterminado, en este caso 0
                        }

                        afactura.NUMERO_FACTURA = dr["NUMERO_FACTURA"].ToString();
                        afactura.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);
                        serie.FACTURA = afactura;

                        oserie.Add(serie);

                    }
                }
            }

            return Json(new { data = oserie }, JsonRequestBehavior.AllowGet);
        }

        // 

        [HttpGet]
        public JsonResult GetSerieEmpleado(int idseries)
        {
            List<Ver_empleado> aserieempleado = new List<Ver_empleado>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                string query = @"
                SELECT 
                    RH.ID_REGISTRO_SERIE_HERRAMIENTA, 
                    E.ID_EMPLEADO, 
                    E.DNI, 
                    E.PRIMER_NOMBRE AS NOMBRE, 
                    E.PRIMER_APELLIDO AS APELLIDO, 
                    S.NOMBRE_SUCURSAL AS SUCURSAL, 
                    D.DECRIPCION_DEPARTAMENTO AS DEPARTAMENTO, 
                    R.DECRIPCION_ROL AS ROL
                FROM 
                    EMPLEADO_REGISTRO_SERIE_HERRAMIENTA EH
                INNER JOIN 
                    EMPLEADO E ON EH.ID_EMPLEADO = E.ID_EMPLEADO
                INNER JOIN 
                    REGISTRO_SERIE_HERRAMIENTA RH ON EH.ID_REGISTRO_SERIE_HERRAMIENTA = RH.ID_REGISTRO_SERIE_HERRAMIENTA
                INNER JOIN 
                    SUCURSAL S ON S.ID_SUCURSAL = E.ID_SUCURSAL
                INNER JOIN 
                    DEPARTAMENTO D ON D.ID_DEPARTAMENTO = E.ID_DEPARTAMENTO
                INNER JOIN 
                    ROL R ON E.ID_ROL = R.ID_ROL
                WHERE 
                    RH.ID_REGISTRO_SERIE_HERRAMIENTA = @IdSeries";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.Parameters.AddWithValue("@IdSeries", idseries);

                oconexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Ver_empleado serieempleado = new Ver_empleado
                    {
                        IdRegistroSerieHerramienta = Convert.ToInt32(reader["ID_REGISTRO_SERIE_HERRAMIENTA"]),
                        IdEmpleado = Convert.ToInt32(reader["ID_EMPLEADO"]),
                        Dni = reader["DNI"].ToString(),
                        Nombre = reader["NOMBRE"].ToString(),
                        Apellido = reader["APELLIDO"].ToString(),
                        Sucursal = reader["SUCURSAL"].ToString(),
                        Departamento = reader["DEPARTAMENTO"].ToString(),
                        Rol = reader["ROL"].ToString()
                    };

                    aserieempleado.Add(serieempleado);
                }
            }

            return Json(aserieempleado, JsonRequestBehavior.AllowGet);
        }

        //ver datos en select
        [HttpGet]
        public JsonResult GetSerieEmpleados()
        {
            List<Ver_empleado> aserieempleado = new List<Ver_empleado>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                string query = @"
                SELECT 
                    E.ID_EMPLEADO, 
                    E.DNI, 
                    E.PRIMER_NOMBRE AS NOMBRE, 
                    E.PRIMER_APELLIDO AS APELLIDO, 
                    S.NOMBRE_SUCURSAL AS SUCURSAL, 
                    D.DECRIPCION_DEPARTAMENTO AS DEPARTAMENTO, 
                    R.DECRIPCION_ROL AS ROL
                FROM EMPLEADO E 
                INNER JOIN 
                    SUCURSAL S ON S.ID_SUCURSAL = E.ID_SUCURSAL
                INNER JOIN 
                    DEPARTAMENTO D ON D.ID_DEPARTAMENTO = E.ID_DEPARTAMENTO
                INNER JOIN 
                    ROL R ON E.ID_ROL = R.ID_ROL";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                //cmd.Parameters.AddWithValue("@IdSeries", idseries);

                oconexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Ver_empleado serieempleado = new Ver_empleado
                    {
                        IdEmpleado = Convert.ToInt32(reader["ID_EMPLEADO"]),
                        Dni = reader["DNI"].ToString(),
                        Nombre = reader["NOMBRE"].ToString(),
                        Apellido = reader["APELLIDO"].ToString(),
                        Sucursal = reader["SUCURSAL"].ToString(),
                        Departamento = reader["DEPARTAMENTO"].ToString(),
                        Rol = reader["ROL"].ToString()
                    };

                    aserieempleado.Add(serieempleado);
                }
            }

            return Json(aserieempleado, JsonRequestBehavior.AllowGet);
        }



        //guardar empleados segun herramienta json
        [HttpPost]
        public JsonResult GuardarEmpleadoHerramienta(int idEmpleado, int idHerramienta)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
                {
                    string query = "INSERT INTO EMPLEADO_REGISTRO_SERIE_HERRAMIENTA (ID_EMPLEADO, ID_REGISTRO_SERIE_HERRAMIENTA) VALUES (@IdEmpleado, @IdHerramienta)";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@IdHerramienta", idHerramienta);

                    oconexion.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Json(new { success = false, message = ex.Message });
            }
        }

        // eliminar el empleado del modal de la relacion del empleado registro serie 
        [HttpPost]
        public JsonResult EliminarEmpleado(int idEmpleado, int idSerie)
        {
            bool success = false;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM EMPLEADO_REGISTRO_SERIE_HERRAMIENTA WHERE ID_EMPLEADO = @IdEmpleado AND ID_REGISTRO_SERIE_HERRAMIENTA = @IdSerie", oconexion);
                    cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@IdSerie", idSerie);

                    oconexion.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    success = rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return Json(new { success = success });
        }

        //CREATE SERIE HERRAMIENTA

        [HttpGet]
        public ActionResult Registrarserie()
        {
            //LISTA SUCURSAL

            List<Sucursal> osucursal = new List<Sucursal>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM SUCURSAL", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Sucursal sucursal = new Sucursal();
                    sucursal.ID_SUCURSAL = Convert.ToInt32(reader["ID_SUCURSAL"]);
                    sucursal.NOMBRE_SUCURSAL = reader["NOMBRE_SUCURSAL"].ToString();
                    osucursal.Add(sucursal);
                }
            }
            ViewBag.Sucursal = new SelectList(osucursal, "ID_SUCURSAL", "NOMBRE_SUCURSAL");

            //LISTA HERRAMIENTA

            List<Herramienta> oherramienta = new List<Herramienta>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM HERRAMIENTA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Herramienta herramienta = new Herramienta();
                    herramienta.ID_HERRAMIENTA = Convert.ToInt32(reader["ID_HERRAMIENTA"]);
                    herramienta.MODELO = reader["MODELO"].ToString();
                    oherramienta.Add(herramienta);
                }
            }
            ViewBag.Herramienta = new SelectList(oherramienta, "ID_HERRAMIENTA", "MODELO");

            //LISTA ESTADO HERRAMIENTA

            List<Estado_herramienta> oestado = new List<Estado_herramienta>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ESTADO_HERRAMIENTA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Estado_herramienta estado = new Estado_herramienta();
                   estado.ID_ESTADO_HERRAMIENTA = Convert.ToInt32(reader["ID_ESTADO_HERRAMIENTA"]);
                   estado.DECRIPCION_ESTADO_HERRAMIENTA = reader["DECRIPCION_ESTADO_HERRAMIENTA"].ToString();
                    oestado.Add(estado);
                }
            }
            ViewBag.Estado_herramienta = new SelectList(oestado, "ID_ESTADO_HERRAMIENTA", "DECRIPCION_ESTADO_HERRAMIENTA");

            //LISTA FACTURA

            List<Factura> ofactura = new List<Factura>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM FACTURA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Factura factura = new Factura();
                    factura.ID_FACTURA = Convert.ToInt32(reader["ID_FACTURA"]);
                    factura.NUMERO_FACTURA = reader["NUMERO_FACTURA"].ToString();
                    ofactura.Add(factura);
                }
            }
            ViewBag.Factura = new SelectList(ofactura, "ID_FACTURA", "NUMERO_FACTURA");

            return View();
        }

        [HttpPost]
        public ActionResult Registrarserie(Serie oserie)
        {

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_REGISTRO_SERIE_HARRAMIENTA", oconexion);

                cmd.Parameters.AddWithValue("ID_SUCURSAL", oserie.ID_SUCURSAL);
                cmd.Parameters.AddWithValue("ID_HERRAMIENTA", oserie.ID_HERRAMIENTA);
                cmd.Parameters.AddWithValue("UBICACION_FISICA", oserie.UBICACION_FISICA);
                cmd.Parameters.AddWithValue("NUMERO_SERIE", oserie.NUMERO_SERIE);
                cmd.Parameters.AddWithValue("PRECIO", oserie.PRECIO);
                cmd.Parameters.AddWithValue("ID_ESTADO_HERRAMIENTA", oserie.ID_ESTADO_HERRAMIENTA);
                cmd.Parameters.AddWithValue("ID_FACTURA", oserie.ID_FACTURA);
                cmd.Parameters.AddWithValue("COMENTARIO", oserie.COMENTARIO);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("serie", "Serie_herramienta");
        }

        //EDITAR SERIE HERRAMIENTA

        [HttpGet]
        public ActionResult Editarserieher(int? IdSERIE)
        {
            //Lista sucursal
            List<Sucursal> osucursal = new List<Sucursal>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM SUCURSAL", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Sucursal sucursal = new Sucursal();
                    sucursal.ID_SUCURSAL = Convert.ToInt32(reader["ID_SUCURSAL"]);
                    sucursal.NOMBRE_SUCURSAL = reader["NOMBRE_SUCURSAL"].ToString();
                    osucursal.Add(sucursal);
                }
            }
            ViewBag.Sucursal = new SelectList(osucursal, "ID_SUCURSAL", "NOMBRE_SUCURSAL");

            //Lita herramienta
            List<Herramienta> oherramienta = new List<Herramienta>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM HERRAMIENTA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Herramienta herramienta = new Herramienta();
                    herramienta.ID_HERRAMIENTA = Convert.ToInt32(reader["ID_HERRAMIENTA"]);
                    herramienta.MODELO = reader["MODELO"].ToString();
                    oherramienta.Add(herramienta);
                }
            }
            ViewBag.Herramienta = new SelectList(oherramienta, "ID_HERRAMIENTA", "MODELO");

            //Lista Estado herramienta
            List<Estado_herramienta> oestado = new List<Estado_herramienta>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ESTADO_HERRAMIENTA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Estado_herramienta estado = new Estado_herramienta();
                    estado.ID_ESTADO_HERRAMIENTA = Convert.ToInt32(reader["ID_ESTADO_HERRAMIENTA"]);
                    estado.DECRIPCION_ESTADO_HERRAMIENTA = reader["DECRIPCION_ESTADO_HERRAMIENTA"].ToString();
                    oestado.Add(estado);
                }
            }
            ViewBag.Estado_herramienta = new SelectList(oestado, "ID_ESTADO_HERRAMIENTA", "DECRIPCION_ESTADO_HERRAMIENTA");

            //Lista Factura
            List<Factura> ofactura = new List<Factura>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM FACTURA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Factura factura = new Factura();
                    factura.ID_FACTURA = Convert.ToInt32(reader["ID_FACTURA"]);
                    factura.NUMERO_FACTURA = reader["NUMERO_FACTURA"].ToString();
                    ofactura.Add(factura);
                }
            }
            ViewBag.Factura = new SelectList(ofactura, "ID_FACTURA", "NUMERO_FACTURA");

            Serie pserie = oserie.Where(c => c.ID_REGISTRO_SERIE_HERRAMIENTA == IdSERIE).FirstOrDefault();

            return View(pserie);
        }

        [HttpPost]
        public ActionResult Editarserieher(Serie oserie)
        {

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_REGISTRO_SERIE_HERRAMIENTA", oconexion);

                cmd.Parameters.AddWithValue("ID_REGISTRO_SERIE_HERRAMIENTA", oserie.ID_REGISTRO_SERIE_HERRAMIENTA);
                cmd.Parameters.AddWithValue("ID_SUCURSAL", oserie.ID_SUCURSAL);
                cmd.Parameters.AddWithValue("ID_HERRAMIENTA", oserie.ID_HERRAMIENTA);
                cmd.Parameters.AddWithValue("UBICACION_FISICA", oserie.UBICACION_FISICA);
                cmd.Parameters.AddWithValue("NUMERO_SERIE", oserie.NUMERO_SERIE);
                cmd.Parameters.AddWithValue("PRECIO", oserie.PRECIO);
                cmd.Parameters.AddWithValue("ID_ESTADO_HERRAMIENTA", oserie.ID_ESTADO_HERRAMIENTA);
                cmd.Parameters.AddWithValue("ID_FACTURA", oserie.ID_FACTURA);
                cmd.Parameters.AddWithValue("COMENTARIO", oserie.COMENTARIO);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();


            }
            return RedirectToAction("serie", "Serie_herramienta");
        }

        //ELIMINAR REGISTRO SERIE HERRAMIENTA

        [HttpGet]
        public ActionResult Eliminarserieher(int? Idserie)
        {
            Serie aserie = oserie.Where(c => c.ID_REGISTRO_SERIE_HERRAMIENTA== Idserie).FirstOrDefault();
            return View(aserie);
        }

        [HttpPost]
        public ActionResult Eliminarserieher(string Idserie)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_REGISTRO_SERIE_HERRAMIENTA", oconexion);
                cmd.Parameters.AddWithValue("ID_REGISTRO_SERIE_HERRAMIENTA", Idserie);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
                }
                // Si la eliminación es exitosa, redirigir a la vista deseada
                return RedirectToAction("serie", "Serie_herramienta");
            }
            catch (SqlException ex)
            {
                // En caso de un conflicto, retornar un mensaje de error a la vista
                ViewBag.ErrorMessage = "No puede eliminar el departamento porque hay registros relacionados";
                // Aquí podrías registrar el error en un log si es necesario

                // Redirigir a la vista actual con el mensaje de error
                return View();  // Asegúrate de que "Marca" sea la vista correcta
            }
        }
    }
}