using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces.Repositories;
using FleetManagement.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infra.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddAsync(Vehicle vehicle)
        {
            await _context.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            return vehicle.Chassis;
        }

        public async Task<Vehicle?> GetVehicleById(int id)
        {
            return await _context.Vehicles
                .Include(v => v.Color)
                .Include(v => v.VehicleType)
                .Where(v => v.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetVehicles(int offset, int recordsQty)
        {
            var vehicleList = await _context.Vehicles
                .Include(v => v.Color)
                .Include(v => v.VehicleType)
                .Skip(offset).Take(recordsQty).ToListAsync();
            return vehicleList;
        }

        public async Task RemoveAsync(Vehicle vehicle)
        {
            _context.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Update(vehicle);
            await _context.SaveChangesAsync();
        }
    }
}
