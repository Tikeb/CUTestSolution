using System.Net.Http;
using System.Threading.Tasks;
using UI.Models;

namespace UI.Interfaces.Services
{
    /// <summary>
    /// IHttpClientService interface.
    /// </summary>
    public interface IHttpClientService
    {
        Task<APICallResult<T>> MakeRequest<T>(HttpMethod method, string endPoint);

        Task<APICallResult<T>> MakeRequest<T>(HttpMethod method, string endPoint, string requestContent);
    }
}