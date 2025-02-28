using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;

namespace SWD392.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly ApplicationDbContext _context;

    public OrderDetailRepository(ApplicationDbContext context)
    {
        _context = context;
    }

        public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsAsync()
        {
            var orderDetails = await _context.OrderDetails
                .Select(od => new OrderDetailDTO
                {
                    Id = od.Id,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    Subtotal = od.Subtotal,
                    ImagesProduct = _context.images
                        .Where(img => img.ProductId == od.ProductId)
                        .Select(img => new ImageDTO
                        {
                            Id = img.Id,
                            ImageUrl = img.ImageUrl,
                            ProductId = img.ProductId
                        }).ToList() 
                })
                .ToListAsync(); // ✅ Đưa .ToListAsync() ra ngoài

            

            return orderDetails;
        }


        public async Task<List<OrderDetailDTO>> GetOrderDetailByIdAsync(int id)
        {
            var orderDetails = await _context.OrderDetails
            .Where(od => od.Id == id)
            .Select(od => new OrderDetailDTO
            {
                Id = od.Id,
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                Subtotal = od.Subtotal
            })
            .ToListAsync(); // ✅ Đưa .ToListAsync() ra ngoài để tránh lỗi

            // ✅ Tách phần lấy ảnh ra ngoài
            foreach (var detail in orderDetails)
            {
                detail.ImagesProduct = await _context.images
                    .Where(img => img.ProductId == detail.ProductId)
                    .Select(img => new ImageDTO
                    {
                        Id = img.Id,
                        ImageUrl = img.ImageUrl,
                        ProductId = img.ProductId
                    })
                    .ToListAsync();
            }

            return orderDetails;
        }

    public async Task<OrderDetail> CreateOrderDetailAsync(OrderDetailDTO orderDetailDto)
    {
        var orderDetail = new OrderDetail
        {
            OrderId = orderDetailDto.OrderId,
            ProductId = orderDetailDto.ProductId,
            Quantity = orderDetailDto.Quantity,
            Subtotal = orderDetailDto.Subtotal
        };

        _context.OrderDetails.Add(orderDetail);
        await _context.SaveChangesAsync();

        return orderDetail;
    }


    public async Task<List<OrderDetailDTO>> GetOrderDetailByOrderIdAsync(int id)
    {
        var orderDetails = await _context.OrderDetails
            .Include(od => od.Product.Images) // Lấy luôn danh sách ảnh
            .Where(od => od.OrderId == id)
            .Select(od => new OrderDetailDTO
            {
                Id = od.Id,
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                Subtotal = od.Subtotal,
                ImagesProduct = od.Product.Images.Select(i => new ImageDTO
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    ProductId = i.ProductId
                }).ToList()
            })
            .ToListAsync();

        return orderDetails;
    }

}
}