﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Configuration
{
    internal sealed class ServiceRecipientEntityTypeConfiguration : IEntityTypeConfiguration<ServiceRecipient>
    {
        public void Configure(EntityTypeBuilder<ServiceRecipient> builder)
        {
            builder.ToTable("ServiceRecipients", Schemas.Ordering);

            builder.HasKey(r => r.OdsCode).HasName("PK_ServiceRecipients");

            builder.Property(r => r.Name).HasMaxLength(256);
            builder.Property(r => r.OdsCode).HasMaxLength(8);
            builder.Property(r => r.LastUpdated).HasDefaultValue(DateTime.UtcNow);

            builder.HasOne(r => r.LastUpdatedByUser)
                .WithMany()
                .HasForeignKey(r => r.LastUpdatedBy)
                .HasConstraintName("FK_ServiceRecipients_LastUpdatedBy");
        }
    }
}
