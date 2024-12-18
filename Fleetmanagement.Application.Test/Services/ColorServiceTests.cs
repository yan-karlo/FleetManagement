using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Services;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace FleetManagement.Tests.Services
{
    public class ColorServiceTests
    {
        private readonly Mock<IColorRepository> _mockColorRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ColorService _colorService;

        public ColorServiceTests()
        {
            _mockColorRepository = new Mock<IColorRepository>();
            _mockMapper = new Mock<IMapper>();
            _colorService = new ColorService(_mockColorRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllColors_ShouldReturnMappedColors()
        {
            // Arrange
            var colorEntities = new List<Color>
            {
                new Color(1, "Red"),
                new Color(2, "Blue")
            };

            var colorDTOs = new List<ColorDTO>
            {
                new ColorDTO { Id = 1, Name = "Red" },
                new ColorDTO { Id = 2, Name = "Blue" }
            };

            _mockColorRepository.Setup(repo => repo.GetAllColors()).ReturnsAsync(colorEntities);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ColorDTO>>(colorEntities)).Returns(colorDTOs);

            // Act
            var result = await _colorService.GetAllColors();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Red", result.First().Name);
        }

        [Fact]
        public async Task AddAsync_ShouldAddColorAndReturnId()
        {
            // Arrange
            var colorName = "Green";
            var colorEntity = new Color(colorName);
            _mockColorRepository.Setup(repo => repo.AddAsync(It.IsAny<Color>())).ReturnsAsync(1);

            // Act
            var result = await _colorService.AddAsync(colorName);

            // Assert
            Assert.Equal(1, result);
            _mockColorRepository.Verify(repo => repo.AddAsync(It.Is<Color>(c => c.Name == colorName)), Times.Once);
        }

        [Fact]
        public async Task GetColorById_ShouldReturnMappedColor()
        {
            // Arrange
            var colorEntity = new Color(1, "Yellow");
            var colorDTO = new ColorDTO { Id = 1, Name = "Yellow" };

            _mockColorRepository.Setup(repo => repo.GetColorById(1)).ReturnsAsync(colorEntity);
            _mockMapper.Setup(mapper => mapper.Map<ColorDTO>(colorEntity)).Returns(colorDTO);

            // Act
            var result = await _colorService.GetColorById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Yellow", result.Name);
        }

        [Fact]
        public async Task GetColors_ShouldReturnPagedColors()
        {
            // Arrange
            var colorEntities = new List<Color>
            {
                new Color(1, "Black"),
                new Color(2, "White")
            };

            var colorDTOs = new List<ColorDTO>
            {
                new ColorDTO { Id = 1, Name = "Black" },
                new ColorDTO { Id = 2, Name = "White" }
            };

            _mockColorRepository.Setup(repo => repo.GetColors(0, 2)).ReturnsAsync(colorEntities);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ColorDTO>>(colorEntities)).Returns(colorDTOs);

            // Act
            var result = await _colorService.GetColors(0, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Black", result.First().Name);
        }
    }
}
