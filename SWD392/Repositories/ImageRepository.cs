using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
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

        public async Task DeleteImageAsync(int id)
        {
            var deleteImg = _context.images!.SingleOrDefault(s => s.Id == id);
            if (deleteImg != null)
            {
                _context.images!.Remove(deleteImg);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ImageModel>> GetAllImagesAsync()
        {
            var images = await _context.images!.ToListAsync();
            return _mapper.Map<List<ImageModel>>(images);
        }

        public async Task<ImageModel> GetImagesAsync(int id)
        {
            var images = await _context.images.FindAsync(id);
            return _mapper.Map<ImageModel>(images);
        }

        public async Task UpdateImageAsync(int id, ImageModel model)
        {
            if (id == model.Id)
            {
                var updateImage = _mapper.Map<Image>(model);
                _context.images!.Update(updateImage);
                await _context.SaveChangesAsync();

            }
        }
    }
}
