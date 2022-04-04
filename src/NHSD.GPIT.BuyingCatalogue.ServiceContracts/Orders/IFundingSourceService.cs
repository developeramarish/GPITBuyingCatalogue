﻿using System.Threading.Tasks;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;

namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.Orders
{
    public interface IFundingSourceService
    {
        Task SetFundingSource(CallOffId callOffId, string internalOrgId, bool? onlyGms, bool? confirmed = null);
    }
}
