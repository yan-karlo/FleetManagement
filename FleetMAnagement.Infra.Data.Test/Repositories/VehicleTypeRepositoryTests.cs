using FleetManagement.Domain.Entities;
using FleetManagement.Infra.Data.Context;
using FleetManagement.Infra.Data.Repositories;
using FleetMAnagement.Infra.Data.Test.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Tests.Repositories
{
    public class VehicleTypeRepositoryTests
    {
        private readonly VehicleTypeRepository _repository;
        private readonly ApplicationDbContext _context;

        public VehicleTypeRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique InMemory DB
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new VehicleTypeRepository(_context);
        }

        [Fact]
        public async Task GetAllVehicleTypes_ShouldReturnAllVehicleTypes()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.VehicleTypes.AddRange(VehicleMockedDataHelper.GetVehicleTypes());
            _context.SaveChanges();

            // Act
            var result = await _repository.GetAllVehicleTypes();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal("Car", result.First().Name);
        }

        [Fact]
        public async Task AddAsync_ShouldAddVehicleType()
        {
            // Arrange
            var vehicleType = new VehicleType(4, "Bus", 50);

            // Act
            var result = await _repository.AddAsync(vehicleType);

            // Assert
            Assert.Equal(4, result);
            Assert.Equal(1, _context.VehicleTypes.Count());
            Assert.Equal("Bus", _context.VehicleTypes.Last().Name);
        }

        [Fact]
        public async Task AddAsync_InvalidVehicleType_ShouldThrowException()
        {
            // Arrange
            var vehicleType = new VehicleType(4, null, 50); // Invalid vehicle type (Name is null)

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _repository.AddAsync(vehicleType));
        }

        [Fact]
        public async Task GetVehicleTypeById_ShouldReturnVehicleType()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var vehicleType = new VehicleType(1, "Car", 5);
            _context.VehicleTypes.Add(vehicleType);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetVehicleTypeById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Car", result.Name);
        }

        [Fact]
        public async Task GetVehicleTypeById_InvalidId_ShouldReturnNull()
        {
            // Act
            _context.Database.EnsureDeleted();
            var result = await _repository.GetVehicleTypeById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveVehicleType()
        {
            // Arrange
            var vehicleType = new VehicleType(1, "Car", 5);
            _context.VehicleTypes.Add(vehicleType);
            _context.SaveChanges();

            // Act
            await _repository.RemoveAsync(vehicleType);

            // Assert
            Assert.Equal(0, _context.VehicleTypes.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateVehicleType()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var vehicleType = new VehicleType(1, "Car", 5);
            _context.VehicleTypes.Add(vehicleType);
            _context.SaveChanges();

            // Act
            vehicleType.Name = "Truck";
            await _repository.UpdateAsync(vehicleType);

            // Assert
            Assert.Equal("Truck", _context.VehicleTypes.First().Name);
        }

        [Fact]
        public async Task UpdateAsync_InvalidVehicleType_ShouldThrowException()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var vehicleType = new VehicleType(1, null, 5); // Invalid vehicle type (Name is null)

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _repository.UpdateAsync(vehicleType));
        }
    }
}
