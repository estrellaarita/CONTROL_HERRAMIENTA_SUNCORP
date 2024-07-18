using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Empleado
    {
        public int ID_EMPLEADO { get; set; }
        public string DNI { get; set; }
        public string PRIMER_NOMBRE { get; set; }
        public string SEGUNDO_NOMBRE { get; set; }
        public string PRIMER_APELLIDO { get; set; }
        public string SEGUNDO_APELLIDO { get; set; }
        public int ID_GENERO { get; set; }
        public int ID_SUCURSAL { get; set; }
        public int ID_DEPARTAMENTO { get; set; }
        public int ID_ROL { get; set; }
        public int ID_ESTADO_EMPLEADO { get; set; }
        public Genero GENERO { get; set; }
        public Sucursal SUCURSAL { get; set; }
        public Departamento DEPARTAMENTO { get; set; }
        public Rol ROL { get; set; }
        public Estado ESTADO_EMPLEADO { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }

    public class Genero
    {
        public int ID_GENERO { get; set; }
        public string DESCRIPCION_GENERO { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }

    }

    public class Estado
    {
        public int ID_ESTADO_EMPLEADO { get; set; }
        public string DESCRIPCION_ESTADO_EMPLEADO { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }

    }
}