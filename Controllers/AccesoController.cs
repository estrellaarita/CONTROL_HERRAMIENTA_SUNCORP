using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System.Web.Services.Description;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{

  
    public class AccesoController : Controller
    {
        // PARAMETROS PARA LA CONEXION DE LA BD
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        //GET : ACESSO
        public ActionResult login()
        {
            return View();
        }

        public ActionResult registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registrar(usuario ousuario)
        {
            bool registrado;
            string mensaje;

            if(ousuario.CLAVE == ousuario.confirmarclave){

                ousuario.CLAVE = ConvertirSha256(ousuario.CLAVE);
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("SP_REGISTRARUSUARIO", cn);
                cmd.Parameters.AddWithValue("NOMBRE_COMPLETO", ousuario.NOMBRE_COMPLETO);
                cmd.Parameters.AddWithValue("USUARIO", ousuario.USUARIO);
                cmd.Parameters.AddWithValue("CLAVE", ousuario.CLAVE);
                cmd.Parameters.Add("REGISTRADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["REGISTRADO"].Value);
                mensaje = cmd.Parameters["MENSAJE"].Value.ToString();
            }


                ViewData["MENSAJE"] = mensaje;

                if (registrado)
                {
                    return RedirectToAction("login", "Acceso");
                }
                else
                {
                    return View();
                }

            }

        [HttpPost]
        public ActionResult login(usuario ousuario)
        {
            ousuario.CLAVE = ConvertirSha256(ousuario.CLAVE);

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("SP_VALIDARUSUAIRO", cn);
                cmd.Parameters.AddWithValue("USUARIO", ousuario.USUARIO);
                cmd.Parameters.AddWithValue("CLAVE", ousuario.CLAVE);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                ousuario.ID_USUARIO_BD = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            }

            if (ousuario.ID_USUARIO_BD != 0)
            {

                Session["USUARIO"] = ousuario;
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario no encontrado";
                return View();
            }


        }


        public static string ConvertirSha256(string texto)
        {
            //using System.Text;
            //USAR LA REFERENCIA DE "System.Security.Cryptography"

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}