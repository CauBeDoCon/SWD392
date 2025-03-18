using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.DTOs.Pagination;

namespace SWD392.Repositories
{
    public interface IDiscountRepository
    {
        Task<PagedResult<DiscountDto>> GetAllDiscountAsync(int pageNumber, int pageSize);
        public Task<DiscountDto> GetDiscountAsync(int id);

        public Task<int> AddDiscountAsync(DiscountRequestDto model);

        public Task UpdateDiscountAsync(int id, DiscountDto model);
        public Task<string> DeleteDiscountAsync(int id);
        Task<List<DiscountDto>> GetDiscountALLAsync();
        Task UpdateDiscountStatusAsync(int id);
    }
}