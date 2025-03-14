using Vitese_Payment_Processing.API.BaseClient;
using Vitese_Payment_Processing.Logging;
using Vitese_Payment_Processing.Model;
using Xunit;
using Reqnroll;

namespace Vitese_Payment_Test
{
    [Binding]
    public class ApiPaymentTests : IClassFixture<NUnit.Framework.Internal.TestFixture>
    {
        private IApiClient _apiClient;
        private ILogger _logger = LoggerFactory.CreateLogger(typeof(ApiPaymentTests));
        private AppConfig _config;
        private ScenarioContext _scenarioContext;
        private PaymentResponse _response;

        public ApiPaymentTests(IApiClient apiClient, AppConfig config, ScenarioContext scenarioContext)
        {
            _apiClient = apiClient;
            _config = config;
            _scenarioContext = scenarioContext;
        }

        [Fact]
        public async Task PaymentFinalization_Success()
        {
            // Arrange
            _config = _scenarioContext.Get<AppConfig>();
            _apiClient = _scenarioContext.Get<ApiClient>();
            var request = new PaymentRequest
            {
                PaymentId = Guid.NewGuid().ToString(),
                Amount = 100.00m,
                Currency = "£"
            };

            // Act
            var response = await _apiClient.TestharnessPostAsync<PaymentResponse>("endpoint",request);

            // Assert
            Assert.AreEqual("Settled", response.Status);
            Assert.NotNull(response.TransactionId);
            _logger.Write("Verified successful payment finalization for payment Id:" + request.PaymentId, EventSeverity.Informational);
        }
        public async Task GivenAGETRequestIsMadeEndPointOne(string p0)
        {
            _config = _scenarioContext.Get<AppConfig>();
            _response = await _apiClient.GetAsync<PaymentResponse>($"{_config.TestHarness.PaymentProcessingEndpoint}" + p0);
        }
    }
}
