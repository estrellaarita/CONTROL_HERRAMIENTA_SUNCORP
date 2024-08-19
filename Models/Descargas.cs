using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Descargas
    {
        public int ID_DESECHOS { get; set; }
        public int ID_USUARIO_BD { get; set; }
        public DateTime FECHA_DESECHO { get; set; }
        public int ID_REGISTRO_SERIE_HERRAMIENTA{ get; set; }
        public string COMENTARIO { get; set; }
        public byte[] FOTO { get; set; }
        public usuario USUARIO_BD { get; set; }
        public Serie SERIE { get; set; }
        public string Tipoherramienta  { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }

    }
}