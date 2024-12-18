using FleetManagement.API.Controllers;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Presentation.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.API.Test.Controllers
{
    public class VehiclesApiControllerTest
    {
        private readonly VehiclesController _controller;
        private readonly Mock<IVehicleService> _mockVehicleService;

        public VehiclesApiControllerTest()
        {
            _mockVehicleService = new Mock<IVehicleService>();
            _controller = new VehiclesController(_mockVehicleService.Object);
        }

        #region Testes para o método GetVehicles

        [Fact]
        public async Task GetVehicles_ShouldReturnOk_WhenVehiclesExist()
        {
            // Arrange
            var vehicleList = new List<VehicleOutputDTO>
            {
                new VehicleOutputDTO { Id = 1, ChassisSeries = "ABCD", ChassisNumber = "1234", ColorName = "Red", VehicleTypeName = "SUV", VehicleTypePassengersCapacity = "5" },
                new VehicleOutputDTO { Id = 2, ChassisSeries = "EFGH", ChassisNumber = "5678", ColorName = "Blue", VehicleTypeName = "Sedan", VehicleTypePassengersCapacity = "4" }
            };
            _mockVehicleService.Setup(service => service.GetVehicles(0, 20)).ReturnsAsync(vehicleList);

            // Act
            var result = await _controller.GetVehicles(0, 20);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<VehicleOutputDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetVehicles_ShouldReturnNotFound_WhenNoVehiclesExist()
        {
            // Arrange
            var expectedErrorMessage = "No vehicles.";
            var expectedStatusCode = StatusCodes.Status404NotFound;
            _mockVehicleService.Setup(service => service.GetVehicles(0, 20)).ThrowsAsync(new KeyNotFoundException(expectedErrorMessage));

            // Act
            var result = await _controller.GetVehicles(0, 20);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<NotFoundObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetVehicles_ShouldReturnBadRequest_WhenOffSetIsNegative()
        {
            // Arrange 
            var expectedErrorMessage = "The off-set and records quantity must be greater or equals than 0 (zero).";
            var expectedStatusCode = StatusCodes.Status400BadRequest;
            // Act
            var result = await _controller.GetVehicles(-1, 20);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<BadRequestObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, badRequestResult.StatusCode);
        }
        #endregion

        #region Testes para o método GetVehicleById

        [Fact]
        public async Task GetVehicleById_ShouldReturnOk_WhenVehicleExists()
        {
            // Arrange
            var vehicle = new VehicleOutputDTO { Id = 1, ChassisSeries = "ABCD", ChassisNumber = "1234", ColorName = "Red", VehicleTypeName = "SUV", VehicleTypePassengersCapacity = "5" };
            _mockVehicleService.Setup(service => service.GetVehicleById(1)).ReturnsAsync(vehicle);

            // Act
            var result = await _controller.GetVehicleById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<VehicleOutputDTO>(okResult.Value);
            Assert.Equal("Red", returnValue.ColorName);
        }

        [Fact]
        public async Task GetVehicleById_ShouldReturnNotFound_WhenVehicleNotFound()
        {
            // Arrange
            var expectedErrorMessage = "Vehicle not found.";
            var expectedStatusCode = StatusCodes.Status404NotFound;
            _mockVehicleService.Setup(service => service.GetVehicleById(1)).ThrowsAsync(new KeyNotFoundException(expectedErrorMessage));

            // Act
            var result = await _controller.GetVehicleById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<NotFoundObjectResult>(result).Value;

            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, notFoundResult.StatusCode);
        }

        #endregion

        #region Testes para o método Add

        [Fact]
        public async Task Add_ShouldReturnOk_WhenVehicleAddedSuccessfully()
        {
            // Arrange
            var vehicleInputDTO = new VehicleInputDTO
            {
                ChassisSeries = "ABCD",
                ChassisNumber = "1234",
                ColorId = 1,
                VehicleTypeId = 1
            };
            _mockVehicleService.Setup(service => service.AddAsync(vehicleInputDTO)).ReturnsAsync(1);

            // Act
            var result = await _controller.Add(vehicleInputDTO);

            // Assert
            var okResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(1, okResult.Value);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var expectedErrorMessage = "Invalid Data.";
            var expectedStatusCode = StatusCodes.Status400BadRequest;
            var vehicleInputDTO = new VehicleInputDTO();
            _controller.ModelState.AddModelError("ChassisSeries", "Chassis series is required.");

            // Act
            var result = await _controller.Add(vehicleInputDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<BadRequestObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Add_ShouldReturnConflict_WhenVehicleNotAdded()
        {
            // Arrange
            var expectedErrorMessage = "Vehicle not added.";
            var expectedStatusCode = StatusCodes.Status409Conflict;

            var vehicleInputDTO = new VehicleInputDTO
            {
                ChassisSeries = "EFGH",
                ChassisNumber = "5678",
                ColorId = 2,
                VehicleTypeId = 2
            };
            _mockVehicleService.Setup(service => service.AddAsync(vehicleInputDTO)).ThrowsAsync(new DbUpdateException(expectedErrorMessage));

            // Act
            var result = await _controller.Add(vehicleInputDTO);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<ConflictObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, conflictResult.StatusCode);
        }

        #endregion

        #region Testes para o método Update

        [Fact]
        public async Task Update_ShouldReturnOk_WhenVehicleUpdatedSuccessfully()
        {
            // Arrange
            var vehicleOutputDTO = new VehicleOutputDTO { Id = 1, ChassisSeries = "ABCD", ChassisNumber = "1234", ColorName = "Red", VehicleTypeName = "SUV", VehicleTypePassengersCapacity = "5" };
            _mockVehicleService.Setup(service => service.UpdateAsync(vehicleOutputDTO)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(vehicleOutputDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var expectedErrorMessage = "Invalid data.";
            var expectedStatusCode = StatusCodes.Status400BadRequest;
            var vehicleOutputDTO = new VehicleOutputDTO { Id = 0, ChassisSeries = "Invalid", ChassisNumber = "-1", ColorName = "Unknown", VehicleTypeName = "Unknown", VehicleTypePassengersCapacity = "0" };
            _controller.ModelState.AddModelError("Id", "Invalid ID");

            // Act
            var result = await _controller.Update(vehicleOutputDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<BadRequestObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, badRequestResult.StatusCode);
        }


        #endregion

        #region Testes para o método Remove

        [Fact]
        public async Task Remove_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            // Arrange 
            var expectedErrorMessage = "Vehicle id is required.";
            var expectedStatusCode = StatusCodes.Status400BadRequest;

            // Act
            var result = await _controller.Remove(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<BadRequestObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Remove_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var expectedErrorMessage = "Conflict.";
            var expectedStatusCode = StatusCodes.Status500InternalServerError;
            _mockVehicleService.Setup(service => service.RemoveAsync(1)).ThrowsAsync(new Exception(expectedErrorMessage));

            // Act
            var result = await _controller.Remove(1);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            var errorResult = (ErrorDTO)Assert.IsType<ObjectResult>(result).Value;
            Assert.Equal(expectedErrorMessage, errorResult.ErrorMessage);
            Assert.Equal(expectedStatusCode, badRequestResult.StatusCode);
        }

        #endregion
    }
}
