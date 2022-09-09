using Borigran.OneData.Authorization.Domain;
using Borigran.OneData.Platform.Http;

namespace Borigran.OneData.Authorization.Impl
{
    public class SmsSender : ISmsSender
    {
        private readonly IHttpHelper httpHelper;

        public SmsSender(IHttpHelper httpHelper)
        {
            this.httpHelper = httpHelper;
        }

        public SmsServiceResponse SendAuthCode(string phoneNumber, int code)
        {
            return new SmsServiceResponse();
        }
    }
}
