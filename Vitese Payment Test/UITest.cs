using Microsoft.Playwright;
using Reqnroll;
using Vitese_Payment_Processing.Commons.PaymentMethod;
using Vitese_Payment_Processing.Model;
using Vitese_Payment_Processing.UI.BaseClient;
using Vitese_Payment_Processing.UI.Pages;
using Xunit;

namespace Vitese_Payment_Processing.UI.Tests
{
    public class PaymentProcessingPageTests : PageTest
    {
        private IBrowserClient _browser;
        private IPage _page;
        private PaymentProcessingPage _paymentPage;
        private ScenarioContext _scenarioContext;


        public PaymentProcessingPageTests(IBrowserClient browser, IPage page, ScenarioContext scenarioContext)
        {
            _browser = browser;
            _page = page;
            _scenarioContext = scenarioContext;
        }

        public async Task InitializeAsync()
        {
            _browser = _scenarioContext.Get<BrowserClient>();

            _page = await _browser.InitalizePlaywright();
            await _page.GotoAsync("");

            _paymentPage = new PaymentProcessingPage(_page);
        }

        public async Task DisposeAsync()
        {
        }

        [Fact]
        public async Task SelectCountry_ShouldUpdateCountrySelection()
        {
            // Arrange
            const string testCountry = "GB";

            // Act
            await _paymentPage.SelectCountry(testCountry);

            // Assert
            var selectedCountry = await _page.EvaluateAsync<string>(
                "() => document.querySelector('[data-testid=\"country-select\"]').value");
            Assert.AreEqual(testCountry, selectedCountry);
        }

        [Fact]
        public async Task SelectPaymentMethod_ShouldDisplayCorrectPaymentForm()
        {
            // Arrange
            var paymentMethod = PaymentMethod.PayPal;

            // Act
            await _paymentPage.SelectPaymentMethod(paymentMethod);

            // Assert
            var isVisible = await _page.IsVisibleAsync("[data-testid='paypal-email']");
            Assert.True(isVisible);
        }

        [Fact]
        public async Task SubmitPayment_ShouldShowSuccessStatus()
        {
            // Arrange
            var paymentDetails = new PaymentDetails
            {
                PaymentMethod = "PayPal UK",
                Email = "test@example.com",
                Password = "securepassword123"
            };

            // Act
            await _paymentPage.SelectCountry("GB");
            await _paymentPage.SelectPaymentMethod(PaymentMethod.PayPal);
            await _paymentPage.FillPaymentDetails(paymentDetails);
            await _paymentPage.SubmitPayment();

            // Assert
            var status = await _paymentPage.GetPaymentStatus();
            Assert.AreEqual("Payment Successful", status);
        }

        [Fact]
        public async Task FillCreditCardDetails_ShouldPopulateFormFields()
        {
            // Arrange
            var paymentDetails = new PaymentDetails
            {
                PaymentMethod = "CreditCard",
                CardNumber = "4111111111111111",
                Expiry = "12/25",
                CVV = "123"
            };

            // Act
            await _paymentPage.SelectPaymentMethod(PaymentMethod.CreditCard);
            await _paymentPage.FillPaymentDetails(paymentDetails);

            // Assert
            var cardNumber = await _page.InputValueAsync("[data-testid='card-number']");
            var expiry = await _page.InputValueAsync("[data-testid='expiry-date']");
            var cvv = await _page.InputValueAsync("[data-testid='cvv']");

            Assert.AreEqual(paymentDetails.CardNumber, cardNumber);
            Assert.AreEqual(paymentDetails.Expiry, expiry);
            Assert.AreEqual(paymentDetails.CVV, cvv);
        }

        [Fact]
        public async Task SelectInvalidPaymentMethod_ShouldThrowException()
        {
            // Arrange
            var invalidDetails = new PaymentDetails
            {
                PaymentMethod = "InvalidMethod"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _paymentPage.FillPaymentDetails(invalidDetails));
        }
    }
}