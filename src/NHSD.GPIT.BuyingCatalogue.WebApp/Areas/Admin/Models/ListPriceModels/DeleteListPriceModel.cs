﻿using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.WebApp.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.ListPriceModels
{
    public sealed class DeleteListPriceModel : NavBaseModel
    {
        public DeleteListPriceModel()
        {
        }

        public DeleteListPriceModel(
            CatalogueItem item)
        {
            ItemName = item.Name;
        }

        public string ItemName { get; init; }
    }
}
