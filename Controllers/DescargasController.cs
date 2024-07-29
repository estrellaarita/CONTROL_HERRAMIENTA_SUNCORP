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
    public class DescargasController : Controller
    {
        static string cadena = "Data Source=DESKTOP-22LJCAJ;Initial Catalog=BD_CONTROL_INVENTARIO_HERRAMIENTAS_SUNCORP;Integrated Security=True";

        private static List<Descargas> odescargas = new List<Descargas>();

        // GET: Descargas
        public ActionResult Descargas()
        {
            odescargas = new List<Descargas>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT f.*, u.*, p.* FROM FACTURA f INNER JOIN USUARIO_BD u ON f.ID_USUARIO_BD = u.ID_USUARIO_BD INNER JOIN PROVEEDOR p ON f.ID_PROVEEDOR = p.ID_PROVEEDOR", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Descargas descargas = new Descargas();

                        descargas.ID_DESECHOS = Convert.ToInt32(dr["ID_DESECHOS"]);
                        descargas.ID_USUARIO_BD = Convert.ToInt32(dr["ID_USUARIO_BD"]);
                        descargas.FECHA_DESECHO = Convert.ToDateTime(dr["FECHA_DESECHO"]);
                        descargas.ID_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(dr["ID_REGISTRO_SERIE_HERRAMIENTA"]);
                        descargas.COMENTARIO = dr["COMENTARIO"].ToString();
                        //descargas.FOTO = (binary[])dr["FOTO"];

                        //USUARIO BD
                        usuario eusuario = new usuario();

                        eusuario.ID_USUARIO_BD = Convert.ToInt32(dr["ID_USUARIO_BD"]);
                        eusuario.USUARIO = dr["USUARIO"].ToString();
                        eusuario.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        descargas.USUARIO_BD = eusuario;

                        //SERIE
                        Serie aserie = new Serie();

                        aserie.ID_REGISTRO_SERIE_HERRAMIENTA = Convert.ToInt32(dr["ID_REGISTRO_SERIE_HERRAMIENTA"]);
                        aserie.NUMERO_SERIE = dr["NUMERO_SERIE"].ToString();
                        aserie.FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]);

                        descargas.SERIE = aserie;

                        odescargas.Add(descargas);

                    }
                }
            }
            return View();
        }
    }

    /*public ActionResult convertirImagen(int codigo)
    {
        using (var context = new ProyectoaDbContext())

        {
            var imgen = (from articulo in context.Articulo
                         where articulo.ID_DESECHOS == codigo
                         select articulo.IMAGEN1).FirstOrDefault();
            return File(imagen, "Imagenes/jpg");


    }*/
    
}