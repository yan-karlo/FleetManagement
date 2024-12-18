using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces.Repositories;
using FleetManagement.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infra.Data.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly ApplicationDbContext _context;

        public ColorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Color>> GetAllColors()
        {
            return await _context.Colors.OrderBy(c => c.Name).ToListAsync();
        }
        public async Task<int> AddAsync(Color color)
        {
            await _context.AddAsync(color);
            await _context.SaveChangesAsync();
            return color.Id;
        }

        public async Task<Color> GetColorById(int id)
        {
            return await _context.Colors.FindAsync(id);
        }

        public async Task<IEnumerable<Color>> GetColors(int offSet, int recordsQty)
        {
            return await _context.Colors.Skip(offSet).Take(recordsQty).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<int> RemoveAsync(Color color)
        {
            _context.Remove(color);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Color color)
        {
            _context.Update(color);
            return await _context.SaveChangesAsync();
        }
    }
}
