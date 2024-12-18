//using AutoMapper;
//using FleetManagement.Application.DTOs;
//using FleetManagement.Application.Services;
//using FleetManagement.Domain.Entities;
//using FleetManagement.Domain.Interfaces.Repositories;
//using Moq;
//using Xunit;

//namespace FleetManagement.Tests.Services
//{
//    public class VehicleServiceTests
//    {
//        private readonly Mock<IVehicleRepository> _mockVehicleRepository;
//        private readonly Mock<IMapper> _mockMapper;
//        private readonly VehicleService _vehicleService;

//        public VehicleServiceTests()
//        {
//            _mockVehicleRepository = new Mock<IVehicleRepository>();
//            _mockMapper = new Mock<IMapper>();
//            _vehicleService = new VehicleService(_mockVehicleRepository.Object, _mockMapper.Object);
//        }

//        [Fact]
//        public async Task AddAsync_ShouldAddVehicleAndReturnChassis()
//        {
//            // Arrange
//            var vehicleInputDTO = new VehicleInputDTO { ChassisSeries = "ABC123", ColorId = 1, VehicleTypeId = 1 };
//            var vehicleEntity = new Vehicle { ChassisSeries = "ABC123", ColorId = 1, VehicleTypeId = 1 };
//            var chassis = "ABC123";

//            _mockMapper.Setup(mapper => mapper.Map<Vehicle>(vehicleInputDTO)).Returns(vehicleEntity);
//            _mockVehicleRepository.Setup(repo => repo.AddAsync(vehicleEntity)).ReturnsAsync(chassis);

//            // Act
//            var result = await _vehicleService.AddAsync(vehicleInputDTO);

//            // Assert
//            Assert.Equal(chassis, result);
//            _mockVehicleRepository.Verify(repo => repo.AddAsync(It.Is<Vehicle>(v => v.ChassisSeries == "ABC123")), Times.Once);
//        }

//        [Fact]
//        public async Task GetVehicleById_ShouldReturnMappedVehicle()
//        {
//            // Arrange
//            var vehicleEntity = new Vehicle { Id = 1, ChassisSeries = "ABC123", ColorId = 1, VehicleTypeId = 1 };
//            var vehicleOutputDTO = new VehicleOutputDTO { Id = 1, ChassisSeries = "ABC123" };

//            _mockVehicleRepository.Setup(repo => repo.GetVehicleById(1)).ReturnsAsync(vehicleEntity);
//            _mockMapper.Setup(mapper => mapper.Map<VehicleOutputDTO>(vehicleEntity)).Returns(vehicleOutputDTO);

//            // Act
//            var result = await _vehicleService.GetVehicleById(1);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("ABC123", result.ChassisSeries);
//        }

//        [Fact]
//        public async Task GetVehicles_ShouldReturnPagedMappedVehicles()
//        {
//            // Arrange
//            var vehicleEntities = new List<Vehicle>
//            {
//                new Vehicle { Id = 1, ChassisSeries = "ABC123", ColorId = 1, VehicleTypeId = 1 },
//                new Vehicle { Id = 2, ChassisSeries = "DEF456", ColorId = 2, VehicleTypeId = 2 }
//            };

//            var vehicleOutputDTOs = new List<VehicleOutputDTO>
//            {
//                new VehicleOutputDTO { Id = 1, ChassisSeries = "ABC123" },
//                new VehicleOutputDTO { Id = 2, ChassisSeries = "DEF456" }
//            };

//            _mockVehicleRepository.Setup(repo => repo.GetVehicles(0, 2)).ReturnsAsync(vehicleEntities);
//            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<VehicleOutputDTO>>(vehicleEntities)).Returns(vehicleOutputDTOs);

//            // Act
//            var result = await _vehicleService.GetVehicles(0, 2);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(2, result.Count());
//            Assert.Equal("ABC123", result.First().ChassisSeries);
//        }

//        [Fact]
//        public async Task RemoveAsync_ShouldRemoveVehicle()
//        {
//            // Arrange
//            var vehicleId = 1;

//            // Act
//            await _vehicleService.RemoveAsync(vehicleId);

//            // Assert
//            _mockVehicleRepository.Verify(repo => repo.RemoveAsync(It.Is<Vehicle>(v => v.Id == vehicleId)), Times.Once);
//        }

//        [Fact]
//        public async Task UpdateAsync_ShouldUpdateVehicle()
//        {
//            // Arrange
//            var vehicleInputDTO = new VehicleInputDTO { ChassisSeries = "ABC123", ColorId = 1, VehicleTypeId = 1 };
//            var vehicleEntity = new Vehicle { ChassisSeries = "ABC123", ColorId = 1, VehicleTypeId = 1 };

//            _mockMapper.Setup(mapper => mapper.Map<Vehicle>(vehicleInputDTO)).Returns(vehicleEntity);

//            // Act
//            await _vehicleService.UpdateAsync(vehicleInputDTO);

//            // Assert
//            _mockVehicleRepository.Verify(repo => repo.UpdateAsync(It.Is<Vehicle>(v => v.ChassisSeries == "ABC123")), Times.Once);
//        }
//    }
//}
