using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Services;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace FleetManagement.Tests.Services
{
    public class VehicleTypeServiceTests
    {
        private readonly Mock<IVehicleTypeRepository> _mockVehicleTypeRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly VehicleTypeService _vehicleTypeService;

        public VehicleTypeServiceTests()
        {
            _mockVehicleTypeRepository = new Mock<IVehicleTypeRepository>();
            _mockMapper = new Mock<IMapper>();
            _vehicleTypeService = new VehicleTypeService(_mockVehicleTypeRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddVehicleTypeAndReturnId()
        {
            // Arrange
            var vehicleTypeInputDTO = new VehicleTypeInputDTO { Name = "Car", PassengersCapacity = 4 };
            var vehicleTypeEntity = new VehicleType(0, "Car", 4);

            _mockMapper.Setup(mapper => mapper.Map<VehicleType>(vehicleTypeInputDTO)).Returns(vehicleTypeEntity);
            _mockVehicleTypeRepository.Setup(repo => repo.AddAsync(vehicleTypeEntity)).ReturnsAsync(1);

            // Act
            var result = await _vehicleTypeService.AddAsync(vehicleTypeInputDTO);

            // Assert
            Assert.Equal(1, result);
            _mockVehicleTypeRepository.Verify(repo => repo.AddAsync(It.Is<VehicleType>(v => v.Name == "Car" && v.PassengersCapacity == 4)), Times.Once);
        }

        [Fact]
        public async Task GetVehicleTypeById_ShouldReturnMappedVehicleType()
        {
            // Arrange
            var vehicleTypeEntity = new VehicleType(1, "Truck", 10);
            var vehicleTypeDTO = new VehicleTypeDTO { Id = 1, Name = "Truck", PassengersCapacity = 10 };

            _mockVehicleTypeRepository.Setup(repo => repo.GetVehicleTypeById(1)).ReturnsAsync(vehicleTypeEntity);
            _mockMapper.Setup(mapper => mapper.Map<VehicleTypeDTO>(vehicleTypeEntity)).Returns(vehicleTypeDTO);

            // Act
            var result = await _vehicleTypeService.GetVehicleTypeById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Truck", result.Name);
        }

        [Fact]
        public async Task GetAllVehicleTypes_ShouldReturnMappedVehicleTypes()
        {
            // Arrange
            var vehicleTypeEntities = new List<VehicleType>
            {
                new VehicleType(1, "Car", 4),
                new VehicleType(2, "Truck", 10)
            };

            var vehicleTypeDTOs = new List<VehicleTypeDTO>
            {
                new VehicleTypeDTO { Id = 1, Name = "Car",  PassengersCapacity = 4 },
                new VehicleTypeDTO { Id = 2, Name = "Truck",  PassengersCapacity = 10 }
            };

            _mockVehicleTypeRepository.Setup(repo => repo.GetAllVehicleTypes()).ReturnsAsync(vehicleTypeEntities);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<VehicleTypeDTO>>(vehicleTypeEntities)).Returns(vehicleTypeDTOs);

            // Act
            var result = await _vehicleTypeService.GetAllVehicleTypes();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Car", result.First().Name);
        }

        [Fact]
        public async Task GetVehicleTypes_ShouldReturnPagedVehicleTypes()
        {
            // Arrange
            var vehicleTypeEntities = new List<VehicleType>
            {
                new VehicleType(1, "Bike", 2),
                new VehicleType(2, "Bus", 50)
            };

            var vehicleTypeDTOs = new List<VehicleTypeDTO>
            {
                new VehicleTypeDTO { Id = 1, Name = "Bike",  PassengersCapacity = 2 },
                new VehicleTypeDTO { Id = 2, Name = "Bus",  PassengersCapacity = 50 }
            };

            _mockVehicleTypeRepository.Setup(repo => repo.GetVehicleTypes(0, 2)).ReturnsAsync(vehicleTypeEntities);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<VehicleTypeDTO>>(vehicleTypeEntities)).Returns(vehicleTypeDTOs);

            // Act
            var result = await _vehicleTypeService.GetVehicleTypes(0, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Bike", result.First().Name);
        }
    }
}
