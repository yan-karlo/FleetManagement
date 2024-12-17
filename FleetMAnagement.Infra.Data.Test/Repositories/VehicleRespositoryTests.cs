using FleetManagement.Domain.Entities;
using FleetManagement.Infra.Data.Context;
using FleetManagement.Infra.Data.Repositories;
using FleetMAnagement.Infra.Data.Test.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Tests.Repositories
{
    public class VehicleRepositoryTests
    {
        private readonly VehicleRepository _repository;
        private readonly ApplicationDbContext _context;

        public VehicleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique InMemory DB
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new VehicleRepository(_context);
        }

        [Fact]
        public async Task GetVehicles_ShouldReturnAllVehicles()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.Colors.AddRange(VehicleMockedDataHelper.GetColors());
            _context.VehicleTypes.AddRange(VehicleMockedDataHelper.GetVehicleTypes());
            _context.Vehicles.AddRange(VehicleMockedDataHelper.GetVehicles());
            _context.SaveChanges();

            // Act
            var result = await _repository.GetVehicles(0, 2);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("ABCD", result.First().ChassisSeries);
        }

        [Fact]
        public async Task AddAsync_ShouldAddVehicle()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var vehicle = VehicleMockedDataHelper.GetVehicles()[0];

            // Act
            var result = await _repository.AddAsync(vehicle);

            // Assert
            Assert.Equal(1, _context.Vehicles.Count());
            Assert.Equal(vehicle.ChassisSeries, _context.Vehicles.First().ChassisSeries);
        }

        [Fact]
        public async Task AddAsync_InvalidVehicle_ShouldThrowException()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var vehicle = new Vehicle { Id = 1, ChassisSeries = null, ColorId = 1, VehicleTypeId = 1 }; // Invalid Vehicle (ChassisSeries is null)

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _repository.AddAsync(vehicle));
        }

        [Fact]
        public async Task GetVehicleById_ShouldReturnVehicle()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated(); // Certifique-se de criar o banco

            var color = new Color(1, "Red");
            var vehicleType = new VehicleType(1, "Car", 5);

            await _context.Colors.AddAsync(color);
            await _context.VehicleTypes.AddAsync(vehicleType);
            await _context.SaveChangesAsync();

            var vehicle = new Vehicle { Id = 1, ChassisSeries = "ABC123", ColorId = color.Id, VehicleTypeId = vehicleType.Id };
            await _repository.AddAsync(vehicle);

            // Verifique se o veículo foi salvo no contexto
            var debugVehicle = await _context.Vehicles.ToListAsync();
            Assert.Single(debugVehicle); // Confirma que o veículo foi salvo

            // Act
            var result = await _repository.GetVehicleById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ABC123", result.ChassisSeries);
        }

        [Fact]
        public async Task GetVehicleById_InvalidId_ShouldReturnNull()
        {
            // Arrange
            _context.Database.EnsureDeleted();

            // Act
            var result = await _repository.GetVehicleById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveVehicle()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var vehicle = new Vehicle { Id = 1, ChassisSeries = "ABC123", ColorId = 1, VehicleTypeId = 1 };
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            // Act
            await _repository.RemoveAsync(vehicle);

            // Assert
            Assert.Equal(0, _context.Vehicles.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateVehicle()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var vehicle = new Vehicle { Id = 1, ChassisSeries = "ABC123", ColorId = 1, VehicleTypeId = 1 };
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            // Act
            vehicle.ChassisSeries = "AA789";
            await _repository.UpdateAsync(vehicle);

            // Assert
            Assert.Equal("AA789", _context.Vehicles.First().ChassisSeries);
        }

        [Fact]
        public async Task UpdateAsync_InvalidVehicle_ShouldThrowException()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var vehicle = new Vehicle { Id = 1, ChassisSeries = null, ColorId = 1, VehicleTypeId = 1 }; // Invalid Vehicle (ChassisSeries is null)

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _repository.UpdateAsync(vehicle));
        }
    }
}
