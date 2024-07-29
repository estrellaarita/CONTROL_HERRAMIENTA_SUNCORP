using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Factura
    {
        public int ID_FACTURA { get; set; }
        public string NUMERO_FACTURA{ get; set; }
        public int ID_USUARIO_BD { get; set; }
        public System.DateTime FECHA_COMPRA { get; set; }
        public int ID_PROVEEDOR { get; set; }
        public string COMENTARIO { get; set; }
        public byte[] FOTO { get; set; }
        public usuario USUARIO_BD { get; set; }
        public Proveedor PROVEEDOR { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }
}