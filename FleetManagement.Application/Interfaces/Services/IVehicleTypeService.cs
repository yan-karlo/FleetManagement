using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces.Services
{
    public interface IVehicleTypeService
    {
        Task<IEnumerable<VehicleTypeDTO>> GetAllVehicleTypes();
        Task<IEnumerable<VehicleTypeDTO>> GetVehicleTypes(int offSet, int recordsQty);
        Task<VehicleTypeDTO> GetVehicleTypeById(int id);
        Task<int> AddAsync(VehicleTypeInputDTO vehicleType);

        Task RemoveAsync(int  id);
        Task UpdateAsync(VehicleTypeDTO vehicleType);
    }
}
