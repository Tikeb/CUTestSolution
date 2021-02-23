using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UI.Interfaces.Services;
using UI.Models;

namespace UI.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory clientFactory;
        private JsonSerializerOptions apiJsonSerializerOptions;

        public HttpClientService(
            IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<APICallResult<T>> MakeRequest<T>(HttpMethod method, string endPoint)
        {
            using var request = new HttpRequestMessage(method, endPoint);
            using var client = this.clientFactory.CreateClient();

            HttpResponseMessage httpResponseMessage = await client.SendAsync(request, CancellationToken.None)
                .ConfigureAwait(true);

            APICallResult<T> apiCallResult = new APICallResult<T>
            {
                IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode,
                HttpStatusCode = httpResponseMessage.StatusCode,
            };

            string apiResponse = await this.GetAPIResponse(httpResponseMessage).ConfigureAwait(true);

            if (!string.IsNullOrEmpty(apiResponse))
            {
                apiCallResult.ResultObject = this.Deserialize<T>(apiResponse);
            }

            return apiCallResult;
        }

        public async Task<APICallResult<T>> MakeRequest<T>(HttpMethod method, string endPoint, string requestContent)
        {
            using var request = new HttpRequestMessage(method, endPoint)
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json"),
            };
            using var client = this.clientFactory.CreateClient();

            HttpResponseMessage httpResponseMessage = await client.SendAsync(request, CancellationToken.None)
                .ConfigureAwait(true);

            APICallResult<T> apiCallResult = new APICallResult<T>
            {
                IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode,
                HttpStatusCode = httpResponseMessage.StatusCode,
            };

            string apiResponse = await this.GetAPIResponse(httpResponseMessage).ConfigureAwait(true);
            if (!string.IsNullOrEmpty(apiResponse))
            {
                apiCallResult.ResultObject = this.Deserialize<T>(apiResponse);
            }

            return apiCallResult;
        }

        private async Task<string> GetAPIResponse(HttpResponseMessage httpResponseMessage)
        {
            string result = string.Empty;

            if (httpResponseMessage.IsSuccessStatusCode
                && httpResponseMessage.Content != null
                && httpResponseMessage.Content.Headers != null
                && httpResponseMessage.Content.Headers.ContentLength > 0)
            {
                result = await this.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(true);
            }

            return result;
        }

        private async Task<string> ReadAsStringAsync(HttpResponseMessage responseMessage)
        {
            if (responseMessage == null)
            {
                throw new ArgumentNullException(nameof(responseMessage));
            }

            return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(true);
        }

        private T Deserialize<T>(string objectToBeDeserialized)
        {
            return JsonSerializer.Deserialize<T>(objectToBeDeserialized, this.ApiJsonSerializerOptions);
        }

        protected JsonSerializerOptions ApiJsonSerializerOptions
        {
            get
            {
                if (this.apiJsonSerializerOptions == null)
                {
                    this.apiJsonSerializerOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true,
                        PropertyNameCaseInsensitive = true,
                    };
                }

                return this.apiJsonSerializerOptions;
            }
        }
    }
}