using Microsoft.Playwright;

namespace Vitese_Payment_Processing.UI.BaseClient
{
    public interface IBrowserClient
    {
        Task<IPage> InitalizePlaywright();
        Task<IPage> InitalizePlaywrightTracingAsync();
    }
}
