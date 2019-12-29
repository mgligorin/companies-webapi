using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiroslavGligorinFinalniTest.Controllers;
using MiroslavGligorinFinalniTest.Interfaces;
using MiroslavGligorinFinalniTest.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace MiroslavGligorinFinalniTest.Tests.Controllers
{
    [TestClass]
    public class ZaposleniControllerTest
    {
        [TestMethod]
        public void GetReturnsAgentWithSameId()
        {
            // Arrange
            var mockRepository = new Mock<IZaposleniRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(new Zaposleni { Id = 1, ImeIPrezime = "Pera Peric", GodinaRodjenja = 1980, GodinaZaposlenja = 2008, Plata = 3000 });

            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(1);
            var contentResult = actionResult as OkNegotiatedContentResult<Zaposleni>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id);
        }

        // --------------------------------------------------------------------------------------

        [TestMethod]
        public void GetReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IZaposleniRepository>();
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteReturnsNotFound()
        {
            // Arrange 
            var mockRepository = new Mock<IZaposleniRepository>();
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        // --------------------------------------------------------------------------------------

        [TestMethod]
        public void DeleteReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<IZaposleniRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(new Zaposleni { Id = 1, ImeIPrezime = "Pera Peric", GodinaRodjenja = 1980, GodinaZaposlenja = 2008, Plata = 3000 });
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        // --------------------------------------------------------------------------------------

        [TestMethod]
        public void PutReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IZaposleniRepository>();
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Put(2, new Zaposleni { Id = 1, ImeIPrezime = "Pera Peric", GodinaRodjenja = 1980, GodinaZaposlenja = 2008, Plata = 3000 });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        // -------------------------------------------------------------------------------------

        [TestMethod]
        public void PostMethodSetsLocationHeader()
        {
            // Arrange
            var mockRepository = new Mock<IZaposleniRepository>();
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Post(new Zaposleni { Id = 1, ImeIPrezime = "Pera Peric", GodinaRodjenja = 1980, GodinaZaposlenja = 2008, Plata = 3000 });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Zaposleni>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(1, createdResult.RouteValues["id"]);
        }

        // ------------------------------------------------------------------------------------------

        [TestMethod]
        public void GetReturnsMultipleObjects()
        {
            // Arrange
            List<Zaposleni> zaposleni = new List<Zaposleni>();
            zaposleni.Add(new Zaposleni { Id = 1, ImeIPrezime = "Pera Peric", GodinaRodjenja = 1980, GodinaZaposlenja = 2008, Plata = 3000 });
            zaposleni.Add(new Zaposleni { Id = 2, ImeIPrezime = "Mika Mikic", GodinaRodjenja = 1976, GodinaZaposlenja = 2005, Plata = 6000 });

            var mockRepository = new Mock<IZaposleniRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(zaposleni.AsEnumerable());
            var controller = new ZaposleniController(mockRepository.Object);

            // Act
            IEnumerable<Zaposleni> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(zaposleni.Count, result.ToList().Count);
            Assert.AreEqual(zaposleni.ElementAt(0), result.ElementAt(0));
            Assert.AreEqual(zaposleni.ElementAt(1), result.ElementAt(1));
        }
    }
}
