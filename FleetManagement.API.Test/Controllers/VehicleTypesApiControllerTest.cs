using FleetManagement.API.Controllers;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FleetManagement.API.Test.Controllers
{
    public class VehicleTypesApiControllerTest
    {
        private readonly VehicleTypesController _controller;
        private readonly Mock<IVehicleTypeService> _mockVehicleTypeService;

        public VehicleTypesApiControllerTest()
        {
            _mockVehicleTypeService = new Mock<IVehicleTypeService>();
            _controller = new VehicleTypesController(_mockVehicleTypeService.Object);
        }

        #region Testes para o método GetAll

        [Fact]
        public async Task GetAll_ShouldReturnOk_WhenVehicleTypesExist()
        {
            // Arrange
            var vehicleTypeList = new List<VehicleTypeDTO>
            {
                new VehicleTypeDTO { Id = 1, Name = "Car" },
                new VehicleTypeDTO { Id = 2, Name = "Truck" }
            };
            _mockVehicleTypeService.Setup(service => service.GetAllVehicleTypes()).ReturnsAsync(vehicleTypeList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<VehicleTypeDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAll_ShouldReturnNotFound_WhenNoVehicleTypesExist()
        {
            // Arrange
            _mockVehicleTypeService.Setup(service => service.GetAllVehicleTypes()).ReturnsAsync((List<VehicleTypeDTO>)null);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("VehicleTypess not found.", notFoundResult.Value);
        }

        #endregion

        #region Testes para o método GetById

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenVehicleTypeExists()
        {
            // Arrange
            var vehicleType = new VehicleTypeDTO { Id = 1, Name = "Car" };
            _mockVehicleTypeService.Setup(service => service.GetVehicleTypeById(1)).ReturnsAsync(vehicleType);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<VehicleTypeDTO>(okResult.Value);
            Assert.Equal("Car", returnValue.Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            // Act
            var result = await _controller.GetById(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("VehicleTypes id is required.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenVehicleTypeNotFound()
        {
            // Arrange
            _mockVehicleTypeService.Setup(service => service.GetVehicleTypeById(1)).ReturnsAsync((VehicleTypeDTO)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("VehicleTypes not found.", notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        #endregion

        #region Testes para o método Add

        [Fact]
        public async Task Add_ShouldReturnCreated_WhenVehicleTypeAddedSuccessfully()
        {
            // Arrange
            var vehicleTypeInputDTO = new VehicleTypeInputDTO { Name = "Bike" };
            _mockVehicleTypeService.Setup(service => service.AddAsync(vehicleTypeInputDTO)).ReturnsAsync(1);

            // Act
            var result = await _controller.Add(vehicleTypeInputDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.Equal("", createdResult.Location);
            Assert.Equal(1, createdResult.Value);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var vehicleTypeInputDTO = new VehicleTypeInputDTO();
            _controller.ModelState.AddModelError("Name", "Name is required.");

            // Act
            var result = await _controller.Add(vehicleTypeInputDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Add_ShouldReturnConflict_WhenExceptionOccurs()
        {
            // Arrange
            var vehicleTypeInputDTO = new VehicleTypeInputDTO { Name = "Bus" };
            _mockVehicleTypeService.Setup(service => service.AddAsync(vehicleTypeInputDTO)).ThrowsAsync(new Exception("Conflict"));

            // Act
            var result = await _controller.Add(vehicleTypeInputDTO);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal("Conflict", conflictResult.Value);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        #endregion

        #region Testes para o método Update

        [Fact]
        public async Task Update_ShouldReturnOk_WhenVehicleTypeUpdatedSuccessfully()
        {
            // Arrange
            var vehicleTypeDTO = new VehicleTypeDTO { Id = 1, Name = "Van" };
            _mockVehicleTypeService.Setup(service => service.UpdateAsync(vehicleTypeDTO)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(vehicleTypeDTO);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var vehicleTypeDTO = new VehicleTypeDTO { Id = 0, Name = "Invalid", PassengersCapacity = -100 };
            _controller.ModelState.AddModelError("Id", "Invalid ID");

            // Act
            var result = await _controller.Update(vehicleTypeDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenVehicleTypeIdIsInvalid()
        {
            // Arrange
            var vehicleTypeDTO = new VehicleTypeDTO { Id = 0, Name = "Invalid" };
            _controller.ModelState.AddModelError("Id", "Invalid ID");

            // Act
            var result = await _controller.Update(vehicleTypeDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnConflict_WhenExceptionOccurs()
        {
            // Arrange
            var vehicleTypeDTO = new VehicleTypeDTO { Id = 1, Name = "SUV" };
            _mockVehicleTypeService.Setup(service => service.UpdateAsync(vehicleTypeDTO)).ThrowsAsync(new Exception("Conflict"));

            // Act
            var result = await _controller.Update(vehicleTypeDTO);

            // Assert
            var conflictResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, conflictResult.StatusCode);
        }

        #endregion

        #region Testes para o método Remove

        [Fact]
        public async Task Remove_ShouldReturnNoContent_WhenVehicleTypeRemovedSuccessfully()
        {
            // Arrange
            _mockVehicleTypeService.Setup(service => service.RemoveAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Remove(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
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
            _mockVehicleTypeService.Setup(service => service.RemoveAsync(1)).ThrowsAsync(new Exception("BadRequest"));

            // Act
            var result = await _controller.Remove(1);

            // Assert
            var conflictResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("BadRequest", conflictResult.Value);
        }

        #endregion
    }
}
