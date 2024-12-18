//using FleetManagement.Application.DTOs;
//using FleetManagement.Application.Interfaces.Services;
//using FleetManagement.WebUI.Controllers;
//using Microsoft.AspNetCore.Mvc;
//using Moq;

//namespace FleetManagement.Tests.Controllers;


//public class VehicleTypesControllerTests
//{
//    private readonly VehicleTypesController _controller;
//    private readonly Mock<IVehicleTypeService> _mockVehicleTypeService;

//    public VehicleTypesControllerTests()
//    {
//        _mockVehicleTypeService = new Mock<IVehicleTypeService>();
//        _controller = new VehicleTypesController(_mockVehicleTypeService.Object);
//    }

//    #region Testing For Index

//    [Fact]
//    public async Task Index_ShouldReturnView_WithVehicleTypes_OnSuccess()
//    {
//        // Arrange
//        var vehicleTypes = new List<VehicleTypeDTO>
//        {
//            new VehicleTypeDTO { Id = 1, Name = "SUV" },
//            new VehicleTypeDTO { Id = 2, Name = "Sedan" }
//        };

//        _mockVehicleTypeService.Setup(service => service.GetVehicleTypes(It.IsAny<int>(), It.IsAny<int>()))
//            .ReturnsAsync(vehicleTypes);

//        // Act
//        var result = await _controller.Index(0, 20);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        var model = Assert.IsAssignableFrom<IEnumerable<VehicleTypeDTO>>(viewResult.Model);
//        Assert.Equal(2, model.Count());
//    }

//    [Fact]
//    public async Task Index_ShouldReturnView_WithError_WhenServiceThrowsException()
//    {
//        // Arrange
//        _mockVehicleTypeService.Setup(service => service.GetVehicleTypes(It.IsAny<int>(), It.IsAny<int>()))
//            .ThrowsAsync(new Exception("Service error"));

//        // Act
//        var result = await _controller.Index(0, 20);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    #endregion

//    #region Testing For Create (GET e POST)

//    [Fact]
//    public async Task Create_ShouldReturnView_OnGetRequest()
//    {
//        // Act
//        var result = await _controller.Create();

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//    }

//    [Fact]
//    public async Task Create_ShouldRedirectToIndex_OnSuccess()
//    {
//        // Arrange
//        var vehicleTypeInputDTO = new VehicleTypeInputDTO { Name = "SUV" };

//        _mockVehicleTypeService.Setup(service => service.AddAsync(It.IsAny<VehicleTypeInputDTO>()))
//            .ReturnsAsync(1);

//        // Act
//        var result = await _controller.Create(vehicleTypeInputDTO);

//        // Assert
//        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
//        Assert.Equal("Index", redirectResult.ActionName);
//    }

//    [Fact]
//    public async Task Create_ShouldReturnView_OnFailure_WhenServiceThrowsException()
//    {
//        // Arrange
//        var vehicleTypeInputDTO = new VehicleTypeInputDTO { Name = "SUV" };

//        _mockVehicleTypeService.Setup(service => service.AddAsync(It.IsAny<VehicleTypeInputDTO>()))
//            .ThrowsAsync(new Exception("Service error"));

//        // Act
//        var result = await _controller.Create(vehicleTypeInputDTO);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    //#endregion

//    #region Testing For Edit (GET e POST)

//    [Fact]
//    public async Task Edit_ShouldReturnView_WhenVehicleTypeFound()
//    {
//        // Arrange
//        var vehicleTypeDTO = new VehicleTypeDTO { Id = 1, Name = "SUV" };
//        _mockVehicleTypeService.Setup(service => service.GetVehicleTypeById(It.IsAny<int>()))
//            .ReturnsAsync(vehicleTypeDTO);

//        // Act
//        var result = await _controller.Edit(1);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        var model = Assert.IsAssignableFrom<VehicleTypeDTO>(viewResult.Model);
//        Assert.Equal("SUV", model.Name);
//    }

//    [Fact]
//    public async Task Edit_ShouldReturnView_WhenVehicleTypeNotFound()
//    {
//        // Arrange
//        _mockVehicleTypeService.Setup(service => service.GetVehicleTypeById(It.IsAny<int>()))
//            .ReturnsAsync((VehicleTypeDTO)null);

