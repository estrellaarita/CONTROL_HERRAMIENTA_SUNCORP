using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    [ValidarSesion]

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
                SqlCommand cmd = new SqlCommand("SELECT " +
                                               "e.DNI, " +
                                               "e.PRIMER_NOMBRE, " +
                                               "e.SEGUNDO_NOMBRE, " +
                                               "e.PRIMER_APELLIDO, " +
                                               "e.SEGUNDO_APELLIDO, " +
                                               "g.DESCRIPCION_GENERO, " +
                                               "s.NOMBRE_SUCURSAL, " +
                                               "d.DECRIPCION_DEPARTAMENTO, " +
                                               "r.DECRIPCION_ROL, " +
                                               "es.DESCRIPCION_ESTADO_EMPLEADO, " +

                                               "COUNT(DISTINCT sr.NUMERO_SERIE) AS NUMERO_DE_HERRAMIENTAS, " +
                                               "MAX(e.FECHA_REGISTRO) AS FECHA_REGISTRO " +
                                               "FROM EMPLEADO e " +

                                               "INNER JOIN GENERO g ON e.ID_GENERO = g.ID_GENERO " +
                                               "INNER JOIN SUCURSAL s ON e.ID_SUCURSAL = s.ID_SUCURSAL " +
                                               "INNER JOIN DEPARTAMENTO d ON e.ID_DEPARTAMENTO = d.ID_DEPARTAMENTO " +
                                               "INNER JOIN ROL r ON e.ID_ROL = r.ID_ROL " +
                                               "INNER JOIN ESTADO_EMPLEADO es ON e.ID_ESTADO_EMPLEADO = es.ID_ESTADO_EMPLEADO " +
                                               "LEFT JOIN EMPLEADO_REGISTRO_SERIE_HERRAMIENTA ersh ON e.ID_EMPLEADO = ersh.ID_EMPLEADO " +
                                               "LEFT JOIN REGISTRO_SERIE_HERRAMIENTA sr ON ersh.ID_REGISTRO_SERIE_HERRAMIENTA = sr.ID_REGISTRO_SERIE_HERRAMIENTA " +
                                               
                                               "GROUP BY " +

                                               "e.DNI, " +
                                               "e.PRIMER_NOMBRE, " +
                                               "e.SEGUNDO_NOMBRE, " +
                                               "e.PRIMER_APELLIDO, " +
                                               "e.SEGUNDO_APELLIDO, " +
                                               "g.DESCRIPCION_GENERO, " +
                                               "s.NOMBRE_SUCURSAL, " +
                                               "d.DECRIPCION_DEPARTAMENTO, " +
                                               "r.DECRIPCION_ROL, " +
                                               "es.DESCRIPCION_ESTADO_EMPLEADO " +
                                               "ORDER BY MAX(e.FECHA_REGISTRO) DESC;", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Empleado empleado = new Empleado();

                       //empleado.ID_EMPLEADO = Convert.ToInt32(dr["ID_EMPLEADO"]);
                        empleado.DNI = dr["DNI"].ToString();
                        empleado.PRIMER_NOMBRE = dr["PRIMER_NOMBRE"].ToString();
                        empleado.SEGUNDO_NOMBRE = dr["SEGUNDO_NOMBRE"].ToString();
                        empleado.PRIMER_APELLIDO = dr["PRIMER_APELLIDO"].ToString();
                        empleado.SEGUNDO_APELLIDO = dr["SEGUNDO_APELLIDO"].ToString();
                        /* empleado.ID_GENERO = Convert.ToInt32(dr["ID_GENERO"]);
                         empleado.ID_SUCURSAL = Convert.ToInt32(dr["ID_SUCURSAL"]);
                         empleado.ID_DEPARTAMENTO = Convert.ToInt32(dr["ID_DEPARTAMENTO"]);
                         empleado.ID_ROL = Convert.ToInt32(dr["ID_ROL"]);
                         empleado.ID_ESTADO_EMPLEADO = Convert.ToInt32(dr["ID_ESTADO_EMPLEADO"]);*/
                        empleado.NUMERO_DE_HERRAMIENTAS = Convert.ToInt32(dr["NUMERO_DE_HERRAMIENTAS"]);
                        empleado.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //GENERO
                        Genero igenero = new Genero();

                      //  igenero.ID_GENERO = Convert.ToInt32(dr["ID_GENERO"]);
                        igenero.DESCRIPCION_GENERO= dr["DESCRIPCION_GENERO"].ToString();
                       // igenero.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.GENERO = igenero;

                        //SUCURSAL
                        Sucursal asucursal = new Sucursal();

                       // asucursal.ID_SUCURSAL = Convert.ToInt32(dr["ID_SUCURSAL"]);
                        asucursal.NOMBRE_SUCURSAL = dr["NOMBRE_SUCURSAL"].ToString();
                      //  asucursal.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.SUCURSAL = asucursal;

                        //DEPARTAMENTO
                        Departamento odepartamento = new Departamento();

                       // odepartamento.ID_DEPARTAMENTO = Convert.ToInt32(dr["ID_DEPARTAMENTO"]);
                        odepartamento.DECRIPCION_DEPARTAMENTO = dr["DECRIPCION_DEPARTAMENTO"].ToString();
                       // odepartamento.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.DEPARTAMENTO = odepartamento;

                        //ROL
                        Rol erol = new Rol();

                      //  erol.ID_ROL = Convert.ToInt32(dr["ID_ROL"]);
                        erol.DECRIPCION_ROL = dr["DECRIPCION_ROL"].ToString();
                       // erol.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.ROL = erol;

                        //ESTADO EMPLEADO
                        Estado iestado= new Estado();

                       // iestado.ID_ESTADO_EMPLEADO = Convert.ToInt32(dr["ID_ESTADO_EMPLEADO"]);
                        iestado.DESCRIPCION_ESTADO_EMPLEADO = dr["DESCRIPCION_ESTADO_EMPLEADO"].ToString();
                       // iestado.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.ESTADO_EMPLEADO = iestado;

                        oempleado.Add(empleado);

                    }
                }
            }
            return View(oempleado);
        }

        //CREAR EMPLEADO
        [HttpGet]
        public ActionResult Registraremple()
        {
            //LISTA GENERO
            List<Genero> ogenero = new List<Genero>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM GENERO", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Genero genero = new Genero();
                    genero.ID_GENERO = Convert.ToInt32(reader["ID_GENERO"]);
                    genero.DESCRIPCION_GENERO = reader["DESCRIPCION_GENERO"].ToString();
                    ogenero.Add(genero);
                }
            }
            ViewBag.Genero = new SelectList(ogenero, "ID_GENERO", "DESCRIPCION_GENERO");

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

            //LISTA DEPARTAMENTO
            List<Departamento> odepartamento = new List<Departamento>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM DEPARTAMENTO", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Departamento departamento = new Departamento();
                    departamento.ID_DEPARTAMENTO = Convert.ToInt32(reader["ID_DEPARTAMENTO"]);
                    departamento.DECRIPCION_DEPARTAMENTO = reader["DECRIPCION_DEPARTAMENTO"].ToString();
                    odepartamento.Add(departamento);
                }
            }
            ViewBag.Departamento = new SelectList(odepartamento, "ID_DEPARTAMENTO", "DECRIPCION_DEPARTAMENTO");

            //LISTA ROL
            List<Rol> orol = new List<Rol>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ROL", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Rol rol = new Rol();
                    rol.ID_ROL = Convert.ToInt32(reader["ID_ROL"]);
                    rol.DECRIPCION_ROL = reader["DECRIPCION_ROL"].ToString();
                    orol.Add(rol);
                }
            }
            ViewBag.Rol = new SelectList(orol, "ID_ROL", "DECRIPCION_ROL");

            //LISTA ESTADO EMPELADO

            List<Estado> oestado = new List<Estado>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ESTADO_EMPLEADO", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Estado estado = new Estado();
                    estado.ID_ESTADO_EMPLEADO = Convert.ToInt32(reader["ID_ESTADO_EMPLEADO"]);
                    estado.DESCRIPCION_ESTADO_EMPLEADO = reader["DESCRIPCION_ESTADO_EMPLEADO"].ToString();
                    oestado.Add(estado);
                }
            }
            ViewBag.Estado = new SelectList(oestado, "ID_ESTADO_EMPLEADO", "DESCRIPCION_ESTADO_EMPLEADO");

            return View();
        }

        [HttpPost]
        public ActionResult Registraremple(Empleado oempleado)
        {

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_EMPLEADO", oconexion);

                cmd.Parameters.AddWithValue("DNI", oempleado.DNI);
                cmd.Parameters.AddWithValue("PRIMER_NOMBRE", oempleado.PRIMER_NOMBRE);
                cmd.Parameters.AddWithValue("SEGUNDO_NOMBRE", oempleado.SEGUNDO_NOMBRE);
                cmd.Parameters.AddWithValue("PRIMER_APELLIDO", oempleado.PRIMER_APELLIDO);
                cmd.Parameters.AddWithValue("SEGUNDO_APELLIDO", oempleado.SEGUNDO_APELLIDO);
                cmd.Parameters.AddWithValue("ID_GENERO", oempleado.ID_GENERO);
                cmd.Parameters.AddWithValue("ID_SUCURSAL", oempleado.ID_SUCURSAL);
                cmd.Parameters.AddWithValue("ID_DEPARTAMENTO", oempleado.ID_DEPARTAMENTO);
                cmd.Parameters.AddWithValue("ID_ROL", oempleado.ID_ROL);
                cmd.Parameters.AddWithValue("ID_ESTADO_EMPLEADO", oempleado.ID_ESTADO_EMPLEADO);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("empleado", "Empleado");
        }

        // ELIMINAR EMPLEADO
        [HttpGet]
        public ActionResult Eliminaremple(int? Idempleado)
        {
            Empleado aempleado = oempleado.Where(c => c.ID_EMPLEADO == Idempleado).FirstOrDefault();
            return View(aempleado);
        }

        [HttpPost]
        public ActionResult Eliminaremple(string Idempleado)
        {
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_EMPLEADO", oconexion);
                cmd.Parameters.AddWithValue("ID_EMPLEADO", Idempleado);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("empleado", "Empleado");
        }

        //ACTUALIZAR EMPLEADO

        [HttpGet]
        public ActionResult Editaremple(int? Idempleado)
        {
            //LISTA GENERO
            List<Genero> ogenero = new List<Genero>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM GENERO", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Genero genero = new Genero();
                    genero.ID_GENERO = Convert.ToInt32(reader["ID_GENERO"]);
                    genero.DESCRIPCION_GENERO = reader["DESCRIPCION_GENERO"].ToString();
                    ogenero.Add(genero);
                }
            }
            ViewBag.Genero = new SelectList(ogenero, "ID_GENERO", "DESCRIPCION_GENERO");

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

            //LISTA DEPARTAMENTO
            List<Departamento> odepartamento = new List<Departamento>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM DEPARTAMENTO", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Departamento departamento = new Departamento();
                    departamento.ID_DEPARTAMENTO = Convert.ToInt32(reader["ID_DEPARTAMENTO"]);
                    departamento.DECRIPCION_DEPARTAMENTO = reader["DECRIPCION_DEPARTAMENTO"].ToString();
                    odepartamento.Add(departamento);
                }
            }
            ViewBag.Departamento = new SelectList(odepartamento, "ID_DEPARTAMENTO", "DECRIPCION_DEPARTAMENTO");

            //LISTA ROL
            List<Rol> orol = new List<Rol>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ROL", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Rol rol = new Rol();
                    rol.ID_ROL = Convert.ToInt32(reader["ID_ROL"]);
                    rol.DECRIPCION_ROL = reader["DECRIPCION_ROL"].ToString();
                    orol.Add(rol);
                }
            }
            ViewBag.Rol = new SelectList(orol, "ID_ROL", "DECRIPCION_ROL");

            //LISTA ESTADO EMPELADO

            List<Estado> oestado = new List<Estado>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ESTADO_EMPLEADO", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Estado estado = new Estado();
                    estado.ID_ESTADO_EMPLEADO = Convert.ToInt32(reader["ID_ESTADO_EMPLEADO"]);
                    estado.DESCRIPCION_ESTADO_EMPLEADO = reader["DESCRIPCION_ESTADO_EMPLEADO"].ToString();
                    oestado.Add(estado);
                }
            }
            ViewBag.Estado = new SelectList(oestado, "ID_ESTADO_EMPLEADO", "DESCRIPCION_ESTADO_EMPLEADO");

            Empleado empleado = oempleado.Where(c => c.ID_EMPLEADO == Idempleado).FirstOrDefault();

            return View(empleado);
        }

        [HttpPost]
        public ActionResult Editaremple(Empleado oempleado)
        {
            bool registrado;
            string mensaje;

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_EMPLEADO", oconexion);

                cmd.Parameters.AddWithValue("ID_EMPLEADO", oempleado.ID_EMPLEADO);
                cmd.Parameters.AddWithValue("DNI", oempleado.DNI);
                cmd.Parameters.AddWithValue("PRIMER_NOMBRE", oempleado.PRIMER_NOMBRE);
                cmd.Parameters.AddWithValue("SEGUNDO_NOMBRE", oempleado.SEGUNDO_NOMBRE);
                cmd.Parameters.AddWithValue("PRIMER_APELLIDO", oempleado.PRIMER_APELLIDO);
                cmd.Parameters.AddWithValue("SEGUNDO_APELLIDO", oempleado.SEGUNDO_APELLIDO);
                cmd.Parameters.AddWithValue("ID_GENERO", oempleado.ID_GENERO);
                cmd.Parameters.AddWithValue("ID_SUCURSAL", oempleado.ID_SUCURSAL);
                cmd.Parameters.AddWithValue("ID_DEPARTAMENTO", oempleado.ID_DEPARTAMENTO);
                cmd.Parameters.AddWithValue("ID_ROL", oempleado.ID_ROL);
                cmd.Parameters.AddWithValue("ID_ESTADO_EMPLEADO", oempleado.ID_ESTADO_EMPLEADO);
                cmd.Parameters.Add("REGISTRADOEMPLEADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MENSAJEEMPLEADO", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["REGISTRADOEMPLEADO"].Value);
                mensaje = cmd.Parameters["MENSAJEEMPLEADO"].Value.ToString();
            }
            ViewData["MENSAJEEMPLEADO"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("empleado", "Empleado");
            }
            else
            {
                return View();
            }

        }
    }
}