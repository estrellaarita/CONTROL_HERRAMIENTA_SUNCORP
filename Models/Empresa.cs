using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Empresa
    {
        public int ID_EMPRESA { get; set; }
        public string RTN { get; set; }
        public string NOMBRE_EMPRESA { get; set; }
        public string CORREO_ELECTRONICO { get; set; }
        public int ID_CORPORACION { get; set; }
       public Corporacion CORPORACION { get; set; }
        public int CANTIDAD_DE_SUCURSALES { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }

    public class Corporacion
    {
        public int ID_CORPORACION { get; set; }
        public string NOMBRE_CORPORACION { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }

    }
}