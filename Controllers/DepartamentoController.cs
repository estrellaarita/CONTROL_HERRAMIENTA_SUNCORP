﻿using CONTROL_HERRAMIENTA_SUNCORP.Models;
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

    public class DepartamentoController : Controller
    {

        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Departamento> odepartamento = new List<Departamento>();
        // GET: Departamento
        public ActionResult Departamento()
        {

            odepartamento = new List<Departamento>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM DEPARTAMENTO", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Departamento departamento = new Departamento();

                        departamento.ID_DEPARTAMENTO = Convert.ToInt32(dr["ID_DEPARTAMENTO"]);
                        departamento.DECRIPCION_DEPARTAMENTO = dr["DECRIPCION_DEPARTAMENTO"].ToString();
                        departamento.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        odepartamento.Add(departamento);

                    }
                }
            }
            return View(odepartamento);
        }

        // Acción para mostrar la vista de búsqueda de departamentos
        public ActionResult Buscar()
        {
            return View();
        }

        // Acción que maneja la búsqueda de departamentos por nombre
        [HttpPost]
        public ActionResult Buscar(string nombre)
        {
            List<Departamento> departamentos = new List<Departamento>();

            // Consulta SQL para buscar departamentos por nombre
            string query = "SELECT ID_DEPARTAMENTO, DECRIPCION_DEPARTAMENTO FROM DEPARTAMENTO WHERE DECRIPCION_DEPARTAMENTO LIKE @nombre";

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.Parameters.AddWithValue("@nombre", "%" + nombre + "%");

                 oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())

                    while (dr.Read())
                    {
                        Departamento departamento = new Departamento();

                        departamento.ID_DEPARTAMENTO = Convert.ToInt32(dr["ID_DEPARTAMENTO"]);
                        departamento.DECRIPCION_DEPARTAMENTO = dr["DECRIPCION_DEPARTAMENTO"].ToString();
                       

                        odepartamento.Add(departamento);
                    }

                oconexion.Close();
            }

            return View(departamentos);
        }


        //REGISTRAR
        [HttpGet]
        public ActionResult registrardept()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registrardept(Departamento odepartamento)
        {


            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_DEPARTAMENTO", oconexion);

                cmd.Parameters.AddWithValue("DESCRIPCION_DEPARTAMENTO", odepartamento.DECRIPCION_DEPARTAMENTO);
               
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

               
            }
            return RedirectToAction("Departamento", "Departamento");
        }
   

        //ACTUALIZAR
        [HttpGet]
        public ActionResult Editardept(int? Iddpt)
        {
            Departamento adepartamento = odepartamento.Where(c => c.ID_DEPARTAMENTO == Iddpt).FirstOrDefault();
            return View(adepartamento);
        }

        [HttpPost]
        public ActionResult Editardept(Departamento odepartamento)
        {
  

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_DEPARTAMENTO", oconexion);

                cmd.Parameters.AddWithValue("ID_DEPARTAMENTO", odepartamento.ID_DEPARTAMENTO);
                cmd.Parameters.AddWithValue("DESCRIPCION_DEPARTAMENTO", odepartamento.DECRIPCION_DEPARTAMENTO);
      
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction("Departamento", "Departamento");
        }

        //ELIMINAR
        [HttpGet]
        public ActionResult Eliminardept(int? Iddpt)
        {
            Departamento adepartamento = odepartamento.Where(c => c.ID_DEPARTAMENTO == Iddpt).FirstOrDefault();
            return View(adepartamento);
        }

        [HttpPost]
        public ActionResult Eliminardept(string Iddpt)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_DEPARTAMENTO", oconexion);
                cmd.Parameters.AddWithValue("ID_DEPARTAMENTO", Iddpt);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
                }
                // Si la eliminación es exitosa, redirigir a la vista deseada
                return RedirectToAction("Departamento", "Departamento");
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
