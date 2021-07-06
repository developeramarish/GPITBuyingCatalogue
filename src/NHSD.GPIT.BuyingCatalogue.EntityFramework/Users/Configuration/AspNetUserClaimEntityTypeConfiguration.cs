﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.GPITBuyingCatalogue;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Users.Configuration
{
    internal sealed class AspNetUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<AspNetUserClaim>
    {
        public void Configure(EntityTypeBuilder<AspNetUserClaim> builder)
        {
            builder.Property(uc => uc.UserId).IsRequired();

            builder.HasOne(uc => uc.User)
                .WithMany(u => u.AspNetUserClaims)
                .HasForeignKey(uc => uc.UserId);

            builder.HasIndex(uc => uc.UserId, "IX_AspNetUserClaims_UserId");
        }
    }
}
