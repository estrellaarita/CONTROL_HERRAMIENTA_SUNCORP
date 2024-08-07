using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Departamento
    {
        public int ID_DEPARTAMENTO { get; set; }
        public string DECRIPCION_DEPARTAMENTO { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }
}