using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Interface;

namespace Test
{
    public class DirectorUnitTest
    {
        [Fact]
        public void CreateDirector_WithValidName_ReturnsOkResult()
        {
            var mockDirectorService = new Mock<IDirectorService>();
            mockDirectorService.Setup(x => x.CheckRegex("TestDirector")).Returns(true);
            mockDirectorService.Setup(x => x.CreateDirector("TestDirector")).Returns(true);

            var controller = new DirectorController(mockDirectorService.Object);

            var result = controller.CreateDirector("TestDirector");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Director created", okResult.Value);
        }

        [Fact]
        public void UpdateDirector_WithValidName_ReturnsOkResult()
        {
            var mockDirectorService = new Mock<IDirectorService>();
            mockDirectorService.Setup(x => x.CheckRegex("UpdatedDirector")).Returns(true);
            mockDirectorService.Setup(x => x.UpdateDirector(It.IsAny<Guid>(), "UpdatedDirector")).Returns(true);

            var controller = new DirectorController(mockDirectorService.Object);

            var result = controller.UpdateDirector(Guid.NewGuid(), "UpdatedDirector");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Director updated", okResult.Value);
        }

        [Fact]
        public void DeleteDirector_WithValidGuid_ReturnsOkResult()
        {
            var mockDirectorService = new Mock<IDirectorService>();
            mockDirectorService.Setup(x => x.DeleteDirector(It.IsAny<Guid>())).Returns(true);

            var controller = new DirectorController(mockDirectorService.Object);

            var result = controller.DeleteDirector(Guid.NewGuid());

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Director deleted", okResult.Value);
        }
    }
}