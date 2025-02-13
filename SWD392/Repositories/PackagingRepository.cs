using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class PackagingRepository : IPackagingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PackagingRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddPackagingAsync(PackagingModel model)
        {
            var newPackaging = _mapper.Map<Packaging>(model);
            _context.packagings!.Add(newPackaging);
            await _context.SaveChangesAsync();
            return newPackaging.Id;
        }

        public async Task DeletePackagingAsync(int id)
        {
            var deleteSkin = _context.packagings!.SingleOrDefault(s => s.Id == id);
            if (deleteSkin != null)
            {
                _context.packagings!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<PackagingModel>> GetAllPackagingsAsync()
        {
            var packagings = await _context.packagings!.ToListAsync();
            return _mapper.Map<List<PackagingModel>>(packagings);
        }

        public async Task<PackagingModel> GetPackagingsAsync(int id)
        {
            var packagings = await _context.packagings.FindAsync(id);
            return _mapper.Map<PackagingModel>(packagings);
        }

        public async Task UpdatePackagingAsync(int id, PackagingModel model)
        {
            if (id == model.Id)
            {
                var updatePackaging = _mapper.Map<Packaging>(model);
                _context.packagings!.Update(updatePackaging);
                await _context.SaveChangesAsync();

            }
        }
    }
}
