﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Suppliers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.SupplierModels;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Validators
{
    public sealed class EditContactModelValidator : AbstractValidator<EditContactModel>
    {
        private readonly ISuppliersService suppliersService;

        public EditContactModelValidator(ISuppliersService suppliersService)
        {
            this.suppliersService = suppliersService;

            RuleFor(m => m)
                .MustAsync(NotBeADuplicateContact)
                .WithMessage("A contact with these contact details already exists for this supplier")
                .OverridePropertyName("edit-contact");
        }

        private async Task<bool> NotBeADuplicateContact(EditContactModel model, CancellationToken cancellationToken)
        {
            _ = cancellationToken;

            var supplier = await suppliersService.GetSupplier(model.SupplierId);

            return !supplier.SupplierContacts
                .Where(sc => sc.Id != model.ContactId)
                .Any(sc => string.Equals(sc.FirstName.Trim(), model.FirstName?.Trim(), StringComparison.CurrentCultureIgnoreCase)
                    && string.Equals(sc.LastName.Trim(), model.LastName?.Trim(), StringComparison.CurrentCultureIgnoreCase)
                    && string.Equals(sc.Email.Trim(), model.Email?.Trim(), StringComparison.CurrentCultureIgnoreCase)
                    && string.Equals(sc.PhoneNumber.Trim(), model.PhoneNumber?.Trim(), StringComparison.CurrentCultureIgnoreCase)
                    && string.Equals(sc.Department.Trim(), model.Department?.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
    }
}