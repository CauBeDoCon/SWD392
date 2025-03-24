using SWD392.DB;
using SWD392.DTOs;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    Task<Appointment?> GetAppointmentByIdAsync(int appointmentId);
    Task<(bool Success, string Message, Appointment? Appointment)> CreateAppointmentAsync(string userId, AppointmentDTO appointmentDto);


    Task<(bool Success, string Message)> ConfirmAppointmentAsync(int appointmentId);
    Task<(bool Success, string Message)> CancelAppointmentAsync(int appointmentId);

    Task<CustomerAppointmentDTO?> GetCustomerAppointmentAsync(string userId);

    Task<bool> UpdatePackageTrackingAsync(int trackingId, UpdatePackageTrackingDTO updateDto);

    Task<List<PackageTrackingDTO>> GetMyPackageTrackingsAsync(string userId);
    Task<List<DoctorAppointmentDTO>> GetDoctorAppointmentsAsync(string doctorId);
    Task<List<ConfirmedAppointmentWithTrackingDTO>> GetConfirmedAppointmentsWithTrackingAsync();
    Task<CustomerTreatmentScheduleDTO?> GetCustomerScheduleByPhoneAsync(string phoneNumber);
    Task<bool> CheckinTreatmentSessionAsync(int trackingId, CheckinTrackingDTO dto);
    Task<(bool Success, string Message)> DeleteCompletedAppointmentAsync(int appointmentId, string userId, string role);



}
