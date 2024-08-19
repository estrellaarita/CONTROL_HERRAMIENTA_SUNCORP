using CONTROL_HERRAMIENTA_SUNCORP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.IO;
using CONTROL_HERRAMIENTA_SUNCORP.Permisos;

namespace CONTROL_HERRAMIENTA_SUNCORP.Controllers
{
    [ValidarSesion]

    public class FacturaController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Factura> ofactura = new List<Factura>();
  
        // GET: Factura
        public ActionResult FACTURA()
        {

            ofactura = new List<Factura>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT " +
          "f.ID_FACTURA, " +
          "f.NUMERO_FACTURA, " +
          "f.ID_USUARIO_BD, " +
          "u.USUARIO, " +
          "f.FECHA_COMPRA, " +
          "f.ID_PROVEEDOR, " +
          "p.NOMBRE_PROVEEDOR, " +
          "f.COMENTARIO, " +
          "f.FOTO, " +
          "f.MONTO_TOTAL, " +
          "f.FECHA_REGISTRO " +
          "FROM FACTURA f " +
          "INNER JOIN USUARIO_BD u ON f.ID_USUARIO_BD = u.ID_USUARIO_BD " +
          "INNER JOIN PROVEEDOR p ON f.ID_PROVEEDOR = p.ID_PROVEEDOR ", oconexion);

                cmd.CommandType = CommandType.Text;

                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Factura factura = new Factura();

                        factura.ID_FACTURA = Convert.ToInt32(dr["ID_FACTURA"]);
                        factura.NUMERO_FACTURA = dr["NUMERO_FACTURA"].ToString();
                        factura.ID_USUARIO_BD = Convert.ToInt32(dr["ID_USUARIO_BD"]);
                        factura.FECHA_COMPRA = Convert.ToDateTime(dr["FECHA_COMPRA"]);
                        factura.ID_PROVEEDOR = Convert.ToInt32(dr["ID_PROVEEDOR"]);
                        factura.MONTO_TOTAL = dr["MONTO_TOTAL"].ToString();
                        factura.COMENTARIO = dr["COMENTARIO"].ToString();

                        factura.FOTO = dr["FOTO"] != DBNull.Value ? (byte[])dr["FOTO"] : null;

                        factura.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //USUARIO BD
                        usuario eusuario = new usuario();

                        eusuario.ID_USUARIO_BD = Convert.ToInt32(dr["ID_USUARIO_BD"]);
                        eusuario.USUARIO = dr["USUARIO"].ToString();
                        eusuario.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        factura.USUARIO_BD = eusuario;

                        //PROVEEDOR
                       Proveedor aproveedor = new Proveedor();

                        aproveedor.ID_PROVEEDOR = Convert.ToInt32(dr["ID_PROVEEDOR"]);
                        aproveedor.NOMBRE_PROVEEDOR = dr["NOMBRE_PROVEEDOR"].ToString();
                        aproveedor.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        factura.PROVEEDOR = aproveedor;

