using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Interface;

namespace Test
{
    public class CountryUnitTest
    {
        [Fact]
        public void CreateCountry_WithValidName_ReturnsOkResult()
        {
            var mockCountryService = new Mock<ICountryService>();
            mockCountryService.Setup(x => x.CheckRegex("TestCountry")).Returns(true);
            mockCountryService.Setup(x => x.CreateCountry("TestCountry")).Returns(true);

            var controller = new CountryController(mockCountryService.Object);

            var result = controller.CreateCountry("TestCountry");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Country created", okResult.Value);
        }

        [Fact]
        public void UpdateCountry_WithValidName_ReturnsOkResult()
        {
            var mockCountryService = new Mock<ICountryService>();
            mockCountryService.Setup(x => x.CheckRegex("UpdatedCountry")).Returns(true);
            mockCountryService.Setup(x => x.UpdateCountry(It.IsAny<Guid>(), "UpdatedCountry")).Returns(true);

            var controller = new CountryController(mockCountryService.Object);

            var result = controller.UpdateCountry(Guid.NewGuid(), "UpdatedCountry");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Country updated", okResult.Value);
        }

        [Fact]
        public void DeleteCountry_WithValidGuid_ReturnsOkResult()
        {
            var mockCountryService = new Mock<ICountryService>();
            mockCountryService.Setup(x => x.DeleteCountry(It.IsAny<Guid>())).Returns(true);

            var controller = new CountryController(mockCountryService.Object);

            var result = controller.DeleteCountry(Guid.NewGuid());

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Country deleted", okResult.Value);
        }
    }
}