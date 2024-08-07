using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Serieempleado
    {
        public int ID_EMPLEADO_REGISTRO_SERIE_HERRAMIENTA { get; set; }
        public int ID_EMPLEADO { get; set; }
        public int ID_REGISTRO_SERIE_HERRAMIENTA { get; set; }
        public Empleado EMPLEADO { get; set; }
        public Serie SERIE { get; set; }
        public Herramienta HERRAMIENTA{ get; set; }
        public Sucursal SUCURSAL { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }
}