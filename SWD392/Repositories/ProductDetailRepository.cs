using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Models;
using SWD392.Repositories;
using AutoMapper;
namespace SWD392.Repositories
{
    public class ProductDetailRepository : IProductDetailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductDetailRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddProductDetailAsync(ProductDetailModel model)
        {
            var newProductDetail = _mapper.Map<ProductDetail>(model);
            _context.productDetails!.Add(newProductDetail);
            await _context.SaveChangesAsync();
            return newProductDetail.Id;
        }

        public async Task DeleteProductDetailAsync(int id)
        {
            var deleteSkin = _context.productDetails!.SingleOrDefault(s => s.Id == id);
            if (deleteSkin != null)
            {
                _context.productDetails!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ProductDetailModel>> GetAllProductDetailsAsync()
        {
            var productDetails = await _context.productDetails!.ToListAsync();
            return _mapper.Map<List<ProductDetailModel>>(productDetails);
        }

        public async Task<ProductDetailModel> GetProductDetailsAsync(int id)
        {
            var productDetails = await _context.productDetails.FindAsync(id);
            return _mapper.Map<ProductDetailModel>(productDetails);
        }

        public async Task UpdateProductDetailAsync(int id, ProductDetailModel model)
        {
            if (id == model.Id)
            {
                var updateProductDetail = _mapper.Map<ProductDetail>(model);
                _context.productDetails!.Update(updateProductDetail);
                await _context.SaveChangesAsync();

            }
        }
    }
}
