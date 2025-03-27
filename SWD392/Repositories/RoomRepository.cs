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
        public async Task<List<RoomDTO>> GetAllRoomsAsync()
        {
            return await _context.Rooms
                .Include(r => r.Doctor)
                .Include(r => r.Package)
                .Select(r => new RoomDTO
                {
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    TimeSlot = r.TimeSlot,
                    SlotMax = r.SlotMax,
                    SlotNow = r.SlotNow,
                    Status = r.Status,
                    DoctorName = r.Doctor.FirstName + " " + r.Doctor.LastName,
                    PackageName = r.Package != null ? r.Package.Name : "Không xác định" 
                })
                .ToListAsync();
        }


        public async Task<bool> CreateRoomAsync(CreateRoomDTO dto)
        {
            var room = new Room
            {
                RoomName = dto.RoomName,
                TimeSlot = dto.TimeSlot,
                SlotMax = dto.SlotMax,
                SlotNow = 0,
                Status = "Available",
                DoctorId = dto.DoctorId,
                CheckinTime = DateTime.Now,
                PackageId = dto.PackageId 
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

        public async Task<bool> UpdateRoomAsync(int roomId, CreateRoomDTO dto)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            room.RoomName = dto.RoomName;
            room.TimeSlot = dto.TimeSlot;
            room.SlotMax = dto.SlotMax;
            room.DoctorId = dto.DoctorId;
            room.PackageId = dto.PackageId;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoomAsync(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            if (room.SlotNow > 0)
            {
                return false;
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
