﻿using NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Common;
using OpenQA.Selenium;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.PublicBrowse
{
    internal static class SolutionsObjects
    {
        public static By SolutionList => ByExtensions.DataTestId("solution-cards");

        public static By SolutionsInList => ByExtensions.DataTestId("solution-card");

        public static By SolutionName => ByExtensions.DataTestId("solution-card-name");

        public static By SolutionSupplierName => ByExtensions.DataTestId("solution-card-supplier");

        public static By SolutionSummary => ByExtensions.DataTestId("solution-card-summary");

        public static By SolutionCapabilityList => ByExtensions.DataTestId("capability-list");

        public static By SortByLink => By.Id("sortby-link");

        public static By SolutionsLink => By.CssSelector("div.nhsuk-grid-column-two-thirds h2 a");

        public static By CapabilitesOverCountLink => ByExtensions.DataTestId("capabilities-overcount-link");

        public static By FilterSolutionsExpander => By.Id("nhsuk-details0");

        public static By FilterSolutionsFramework => By.Id("nhsuk-details1");
    }
}
