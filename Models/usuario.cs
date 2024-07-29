using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class usuario
    {
        public int ID_USUARIO_BD { get; set; }

        public string NOMBRE_COMPLETO { get; set; }

        public  string USUARIO { get; set; }

        public string CLAVE { get; set; }

        public DateTime FECHA_REGISTRO { get; set; }


        public string confirmarclave { get; set; }

    }
}