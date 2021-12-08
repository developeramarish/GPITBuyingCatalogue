﻿using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.AssociatedServices;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.AssociatedServices;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Validators
{
    public sealed class AddAssociatedServiceModelValidator : AbstractValidator<AddAssociatedServiceModel>
    {
        private readonly IAssociatedServicesService associatedServicesService;

        public AddAssociatedServiceModelValidator(IAssociatedServicesService associatedServicesService)
        {
            this.associatedServicesService = associatedServicesService;

            RuleFor(m => m)
                .MustAsync(NotBeADuplicateServiceName)
                .WithMessage("Associated Service name already exists. Enter a different name")
                .OverridePropertyName(m => m.Name);
        }

        private async Task<bool> NotBeADuplicateServiceName(AddAssociatedServiceModel model, CancellationToken cancellationToken)
        {
            _ = cancellationToken;

            return !(await associatedServicesService.AssociatedServiceExistsWithNameForSupplier(model.Name, model.SolutionId.SupplierId, default));
        }
    }
}