using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Ver_empleado
    {
        public int IdRegistroSerieHerramienta { get; set; }
        public int IdEmpleado { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Sucursal { get; set; }
        public string Departamento { get; set; }
        public string Rol { get; set; }
    }
}