using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto3_GuillermoDuque_AndresArteag.Models
{
    public class IntegranteEquipo
    {
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public bool Eliminado { get; set; }
    }
}