using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Tipoherramienta
    {
        public int ID_TIPO_HERRAMIENTA { get; set; }
        public string DECRIPCION_TIPO_HERRAMIENTA { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }
}