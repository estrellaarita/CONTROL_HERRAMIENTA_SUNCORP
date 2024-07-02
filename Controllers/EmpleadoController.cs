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
    public class EmpleadoController : Controller
    {

        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Empleado> oempleado = new List<Empleado>();
        // GET: Empleado
        public ActionResult Empleado()
        {

            oempleado = new List<Empleado>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT e.*, g.*, s.*, d.*, r.*, ee.* FROM EMPLEADO e INNER JOIN GENERO g ON e.ID_GENERO = g.ID_GENERO INNER JOIN SUCURSAL s ON e.ID_SUCURSAL = s.ID_SUCURSAL INNER JOIN DEPARTAMENTO d ON e.ID_DEPARTAMENTO = d.ID_DEPARTAMENTO INNER JOIN ROL r ON e.ID_ROL = r.ID_ROL INNER JOIN ESTADO_EMPLEADO ee ON e.ID_ESTADO_EMPLEADO = ee.ID_ESTADO_EMPLEADO", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Empleado empleado = new Empleado();

                        empleado.ID_EMPLEADO = Convert.ToInt32(dr["ID_EMPLEADO"]);
                        empleado.DNI = dr["DNI"].ToString();
                        empleado.PRIMER_NOMBRE = dr["PRIMER_NOMBRE"].ToString();
                        empleado.SEGUNDO_NOMBRE = dr["SEGUNDO_NOMBRE"].ToString();
                        empleado.PRIMER_APELLIDO = dr["PRIMER_APELLIDO"].ToString();
                        empleado.SEGUNDO_APELLIDO = dr["SEGUNDO_APELLIDO"].ToString();
                        empleado.ID_GENERO = Convert.ToInt32(dr["ID_GENERO"]);
                        empleado.ID_SUCURSAL = Convert.ToInt32(dr["ID_SUCURSAL"]);
                        empleado.ID_DEPARTAMENTO = Convert.ToInt32(dr["ID_DEPARTAMENTO"]);
                        empleado.ID_ROL = Convert.ToInt32(dr["ID_ROL"]);
                        empleado.ID_ESTADO_EMPLADO = Convert.ToInt32(dr["ID_ESTADO_EMPLEADO"]);
                        empleado.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //GENERO
                        Genero igenero = new Genero();

                        igenero.ID_GENERO = Convert.ToInt32(dr["ID_GENERO"]);
                        igenero.DESCRIPCION_GENERO= dr["DESCRIPCION_GENERO"].ToString();
                        igenero.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.GENERO = igenero;

                        //SUCURSAL
                        Sucursal asucursal = new Sucursal();

                        asucursal.ID_SUCURSAL = Convert.ToInt32(dr["ID_SUCURSAL"]);
                        asucursal.NOMBRE_SUCURSAL = dr["NOMBRE_SUCURSAL"].ToString();
                        asucursal.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.SUCURSAL = asucursal;

                        //DEPARTAMENTO
                        Departamento odepartamento = new Departamento();

                        odepartamento.ID_DEPARTAMENTO = Convert.ToInt32(dr["ID_DEPARTAMENTO"]);
                        odepartamento.DECRIPCION_DEPARTAMENTO = dr["DECRIPCION_DEPARTAMENTO"].ToString();
                        odepartamento.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.DEPARTAMENTO = odepartamento;

                        //ROL
                        Rol erol = new Rol();

                        erol.ID_ROL = Convert.ToInt32(dr["ID_ROL"]);
                        erol.DECRIPCION_ROL = dr["DECRIPCION_ROL"].ToString();
                        erol.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.ROL = erol;

                        //ESTADO EMPLEADO
                        Estado iestado= new Estado();

                        iestado.ID_ESTADO_EMPLEADO = Convert.ToInt32(dr["ID_ESTADO_EMPLEADO"]);
                        iestado.DESCRIPCION_ESTADO_EMPLEADO = dr["DESCRIPCION_ESTADO_EMPLEADO"].ToString();
                        iestado.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.ESTADO_EMPELADO = iestado;

                        oempleado.Add(empleado);

                    }
                }
            }
            return View(oempleado);
        }
    }
}