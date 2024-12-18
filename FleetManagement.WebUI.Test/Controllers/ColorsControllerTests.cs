//using FleetManagement.Application.DTOs;
//using FleetManagement.Application.Interfaces.Services;
//using FleetManagement.WebUI.Controllers;
//using Microsoft.AspNetCore.Mvc;
//using Moq;

//namespace FleetManagement.Tests.Controllers;

//public class ColorsControllerTests
//{
//    private readonly ColorsController _controller;
//    private readonly Mock<IColorService> _mockColorService;

//    public ColorsControllerTests()
//    {
//        _mockColorService = new Mock<IColorService>();
//        _controller = new ColorsController(_mockColorService.Object);
//    }

//    #region Testing For Index

//    [Fact]
//    public async Task Index_ShouldReturnView_WithColors_OnSuccess()
//    {
//        // Arrange
//        var colors = new List<ColorDTO>
//        {
//            new ColorDTO { Id = 1, Name = "Red" },
//            new ColorDTO { Id = 2, Name = "Blue" }
//        };

//        _mockColorService.Setup(service => service.GetColors(It.IsAny<int>(), It.IsAny<int>()))
//            .ReturnsAsync(colors);

//        // Act
//        var result = await _controller.Index(0, 20);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        var model = Assert.IsAssignableFrom<IEnumerable<ColorDTO>>(viewResult.Model);
//        Assert.Equal(2, model.Count());
//    }

//    [Fact]
//    public async Task Index_ShouldReturnView_WithError_WhenServiceThrowsException()
//    {
//        // Arrange
//        _mockColorService.Setup(service => service.GetColors(It.IsAny<int>(), It.IsAny<int>()))
//            .ThrowsAsync(new Exception("Service error"));

//        // Act
//        var result = await _controller.Index(0, 20);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    #endregion

//    #region Testing for Create (GET e POST)

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
//        var colorDTO = new ColorDTO { Name = "Red" };

//        _mockColorService.Setup(service => service.AddAsync(It.IsAny<string>()))
//            .ReturnsAsync(1); // Assume sucesso na criação.

//        // Act
//        var result = await _controller.Create(colorDTO);

//        // Assert
//        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
//        Assert.Equal("Index", redirectResult.ActionName);
//    }

//    [Fact]
//    public async Task Create_ShouldReturnView_OnFailure_WhenColorNameIsNull()
//    {
//        // Arrange
//        var colorDTO = new ColorDTO { Name = null };

//        // Act
//        var result = await _controller.Create(colorDTO);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    [Fact]
//    public async Task Create_ShouldReturnView_OnFailure_WhenServiceThrowsException()
//    {
//        // Arrange
//        var colorDTO = new ColorDTO { Name = "Red" };

//        _mockColorService.Setup(service => service.AddAsync(It.IsAny<string>()))
//            .ThrowsAsync(new Exception("Service error"));

//        // Act
//        var result = await _controller.Create(colorDTO);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    #endregion

//    #region Testing for Edit (GET e POST)

//    [Fact]
//    public async Task Edit_ShouldReturnView_WhenColorFound()
//    {
//        // Arrange
//        var colorDTO = new ColorDTO { Id = 1, Name = "Red" };
//        _mockColorService.Setup(service => service.GetColorById(It.IsAny<int>()))
//            .ReturnsAsync(colorDTO);

//        // Act
//        var result = await _controller.Edit(1);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        var model = Assert.IsAssignableFrom<ColorDTO>(viewResult.Model);
//        Assert.Equal("Red", model.Name);
//    }

//    [Fact]
//    public async Task Edit_ShouldReturnView_WhenColorNotFound()
//    {
//        // Arrange
//        _mockColorService.Setup(service => service.GetColorById(It.IsAny<int>()))
//            .ReturnsAsync((ColorDTO)null);

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
//        var colorDTO = new ColorDTO { Id = 1, Name = "Red" };
//        _mockColorService.Setup(service => service.UpdateAsync(It.IsAny<ColorDTO>()))
//            .Returns(Task.CompletedTask);

//        // Act
//        var result = await _controller.Edit(colorDTO);

//        // Assert
//        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
//        Assert.Equal("Index", redirectResult.ActionName);
//    }

//    [Fact]
//    public async Task Edit_ShouldReturnView_OnFailure_WhenServiceThrowsException()
//    {
//        // Arrange
//        var colorDTO = new ColorDTO { Id = 1, Name = "Red" };
//        _mockColorService.Setup(service => service.UpdateAsync(It.IsAny<ColorDTO>()))
//            .ThrowsAsync(new Exception("Service error"));

//        // Act
//        var result = await _controller.Edit(colorDTO);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    #endregion

//    #region Testing For Delete (GET e POST)

//    [Fact]
//    public async Task Delete_ShouldReturnView_WhenColorFound()
//    {
//        // Arrange
//        var colorDTO = new ColorDTO { Id = 1, Name = "Red" };
//        _mockColorService.Setup(service => service.GetColorById(It.IsAny<int>()))
//            .ReturnsAsync(colorDTO);

//        // Act
//        var result = await _controller.Delete(1);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        var model = Assert.IsAssignableFrom<ColorDTO>(viewResult.Model);
//        Assert.Equal("Red", model.Name);
//    }

//    [Fact]
//    public async Task Delete_ShouldReturnView_WhenColorNotFound()
//    {
//        // Arrange
//        _mockColorService.Setup(service => service.GetColorById(It.IsAny<int>()))
//            .ReturnsAsync((ColorDTO)null);

//        // Act
//        var result = await _controller.Delete(1);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    [Fact]
//    public async Task Delete_ShouldRedirectToIndex_OnSuccess()
//    {
//        // Arrange
//        var colorDTO = new ColorDTO { Id = 1, Name = "Red" };
//        _mockColorService.Setup(service => service.RemoveAsync(It.IsAny<int>()))
//            .Returns(Task.CompletedTask);

//        // Act
//        var result = await _controller.Delete(colorDTO);

//        // Assert
//        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
//        Assert.Equal("Index", redirectResult.ActionName);
//    }

//    [Fact]
//    public async Task Delete_ShouldReturnView_OnFailure_WhenServiceThrowsException()
//    {
//        // Arrange
//        var colorDTO = new ColorDTO { Id = 1, Name = "Red" };
//        _mockColorService.Setup(service => service.RemoveAsync(It.IsAny<int>()))
//            .ThrowsAsync(new Exception("Service error"));

//        // Act
//        var result = await _controller.Delete(colorDTO);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    #endregion

//    #region Testing For Details

//    [Fact]
//    public async Task Details_ShouldReturnView_WhenColorFound()
//    {
//        // Arrange
//        var colorDTO = new ColorDTO { Id = 1, Name = "Red" };
//        _mockColorService.Setup(service => service.GetColorById(It.IsAny<int>()))
//            .ReturnsAsync(colorDTO);

//        // Act
//        var result = await _controller.Details(1);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        var model = Assert.IsAssignableFrom<ColorDTO>(viewResult.Model);
//        Assert.Equal("Red", model.Name);
//    }

//    [Fact]
//    public async Task Details_ShouldReturnView_WhenColorNotFound()
//    {
//        // Arrange
//        _mockColorService.Setup(service => service.GetColorById(It.IsAny<int>()))
//            .ReturnsAsync((ColorDTO)null);

//        // Act
//        var result = await _controller.Details(1);

//        // Assert
//        var viewResult = Assert.IsType<ViewResult>(result);
//        Assert.False(_controller.ModelState.IsValid); // ModelState should have errors
//    }

//    #endregion
//}
