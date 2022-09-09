using System.Net.Http;
using System.Threading.Tasks;

namespace Borigran.OneData.Platform.Http
{
    public interface IHttpHelper
    {
        HttpResponseMessage SendWebRequest(HttpRequestMessage request);

        Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request);
    }
}
