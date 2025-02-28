using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;

namespace SWD392.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public WalletRepository(ApplicationDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<WalletDTO> GetWalletBalanceAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Wallet == null)
            {
                return null; // Xử lý tại Controller
            }

            return new WalletDTO
            {
                WalletId = user.Wallet.WalletId,
                AmountofMoney = (int)user.Wallet.AmountOfMoney
            };
        }

        public async Task UpdateWalletBalanceAsync(WalletDTO walletDTO)
        {
             var existingEntity = await _context.Wallets!.FindAsync(walletDTO.WalletId);
           
             if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Danh mục với ID {walletDTO.WalletId} không tìm thấy.");
            }
            // ✅ Sử dụng AutoMapper để cập nhật dữ liệu trực tiếp
            _mapper.Map(walletDTO, existingEntity);

            // ✅ Đánh dấu entity đã chỉnh sửa để EF theo dõi
            _context.Wallets.Update(existingEntity);
            await _context.SaveChangesAsync();

        }
    }
}