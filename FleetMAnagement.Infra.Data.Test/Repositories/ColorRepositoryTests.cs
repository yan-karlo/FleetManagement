using FleetManagement.Domain.Entities;
using FleetManagement.Infra.Data.Context;
using FleetManagement.Infra.Data.Repositories;
using FleetMAnagement.Infra.Data.Test.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Tests.Repositories
{
    public class ColorRepositoryTests
    {
        private readonly ColorRepository _repository;
        private readonly ApplicationDbContext _context;

        public ColorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique InMemory DB
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ColorRepository(_context);
        }

        [Fact]
        public async Task GetAllColors_ShouldReturnAllColors()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.Colors.AddRange(VehicleMockedDataHelper.GetColors());
            _context.SaveChanges();

            // Act
            var result = await _repository.GetAllColors();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal("Blue", result.First().Name);
        }

        [Fact]
        public async Task AddAsync_ShouldAddColor()
        {
            // Arrange
            var color = new Color(1, "Red");

            // Act
            var result = await _repository.AddAsync(color);

            // Assert
            Assert.Equal(1, result);
            Assert.Equal(1, _context.Colors.Count());
            Assert.Equal("Red", _context.Colors.First().Name);
        }

        [Fact]
        public async Task AddAsync_InvalidColor_ShouldThrowException()
        {
            // Arrange
            var color = new Color(1, null); // Invalid color (Name is null)

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _repository.AddAsync(color));
        }

        [Fact]
        public async Task GetColorById_ShouldReturnColor()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var color = new Color(1, "Red");
            _context.Colors.Add(color);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetColorById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Red", result.Name);
        }

        [Fact]
        public async Task GetColorById_InvalidId_ShouldReturnNull()
        {
            // Act
            _context.Database.EnsureDeleted();
            var result = await _repository.GetColorById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveColor()
        {
            // Arrange
            var color = new Color(1, "Red");
            _context.Colors.Add(color);
            _context.SaveChanges();

            // Act
            await _repository.RemoveAsync(color);

            // Assert
            Assert.Equal(0, _context.Colors.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateColor()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var color = new Color(1, "Red");
            _context.Colors.Add(color);
            _context.SaveChanges();

            // Act
            color.Name = "Blue";
            await _repository.UpdateAsync(color);

            // Assert
            Assert.Equal("Blue", _context.Colors.First().Name);
        }

        [Fact]
        public async Task UpdateAsync_InvalidColor_ShouldThrowException()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var color = new Color(1, null); // Invalid color (Name is null)

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _repository.UpdateAsync(color));
        }
    }
}
