using SWD392.DB;
using SWD392.DTOs;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    Task<Appointment?> GetAppointmentByIdAsync(int appointmentId);
    Task<Appointment?> CreateAppointmentAsync(string userId, AppointmentDTO appointmentDto);

    Task<(bool Success, string Message)> ConfirmAppointmentAsync(int appointmentId);
    Task<(bool Success, string Message)> CancelAppointmentAsync(int appointmentId);

    Task<CustomerAppointmentDTO?> GetCustomerAppointmentAsync(string userId);

    Task<bool> UpdatePackageTrackingAsync(int trackingId, UpdatePackageTrackingDTO updateDto);

    Task<List<PackageTrackingDTO>> GetMyPackageTrackingsAsync(string userId);

}
