using SWD392.Models;

namespace SWD392.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context,VnPaymentRequestModel model,string userId);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
