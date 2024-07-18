using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Proveedor
    {
        public int ID_PROVEEDOR { get; set; }
        public string RTN { get; set; }
        public string NOMBRE_PROVEEDOR { get; set; }
        public string CORREO_ELECTRONICO { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }
}