using SWD392.Models;

namespace SWD392.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context,VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
