﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;

namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.Orders
{
    public interface IOrderPriceService
    {
        Task UpdatePrice(int orderId, CatalogueItemId catalogueItemId, List<OrderPricingTierDto> agreedPrices);

        Task UpsertPrice(int orderId, CataloguePrice price, List<OrderPricingTierDto> agreedPrices);
    }
}
