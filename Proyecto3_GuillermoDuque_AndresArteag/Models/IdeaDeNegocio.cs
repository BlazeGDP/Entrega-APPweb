using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto3_GuillermoDuque_AndresArteag.Models
{
    public class IdeaDeNegocio
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string ImpactoSocialOEconomico { get; set; }
        [Display(Name = "Departamentos que se benefician")]
        public List<Departamento> DepartamentosBeneficiarios { get; set; }
        public int ValorInversion { get; set; }
        public int IngresosPrimeros3Anios { get; set; }
        public List<IntegranteEquipo> IntegrantesEquipo { get;  set; }
        public List<string> Herramienta4RIUtilizada { get; set; }

        public IdeaDeNegocio()
        {
            DepartamentosBeneficiarios = new List<Departamento>();
            IntegrantesEquipo = new List<IntegranteEquipo>();
        }
        public int Rentabilidad
        {
            get { return IngresosPrimeros3Anios - ValorInversion; }
        }
    }
}