using FleetManagement.Domain.Entities;

namespace FleetManagement.Domain.Interfaces.Repositories
{
    public interface IVehicleTypeRepository
    {
        Task<IEnumerable<VehicleType>> GetAllVehicleTypes();
        Task<IEnumerable<VehicleType>> GetVehicleTypes(int offSet, int recordsQty);
        Task<VehicleType> GetVehicleTypeById(int id);
        Task<int> AddAsync(VehicleType VehicleType);

        Task RemoveAsync(VehicleType VehicleType);
        Task UpdateAsync(VehicleType VehicleType);
    }
}
