using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
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

        public async Task<string> DeleteCategoryAsync(int id)
        {
            var deleteCategory = await _context.categories!.FindAsync(id);

            if (deleteCategory == null)
            {
                throw new KeyNotFoundException($"Thể loại với ID {id} không tìm thấy.");
            }

            _context.categories.Remove(deleteCategory);
            await _context.SaveChangesAsync();

            return $"Thể loại với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<CategoryModel>> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.categories!.CountAsync();

            var categories = await _context.categories!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<CategoryModel>>(categories);

            return new PagedResult<CategoryModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<CategoryModel> GetCategoriesAsync(int id)
        {
            var category = await _context.categories.FindAsync(id);

            if (category == null)
            {
                throw new KeyNotFoundException($"Thể loại với ID {id} không tìm thấy.");
            }

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task UpdateCategoryAsync(int id, CategoryModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.categories!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Thể loại với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateCategory = _mapper.Map<Category>(model);

            _context.categories.Attach(updateCategory);
            _context.Entry(updateCategory).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
