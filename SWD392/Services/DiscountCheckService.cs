using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.Repositories;

namespace SWD392.Services
{
    public class DiscountCheckService : BackgroundService 
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DiscountCheckService> _logger;
        public DiscountCheckService(IServiceProvider serviceProvider, ILogger<DiscountCheckService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested){
                try{
                    _logger.LogInformation("Đang kiểm tra discount...");
                     using (var scope = _serviceProvider.CreateScope())
                    {
                        var discountRepository = scope.ServiceProvider.GetRequiredService<IDiscountRepository>();
                        var discounts = await discountRepository.GetDiscountALLAsync();
                        foreach(var discount in discounts){  
                            if(discount.max_usage == 0){
                                await discountRepository.UpdateDiscountStatusAsync(discount.id);
                               _logger.LogInformation($"✅ Đã dung discount {discount.id} do hết lượt sử dụng.");
                            } 
                            if(DateTime.Now >= discount.EndDate){
                                await discountRepository.UpdateDiscountStatusAsync(discount.id);
                                _logger.LogInformation($"✅ Đã dung discount {discount.id} do hết hạn vào {discount.EndDate:yyyy-MM-dd}.");
                            }
                        }
                    }
                }catch(Exception e){
                    _logger.LogError($"Error occurred while checking discount: {e.Message}");
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
}