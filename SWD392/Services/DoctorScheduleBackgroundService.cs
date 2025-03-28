using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SWD392.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

public class DoctorScheduleBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DoctorScheduleBackgroundService> _logger;

    public DoctorScheduleBackgroundService(IServiceProvider serviceProvider, ILogger<DoctorScheduleBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Đang kiểm tra và cập nhật lịch cho bác sĩ...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
                    var doctorRepository = scope.ServiceProvider.GetRequiredService<IDoctorRepository>();

                    var doctors = await doctorRepository.GetAllDoctorsAsync();
                    DateTime expiredDate = DateTime.Today.AddDays(-1);      
                    DateTime nextDateToCreate = DateTime.Today.AddDays(7);   

                    foreach (var doctor in doctors)
                    {

                        bool hasOldSchedule = await bookingRepository.HasScheduleForDateAsync(doctor.Id, expiredDate);
                        if (hasOldSchedule)
                        {
                            await bookingRepository.DeleteDoctorBookingsForDateAsync(doctor.Id, expiredDate);
                            _logger.LogInformation($"🗑️ Đã xóa lịch khám ngày {expiredDate:yyyy-MM-dd} của bác sĩ {doctor.Id}.");
                        }

                        bool hasNewSchedule = await bookingRepository.HasScheduleForDateAsync(doctor.Id, nextDateToCreate);
                        if (!hasNewSchedule)
                        {
                            await bookingRepository.CreateDoctorBookingsAsync(doctor.Id, 1);
                            _logger.LogInformation($"✅ Đã tạo lịch cho bác sĩ {doctor.Id} vào ngày {nextDateToCreate:yyyy-MM-dd}.");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Lỗi khi cập nhật lịch bác sĩ: {ex.Message}");
            }

            await DelayUntilMidnight(stoppingToken);
        }
    }

    private async Task DelayUntilMidnight(CancellationToken stoppingToken)
    {
        var now = DateTime.Now;
        var midnight = now.Date.AddDays(1);
        var delay = midnight - now;

        _logger.LogInformation($"Service sẽ chạy lại vào {midnight:yyyy-MM-dd HH:mm:ss}");

        await Task.Delay(delay, stoppingToken);
    }



}
