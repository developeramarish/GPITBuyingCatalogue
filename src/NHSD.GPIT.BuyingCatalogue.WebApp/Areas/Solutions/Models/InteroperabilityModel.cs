﻿using System;
using System.Linq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Solutions.Models
{
    public class InteroperabilityModel : SolutionDisplayBaseModel
    {
        public InteroperabilityModel()
        {
        }

        public InteroperabilityModel(CatalogueItem catalogueItem, CatalogueItemContentStatus contentStatus)
            : base(catalogueItem, contentStatus)
        {
            if (catalogueItem is null)
                throw new ArgumentNullException(nameof(catalogueItem));

            var integrations = catalogueItem.Solution?.GetIntegrations();

            IntegrationsUrl = catalogueItem.Solution.IntegrationsUrl;
            IM1Integrations = integrations.Where(i => i.IntegrationType.EqualsIgnoreCase("IM1")).ToArray();
            IM1IntegrationQualifiers = IM1Integrations.Select(i => i.Qualifier).Distinct().ToArray();
            GpConnectIntegrations = integrations.Where(i => i.IntegrationType.EqualsIgnoreCase("GP Connect")).ToArray();
            GpConnectIntegrationQualifiers = GpConnectIntegrations.Select(i => i.Qualifier).Distinct().ToArray();
        }

        public Integration[] IM1Integrations { get; set; }

        public string[] IM1IntegrationQualifiers { get; set; }

        public Integration[] GpConnectIntegrations { get; set; }

        public string[] GpConnectIntegrationQualifiers { get; set; }

        public string IntegrationsUrl { get; set; }

        public override int Index => 7;
    }
}
