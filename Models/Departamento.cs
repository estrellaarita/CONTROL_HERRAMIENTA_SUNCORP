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

       // [MaxLength(80, ErrorMessage = "La descripción del departamento debe tener un máximo de 50 caracteres.")]
        public string DECRIPCION_DEPARTAMENTO { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }
}