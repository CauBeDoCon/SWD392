using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
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

        public async Task<string> DeleteProductDetailAsync(int id)
        {
            var deleteProductDetail = await _context.productDetails!.FindAsync(id);

            if (deleteProductDetail == null)
            {
                throw new KeyNotFoundException($"Chi tiết sản phẩm với ID {id} không tìm thấy.");
            }

            _context.productDetails.Remove(deleteProductDetail);
            await _context.SaveChangesAsync();

            return $"Chi tiết sản phẩm với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<ProductDetailModel>> GetAllProductDetailsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.productDetails!.CountAsync();

            var productDetails = await _context.productDetails!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<ProductDetailModel>>(productDetails);

            return new PagedResult<ProductDetailModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ProductDetailModel> GetProductDetailsAsync(int id)
        {
            var productDetails = await _context.productDetails.FindAsync(id);
            return _mapper.Map<ProductDetailModel>(productDetails);
        }

        public async Task UpdateProductDetailAsync(int id, ProductDetailModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.productDetails!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Chi tiết sản phẩm với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateProductDetail = _mapper.Map<ProductDetail>(model);

            _context.productDetails.Attach(updateProductDetail);
            _context.Entry(updateProductDetail).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
