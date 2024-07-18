using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Serie
    {
        public int ID_REGISTRO_SERIE_HERRAMIENTA { get; set; }
        public int ID_SUCURSAL { get; set; }
        public int ID_HERRAMIENTA { get; set; }
        public string UBICACION_FISICA { get; set; }
        public string NUMERO_SERIE { get; set; }
        public string PRECIO { get; set; }
        public int ID_ESTADO_HERRAMIENTA { get; set; }
        public int NUMERO_FACTURA { get; set; }
        public string COMENTARIO { get; set; }
        public Sucursal SUCURSAL { get; set; }
        public Herramienta HERRAMIENTA { get; set; }
        public Estado_herramienta ESTADO_HERRAMIENTA { get; set; }
        public Factura NUMERO_FACTURAS { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }

    public class Estado_herramienta
    {
        public int ID_ESTADO_HERRAMIENTA { get; set; }
        public string DECRIPCION_ESTADO_HERRAMIENTA { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }

    }

}