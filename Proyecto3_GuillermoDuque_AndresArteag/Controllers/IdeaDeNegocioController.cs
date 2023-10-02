using Proyecto3_GuillermoDuque_AndresArteag.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto3_GuillermoDuque_AndresArteag.Controllers
{
    public class IdeaDeNegocioController : Controller
    {

        
        private static List<IdeaDeNegocio> ListaIdeasNegocio = new List<IdeaDeNegocio>();

        
        private static List<Departamento> ListaDepartamentos = new List<Departamento>();

        
        private static List<IntegranteEquipo> ListaIntegrantes = new List<IntegranteEquipo>();

        
        public ActionResult Index()
        {
            return View(ListaIdeasNegocio);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult CreateConDepartamento(IdeaDeNegocio idea, string NombreDepartamentoHidden)
        {
            if (ModelState.IsValid)
            {
                
                if (ListaIdeasNegocio.Any(i => i.Nombre == idea.Nombre))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una idea de negocio con este nombre.");
                    return View(idea);
                }

                
                idea.Codigo = GenerarCodigoUnico();

                if (Request.Form.AllKeys.Any(key => key.StartsWith("IntegrantesEquipo")))
                {
                    idea.IntegrantesEquipo = new List<IntegranteEquipo>();

                    for (int i = 0; i < Request.Form.Count; i += 5)
                    {
                        idea.IntegrantesEquipo.Add(new IntegranteEquipo
                        {
                            Identificacion = Request.Form[i],
                            Nombre = Request.Form[i + 1],
                            Apellidos = Request.Form[i + 2],
                            Rol = Request.Form[i + 3],
                            Email = Request.Form[i + 4]
                        });
                    }
                }

                
                var existingDepartamento = ListaDepartamentos.FirstOrDefault(d => d.Nombre == NombreDepartamentoHidden);

                if (existingDepartamento != null)
                {
                    
                    idea.DepartamentosBeneficiarios = new List<Departamento> { existingDepartamento };
                }
                else
                {
                    
                    var nuevoDepartamento = new Departamento
                    {
                        Nombre = NombreDepartamentoHidden,
                        Codigo = GenerarCodigoUnico()
                    };

                    
                    idea.DepartamentosBeneficiarios = new List<Departamento> { nuevoDepartamento };

                    
                    ListaDepartamentos.Add(nuevoDepartamento);
                }

                
                ListaIdeasNegocio.Add(idea);


                
                ViewBag.MostrarCodigoUnico = true;
                ViewBag._CodigoUnico = idea.Codigo;

                
                Response.Cookies.Add(new HttpCookie("MostrarCodigoUnico", "true"));

                return View("_CodigoUnico"); 
            }


            return View();
        }
        public ActionResult CodigoUnico()
        {
            return View("_CodigoUnico");
        }

        [HttpPost]
        public ActionResult Create(IdeaDeNegocio idea, string Herramienta4RIUtilizadaInput)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    if (ListaIdeasNegocio.Any(i => i.Nombre == idea.Nombre))
                    {
                        ModelState.AddModelError("Nombre", "Ya existe una idea de negocio con este nombre.");
                        return View(idea);
                    }

                    
                    idea.Codigo = GenerarCodigoUnico();

                    
                    var herramientas = Herramienta4RIUtilizadaInput.Split(',').Select(s => s.Trim()).ToList();

                    
                    idea.Herramienta4RIUtilizada = herramientas;

                    
                    if (idea.ValorInversion == null || idea.IngresosPrimeros3Anios == null ||
                        idea.ValorInversion == 0 || idea.IngresosPrimeros3Anios == 0)
                    {
                        throw new Exception("Los valores de inversión e ingresos no pueden ser nulos o 0.");
                    }

                    
                    if (idea.ValorInversion - idea.IngresosPrimeros3Anios < 0)
                    {
                        throw new Exception("La diferencia entre inversión e ingresos no puede ser negativa.");
                    }

                    
                    ListaIdeasNegocio.Add(idea);

                    
                    Response.Cookies.Add(new HttpCookie("MostrarCodigoUnico", "true"));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(idea);
                }
            }
            return View(idea);
    }

        
        public ActionResult Edit(string codigo)
        {
            
            IdeaDeNegocio idea = ListaIdeasNegocio.FirstOrDefault(i => i.Codigo == codigo);

            if (idea == null)
            {
                ModelState.AddModelError("Codigo", "Código incorrecto. Inténtelo de nuevo.");
                return View();
            }

            
            return View(idea);
        }

        
        [HttpPost]
        public ActionResult Edit(IdeaDeNegocio idea)
        {
            if (ModelState.IsValid)
            {
                
                IdeaDeNegocio existingIdea = ListaIdeasNegocio.FirstOrDefault(i => i.Codigo == idea.Codigo);

                if (existingIdea == null)
                {
                    ModelState.AddModelError("Codigo", "Código incorrecto. Inténtelo de nuevo.");
                    return View(idea); 
                }

                
                
                existingIdea.ValorInversion = idea.ValorInversion;
                existingIdea.IngresosPrimeros3Anios = idea.IngresosPrimeros3Anios;

                return RedirectToAction("Index", new { codigo = existingIdea.Codigo });
            }
            return View(idea);
        }


        private string GenerarCodigoUnico()
        {
            const string caracteresPermitidos = "0123456789ABCD";
            char[] codigo = new char[6];
            Random random = new Random(); 

            for (int i = 0; i < codigo.Length; i++)
            {
                codigo[i] = caracteresPermitidos[random.Next(caracteresPermitidos.Length)];
            }
            return new string(codigo);
        }
        public ActionResult VerIdea(string codigo)
        {
            
            IdeaDeNegocio idea = ListaIdeasNegocio.FirstOrDefault(i => i.Codigo == codigo);

            if (idea == null)
            {
                
                return HttpNotFound();
            }

            
            var departamentos = idea.DepartamentosBeneficiarios;
            var integrantes = idea.IntegrantesEquipo;

            
            ViewBag.Departamentos = departamentos;
            ViewBag.Integrantes = integrantes;

            return View(idea);
        }



        public ActionResult Estadisticas()
        {
            var ideas = ListaIdeasNegocio;

      
            var ideasOrdenadas = ideas.OrderByDescending(i => i.Rentabilidad);
            var ideasMasRentables = ideasOrdenadas.Take(3);

            var sumaTotalIngresos = ideas.Sum(i => i.IngresosPrimeros3Anios);

           
            var sumaTotalInversion = ideas.Sum(i => i.ValorInversion);


            var ideasConIA = ideas.Where(i => i.Herramienta4RIUtilizada.Any(h => h.ToLower().Contains("inteligencia artificial"))).ToList();


            var ideasDesarrolloSostenible = ideas.Where(i => i.ImpactoSocialOEconomico.ToLower().Contains("desarrollo sostenible")).ToList();

            IdeaDeNegocio ideaMayorImpacto = ListaIdeasNegocio.OrderByDescending(idea => idea.DepartamentosBeneficiarios.Count).FirstOrDefault();
            IdeaDeNegocio ideaMayorIngresos = ListaIdeasNegocio.OrderByDescending(idea => idea.IngresosPrimeros3Anios).FirstOrDefault();

            ViewBag.IdeasMasRentables = ideasMasRentables;
            ViewBag.IdeaMayorImpacto = ideaMayorImpacto;
            ViewBag.SumaTotalIngresos = sumaTotalIngresos;
            ViewBag.SumaTotalInversion = sumaTotalInversion;
            ViewBag.IdeasConIA = ideasConIA;
            ViewBag.IdeasDesarrolloSostenible = ideasDesarrolloSostenible;

            if (ideaMayorIngresos != null)
            {
                ViewBag.IdeaMayorIngresos = ideaMayorIngresos;
            }
            
            return View();

        }

    }
}