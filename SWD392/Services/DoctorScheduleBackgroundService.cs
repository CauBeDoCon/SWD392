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
                _logger.LogInformation("Đang kiểm tra và tạo lịch mới cho bác sĩ...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
                    var doctorRepository = scope.ServiceProvider.GetRequiredService<IDoctorRepository>();

                    var doctors = await doctorRepository.GetAllDoctorsAsync();
                    foreach (var doctor in doctors)
                    {
                       
                        DateTime nextWeekSameDay = DateTime.Today.AddDays(7);

                
                        bool hasSchedule = await bookingRepository.HasScheduleForDateAsync(doctor.Id, nextWeekSameDay);

                        if (!hasSchedule)
                        {
                            await bookingRepository.CreateDoctorBookingsAsync(doctor.Id, nextWeekSameDay);
                            _logger.LogInformation($"Lịch làm việc đã được tạo cho bác sĩ {doctor.Id} vào ngày {nextWeekSameDay:yyyy-MM-dd}.");
                        }
                        else
                        {
                            _logger.LogInformation($"Lịch ngày {nextWeekSameDay:yyyy-MM-dd} của bác sĩ {doctor.Id} đã tồn tại, không cần tạo.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi cập nhật lịch bác sĩ: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

}
