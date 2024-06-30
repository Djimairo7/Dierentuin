using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Dierentuin.Controllers;
using Dierentuin.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TestProject1.Controllers
{
    public class HomeControllerTests
    {
        private readonly HomeController _controller;
        private readonly Mock<ILogger<HomeController>> _mockLogger;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object);

            // Mock HttpContext
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            _controller.ControllerContext = controllerContext;
        }

        [Fact]
        public void Index_ReturnsAViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsAViewResult_WithErrorViewModel()
        {
            // Act
            var result = _controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.ViewData.Model);
            Assert.Equal(Activity.Current?.Id ?? _controller.HttpContext.TraceIdentifier, model.RequestId);
        }
    }
}
