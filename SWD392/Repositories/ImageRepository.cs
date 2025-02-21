using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ImageRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddImageAsync(ImageModel model)
        {
            var newImage = _mapper.Map<Image>(model);
            _context.images!.Add(newImage);
            await _context.SaveChangesAsync();
            return newImage.Id;
        }

        public async Task<string> DeleteImageAsync(int id)
        {
            var deleteImage = await _context.images!.FindAsync(id);

            if (deleteImage == null)
            {
                throw new KeyNotFoundException($"Hình ảnh với ID {id} không tìm thấy.");
            }

            _context.images.Remove(deleteImage);
            await _context.SaveChangesAsync();

            return $"Hình ảnh với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<ImageModel>> GetAllImagesAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.images!.CountAsync();

            var images = await _context.images!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<ImageModel>>(images);

            return new PagedResult<ImageModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        
        public async Task<ImageModel> GetImagesAsync(int id)
        {
            var images = await _context.images.FindAsync(id);
            return _mapper.Map<ImageModel>(images);
        }

        public async Task UpdateImageAsync(int id, ImageModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.images!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Hình ảnh với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateImage = _mapper.Map<Image>(model);

            _context.images.Attach(updateImage);
            _context.Entry(updateImage).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Image>> GetImagesByProductID(int productId)
        {
                return await _context.images
            .Where(img => img.ProductId == productId)
            .ToListAsync();
        }
    }
}
