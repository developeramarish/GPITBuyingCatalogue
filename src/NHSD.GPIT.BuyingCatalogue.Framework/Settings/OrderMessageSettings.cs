﻿using System.Diagnostics.CodeAnalysis;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Email;

namespace NHSD.GPIT.BuyingCatalogue.Framework.Settings
{
    [ExcludeFromCodeCoverage]
    public sealed class OrderMessageSettings
    {
        public EmailMessageTemplate EmailMessageTemplate { get; set; }

        public EmailAddressTemplate Recipient { get; set; }
    }
}