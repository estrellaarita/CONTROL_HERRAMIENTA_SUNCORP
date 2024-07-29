﻿using CONTROL_HERRAMIENTA_SUNCORP.Models;
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
                SqlCommand cmd = new SqlCommand("SELECT rsh.*, s.*, h.*,e.*,f.* FROM REGISTRO_SERIE_HERRAMIENTA rsh INNER JOIN SUCURSAL s ON rsh.ID_SUCURSAL = s.ID_SUCURSAL INNER JOIN HERRAMIENTA h ON rsh.ID_HERRAMIENTA = h.ID_HERRAMIENTA INNER JOIN ESTADO_HERRAMIENTA e ON rsh.ID_ESTADO_HERRAMIENTA = e.ID_ESTADO_HERRAMIENTA Left join FACTURA f ON rsh.ID_FACTURA = f.ID_FACTURA", oconexion);
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


                        if (dr["ID_FACTURA"] != DBNull.Value)
                        {
                            serie.ID_FACTURA = Convert.ToInt32(dr["ID_FACTURA"]);
                        }
                        else
                        {
                            // Aquí decides qué hacer si el valor es DBNull.Value (es decir, NULL en la base de datos)
                            // Por ejemplo, podrías asignar un valor predeterminado o dejar serie.ID_FACTURA como 0 (dependiendo de tu lógica)
                            serie.ID_FACTURA = 0; // Asignando un valor predeterminado, en este caso 0
                        }

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

        //CREATE SERIE HERRAMIENTA

        [HttpGet]
        public ActionResult Registrarserie()
        {

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
    }
}