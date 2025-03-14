using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Vitese_Payment_Processing.UI.Pages
{
    public class BasePage
    {
        protected IPage Page;

        private ILocator _viteseHome;
        private ILocator _logoutMenuItem;

        public BasePage(IPage page)
        {
            this.Page = page;
            _viteseHome = Page.Locator("");
            _logoutMenuItem = Page.Locator("#logout_sidebar_link");

        }

        public async Task GotoPageAsync(string page)
        {
            await Page.GotoAsync(page);
        }
    }
}
