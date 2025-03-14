using Microsoft.Playwright;
using Vitese_Payment_Processing.Commons.PaymentMethod;
using Vitese_Payment_Processing.Logging;
using Vitese_Payment_Processing.Model;

namespace Vitese_Payment_Processing.UI.Pages
{
    public class PaymentProcessingPage : BasePage
    {
        private readonly IPage _page;
        ILogger _logger = LoggerFactory.CreateLogger(typeof(PaymentProcessingPage));

        public PaymentProcessingPage(IPage page) :base(page)
        {
            _page = page;
        }
        public async Task SelectCountry(string country)
        {
            _logger.Write($"Selecting country: {country}", EventSeverity.Informational);
            await _page.SelectOptionAsync("[data-testid='country-select']", new SelectOptionValue { Value = country });
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task SelectPaymentMethod(PaymentMethod methodName)
        {
            _logger.Write($"Selecting payment method: {methodName}", EventSeverity.Informational);
            var selector = $"[data-testid='payment-method']:has-text('{methodName}')";
            await _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            await _page.ClickAsync(selector);
        }

        public async Task SubmitPayment()
        {
            _logger.Write("Submitting payment", EventSeverity.Informational);
            await _page.ClickAsync("[data-testid='submit-payment']");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task<string> GetPaymentStatus()
        {
            return await _page.InnerTextAsync("[data-testid='payment-status']");
        }

        public async Task FillPaymentDetails(PaymentDetails details)
        {
            switch (details.PaymentMethod)
            {
                case "PayPal UK":
                    await FillPayPalDetails(details.Email, details.Password);
                    break;
                case "CreditCard":
                    await FillCreditCardDetails(details.CardNumber, details.Expiry, details.CVV);
                    break;
                default:
                    throw new ArgumentException($"Unsupported payment method: {details.PaymentMethod}");
            }
        }

        private async Task FillPayPalDetails(string email, string password)
        {
            _logger.Write("Filling PayPal UK details", EventSeverity.Informational);
            await _page.FillAsync("[data-testid='paypal-email']", email);
            await _page.FillAsync("[data-testid='paypal-password']", password);
        }

        private async Task FillCreditCardDetails(string cardNumber, string expiry, string cvv)
        {
            await _page.FillAsync("[data-testid='card-number']", cardNumber);
            await _page.FillAsync("[data-testid='expiry-date']", expiry);
            await _page.FillAsync("[data-testid='cvv']", cvv);
        }
    }
}
