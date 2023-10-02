using Proyecto3_GuillermoDuque_AndresArteag.Controllers;
using Proyecto3_GuillermoDuque_AndresArteag.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto3_GuillermoDuque_AndresArteag.Tests
{
    [TestClass]
    public class IdeaDeNegocioControllerTests
    {
        [TestMethod]
        public void Create_ValidIdea_RedirectsToIndex()
        {
            // Arrange
            var controller = new IdeaDeNegocioController();
            var idea = new IdeaDeNegocio
            {
                Nombre = "Idea Válida",
                ValorInversion = 10000,
                IngresosPrimeros3Anios = 20000,
                Herramienta4RIUtilizada = "inteligencia artificial, Big Data",
            };

            // Act
            var result = controller.Create(idea, idea.Herramienta4RIUtilizada) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Create_InvalidIdeaWithException_ShouldReturnViewWithModelError()
        {
            // Arrange
            var controller = new IdeaDeNegocioController();
            var idea = new IdeaDeNegocio
            {
                Nombre = "Idea Inválida",
                ValorInversion = null, 
                IngresosPrimeros3Anios = -1000, 
                Herramienta4RIUtilizada = "inteligencia artificial, Big Data",
            };

            // Act
            var result = controller.Create(idea, idea.Herramienta4RIUtilizada) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsTrue(controller.ModelState.ContainsKey(string.Empty)); 
        }

        [TestMethod]
        public void Create_ValidIdeaWithTeamMembers_RedirectsToIndex()
        {
            // Arrange
            var controller = new IdeaDeNegocioController();
            var idea = new IdeaDeNegocio
            {
                Nombre = "Idea Con Equipo",
                ValorInversion = 10000,
                IngresosPrimeros3Anios = 20000,
                Herramienta4RIUtilizada = "AI, Big Data",
            };
            idea.IntegrantesEquipo = new List<IntegranteEquipo>
    {
        new IntegranteEquipo { Nombre = "John Doe", Rol = "Desarrollador" },
        new IntegranteEquipo { Nombre = "Jane Smith", Rol = "Diseñador" }
    };

            // Act
            var result = controller.Create(idea, idea.Herramienta4RIUtilizada) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
        [TestMethod]
        public void Edit_ValidIdea_RedirectsToIndex()
        {
            // Arrange
            var controller = new IdeaDeNegocioController();
            var existingIdea = new IdeaDeNegocio
            {
                Codigo = "123",
                Nombre = "Idea Original",
                ValorInversion = 5000,
                IngresosPrimeros3Anios = 10000,
                Herramienta4RIUtilizada = "AI, Big Data",
            };
            IdeaDeNegocio updatedIdea = new IdeaDeNegocio
            {
                Codigo = "123",
                Nombre = "Idea Actualizada",
                ValorInversion = 8000,
                IngresosPrimeros3Anios = 15000,
                Herramienta4RIUtilizada = "AI, IoT",
            };

            // Act
            var result = controller.Edit(updatedIdea) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);

            
            Assert.AreEqual("Idea Actualizada", existingIdea.Nombre);
            Assert.AreEqual(8000, existingIdea.ValorInversion);
            Assert.AreEqual(15000, existingIdea.IngresosPrimeros3Anios);
            CollectionAssert.AreEqual(new List<string> { "AI", "IoT" }, existingIdea.Herramienta4RIUtilizada);
        }
        [TestMethod]
        public void Edit_NonExistentIdea_ReturnsErrorView()
        {
            // Arrange
            var controller = new IdeaDeNegocioController();
            var nonExistentIdea = new IdeaDeNegocio
            {
                Codigo = "999", 
                Nombre = "Idea No Existente",
                ValorInversion = 2000,
                IngresosPrimeros3Anios = 4000,
                Herramienta4RIUtilizada = "AI",
            };

            // Act
            var result = controller.Edit(nonExistentIdea) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsTrue(controller.ModelState.ContainsKey("Codigo"));
        }

    }
}