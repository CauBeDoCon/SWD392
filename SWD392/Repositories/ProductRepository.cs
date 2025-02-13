using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
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
            var product = await _context.products.FindAsync(id);
            return _mapper.Map<ProductModel>(product);
        }

        public async Task<int> AddProductAsync(Models.ProductModel model)
        {
            var newProduct= _mapper.Map<DB.Product>(model);
            _context.products!.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct.Id;
        }

        public async Task DeleteProductAsync(int id)
        {
            var deleteSkin=_context.products!.SingleOrDefault(s=>s.Id == id);
            if (deleteSkin != null)
            {
                _context.products!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Models.ProductModel>> GetAllProductAsync()
        {
            var products = await _context.products!.ToListAsync();
            return _mapper.Map<List<Models.ProductModel>>(products);
        }

        public async Task<Models.ProductModel> GetProductAsync(int id)
        {
            var products = await _context.products.FindAsync(id);
            return _mapper.Map<Models.ProductModel>(products);
        }

        public async Task UpdateProductAsync(int id, Models.ProductModel model)
        {
            if(id == model.Id)
            {
                var updateProduct= _mapper.Map<DB.Product>(model);
                _context.products!.Update(updateProduct);
                await _context.SaveChangesAsync();

            }
        }
    }
}
