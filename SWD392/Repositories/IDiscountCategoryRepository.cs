using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.DTOs.Pagination;

namespace SWD392.Repositories
{
    public interface IDiscountCategoryRepository
    {
        Task<PagedResult<DiscountCategoryResponseDto>> GetAllDiscountCategorysAsync(int pageNumber, int pageSize);
        public Task<DiscountCategoryDto> GetDiscountCategorysAsync(int id);

        public Task<int> AddDiscountCategoryAsync(DiscountCategoryDto model);

        public Task UpdateDiscountCategoryAsync(int id, DiscountCategoryDto model);
        public Task<string> DeleteDiscountCategoryAsync(int id);
    }
}