using Borigran.OneData.Authorization.Domain;

namespace Borigran.OneData.Authorization
{
    public interface ISmsSender
    {
        SmsServiceResponse SendAuthCode(string phoneNumber, int code);
    }
}
