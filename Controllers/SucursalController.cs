﻿using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    [ValidarSesion]
    public class SucursalController : Controller
    {

        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Sucursal> osucursal = new List<Sucursal>();
        // GET: Sucursal
        public ActionResult sucursal()
        {
            osucursal = new List<Sucursal>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT i.*, e.* FROM SUCURSAL i INNER JOIN EMPRESA e ON i.ID_EMPRESA = e.ID_EMPRESA", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Sucursal sucursal = new Sucursal();

                        sucursal.ID_SUCURSAL = Convert.ToInt32(dr["ID_SUCURSAL"]);
                        sucursal.NOMBRE_SUCURSAL = dr["NOMBRE_SUCURSAL"].ToString();
                        sucursal.CORREO_ELECTRONICO = dr["CORREO_ELECTRONICO"].ToString();
                        sucursal.DIRECCION = dr["DIRECCION"].ToString();
                        sucursal.ID_EMPRESA = Convert.ToInt32(dr["ID_EMPRESA"]);
                        sucursal.COMENTARIO = dr["COMENTARIO"].ToString();
                        sucursal.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        Empresa iempresa = new Empresa();

                        iempresa.ID_EMPRESA = Convert.ToInt32(dr["ID_EMPRESA"]);
                        iempresa.NOMBRE_EMPRESA = dr["NOMBRE_EMPRESA"].ToString();
                        iempresa.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        sucursal.EMPRESA = iempresa;

                        osucursal.Add(sucursal);

                    }
                }
            }
            return View(osucursal);
        }

        //CREATE SUCURSAL

        [HttpGet]
        public ActionResult Crear()
        {

            List<Empresa> aempresa = new List<Empresa>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM EMPRESA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Empresa rempresa = new Empresa();
                    rempresa.ID_EMPRESA = Convert.ToInt32(reader["ID_EMPRESA"]);
                    rempresa.NOMBRE_EMPRESA = reader["NOMBRE_EMPRESA"].ToString();
                    aempresa.Add(rempresa);
                }
            }
            ViewBag.iEmpresas = new SelectList(aempresa, "ID_EMPRESA", "NOMBRE_EMPRESA");


            return View();
        }

        [HttpPost]
        public ActionResult Crear(Sucursal osucursal)
        {
           
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_SUCURSAL", oconexion);

                
                cmd.Parameters.AddWithValue("NOMBRE_SUCURSAL", osucursal.NOMBRE_SUCURSAL);
                cmd.Parameters.AddWithValue("CORREO_ELECTRONICO", osucursal.CORREO_ELECTRONICO);
                cmd.Parameters.AddWithValue("DIRECCION", osucursal.DIRECCION);
                cmd.Parameters.AddWithValue("ID_EMPRESA", osucursal.ID_EMPRESA);
                cmd.Parameters.AddWithValue("COMENTARIO", osucursal.COMENTARIO);
              
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("sucursal", "Sucursal");
        }


        //EDITAR SUCURSAL

        [HttpGet]
        public ActionResult Editarsucur(int? Idsucursal)
        {
            List<Empresa> aempresa = new List<Empresa>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM EMPRESA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Empresa rempresa = new Empresa();
                    rempresa.ID_EMPRESA = Convert.ToInt32(reader["ID_EMPRESA"]);
                    rempresa.NOMBRE_EMPRESA = reader["NOMBRE_EMPRESA"].ToString();
                    aempresa.Add(rempresa);
                }
            }

            ViewBag.iEmpresas = new SelectList(aempresa, "ID_EMPRESA", "NOMBRE_EMPRESA");

            Sucursal arsucursal = osucursal.Where(c => c.ID_SUCURSAL == Idsucursal).FirstOrDefault();

            
            return View(arsucursal);
        }

        [HttpPost]
        public ActionResult Editarsucur(Sucursal osucursal)
        {
      

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_SUCURSAL", oconexion);

                cmd.Parameters.AddWithValue("ID_SUCURSAL", osucursal.ID_SUCURSAL);
                cmd.Parameters.AddWithValue("NOMBRE_SUCURSAL", osucursal.NOMBRE_SUCURSAL);
                cmd.Parameters.AddWithValue("CORREO_ELECTRONICO", osucursal.CORREO_ELECTRONICO);
                cmd.Parameters.AddWithValue("DIRECCION", osucursal.DIRECCION);
                cmd.Parameters.AddWithValue("ID_EMPRESA", osucursal.ID_EMPRESA);
                cmd.Parameters.AddWithValue("COMENTARIO", osucursal.COMENTARIO);
                
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction("sucursal", "Sucursal");
        }


        //ELIMINAR SUCURSAL

        [HttpGet]
        public ActionResult Eliminarsucur(int? Idsucursal)
        {
            Sucursal asucursal = osucursal.Where(c => c.ID_SUCURSAL == Idsucursal).FirstOrDefault();
            return View(asucursal);
        }

        [HttpPost]
        public ActionResult Eliminarsucur(string Idsucursal)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_SUCURSAL", oconexion);
                cmd.Parameters.AddWithValue("ID_SUCURSAL", Idsucursal);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
                }
                // Si la eliminación es exitosa, redirigir a la vista deseada
                return RedirectToAction("sucursal", "Sucursal");
            }
            catch (SqlException ex)
            {
                // En caso de un conflicto, retornar un mensaje de error a la vista
                ViewBag.ErrorMessage = "No puede eliminar la sucursal porque hay registros relacionados";
                // Aquí podrías registrar el error en un log si es necesario

                // Redirigir a la vista actual con el mensaje de error
                return View();  // Asegúrate de que "Marca" sea la vista correcta
            }
        }


    }

}





