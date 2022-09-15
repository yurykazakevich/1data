using System.Net.Http;
using System.Threading.Tasks;

namespace Borigran.OneData.Platform.Http
{
    public interface IHttpHelper
    {
        Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request);
    }
}
