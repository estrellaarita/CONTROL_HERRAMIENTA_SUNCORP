using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Sucursal
    {
        public int ID_SUCURSAL { get; set; }
        public string NOMBRE_SUCURSAL { get; set; }
        public string CORREO_ELECTRONICO { get; set; }
        public string DIRECCION { get; set; }
        public int ID_EMPRESA { get; set; }
        public string COMENTARIO { get; set; }
        public empresa EMPRESA { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }


    public class empresa
    {
        public int ID_EMPRESA { get; set; }
        public string RTN { get; set; }
        public string NOMBRE_EMPRESA { get; set; }
        public string CORREO_ELECTRONICO { get; set; }
        public int ID_CORPORACION { get; set; }
        public Corporacion CORPORACION { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }
}
