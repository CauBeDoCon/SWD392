using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            var products = await _context.products.ToListAsync();
            return _mapper.Map<List<ProductModel>>(products);
        }
        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            var product = await _context.products
        .Include(p => p.Brand)
        .Include(p => p.Category)
        .Include(p => p.ProductDetail)
        .Include(p=>p.Packaging)
        .Include(p=>p.BrandOrigin)
        .Include(p=>p.ManufacturedCountry)
        .Include(p=>p.Manufacturer)
        .Include(p=>p.Unit)
        .FirstOrDefaultAsync(p => p.Id == id);
            // return _mapper.Map<ProductModel>(product);
             return product != null ? _mapper.Map<ProductModel>(product) : null;
        }

        public async Task<int> AddProductAsync(Models.ProductModel model)
        {
            var newProduct= _mapper.Map<DB.Product>(model);
            _context.products!.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct.Id;
        }

        public async Task<string> DeleteProductAsync(int id)
        {
            var deleteProduct = await _context.products!.FindAsync(id);

            if (deleteProduct == null)
            {
                throw new KeyNotFoundException($"Sản phẩm với ID {id} không tìm thấy.");
            }

            _context.products.Remove(deleteProduct);
            await _context.SaveChangesAsync();

            return $"Sản phẩm với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<ProductModel>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.products!.CountAsync();

            var products = await _context.products!
        .Include(p => p.Unit)
        .Include(p => p.Brand)
        .Include(p => p.Packaging)
        .Include(p => p.Category)
        .Include(p => p.BrandOrigin)
        .Include(p => p.Manufacturer)
        .Include(p => p.ManufacturedCountry)
        .Include(p => p.ProductDetail)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

            var mappedData = _mapper.Map<List<ProductModel>>(products);

            return new PagedResult<ProductModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Models.ProductModel> GetProductAsync(int id)
        {
            var products = await _context.products.FindAsync(id);
            return _mapper.Map<Models.ProductModel>(products);
        }

        public async Task UpdateProductAsync(int id, Models.ProductModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductDetail)
                .Include(p => p.Packaging)
                .Include(p => p.BrandOrigin)
                .Include(p => p.ManufacturedCountry)
                .Include(p => p.Manufacturer)
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Sản phẩm với ID {id} không tìm thấy.");
            }

            _mapper.Map(model, existingEntity); // Cập nhật các thuộc tính từ model vào existingEntity

            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductModel>> GetProductsByPriceUnder100Async(int pageNumber, int pageSize)
        {
            var query = _context.products.Where(p => p.Price < 100);

            int totalCount = await query.CountAsync();
            var products = await query
                .Include(p => p.Unit)
                .Include(p => p.Brand)
                .Include(p => p.Packaging)
                .Include(p => p.Category)
                .Include(p => p.BrandOrigin)
                .Include(p => p.Manufacturer)
                .Include(p => p.ManufacturedCountry)
                .Include(p => p.ProductDetail)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductModel>
            {
                Items = _mapper.Map<List<ProductModel>>(products),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<ProductModel>> GetProductsByPriceOver200Async(int pageNumber, int pageSize)
        {
            var query = _context.products.Where(p => p.Price > 200);

            int totalCount = await query.CountAsync();
            var products = await query
                .Include(p => p.Unit)
                .Include(p => p.Brand)
                .Include(p => p.Packaging)
                .Include(p => p.Category)
                .Include(p => p.BrandOrigin)
                .Include(p => p.Manufacturer)
                .Include(p => p.ManufacturedCountry)
                .Include(p => p.ProductDetail)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductModel>
            {
                Items = _mapper.Map<List<ProductModel>>(products),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<ProductModel>> GetProductsByCategoryNameAsync(string categoryName, int pageNumber, int pageSize)
        {
            var query = _context.products
                .Include(p => p.Category)
                .Where(p => p.Category.Name.ToLower() == categoryName.ToLower());

            int totalCount = await query.CountAsync();
            var products = await query
                .Include(p => p.Unit)
                .Include(p => p.Brand)
                .Include(p => p.Packaging)
                .Include(p => p.BrandOrigin)
                .Include(p => p.Manufacturer)
                .Include(p => p.ManufacturedCountry)
                .Include(p => p.ProductDetail)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductModel>
            {
                Items = _mapper.Map<List<ProductModel>>(products),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<ProductModel>> GetProductsBySolutionNameAsync(string solutionName, int pageNumber, int pageSize)
        {
            var query = _context.products
                .Include(p => p.Category)
                .ThenInclude(c => c.Solution)
                .Where(p => p.Category.Solution.Name.ToLower() == solutionName.ToLower());

            int totalCount = await query.CountAsync();
            var products = await query
                .Include(p => p.Unit)
                .Include(p => p.Brand)
                .Include(p => p.Packaging)
                .Include(p => p.BrandOrigin)
                .Include(p => p.Manufacturer)
                .Include(p => p.ManufacturedCountry)
                .Include(p => p.ProductDetail)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductModel>
            {
                Items = _mapper.Map<List<ProductModel>>(products),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


    }
}
