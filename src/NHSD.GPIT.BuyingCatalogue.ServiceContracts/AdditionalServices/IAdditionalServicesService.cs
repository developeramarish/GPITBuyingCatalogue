﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;

namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.AdditionalServices
{
    public interface IAdditionalServicesService
    {
        Task<List<CatalogueItem>> GetAdditionalServicesBySolutionId(CatalogueItemId catalogueItemId);

        Task<List<CatalogueItem>> GetAdditionalServicesBySolutionIds(IEnumerable<CatalogueItemId> solutionIds);
    }
}
