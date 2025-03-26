using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;

namespace SWD392.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRoomAsync(CreateRoomDTO dto)
        {
            var room = new Room
            {
                RoomName = dto.RoomName,
                SlotMax = dto.SlotMax,
                SlotNow = 0,
                Status = "Available",
                DoctorId = dto.DoctorId,
                CheckinTime = dto.CheckinTime
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool Success, string Message)> CheckinCustomerAsync(CheckinCustomerDTO dto)
        {
            var room = await _context.Rooms.FindAsync(dto.RoomId);
            if (room == null) return (false, "Không tìm thấy phòng.");
            if (room.Status == "Full") return (false, "Phòng đã đầy.");

            var appointment = await _context.Appointments.FindAsync(dto.AppointmentId);
            if (appointment == null || appointment.Status != "Confirmed")
                return (false, "Lịch hẹn không hợp lệ hoặc chưa được xác nhận.");

            var alreadyCheckedIn = await _context.RoomCheckins
                .AnyAsync(rc => rc.AppointmentId == dto.AppointmentId);
            if (alreadyCheckedIn)
                return (false, "Khách hàng đã được checkin trước đó.");

            var checkin = new RoomCheckin
            {
                RoomId = dto.RoomId,
                AppointmentId = dto.AppointmentId,
                CustomerId = appointment.UserId,
                CheckinTime = DateTime.Now
            };

            room.SlotNow++;
            if (room.SlotNow >= room.SlotMax)
            {
                room.Status = "Full";
            }

            _context.RoomCheckins.Add(checkin);
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return (true, "Checkin thành công.");
        }
    }
}
