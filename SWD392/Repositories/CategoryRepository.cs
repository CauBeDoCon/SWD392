using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddCategoryAsync(CategoryModel model)
        {
            var newCategory = _mapper.Map<Category>(model);
            _context.categories!.Add(newCategory);
            await _context.SaveChangesAsync();
            return newCategory.Id;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var deleteSkin = _context.categories!.SingleOrDefault(s => s.Id == id);
            if (deleteSkin != null)
            {
                _context.categories!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            var categories = await _context.categories!.ToListAsync();
            return _mapper.Map<List<CategoryModel>>(categories);
        }

        public async Task<CategoryModel> GetCategoriesAsync(int id)
        {
            var categories = await _context.categories.FindAsync(id);
            return _mapper.Map<CategoryModel>(categories);
        }

        public async Task UpdateCategoryAsync(int id, CategoryModel model)
        {
            if (id == model.Id)
            {
                var updateCategory = _mapper.Map<Category>(model);
                _context.categories!.Update(updateCategory);
                await _context.SaveChangesAsync();

            }
        }
    }
}
