using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces.Repositories;

namespace FleetManagement.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(VehicleInputDTO vehicleInputDTO)
        {
            var vehicleEntity = _mapper.Map<Vehicle>(vehicleInputDTO);
            var vahicleId = await _vehicleRepository.AddAsync(vehicleEntity);
            return vahicleId;
        }

        public async Task<VehicleOutputDTO> GetVehicleById(int id)
        {
            var vehicleEntity = await _vehicleRepository.GetVehicleById(id);
            if (vehicleEntity == null) throw new KeyNotFoundException("Vehicle not found.");

            return _mapper.Map<VehicleOutputDTO>(vehicleEntity);
        }

        public async Task<IEnumerable<VehicleOutputDTO>> GetVehicles(int offSet, int recordsQty)
        {
            var vehicleEntityList = await _vehicleRepository.GetVehicles(offSet, recordsQty);
            if (vehicleEntityList == null || !vehicleEntityList.Any()) throw new KeyNotFoundException("No vehicles found.");

            return _mapper.Map<IEnumerable<VehicleOutputDTO>>(vehicleEntityList);
        }

        public async Task RemoveAsync(int id)
        {
            var affectedRecords = await _vehicleRepository.RemoveAsync(new Vehicle(id, "", 0, 0, 0));
            if (affectedRecords == 0) throw new KeyNotFoundException("Vehicle not found to be deleted.");
        }

        public async Task UpdateAsync(VehicleInputDTO vehicle)
        {
            var vehicleEntity = _mapper.Map<Vehicle>(vehicle);
            var affectedRecords = await _vehicleRepository.UpdateAsync(vehicleEntity);

            if (affectedRecords == 0) throw new KeyNotFoundException("Vehicle not found to be updated.");
        }
    }
}
