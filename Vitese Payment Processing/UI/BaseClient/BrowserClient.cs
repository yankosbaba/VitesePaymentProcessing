using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Playwright;
using Microsoft.VisualBasic;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace Vitese_Payment_Processing.UI.BaseClient
{
    public class BrowserClient : IBrowserClient
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        public IPage? _page { get; set; }
        public IBrowserContext? _context { get; set; }
        private readonly IConfiguration? _configuration;

        public BrowserClient(IPlaywright? playwright, IBrowser? browser, IPage? page, IBrowserContext? context, IConfiguration configuration)
        {
            _playwright = playwright;
            _browser = browser;
            _page = page;
            _context = context;
            _configuration = configuration;
        }

        public async Task<IPage> InitalizePlaywright()
        {
            var browser = await InitBrowserAsync();
            _context = await browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            return _page;
        }
        private async Task<IBrowser> InitBrowserAsync()
        {
            _playwright = await Playwright.CreateAsync();

            string? browserType = _configuration["BROWSER_TYPE"];
            BrowserTypeLaunchOptions launchOptions = new BrowserTypeLaunchOptions { Headless = false };
            return browserType switch
            {
                "Chromium" => await _playwright.Chromium.LaunchAsync(launchOptions),
                "Firefox" => await _playwright.Firefox.LaunchAsync(launchOptions),
                "Webkit" => await _playwright.Webkit.LaunchAsync(launchOptions),
                _ => await _playwright.Chromium.LaunchAsync(launchOptions)
            };
        }
        public async Task<IPage> InitalizePlaywrightTracingAsync()
        {
            var browser = await InitBrowserAsync();
            _context = await browser.NewContextAsync();
            // Sample for tracing
            await _context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true
            });
            _page = await _context.NewPageAsync();
            return  _page;
        }

        public async Task DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }
    }
}
