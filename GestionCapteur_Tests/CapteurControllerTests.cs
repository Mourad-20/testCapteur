using GestioCapteur.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Model.DataModel;
using Model.ViewModel;
using Moq;
using Repositories;
using Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GestionCapteur_Tests
{
     public class CapteurControllerTests
    {

        private readonly CapteurController _controller;
        private readonly Mock<ICapteurRepository> _mockCapteurRepo;
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly DbContextOptions<ContextProject> _options;


       
        private readonly List<Capteur> data = new List<Capteur>
        {
        new Capteur() { Id = 1, Name = "cap01", Type = "type01", Value = 10 },
        new Capteur() { Id = 2, Name = "cap02", Type = "type02", Value = 20 },
        new Capteur() { Id = 3, Name = "cap03", Type = "type03", Value = 30 },
        new Capteur() { Id = 4, Name = "cap04", Type = "type04", Value = 40 },
        new Capteur() { Id = 5, Name = "cap05", Type = "type05", Value = 50 },
        new Capteur() { Id = 6, Name = "cap06", Type = "type06", Value = 60 },
        new Capteur() { Id = 7, Name = "cap07", Type = "type07", Value = 70 },
        new Capteur() { Id = 8, Name = "cap08", Type = "type08", Value = 80 },
        new Capteur() { Id = 9, Name = "cap09", Type = "type09", Value = 90 },
        new Capteur() { Id = 10, Name = "cap10", Type = "type10", Value = 100 },
        new Capteur() { Id = 11, Name = "cap11", Type = "type11", Value = 110 },
        new Capteur() { Id = 12, Name = "cap12", Type = "type12", Value = 120 },
        new Capteur() { Id = 13, Name = "cap13", Type = "type13", Value = 130 },
        new Capteur() { Id = 14, Name = "cap14", Type = "type14", Value = 140 },
        new Capteur() { Id = 15, Name = "cap15", Type = "type15", Value = 150 },
        new Capteur() { Id = 16, Name = "cap16", Type = "type16", Value = 160 },
        new Capteur() { Id = 17, Name = "cap17", Type = "type17", Value = 170 },
        new Capteur() { Id = 18, Name = "cap18", Type = "type18", Value = 180 },
        new Capteur() { Id = 19, Name = "cap19", Type = "type19", Value = 190 },
        new Capteur() { Id = 20, Name = "cap20", Type = "type20", Value = 200 }

        };
        public CapteurControllerTests()
        {
            var serviceCollection = new ServiceCollection();

            _options = new DbContextOptionsBuilder<ContextProject>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Créez les mocks
            _mockCapteurRepo = new Mock<ICapteurRepository>();
            _mockCache = new Mock<IDistributedCache>();

            _mockCapteurRepo.Setup(repo => repo.GetAll()).ReturnsAsync(data);
            _mockCapteurRepo.Setup(repo => repo.GetById(1)).ReturnsAsync(data[0]);
            _mockCapteurRepo.Setup(repo => repo.Delete(data[0])).Returns(true);
           // _mockCache.Setup(cache => cache.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            // Créez le contexte en mémoire
            var context = new ContextProject(_options);

            context.Capteur.AddRange(data);
            context.SaveChanges();

            _controller = new CapteurController(context, _mockCapteurRepo.Object, _mockCache.Object);
        }

        [Fact]
        public async Task GetAll_returnAll()
        {
             var result = await _controller.GetAll();
            var actionResult = Assert.IsType<ActionResult<IEnumerable<CapteurVM>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<List<CapteurVM>>(okResult.Value);
            Assert.Equal(data.Count, returnValue.Count);
        }
        [Fact]
        public async Task DeletesCapteurAndRemovesCache()
        {
            // Arrange
            

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<object>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(200, objectResult.StatusCode);
            var returnValue = objectResult.Value as ResponsDeletCapteur;
           
            Assert.Equal("supprimé avec succès", returnValue.Message);

            // Verify that the cache was removed
            //_mockCache.Verify(cache => cache.RemoveAsync(It.Is<string>(key => key == "Capteur_1")), Times.Once);
        }

        [Fact]
        public async Task Delete_CapteurNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockCapteurRepo.Setup(repo => repo.GetById(999)).ReturnsAsync((Capteur)null);

            // Act
            var result = await _controller.Delete(999);

            // Assert
            var actionResult = Assert.IsType<ActionResult<object>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Not Found", notFoundResult.Value);
        }
    }
}
