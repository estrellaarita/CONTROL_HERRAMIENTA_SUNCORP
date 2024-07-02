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

                cmd.Parameters.AddWithValue("DEPARTAMENTO", odepartamento.DECRIPCION_DEPARTAMENTO);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Departamento", "Departamento");
        }

        //EDITAR
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
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_DEPARTAMENTO", oconexion);
                cmd.Parameters.AddWithValue("ID_DEPARTAMENTO", Iddpt);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Departamento", "Departamento");
        }
    }
}
