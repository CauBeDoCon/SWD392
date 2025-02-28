using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.DTOs;

namespace SWD392.Repositories
{
    public interface IWalletRepository
    {
        Task<WalletDTO> GetWalletBalanceAsync(string userId);
        Task UpdateWalletBalanceAsync(WalletDTO walletDTO);
    }
}