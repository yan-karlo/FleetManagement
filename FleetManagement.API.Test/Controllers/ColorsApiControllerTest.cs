using FleetManagement.API.Controllers;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Presentation.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var expectedStatusCode = StatusCodes.Status200OK;
            var colorList = new List<ColorDTO>
            {
                new ColorDTO { Id = 1, Name = "Red" },
                new ColorDTO { Id = 2, Name = "Blue" }
            };
            _mockColorService.Setup(service => service.GetAllColors()).ReturnsAsync(colorList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ColorDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal(expectedStatusCode, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAll_ShouldReturnNotFound_WhenNoColorsExist()
        {
            // Arrange
            var expectedErrorMessage = "Color not found.";
            var expectedStatusCode = StatusCodes.Status404NotFound;
            _mockColorService.Setup(service => service.GetAllColors()).ThrowsAsync(new KeyNotFoundException(expectedErrorMessage));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<NotFoundObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, notFoundResult.StatusCode);
        }

        #endregion

        #region Testes para o método GetById

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenColorExists()
        {
            // Arrange
            var expectedStatusCode = StatusCodes.Status200OK;
            var expectedColorName = "Red";
            var color = new ColorDTO { Id = 1, Name = expectedColorName };
            _mockColorService.Setup(service => service.GetColorById(1)).ReturnsAsync(color);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ColorDTO>(okResult.Value);
            Assert.Equal(expectedColorName, returnValue.Name);
            Assert.Equal(expectedStatusCode, okResult.StatusCode);
        }

        [Fact]
        public async Task GetById_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var expectedStatusCode = StatusCodes.Status400BadRequest;
            var expectedErrorMessage = "Color id is required.";
            var wrongId = -1;

            // Act
            var result = await _controller.GetById(wrongId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<BadRequestObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenColorNotFound()
        {
            // Arrange
            var expectedErrorMessage = "Color not found.";
            var expectedStatusCode = StatusCodes.Status404NotFound;
            _mockColorService.Setup(service => service.GetColorById(1)).ThrowsAsync(new KeyNotFoundException(expectedErrorMessage));

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<NotFoundObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, notFoundResult.StatusCode);
        }

        #endregion

        #region Testes para o método Add

        [Fact]
        public async Task Add_ShouldReturnOk_WhenColorAddedSuccessfully()
        {
            // Arrange
            var expectedStatusCode = StatusCodes.Status201Created;
            string colorName = "Green";
            _mockColorService.Setup(service => service.AddAsync(colorName)).ReturnsAsync(1);

            // Act
            var result = await _controller.Add(colorName);

            // Assert
            var okResult = Assert.IsType<CreatedResult>(result);
            var returnValue = Assert.IsType<CreatedResult>(okResult).Value;
            Assert.Equal(1, returnValue);
            Assert.Equal(expectedStatusCode, okResult.StatusCode);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenColorNameIsNull()
        {
            // Arrange
            var expectedStatusCode = StatusCodes.Status400BadRequest;
            // Act
            var result = await _controller.Add(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedStatusCode, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Add_ShouldReturnConflict_WhenExceptionOccurs()
        {
            // Arrange
            var expectedErrorMessage = "Conflict.";
            var expectedStatusCode = StatusCodes.Status409Conflict;
            string colorName = "Yellow";
            _mockColorService.Setup(service => service.AddAsync(colorName)).ThrowsAsync(new DbUpdateException(expectedErrorMessage));

            // Act
            var result = await _controller.Add(colorName);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<ConflictObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, conflictResult.StatusCode);

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
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var expectedErrorMessage = "Invalid data.";
            var expectedStatusCode = StatusCodes.Status400BadRequest;
            var colorDTO = new ColorDTO { Id = 0, Name = "Invalid" };
            _controller.ModelState.AddModelError("Id", expectedErrorMessage);

            // Act
            var result = await _controller.Update(colorDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<BadRequestObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnConflict_WhenExceptionOccurs()
        {
            // Arrange
            var expectedErrorMessage = "Conflict";
            var expectedStatusCode = StatusCodes.Status409Conflict;
            var colorDTO = new ColorDTO { Id = 1, Name = "Orange" };
            _mockColorService.Setup(service => service.UpdateAsync(colorDTO)).ThrowsAsync(new DbUpdateException(expectedErrorMessage));

            // Act
            var result = await _controller.Update(colorDTO);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<ConflictObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, conflictResult.StatusCode);
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
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Remove_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var expectedStatusCode = StatusCodes.Status400BadRequest;
            _controller.ModelState.AddModelError("Id", "Invalid ID");

            // Act
            var result = await _controller.Remove(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedStatusCode, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Remove_ShouldReturnConflict_WhenExceptionOccurs()
        {
            // Arrange
            var expecetedErrorMessage = "Conflict.";
            var expectedStatusCode = StatusCodes.Status409Conflict;
            _mockColorService.Setup(service => service.RemoveAsync(1)).ThrowsAsync(new DbUpdateException(expecetedErrorMessage));

            // Act
            var result = await _controller.Remove(1);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<ConflictObjectResult>(result).Value;
            Assert.Equal(expectedStatusCode, conflictResult.StatusCode);
            Assert.Equal(expecetedErrorMessage, errorResult.ErrorMessage);
        }

        #endregion
    }
}
