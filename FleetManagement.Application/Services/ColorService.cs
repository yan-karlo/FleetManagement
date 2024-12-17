using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces.Repositories;

namespace FleetManagement.Application.Services
{
    public class ColorService : IColorService
    {
        private IColorRepository _colorRepository;
        private readonly IMapper _mapper;

        public ColorService(IColorRepository colorRepository, IMapper mapper)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColorDTO>> GetAllColors()
        {
            var colorsList = await _colorRepository.GetAllColors();
            return _mapper.Map<IEnumerable<ColorDTO>>(colorsList);
        }
        public async Task<int> AddAsync(string colorName)
        {
            var colorEntityId = await _colorRepository.AddAsync(new Color(colorName));
            return colorEntityId;
        }

        public async Task<ColorDTO> GetColorById(int id)
        {
            var colorEntity = await _colorRepository.GetColorById(id);
            return _mapper.Map<ColorDTO>(colorEntity);
        }

        public async Task<IEnumerable<ColorDTO>> GetColors(int offSet, int recordsQty)
        {
            var colorEntityList = await _colorRepository.GetColors(offSet, recordsQty);
            return _mapper.Map<IEnumerable<ColorDTO>>(colorEntityList);
        }

        public async Task RemoveAsync(int id)
        {
            await _colorRepository.RemoveAsync(new Color(id, ""));
        }

        public async Task UpdateAsync(ColorDTO color)
        {
            var colorEntity = _mapper.Map<Color>(color);
            await _colorRepository.UpdateAsync(colorEntity);
        }
    }
}