                        ofactura.Add(factura);

                    }
                }
            }
            return View(ofactura);
        }

        [HttpGet]
        public JsonResult ListaFactura()
        {

            ofactura = new List<Factura>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT " +
        "f.ID_FACTURA, " +
        "f.NUMERO_FACTURA, " +
        "f.ID_USUARIO_BD, " +
        "u.USUARIO, " +
        "f.FECHA_COMPRA, " +
        "f.ID_PROVEEDOR, " +
        "p.NOMBRE_PROVEEDOR, " +
        "f.COMENTARIO, " +
        "f.FOTO, " +
         "f.MONTO_TOTAL,  " +
        "f.FECHA_REGISTRO " +
        "FROM FACTURA f " +
        "INNER JOIN USUARIO_BD u ON f.ID_USUARIO_BD = u.ID_USUARIO_BD " +
        "INNER JOIN PROVEEDOR p ON f.ID_PROVEEDOR = p.ID_PROVEEDOR ", oconexion);

                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Factura factura = new Factura();

                       factura.ID_FACTURA = Convert.ToInt32(dr["ID_FACTURA"]);
                        factura.NUMERO_FACTURA = dr["NUMERO_FACTURA"].ToString();
                        factura.ID_USUARIO_BD = Convert.ToInt32(dr["ID_USUARIO_BD"]);
                        factura.MONTO_TOTAL = dr["MONTO_TOTAL"].ToString();
                        factura.FECHA_COMPRA = Convert.ToDateTime(dr["FECHA_COMPRA"]);
                        factura.ID_PROVEEDOR = Convert.ToInt32(dr["ID_PROVEEDOR"]);
                        factura.COMENTARIO = dr["COMENTARIO"].ToString();

                        factura.FOTO = dr["FOTO"] != DBNull.Value ? (byte[])dr["FOTO"] : null;

                        factura.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        //USUARIO BD
                        usuario eusuario = new usuario();

                       eusuario.ID_USUARIO_BD = Convert.ToInt32(dr["ID_USUARIO_BD"]);
                        eusuario.USUARIO = dr["USUARIO"].ToString();
                        eusuario.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        factura.USUARIO_BD = eusuario;

                        //PROVEEDOR
                        Proveedor aproveedor = new Proveedor();

                       aproveedor.ID_PROVEEDOR = Convert.ToInt32(dr["ID_PROVEEDOR"]);
                        aproveedor.NOMBRE_PROVEEDOR = dr["NOMBRE_PROVEEDOR"].ToString();
                       aproveedor.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        factura.PROVEEDOR = aproveedor;

                        ofactura.Add(factura);

                    }
                }
            }
            return Json(new { data = ofactura }, JsonRequestBehavior.AllowGet);
        }

        //CREATE FACTURA

        [HttpGet]
        public ActionResult Registrarfactura()
        {
            //LISTA DE USUARIO BD

            List<usuario> ousuario = new List<usuario>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM USUARIO_BD", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuario eusuario = new usuario();
                    eusuario.ID_USUARIO_BD = Convert.ToInt32(reader["ID_USUARIO_BD"]);
                    eusuario.USUARIO = reader["USUARIO"].ToString();
                    ousuario.Add(eusuario);
                }
            }
            ViewBag.aUsuario = new SelectList(ousuario, "ID_USUARIO_BD", "USUARIO");

            //LISTA DE PROVEEDOR

            List<Proveedor> oproveedor = new List<Proveedor>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM PROVEEDOR", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Proveedor eproveedor = new Proveedor();
                    eproveedor.ID_PROVEEDOR = Convert.ToInt32(reader["ID_PROVEEDOR"]);
                    eproveedor.NOMBRE_PROVEEDOR = reader["NOMBRE_PROVEEDOR"].ToString();
                    oproveedor.Add(eproveedor);
                }
            }
            ViewBag.iProveedor = new SelectList(oproveedor, "ID_PROVEEDOR", "NOMBRE_PROVEEDOR");

            return View();
        }

        [HttpPost]
        public ActionResult Registrarfactura(Factura ofactura, HttpPostedFileBase upload)
        {

            if (upload != null && upload.ContentLength > 0)
            {
                byte[] imagenData = null;
                using (var imagen = new BinaryReader(upload.InputStream))
                {
                    imagenData = imagen.ReadBytes(upload.ContentLength);
                }
                ofactura.FOTO = imagenData;
            }
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_CREATE_FACTURA", oconexion);

                cmd.Parameters.AddWithValue("NUMERO_FACTURA", ofactura.NUMERO_FACTURA);
                cmd.Parameters.AddWithValue("ID_USUARIO_BD", ofactura.ID_USUARIO_BD);
                cmd.Parameters.AddWithValue("FECHA_COMPRA", ofactura.FECHA_COMPRA);
                cmd.Parameters.AddWithValue("ID_PROVEEDOR", ofactura.ID_PROVEEDOR);
                cmd.Parameters.AddWithValue("COMENTARIO", ofactura.COMENTARIO);
                cmd.Parameters.AddWithValue("FOTO",ofactura.FOTO);
                cmd.Parameters.AddWithValue("MONTO_TOTAL", ofactura.MONTO_TOTAL);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("FACTURA", "Factura");
        }

        public ActionResult ConvertirImagen(int codigo)
        {
   
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                // Abrir la conexión a la base de datos
                oconexion.Open();

                // Preparar el comando SQL para seleccionar la imagen basada en el ID
                SqlCommand cmd = new SqlCommand("SELECT FOTO FROM FACTURA WHERE ID_FACTURA = @codigo", oconexion);
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

        //ELIMINAR FACTURA

        [HttpGet]
        public ActionResult Eliminarfactura(int? Idfactura)
        {
            Factura afactura = ofactura.Where(c => c.ID_FACTURA == Idfactura).FirstOrDefault();
            return View(afactura);
        }

        [HttpPost]
        public ActionResult Eliminarfactura(string Idfactura)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_FACTURA", oconexion);
                cmd.Parameters.AddWithValue("ID_FACTURA", Idfactura);
                
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
                }
                // Si la eliminación es exitosa, redirigir a la vista deseada
                return RedirectToAction("FACTURA", "Factura");
            }
            catch (SqlException ex)
            {
                // En caso de un conflicto, retornar un mensaje de error a la vista
                ViewBag.ErrorMessage = "No puede eliminar la factura porque hay registros relacionados";
                // Aquí podrías registrar el error en un log si es necesario

                // Redirigir a la vista actual con el mensaje de error
                return View();  // Asegúrate de que "Marca" sea la vista correcta
            }
        }


        //EDITAR FACTURA

        [HttpGet]
        public ActionResult Editarfactura(int? Idfactura)
        {

            List<usuario> ousuario = new List<usuario>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM USUARIO_BD", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuario eusuario = new usuario();
                    eusuario.ID_USUARIO_BD = Convert.ToInt32(reader["ID_USUARIO_BD"]);
                    eusuario.USUARIO = reader["USUARIO"].ToString();
                    ousuario.Add(eusuario);
                }
            }
            ViewBag.aUsuario = new SelectList(ousuario, "ID_USUARIO_BD", "USUARIO");


            List<Proveedor> oproveedor = new List<Proveedor>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM PROVEEDOR", oconexion);
                oconexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Proveedor eproveedor = new Proveedor();
                    eproveedor.ID_PROVEEDOR = Convert.ToInt32(reader["ID_PROVEEDOR"]);
                    eproveedor.NOMBRE_PROVEEDOR = reader["NOMBRE_PROVEEDOR"].ToString();
                    oproveedor.Add(eproveedor);
                }
            }
            ViewBag.iProveedor = new SelectList(oproveedor, "ID_PROVEEDOR", "NOMBRE_PROVEEDOR");

            Factura afactura = ofactura.Where(c => c.ID_FACTURA == Idfactura).FirstOrDefault();
            return View(afactura);
        }


        [HttpPost]
        public ActionResult Editarfactura(Factura ofactura, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verifica si se ha cargado una nueva imagen
                    if (upload != null && upload.ContentLength > 0)
                    {
                        using (var imagen = new BinaryReader(upload.InputStream))
                        {
                            ofactura.FOTO = imagen.ReadBytes(upload.ContentLength);
                        }
                    }
                    else
                    {
                        // Si no se ha seleccionado una imagen nueva, obtener la imagen actual de la base de datos
                        using (SqlConnection oconexion = new SqlConnection(cadena))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT FOTO FROM FACTURA WHERE ID_FACTURA = @ID_FACTURA", oconexion);
                        cmd.Parameters.AddWithValue("ID_FACTURA", ofactura.ID_FACTURA);
                        oconexion.Open();
                        byte[] imagenData = (byte[])cmd.ExecuteScalar();
                        ofactura.FOTO = imagenData;
                    }
                }

                    //ACtualiza los datos en la BD
                using (SqlConnection oconexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_FACTURA", oconexion);

                    cmd.Parameters.AddWithValue("ID_FACTURA", ofactura.ID_FACTURA);
                    cmd.Parameters.AddWithValue("NUMERO_FACTURA", ofactura.NUMERO_FACTURA);
                    cmd.Parameters.AddWithValue("ID_USUARIO_BD", ofactura.ID_USUARIO_BD);
                    cmd.Parameters.AddWithValue("FECHA_COMPRA", ofactura.FECHA_COMPRA);
                    cmd.Parameters.AddWithValue("ID_PROVEEDOR", ofactura.ID_PROVEEDOR);
                    cmd.Parameters.AddWithValue("COMENTARIO", ofactura.COMENTARIO);
                    cmd.Parameters.AddWithValue("FOTO", ofactura.FOTO);
                    cmd.Parameters.AddWithValue("MONTO_TOTAL", ofactura.MONTO_TOTAL);

                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    }
                }
                catch (Exception ex)
                {
                    // Maneja la excepción (por ejemplo, loguea el error o muestra un mensaje al usuario)
                    ModelState.AddModelError("", "Se produjo un error al actualizar los datos: " + ex.Message);
                }
            }

            return RedirectToAction("FACTURA", "Factura");
        }

    }
}