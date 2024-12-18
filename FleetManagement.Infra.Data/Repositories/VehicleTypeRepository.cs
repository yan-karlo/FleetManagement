using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces.Repositories;
using FleetManagement.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infra.Data.Repositories
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VehicleType>> GetAllVehicleTypes()
        {
            return await _context.VehicleTypes.OrderBy(vt => vt.Name).ToListAsync();
        }

        public async Task<int> AddAsync(VehicleType vehicleType)
        {
            await _context.AddAsync(vehicleType);
            await _context.SaveChangesAsync();
            return vehicleType.Id;
        }

        public async Task<VehicleType> GetVehicleTypeById(int id)
        {
            return await _context.VehicleTypes.FindAsync(id);
        }

        public async Task<IEnumerable<VehicleType>> GetVehicleTypes(int offSet, int recordsQty)
        {
            return await _context.VehicleTypes.Skip(offSet).OrderBy(vt => vt.Name).Take(recordsQty).ToListAsync();
        }

        public async Task<int> RemoveAsync(VehicleType vehicleType)
        {
            _context.Remove(vehicleType);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(VehicleType vehicleType)
        {
            _context.Update(vehicleType);
            return await _context.SaveChangesAsync();
        }
    }
}
