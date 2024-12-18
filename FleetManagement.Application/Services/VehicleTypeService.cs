using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Application.Services
{
    public class VehicleTypeService : IVehicleTypeService
    {
        private IVehicleTypeRepository _vehicleTypeRepository;
        private readonly IMapper _mapper;

        public VehicleTypeService(IVehicleTypeRepository vehicleTypeRepository, IMapper mapper)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(VehicleTypeInputDTO vehicleTypeInputDTO)
        {
            var vehicleTypeEntity = _mapper.Map<VehicleType>(vehicleTypeInputDTO);
            var vehicleTypeEntityId = await _vehicleTypeRepository.AddAsync(vehicleTypeEntity);
            if (vehicleTypeEntityId == 0) throw new DbUpdateException("Vehicle type not added.");

            return vehicleTypeEntityId;
        }

        public async Task<VehicleTypeDTO> GetVehicleTypeById(int id)
        {
            var vehicleTypeEntity = await _vehicleTypeRepository.GetVehicleTypeById(id);
            if (vehicleTypeEntity == null) throw new KeyNotFoundException("Vehicle type not found.");

            return _mapper.Map<VehicleTypeDTO>(vehicleTypeEntity);
        }

        public async Task<IEnumerable<VehicleTypeDTO>> GetAllVehicleTypes()
        {
            var vehicleTypeEntityList = await _vehicleTypeRepository.GetAllVehicleTypes();
            if (vehicleTypeEntityList == null || !vehicleTypeEntityList.Any()) throw new KeyNotFoundException("No vehicle types found.");

            return _mapper.Map<IEnumerable<VehicleTypeDTO>>(vehicleTypeEntityList);
        }

        public async Task<IEnumerable<VehicleTypeDTO>> GetVehicleTypes(int offSet, int recordsQty)
        {
            var vehicleTypeEntityList = await _vehicleTypeRepository.GetVehicleTypes(offSet, recordsQty);
            if (vehicleTypeEntityList == null || !vehicleTypeEntityList.Any()) throw new KeyNotFoundException("No vehicle types found.");

            return _mapper.Map<IEnumerable<VehicleTypeDTO>>(vehicleTypeEntityList);
        }

        public async Task RemoveAsync(int id)
        {
            var affectedRecords = await _vehicleTypeRepository.RemoveAsync(new VehicleType(id, "", 0));
            if (affectedRecords == 0) throw new KeyNotFoundException("Vehicle type not found to be deleted.");
        }

        public async Task UpdateAsync(VehicleTypeDTO vehicleType)
        {
            var vehicleTypeEntity = _mapper.Map<VehicleType>(vehicleType);
            var affectedRecords = await _vehicleTypeRepository.UpdateAsync(vehicleTypeEntity);
            if (affectedRecords == 0) throw new KeyNotFoundException("Vehicle type not found to be updated.");

        }

    }
}
