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
using System.Collections.ObjectModel;
using System.Collections;
using System.Text;

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
                SqlCommand cmd = new SqlCommand("SP_VERempleado", oconexion);
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
                         empleado.ID_ESTADO_EMPLEADO = Convert.ToInt32(dr["ID_ESTADO_EMPLEADO"]);
                        empleado.NUMERO_DE_HERRAMIENTAS = Convert.ToInt32(dr["NUMERO_DE_HERRAMIENTAS"]);
                        empleado.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //GENERO
                        Genero igenero = new Genero();

                        igenero.ID_GENERO = Convert.ToInt32(dr["ID_GENERO"]);
                        igenero.DESCRIPCION_GENERO = dr["DESCRIPCION_GENERO"].ToString();
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
                        Estado iestado = new Estado();

                        iestado.ID_ESTADO_EMPLEADO = Convert.ToInt32(dr["ID_ESTADO_EMPLEADO"]);
                        iestado.DESCRIPCION_ESTADO_EMPLEADO = dr["DESCRIPCION_ESTADO_EMPLEADO"].ToString();
                        iestado.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.ESTADO_EMPLEADO = iestado;

                        oempleado.Add(empleado);

                    }
                }
            }
            return View(oempleado);
        }


        [HttpGet]
        public JsonResult ListaEmpleados()
        {

            oempleado = new List<Empleado>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_VERempleado", oconexion);
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
                         empleado.ID_ESTADO_EMPLEADO = Convert.ToInt32(dr["ID_ESTADO_EMPLEADO"]);
                        empleado.NUMERO_DE_HERRAMIENTAS = Convert.ToInt32(dr["NUMERO_DE_HERRAMIENTAS"]);
                        empleado.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //GENERO
                        Genero igenero = new Genero();

                        igenero.ID_GENERO = Convert.ToInt32(dr["ID_GENERO"]);
                        igenero.DESCRIPCION_GENERO = dr["DESCRIPCION_GENERO"].ToString();
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
                        Estado iestado = new Estado();

                       iestado.ID_ESTADO_EMPLEADO = Convert.ToInt32(dr["ID_ESTADO_EMPLEADO"]);
                        iestado.DESCRIPCION_ESTADO_EMPLEADO = dr["DESCRIPCION_ESTADO_EMPLEADO"].ToString();
                        iestado.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        empleado.ESTADO_EMPLEADO = iestado;

                        oempleado.Add(empleado);

                    }
                }
            }
            return Json(new { data = oempleado }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSerieHerramientas(int idEmpleado)
        {
            List<Ver_herramientas> serieHerramientas = new List<Ver_herramientas>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                string query = @"
        SELECT 
            RSH.ID_REGISTRO_SERIE_HERRAMIENTA, 
            RSH.NUMERO_SERIE, 
            H.ID_HERRAMIENTA, 
            TPH.DECRIPCION_TIPO_HERRAMIENTA AS HERRAMIENTA, 
            M.DECRIPCION_MARCA AS MARCA, 
            H.MODELO  
        FROM 
            EMPLEADO_REGISTRO_SERIE_HERRAMIENTA EH
        INNER JOIN 
            REGISTRO_SERIE_HERRAMIENTA RSH ON RSH.ID_REGISTRO_SERIE_HERRAMIENTA = EH.ID_REGISTRO_SERIE_HERRAMIENTA
        INNER JOIN 
            EMPLEADO E ON EH.ID_EMPLEADO = E.ID_EMPLEADO
        INNER JOIN 
            HERRAMIENTA H ON RSH.ID_HERRAMIENTA = H.ID_HERRAMIENTA
        INNER JOIN 
            TIPO_HERRAMIENTA TPH ON TPH.ID_TIPO_HERRAMIENTA = H.ID_TIPO_HERRAMIENTA
        INNER JOIN 
            MARCA M ON M.ID_MARCA = H.ID_MARCA 
        WHERE 
            E.ID_EMPLEADO = @IdEmpleado";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);

                oconexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Ver_herramientas serieHerramienta = new Ver_herramientas
                    {
                        IdRegistroSerieHerramienta = Convert.ToInt32(reader["ID_REGISTRO_SERIE_HERRAMIENTA"]),
                        NumeroSerie = reader["NUMERO_SERIE"].ToString(),
                        IdHerramienta = Convert.ToInt32(reader["ID_HERRAMIENTA"]),
                        Herramienta = reader["HERRAMIENTA"].ToString(),
                        Marca = reader["MARCA"].ToString(),
                        Modelo = reader["MODELO"].ToString()
                    };

                    serieHerramientas.Add(serieHerramienta);
                }
            }

            return Json(serieHerramientas, JsonRequestBehavior.AllowGet);
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
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_EMPLEADO", oconexion);
                cmd.Parameters.AddWithValue("ID_EMPLEADO", Idempleado);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
                }
                // Si la eliminación es exitosa, redirigir a la vista deseada
                return RedirectToAction("empleado", "Empleado");
            }
            catch (SqlException ex)
            {
                // En caso de un conflicto, retornar un mensaje de error a la vista
                ViewBag.ErrorMessage = "No puede eliminar el empleado porque hay registros relacionados";
                // Aquí podrías registrar el error en un log si es necesario

                // Redirigir a la vista actual con el mensaje de error
                return View();  // Asegúrate de que "Marca" sea la vista correcta
            }
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

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();


            }
            return RedirectToAction("empleado", "Empleado");
        }

    }
}
