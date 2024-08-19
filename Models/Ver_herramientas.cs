using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROL_HERRAMIENTA_SUNCORP.Models
{
    public class Ver_herramientas
    {
        public int IdRegistroSerieHerramienta { get; set; }
        public string NumeroSerie { get; set; }
        public int IdHerramienta { get; set; }
        public string Herramienta { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
    }
}