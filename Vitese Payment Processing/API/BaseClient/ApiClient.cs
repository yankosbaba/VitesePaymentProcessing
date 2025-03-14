using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Reqnroll;
using Vitese_Payment_Processing.Logging;
using Vitese_Payment_Processing.Model;
using Vitese_Payment_Processing.UI.Pages;

namespace Vitese_Payment_Processing.API.BaseClient
{
    public class ApiClient : IApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ScenarioContext _scenarioContext;
        private readonly AppConfig _appConfig;
        ILogger _logger = LoggerFactory.CreateLogger(typeof(ApiClient));

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public ApiClient(ScenarioContext scenarioContext, IHttpClientFactory clientFactory, AppConfig appConfig, ILogger logger)
        {
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T> GetAsync<T>(string endpoint) where T : class
        {
            var client = _clientFactory.CreateClient();

            try
            {
                using var response = await client.GetAsync(endpoint).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"GET request failed with status code {response.StatusCode}. Response: {content}");
                }

                return typeof(T) == typeof(string)
                    ? (T)(object)content
                    : JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is SocketException)
            {
                _logger.Write("GET request failed for endpoint: " + endpoint, EventSeverity.Error);
                throw;
            }
        }

        public async Task<T> TestharnessPostAsync<T>(string endpoint, object requestObject) where T : class
        {
            var client = _clientFactory.CreateClient("testharness");

            var content = requestObject is string stringContent
                ? stringContent
                : JsonConvert.SerializeObject(requestObject, JsonSerializerSettings);

            using var requestContent = new StringContent(content, Encoding.UTF8, "application/json");

            try
            {
                using var response = await client.PostAsync(endpoint, requestContent).ConfigureAwait(false);
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"POST request failed with status code {response.StatusCode}. Response: {responseContent}");
                }

                return typeof(T) == typeof(string)
                    ? (T)(object)responseContent
                    : JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is SocketException)
            {
                _logger.Write("POST request failed for endpoint:" +endpoint, EventSeverity.Error);
                throw;
            }
        }
    }
}