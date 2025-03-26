using SWD392.DB;
using SWD392.DTOs;

namespace SWD392.Repositories
{
    public interface IRoomRepository
    {
        Task<bool> CreateRoomAsync(CreateRoomDTO dto);
        Task<(bool Success, string Message)> CheckinCustomerAsync(CheckinCustomerDTO dto);
        Task<List<RoomDTO>> GetAllRoomsAsync();


    }
}
