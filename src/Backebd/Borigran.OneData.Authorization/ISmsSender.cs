using Borigran.OneData.Authorization.Domain;

namespace Borigran.OneData.Authorization
{
    public interface ISmsSender
    {
        Task<SmsServiceResponse> SendAuthCodeAsync(string phoneNumber, int code);
    }
}
