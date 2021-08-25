﻿using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Users.Models;

namespace NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations
{
    internal sealed class AspNetUserCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            static ISpecimenBuilder ComposerTransformation(ICustomizationComposer<AspNetUser> c) =>
                c.Without(u => u.AspNetUserClaims)
                    .Without(u => u.AspNetUserLogins)
                    .Without(u => u.AspNetUserRoles)
                    .Without(u => u.AspNetUserTokens);

            fixture.Customize<AspNetUser>(ComposerTransformation);
        }
    }
}