using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Vitese_Payment_Processing.UI.Pages
{
    public class LoginPage : BasePage
    {
        private readonly IPage _page;

        public LoginPage(IPage page):base(page)
        {
            _page = page;
        }

        public async Task NavigateAsync()
            => await _page.GotoAsync("BaseUrl");

        public async Task LoginAsync(string username, string password)
        {
            await _page.FillAsync("#username", username);
            await _page.FillAsync("#password", password);
            await _page.ClickAsync("#login-button");
        }
    }
}
