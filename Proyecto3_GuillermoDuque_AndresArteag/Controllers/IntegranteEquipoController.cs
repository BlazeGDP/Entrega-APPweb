using Proyecto3_GuillermoDuque_AndresArteag.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto3_GuillermoDuque_AndresArteag.Controllers
{
    public class IntegranteEquipoController : Controller
    {
        
        private static List<IntegranteEquipo> ListaIntegrantes = new List<IntegranteEquipo>();

        
        public ActionResult Index()
        {
            return View(ListaIntegrantes);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(IntegranteEquipo integrante)
        {
            if (ModelState.IsValid)
            {
                
                if (ListaIntegrantes.Any(i => i.Identificacion == integrante.Identificacion))
                {
                    ModelState.AddModelError("Identificacion", "Ya existe un integrante con esta identificación.");
                    return View(integrante);
                }

                
                ListaIntegrantes.Add(integrante);

                return RedirectToAction("Index");
            }
            return View(integrante);
        }
    }
}
