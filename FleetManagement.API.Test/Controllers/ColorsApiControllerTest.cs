using FleetManagement.API.Controllers;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace FleetManagement.API.Test.Controllers
{
    public class ColorsApiControllerTest
    {
        private readonly ColorsController _controller;
        private readonly Mock<IColorService> _mockColorService;

        public ColorsApiControllerTest()
        {
            _mockColorService = new Mock<IColorService>();
            _controller = new ColorsController(_mockColorService.Object);
        }

        #region Testes para o método GetAll

        [Fact]
        public async Task GetAll_ShouldReturnOk_WhenColorsExist()
        {
            // Arrange
            var colorList = new List<ColorDTO>
            {
                new ColorDTO { Id = 1, Name = "Red" },
                new ColorDTO { Id = 2, Name = "Blue" }
            };
            _mockColorService.Setup(service => service.GetAllColors()).ReturnsAsync(colorList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ColorDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAll_ShouldReturnNotFound_WhenNoColorsExist()
        {
            // Arrange
            _mockColorService.Setup(service => service.GetAllColors()).ReturnsAsync((List<ColorDTO>)null);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Colors not found.", notFoundResult.Value);
        }

        #endregion

        #region Testes para o método GetById

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenColorExists()
        {
            // Arrange
            var color = new ColorDTO { Id = 1, Name = "Red" };
            _mockColorService.Setup(service => service.GetColorById(1)).ReturnsAsync(color);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ColorDTO>(okResult.Value);
            Assert.Equal("Red", returnValue.Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            // Act
            var result = await _controller.GetById(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Color id is required.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenColorNotFound()
        {
            // Arrange
            _mockColorService.Setup(service => service.GetColorById(1)).ReturnsAsync((ColorDTO)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Color not found.", notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        #endregion

        #region Testes para o método Add

        [Fact]
        public async Task Add_ShouldReturnOk_WhenColorAddedSuccessfully()
        {
            // Arrange
            string colorName = "Green";
            _mockColorService.Setup(service => service.AddAsync(colorName)).ReturnsAsync(1);

            // Act
            var result = await _controller.Add(colorName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<int>(okResult.Value);
            Assert.Equal(1, returnValue);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenColorNameIsNull()
        {
            // Act
            var result = await _controller.Add(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Add_ShouldReturnConflict_WhenExceptionOccurs()
        {
            // Arrange
            string colorName = "Yellow";
            _mockColorService.Setup(service => service.AddAsync(colorName)).ThrowsAsync(new Exception("Conflict"));

            // Act
            var result = await _controller.Add(colorName);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal("Conflict", conflictResult.Value);
            Assert.Equal(409, conflictResult.StatusCode);

        }

        #endregion

        #region Testes para o método Update

        [Fact]
        public async Task Update_ShouldReturnOk_WhenColorUpdatedSuccessfully()
        {
            // Arrange
            var colorDTO = new ColorDTO { Id = 1, Name = "Purple" };
            _mockColorService.Setup(service => service.UpdateAsync(colorDTO)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(colorDTO);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var colorDTO = new ColorDTO { Id = 0, Name = "Invalid" };
            _controller.ModelState.AddModelError("Id", "Invalid ID");

            // Act
            var result = await _controller.Update(colorDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnConflict_WhenExceptionOccurs()
        {
            // Arrange
            var colorDTO = new ColorDTO { Id = 1, Name = "Orange" };
            _mockColorService.Setup(service => service.UpdateAsync(colorDTO)).ThrowsAsync(new Exception("Conflict"));

            // Act
            var result = await _controller.Update(colorDTO);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Conflict", conflictResult.Value);
        }

        #endregion

        #region Testes para o método Remove

        [Fact]
        public async Task Remove_ShouldReturnOk_WhenColorRemovedSuccessfully()
        {
            // Arrange
            _mockColorService.Setup(service => service.RemoveAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Remove(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Remove_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Id", "Invalid ID");

            // Act
            var result = await _controller.Remove(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Remove_ShouldReturnConflict_WhenExceptionOccurs()
        {
            // Arrange
            _mockColorService.Setup(service => service.RemoveAsync(1)).ThrowsAsync(new Exception("Conflict"));

            // Act
            var result = await _controller.Remove(1);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Conflict", conflictResult.Value);
        }

        #endregion
    }
}
