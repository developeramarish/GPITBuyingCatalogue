﻿using System;
using System.Linq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.BuyingCatalogue;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models.BrowserBased
{
    public class SupportedBrowsersModel : MarketingBaseModel
    {
        public SupportedBrowsersModel() : base(null)
        {
        }

        public SupportedBrowsersModel(CatalogueItem catalogueItem) : base(catalogueItem)
        {
            if (catalogueItem is null)
                throw new ArgumentNullException(nameof(catalogueItem));

            BackLink = $"/marketing/supplier/solution/{CatalogueItem.CatalogueItemId}/section/browser-based";

            Browsers = new SupportedBrowserModel[]
            {
                new() { BrowserName = "Google Chrome" },
                new() { BrowserName = "Microsoft Edge" },
                new() { BrowserName = "Mozilla Firefox" },
                new() { BrowserName = "Opera" },
                new() { BrowserName = "Safari" },
                new() { BrowserName = "Chromium" },
                new() { BrowserName = "Internet Explorer 11" },
                new() { BrowserName = "Internet Explorer 10" }
            };

            CheckBrowsers();

            if (ClientApplication.MobileResponsive.HasValue)
                MobileResponsive = ClientApplication.MobileResponsive.ToYesNo();
        }

        public override bool? IsComplete =>
            ClientApplication?.BrowsersSupported != null && ClientApplication.BrowsersSupported.Any() &&
            ClientApplication.MobileResponsive.HasValue;

        public SupportedBrowserModel[] Browsers { get; set; }

        public string MobileResponsive { get; set; }

        private void CheckBrowsers()
        {
            foreach( var browser in Browsers )
            {
                if(ClientApplication.BrowsersSupported != null && 
                    ClientApplication.BrowsersSupported.Any(x=>x.Equals(browser.BrowserName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    browser.Checked = true;
                }
            }
        }
    }
}
