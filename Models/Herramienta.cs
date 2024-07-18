using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Herramienta
    {
        public int ID_HERRAMIENTA { get; set; }
        public int ID_TIPO_HERRAMIENTA { get; set; }
        public int ID_MARCA { get; set; }
        public string MODELO { get; set; }
        public string COMENTARIO { get; set; }
        public Tipoherramienta TIPO { get; set; }
        public Marca MARCA { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }
}