﻿using System.Diagnostics.CodeAnalysis;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Identity;

namespace NHSD.GPIT.BuyingCatalogue.FinalMigration
{
    [ExcludeFromCodeCoverage]
    internal sealed class IdentityServiceStub : IIdentityService
    {
        public int? GetUserId()
        {
            return null;
        }
    }
}