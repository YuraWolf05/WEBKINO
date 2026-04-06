using Microsoft.EntityFrameworkCore;
using webkino.Data;
using webkino.Services.Interfaces;
using webkino.ViewModels;

namespace webkino.Services
{
    public class StudioService : IStudioService
    {
        private readonly AppDbContext _context;

        public StudioService(AppDbContext context)
        {
            _context = context;
        }

        public List<StudioViewModel> GetAll()
        {
            return _context.Studios
                .Include(s => s.Movies)
                .Select(s => new StudioViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Country = s.Country ?? "Невідомо",
                    MoviesCount = s.Movies.Count
                })
                .ToList();
        }

        public List<StudioOptionViewModel> GetOptions()
        {
            return _context.Studios
                .OrderBy(s => s.Name)
                .Select(s => new StudioOptionViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();
        }
    }
}