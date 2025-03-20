using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _context;

    public AppointmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
    {
        return await _context.Appointments.ToListAsync();
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
    {
        return await _context.Appointments.FindAsync(appointmentId);
    }

    public async Task<Appointment?> CreateAppointmentAsync(string userId, AppointmentDTO appointmentDto)
    {
        var package = await _context.Packages.FindAsync(appointmentDto.PackageId);
        if (package == null) return null;

        var newAppointment = new Appointment
        {
            UserId = userId,
            PackageId = appointmentDto.PackageId,
            StartDate = DateTime.Now,
            Status = "Pending"
        };

        _context.Appointments.Add(newAppointment);
        await _context.SaveChangesAsync();

        await _context.Database.ExecuteSqlRawAsync(
            "UPDATE AspNetUsers SET AppointmentId = {0} WHERE Id = {1}", newAppointment.Id, userId
        );

        return newAppointment;
    }

    public async Task<(bool Success, string Message)> ConfirmAppointmentAsync(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Package)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment == null)
        {
            return (false, "Không tìm thấy Appointment.");
        }
        if (appointment.Status == "Confirmed")
        {
            return (false, "Lịch hẹn đã được xác nhận trước đó. Không thể xác nhận lại.");
        }
        var packageSessions = await _context.PackageSessions
            .Where(ps => ps.PackageId == appointment.PackageId)
            .FirstOrDefaultAsync();

        if (packageSessions == null)
        {
            return (false, "Không tìm thấy PackageSessions phù hợp.");
        }

        var startDate = DateTime.Now;

        var treatmentSessions = new List<TreatmentSession>();
        var packageTrackings = new List<PackageTracking>();

        for (int i = 1; i <= appointment.Package.Sessions; i++)
        {
            var treatmentSession = new TreatmentSession
            {
                AppointmentId = appointment.Id,
                SessionNumber = i,
                Date = startDate.AddDays(i * 7),
                TimeSlot = i switch
                {
                    1 => packageSessions.TimeSlot1,
                    2 => packageSessions.TimeSlot2,
                    3 => packageSessions.TimeSlot3,
                    4 => packageSessions.TimeSlot4,
                    _ => null
                },
                Description = i switch
                {
                    1 => packageSessions.Description1,
                    2 => packageSessions.Description2,
                    3 => packageSessions.Description3,
                    4 => packageSessions.Description4,
                    _ => null
                },
                Status = "Scheduled"
            };

            treatmentSessions.Add(treatmentSession);
        }

        await _context.TreatmentSessions.AddRangeAsync(treatmentSessions);
        await _context.SaveChangesAsync();

        var createdSessions = await _context.TreatmentSessions
            .Where(ts => ts.AppointmentId == appointment.Id)
            .ToListAsync();

        foreach (var session in createdSessions)
        {
            var tracking = new PackageTracking
            {
                TreatmentSessionId = session.Id,
                Date = session.Date,
                TimeSlot = session.TimeSlot,
                Status = "In Progress",
                Description = session.Description
            };

            packageTrackings.Add(tracking);
        }

        await _context.PackageTrackings.AddRangeAsync(packageTrackings);

        // 🔥 Cập nhật trạng thái của Appointment thành "Confirmed"
        appointment.Status = "Confirmed";
        _context.Appointments.Update(appointment);

        await _context.SaveChangesAsync();

        return (true, "Lịch hẹn đã được xác nhận, các buổi điều trị và theo dõi đã được tạo.");
    }





    public async Task<(bool Success, string Message)> CancelAppointmentAsync(int appointmentId)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null)
        {
            return (false, "Không tìm thấy lịch hẹn.");
        }

        if (appointment.Status == "Cancelled")
        {
            return (false, "Lịch hẹn đã bị hủy trước đó.");
        }

        appointment.Status = "Cancelled";
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
        return (true, "Lịch hẹn đã bị hủy.");
    }
}
