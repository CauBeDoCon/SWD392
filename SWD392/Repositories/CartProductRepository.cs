using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class CartProductRepository : ICartProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;


        public CartProductRepository(ApplicationDbContext context, IMapper mapper, IProductRepository productRepository)
        {
            _context = context;
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<int> AddCartProductAsync(CartProductModel model)
        {
            var productnumber = _context.products
                .Where(p => p.Id == model.ProductId)
                .Select(p => p.Quantity)
                .FirstOrDefault();
            if (productnumber- model.Quantity <0)
            {   
                throw new Exception("so luong hang ko du");
            }
            var newCartProduct = _mapper.Map<CartProduct>(model);
            _context.cartProducts!.Add(newCartProduct);
            await _context.SaveChangesAsync();
            return newCartProduct.Id ;
        }

        public async Task<string> DeleteCartProductAsync(int id)
        {
            var deleteCartProduct = await _context.cartProducts!.FindAsync(id);

            if (deleteCartProduct == null)
            {
                throw new KeyNotFoundException($"Sản phẩm trong giỏ hàng với ID {id} không tìm thấy.");
            }

            _context.cartProducts.Remove(deleteCartProduct);
            await _context.SaveChangesAsync();

            return $"Sản phẩm trong giỏ hàng với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<CartProductModel>> GetAllCartProductsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.cartProducts!.CountAsync();

            var cartProducts = await _context.cartProducts!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<CartProductModel>>(cartProducts);

            return new PagedResult<CartProductModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<CartProductModel> CheckProductExistInCart(int cartid, int ProductId, CartProductModel model)
        {
            var checkProduct = await _context.cartProducts!.FirstOrDefaultAsync(cp => cp.CartId == cartid && cp.ProductId == ProductId);
            if (checkProduct == null)
            {
                var newProductId = await AddCartProductAsync(model);
                checkProduct = await _context.cartProducts!.FindAsync(newProductId);
            }
            else
            {
                checkProduct.Quantity += model.Quantity;
                _context.Entry(checkProduct).Property(p => p.Quantity).IsModified = true;
            }
            await _context.SaveChangesAsync();
            return checkProduct != null ? _mapper.Map<CartProductModel>(checkProduct) : null;
        }
        public async Task<CartProductModel> GetCartProductsAsync(int id)
        {
            var cartProduct = await _context.cartProducts.FindAsync(id);

            if (cartProduct == null)
            {
                throw new KeyNotFoundException($"Sản phẩm trong giỏ hàng với ID {id} không tìm thấy.");
            }

            return _mapper.Map<CartProductModel>(cartProduct);
        }

        public async Task UpdateCartProductAsync(int id, CartProductModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.cartProducts!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Sản phẩm trong giỏ hàng với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateCartProduct = _mapper.Map<CartProduct>(model);

            _context.cartProducts.Attach(updateCartProduct);
            _context.Entry(updateCartProduct).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
