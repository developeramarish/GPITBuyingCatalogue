﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.ClientApplicationTypeModels.MobileTabletBasedModels
{
    public sealed class MemoryAndStorageModel : ApplicationTypeBaseModel
    {
        public MemoryAndStorageModel()
        {
            MemorySizes = Framework.Constants.SelectLists.MemorySizes;
        }

        public MemoryAndStorageModel(CatalogueItem catalogueItem)
            : base(catalogueItem)
        {
            if (catalogueItem is null)
                throw new ArgumentNullException(nameof(catalogueItem));

            MemorySizes = Framework.Constants.SelectLists.MemorySizes;

            SelectedMemorySize = ClientApplication?.MobileMemoryAndStorage?.MinimumMemoryRequirement;
            Description = ClientApplication?.MobileMemoryAndStorage?.Description;
        }

        [Required(ErrorMessage = "Select a minimum memory size")]
        public string SelectedMemorySize { get; set; }

        public IReadOnlyList<SelectListItem> MemorySizes { get; init; }

        [Required(ErrorMessage = "Enter storage space information")]
        [StringLength(300)]
        public string Description { get; set; }
    }
}