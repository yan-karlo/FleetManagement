//using FleetManagement.Application.DTOs;
//using FleetManagement.Application.Interfaces.Services;
//using FleetManagement.WebUI.Controllers;
//using Microsoft.AspNetCore.Mvc;
//using Moq;

//namespace FleetManagement.WebUI.Test.Controllers
//{
//    public class VehiclesControllerTests
//    {
//        private readonly VehiclesController _controller;
//        private readonly Mock<IVehicleService> _mockVehicleService;
//        private readonly Mock<IColorService> _mockColorService;
//        private readonly Mock<IVehicleTypeService> _mockVehicleTypeService;

//        public VehiclesControllerTests()
//        {
//            _mockVehicleService = new Mock<IVehicleService>();
//            _mockColorService = new Mock<IColorService>();
//            _mockVehicleTypeService = new Mock<IVehicleTypeService>();
//            _controller = new VehiclesController(_mockVehicleService.Object, _mockColorService.Object, _mockVehicleTypeService.Object);
//        }

//        #region Testing For Create

//        [Fact]
//        public async Task Create_ShouldReturnView_WhenCalled()
//        {
//            // Arrange
//            _mockColorService.Setup(service => service.GetAllColors()).ReturnsAsync(new List<ColorDTO>());
//            _mockVehicleTypeService.Setup(service => service.GetAllVehicleTypes()).ReturnsAsync(new List<VehicleTypeDTO>());

//            // Act
//            var result = await _controller.Create();

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.Null(viewResult.Model); // A model should be null if not passed in the GET request
//        }

//        [Fact]
//        public async Task Create_ShouldRedirectToIndex_OnSuccess()
//        {
//            // Arrange
//            var vehicleInputDTO = new VehicleInputDTO
//            {
//                ChassisSeries = "ABC123",
//                ChassisNumber = 123456,
//                ColorId = 1,
//                VehicleTypeId = 1
//            };

//            //_mockVehicleService.Setup(service => service.AddAsync(It.IsAny<VehicleInputDTO>())).Returns((Task<string>)Task.CompletedTask);
//            _mockColorService.Setup(service => service.GetAllColors()).ReturnsAsync(new List<ColorDTO>());
//            _mockVehicleTypeService.Setup(service => service.GetAllVehicleTypes()).ReturnsAsync(new List<VehicleTypeDTO>());

//            // Act
//            var result = await _controller.Create(vehicleInputDTO);

//            // Assert
//            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
//            Assert.Equal("Index", redirectResult.ActionName);
//        }

//        #endregion

//        #region Testing For Edit

//        [Fact]
//        public async Task Edit_ShouldReturnView_WhenVehicleFound()
//        {
//            // Arrange
//            var vehicleOutputDTO = new VehicleOutputDTO
//            {
//                Id = 1,
//                ChassisSeries = "ABC123",
//                ChassisNumber = 123456,
//                ColorId = 1,
//                VehicleTypeId = 1,
//                ColorName = "Red",
//                VehicleTypeName = "Sedan",
//                VehicleTypePassengersCapacity = "5"
//            };

//            _mockVehicleService.Setup(service => service.GetVehicleById(It.IsAny<int>())).ReturnsAsync(vehicleOutputDTO);
//            _mockColorService.Setup(service => service.GetAllColors()).ReturnsAsync(new List<ColorDTO>());
//            _mockVehicleTypeService.Setup(service => service.GetAllVehicleTypes()).ReturnsAsync(new List<VehicleTypeDTO>());

//            // Act
//            var result = await _controller.Edit(1);

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.Equal(vehicleOutputDTO, viewResult.Model);
//        }

//        [Fact]
//        public async Task Edit_ShouldReturnNotFound_WhenVehicleNotFound()
//        {
//            // Arrange
//            _mockVehicleService.Setup(service => service.GetVehicleById(It.IsAny<int>())).ReturnsAsync((VehicleOutputDTO)null);

//            // Act
//            var result = await _controller.Edit(1);

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.Null(viewResult.Model);
//        }

//        [Fact]
//        public async Task Edit_ShouldRedirectToIndex_OnSuccess()
//        {
//            // Arrange
//            var vehicleOutputDTO = new VehicleOutputDTO { Id = 1, ChassisSeries = "ABC123", ChassisNumber = 123456 };
//            _mockVehicleService.Setup(service => service.UpdateAsync(It.IsAny<VehicleInputDTO>())).Returns(Task.CompletedTask);

//            // Act
//            var result = await _controller.Edit(vehicleOutputDTO);

//            // Assert
//            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
//            Assert.Equal("Index", redirectResult.ActionName);
//        }

//        [Fact]
//        public async Task Edit_ShouldReturnView_OnFailure_WhenModelStateIsInvalid()
//        {
//            // Arrange
//            var vehicleOutputDTO = new VehicleOutputDTO
//            {
//                Id = 1,
//                ChassisSeries = "ABC123",
//                ChassisNumber = 123456,
//                ColorId = 1,
//                VehicleTypeId = 1
//            };
//            _controller.ModelState.AddModelError("ChassisSeries", "Chassis series is required");

//            // Act
//            var result = await _controller.Edit(vehicleOutputDTO);

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.Equal(vehicleOutputDTO, viewResult.Model);
//        }

//        #endregion

//        #region Testing For Delete

//        [Fact]
//        public async Task Delete_ShouldReturnView_WhenVehicleFound()
//        {
//            // Arrange
//            var vehicleOutputDTO = new VehicleOutputDTO { Id = 1, ChassisSeries = "ABC123" };
//            _mockVehicleService.Setup(service => service.GetVehicleById(It.IsAny<int>())).ReturnsAsync(vehicleOutputDTO);

//            // Act
//            var result = await _controller.Delete(1);

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.Equal(vehicleOutputDTO, viewResult.Model);
//        }

//        [Fact]
//        public async Task Delete_ShouldReturnNotFound_WhenVehicleNotFound()
//        {
//            // Arrange
//            _mockVehicleService.Setup(service => service.GetVehicleById(It.IsAny<int>())).ReturnsAsync((VehicleOutputDTO)null);

//            // Act
//            var result = await _controller.Delete(1);

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.Null(viewResult.Model);
//        }

//        [Fact]
//        public async Task Delete_ShouldRedirectToIndex_OnSuccess()
//        {
//            // Arrange
//            var vehicleOutputDTO = new VehicleOutputDTO { Id = 1, ChassisSeries = "ABC123" };
//            _mockVehicleService.Setup(service => service.RemoveAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

//            // Act
//            var result = await _controller.Delete(vehicleOutputDTO);

//            // Assert
//            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
//            Assert.Equal("Index", redirectResult.ActionName);
//        }

//        #endregion

//        #region Testing For Details

//        [Fact]
//        public async Task Details_ShouldReturnView_WhenVehicleFound()
//        {
//            // Arrange
//            var vehicleOutputDTO = new VehicleOutputDTO { Id = 1, ChassisSeries = "ABC123" };
//            _mockVehicleService.Setup(service => service.GetVehicleById(It.IsAny<int>())).ReturnsAsync(vehicleOutputDTO);

//            // Act
//            var result = await _controller.Details(1);

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.Equal(vehicleOutputDTO, viewResult.Model);
//        }
//        #endregion
//    }
//}
