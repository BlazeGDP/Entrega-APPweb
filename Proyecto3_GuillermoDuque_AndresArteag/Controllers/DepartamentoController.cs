using Proyecto3_GuillermoDuque_AndresArteag.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto3_GuillermoDuque_AndresArteag.Controllers
{
    public class DepartamentoController : Controller
    {
        
        public List<Departamento> ListaDepartamentos = new List<Departamento>();

        
        public ActionResult Index()
        {
            return View(ListaDepartamentos);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                
                if (ListaDepartamentos.Any(d => d.Nombre == departamento.Nombre))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un departamento con este nombre.");
                    return View(departamento);
                }

                
                ListaDepartamentos.Add(departamento);

                return RedirectToAction("Index");
            }
            return View(departamento);
        }
    }
} 
    