//        // Act
//        var result = await _controller.Edit(1);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    [Fact]
//    public async Task Edit_ShouldRedirectToIndex_OnSuccess()
//    {
//        // Arrange
//        var vehicleTypeDTO = new VehicleTypeDTO { Id = 1, Name = "SUV" };
//        _mockVehicleTypeService.Setup(service => service.UpdateAsync(It.IsAny<VehicleTypeDTO>()))
//            .Returns(Task.CompletedTask);

//        // Act
//        var result = await _controller.Edit(vehicleTypeDTO);

//        // Assert
//        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
//        Assert.Equal("Index", redirectResult.ActionName);
//    }

//    [Fact]
//    public async Task Edit_ShouldReturnView_OnFailure_WhenServiceThrowsException()
//    {
//        // Arrange
//        var vehicleTypeDTO = new VehicleTypeDTO { Id = 1, Name = "SUV" };
//        _mockVehicleTypeService.Setup(service => service.UpdateAsync(It.IsAny<VehicleTypeDTO>()))
//            .ThrowsAsync(new Exception("Service error"));

//        // Act
//        var result = await _controller.Edit(vehicleTypeDTO);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    #endregion

//    #region Testing For Delete (GET e POST)

//    [Fact]
//    public async Task Delete_ShouldReturnView_WhenVehicleTypeFound()
//    {
//        // Arrange
//        var vehicleTypeDTO = new VehicleTypeDTO { Id = 1, Name = "SUV" };
//        _mockVehicleTypeService.Setup(service => service.GetVehicleTypeById(It.IsAny<int>()))
//            .ReturnsAsync(vehicleTypeDTO);

//        // Act
//        var result = await _controller.Delete(vehicleTypeDTO);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        var model = Assert.IsAssignableFrom<VehicleTypeDTO>(viewResult.Model);
//        Assert.Equal("SUV", model.Name);
//    }

//    [Fact]
//    public async Task Delete_ShouldReturnView_WhenVehicleTypeNotFound()
//    {
//        // Arrange
//        _mockVehicleTypeService.Setup(service => service.GetVehicleTypeById(It.IsAny<int>()))
//            .ReturnsAsync((VehicleTypeDTO)null);

//        // Act
//        var result = await _controller.Delete(new VehicleTypeDTO { Id = 1 });

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    [Fact]
//    public async Task Delete_ShouldRedirectToIndex_OnSuccess()
//    {
//        // Arrange
//        _mockVehicleTypeService.Setup(service => service.RemoveAsync(It.IsAny<int>()))
//            .Returns(Task.CompletedTask);

//        // Act
//        var result = await _controller.Delete(1);

//        // Assert
//        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
//        Assert.Equal("Index", redirectResult.ActionName);
//    }

//    [Fact]
//    public async Task Delete_ShouldReturnView_OnFailure_WhenServiceThrowsException()
//    {
//        // Arrange
//        _mockVehicleTypeService.Setup(service => service.RemoveAsync(It.IsAny<int>()))
//            .ThrowsAsync(new Exception("Service error"));

//        // Act
//        var result = await _controller.Delete(1);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    #endregion

//    #region Testing For Details

//    [Fact]
//    public async Task Details_ShouldReturnView_WhenVehicleTypeFound()
//    {
//        // Arrange
//        var vehicleTypeDTO = new VehicleTypeDTO { Id = 1, Name = "SUV" };
//        _mockVehicleTypeService.Setup(service => service.GetVehicleTypeById(It.IsAny<int>()))
//            .ReturnsAsync(vehicleTypeDTO);

//        // Act
//        var result = await _controller.Details(1);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        var model = Assert.IsAssignableFrom<VehicleTypeDTO>(viewResult.Model);
//        Assert.Equal("SUV", model.Name);
//    }

//    [Fact]
//    public async Task Details_ShouldReturnView_WhenVehicleTypeNotFound()
//    {
//        // Arrange
//        _mockVehicleTypeService.Setup(service => service.GetVehicleTypeById(It.IsAny<int>()))
//            .ReturnsAsync((VehicleTypeDTO)null);

//        // Act
//        var result = await _controller.Details(1);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    #endregion
//}
