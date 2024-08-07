using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;
using System.IO;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    [ValidarSesion]
    public class DescargasController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Descargas> odescargas = new List<Descargas>();

        // GET: Descargas
        public ActionResult descargas()
        {
            odescargas = new List<Descargas>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT " +
                   "d.ID_DESECHOS, " +
                   "u.USUARIO, " +
                   "d.FECHA_DESECHO, " +
                   "r.NUMERO_SERIE, " +
                   "d.COMENTARIO, " +
                   "d.FOTO, " +
                   "d.FECHA_REGISTRO " +
                   "FROM DESECHOS d " +
                   "INNER JOIN USUARIO_BD u ON d.ID_USUARIO_BD = u.ID_USUARIO_BD " +
                   "INNER JOIN REGISTRO_SERIE_HERRAMIENTA r ON d.ID_REGISTRO_SERIE_HERRAMIENTA = r.ID_REGISTRO_SERIE_HERRAMIENTA", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Descargas descargas = new Descargas();

                        descargas.ID_DESECHOS = Convert.ToInt32(dr["ID_DESECHOS"]);
                        // descargas.ID_USUARIO_BD = Convert.ToInt32(dr["ID_USUARIO_BD"]);
                        descargas.FECHA_DESECHO = Convert.ToDateTime(dr["FECHA_DESECHO"]).Date;
                        // descargas.ID_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(dr["ID_REGISTRO_SERIE_HERRAMIENTA"]);
                        descargas.COMENTARIO = dr["COMENTARIO"].ToString();
                        descargas.FOTO = dr["FOTO"] != DBNull.Value ? (byte[])dr["FOTO"] : null;
                        descargas.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //USUARIO BD
                        usuario eusuario = new usuario();

                       // eusuario.ID_USUARIO_BD = Convert.ToInt32(dr["ID_USUARIO_BD"]);
                        eusuario.USUARIO = dr["USUARIO"].ToString();
                        //eusuario.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        descargas.USUARIO_BD = eusuario;

                        //SERIE
                        Serie aserie = new Serie();

                       // aserie.ID_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(dr["ID_REGISTRO_SERIE_HERRAMIENTA"]);
                        aserie.NUMERO_SERIE = dr["NUMERO_SERIE"].ToString();
                       // aserie.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        descargas.SERIE = aserie;

                        odescargas.Add(descargas);

                    }
                }
            }
            return View(odescargas);
        }

        //CREATE DESCARGA

        [HttpGet]
        public ActionResult Registrardescarga()
        {
            //LISTA DE USUARIO BD
            List<usuario> pusuario= new List<usuario>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM USUARIO_BD", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuario rusuario = new usuario();
                    rusuario.ID_USUARIO_BD = Convert.ToInt32(reader["ID_USUARIO_BD"]);
                    rusuario.USUARIO = reader["USUARIO"].ToString();
                    pusuario.Add(rusuario);
                }
            }
            ViewBag.tUsuario = new SelectList(pusuario, "ID_USUARIO_BD", "USUARIO");

            //LISTA DE SERIE
            List<Serie> eserie = new List<Serie>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM REGISTRO_SERIE_HERRAMIENTA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Serie xserie = new Serie();
                    xserie.ID_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(reader["ID_REGISTRO_SERIE_HERRAMIENTA"]);
                    xserie.NUMERO_SERIE = reader["NUMERO_SERIE"].ToString();
                    eserie.Add(xserie);
                }
            }
            ViewBag.zSerie = new SelectList(eserie, "ID_REGISTRO_SERIE_HERRAMIENTA", "NUMERO_SERIE");

            return View();
        }

        [HttpPost]
        public ActionResult Registrardescarga(Descargas odescarga, HttpPostedFileBase upload)
        {

            if (upload != null && upload.ContentLength > 0)
            {
                byte[] imagenData = null;
                using (var imagen = new BinaryReader(upload.InputStream))
                {
                    imagenData = imagen.ReadBytes(upload.ContentLength);
                }
                odescarga.FOTO = imagenData;
            }
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_DESECHOS", oconexion);

                cmd.Parameters.AddWithValue("ID_USUARIO_BD", odescarga.ID_USUARIO_BD);
                cmd.Parameters.AddWithValue("FECHA_DESECHO", odescarga.FECHA_DESECHO);
                cmd.Parameters.AddWithValue("ID_REGISTRO_SERIE_HERRAMIENTA", odescarga.ID_REGISTRO_SERIE_HERRAMIENTA);
                cmd.Parameters.AddWithValue("COMENTARIO", odescarga.COMENTARIO);
                cmd.Parameters.AddWithValue("FOTO", odescarga.FOTO);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("descargas", "Descargas");
        }

        public ActionResult ConvertirImagen(int codigo)
        {

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                // Abrir la conexión a la base de datos
                oconexion.Open();

                // Preparar el comando SQL para seleccionar la imagen basada en el ID
                SqlCommand cmd = new SqlCommand("SELECT FOTO FROM DESECHOS WHERE ID_DESECHOS = @codigo", oconexion);
                cmd.Parameters.AddWithValue("@codigo", codigo);

                // Ejecutar el comando y leer los resultados
                SqlDataReader reader = cmd.ExecuteReader();

                // Verificar si se encontró un registro
                if (reader.Read())
                {
                    // Obtener los datos de la imagen como un array de bytes
                    byte[] imagenData = reader["FOTO"] as byte[];

                    // Verificar si los datos de la imagen son válidos
                    if (imagenData != null && imagenData.Length > 0)
                    {
                        // Retornar el archivo de imagen con el tipo MIME adecuado
                        return File(imagenData, "Images/jpg");
                    }
                    else
                    {
                        // Si no hay datos de imagen válidos, retornar NotFound
                        return HttpNotFound("Imagen no encontrada o vacía.");
                    }
                }
                else
                {
                    // Si no se encuentra el registro, retornar NotFound
                    return HttpNotFound("Imagen no encontrada.");
                }
            }
        }

        //ELIMINAR DESCARGAS
        [HttpGet]
        public ActionResult Eliminardescargas(int? Iddescargas)
        {
            Descargas adescarga = odescargas.Where(c => c.ID_DESECHOS == Iddescargas).FirstOrDefault();
            return View(adescarga);
        }

        [HttpPost]
        public ActionResult Eliminardescargas(string Iddescargas)
        {
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_DESECHOS", oconexion);
                cmd.Parameters.AddWithValue("ID_DESECHOS", Iddescargas);
                
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("descargas", "Descargas");
        }

        //ACTUALIZAR DESECHOS
        [HttpGet]
        public ActionResult Editardescargas(int? Iddescargas)
        {


            List<usuario> pusuario = new List<usuario>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM USUARIO_BD", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuario rusuario = new usuario();
                    rusuario.ID_USUARIO_BD = Convert.ToInt32(reader["ID_USUARIO_BD"]);
                    rusuario.USUARIO = reader["USUARIO"].ToString();
                    pusuario.Add(rusuario);
                }
            }
            ViewBag.tUsuario = new SelectList(pusuario, "ID_USUARIO_BD", "USUARIO");


            List<Serie> eserie = new List<Serie>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM REGISTRO_SERIE_HERRAMIENTA", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Serie xserie = new Serie();
                    xserie.ID_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(reader["ID_REGISTRO_SERIE_HERRAMIENTA"]);
                    xserie.NUMERO_SERIE = reader["NUMERO_SERIE"].ToString();
                    eserie.Add(xserie);
                }
            }
            ViewBag.zSerie = new SelectList(eserie, "ID_REGISTRO_SERIE_HERRAMIENTA", "NUMERO_SERIE");

            Descargas adescargas = odescargas.Where(c => c.ID_DESECHOS == Iddescargas).FirstOrDefault();
            return View(adescargas);
        }

        //EDITAR DESCARGAS

        [HttpPost]
        public ActionResult Editardescargas(Descargas odescargas, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    byte[] imagenData = null;
                    using (var imagen = new BinaryReader(upload.InputStream))
                    {
                        imagenData = imagen.ReadBytes(upload.ContentLength);
                    }
                    odescargas.FOTO = imagenData;
                }
                else
                {
                    
                    using (SqlConnection oconexion = new SqlConnection(cadena))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT FOTO FROM DESECHOS WHERE ID_DESECHOS = @ID_DESECHOS", oconexion);
                        cmd.Parameters.AddWithValue("ID_DESECHOS", odescargas.ID_DESECHOS);
                        oconexion.Open();
                        byte[] imagenData = (byte[])cmd.ExecuteScalar();
                        odescargas.FOTO = imagenData;
                    }
                }

                using (SqlConnection oconexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_DESECHOS", oconexion);

                    cmd.Parameters.AddWithValue("ID_DESECHOS", odescargas.ID_DESECHOS);
                    cmd.Parameters.AddWithValue("ID_USUARIO_BD", odescargas.ID_USUARIO_BD);
                    cmd.Parameters.AddWithValue("FECHA_DESECHO", odescargas.FECHA_DESECHO);
                    cmd.Parameters.AddWithValue("ID_REGISTRO_SERIE_HERRAMIENTA", odescargas.ID_REGISTRO_SERIE_HERRAMIENTA);
                    cmd.Parameters.AddWithValue("COMENTARIO", odescargas.COMENTARIO);
                    cmd.Parameters.AddWithValue("FOTO", odescargas.FOTO);

                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("descargas", "Descargas");
        }

    }

}