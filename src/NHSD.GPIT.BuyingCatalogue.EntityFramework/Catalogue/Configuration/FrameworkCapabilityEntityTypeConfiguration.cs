﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.GPITBuyingCatalogue;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Configuration
{
    internal sealed class FrameworkCapabilityEntityTypeConfiguration : IEntityTypeConfiguration<FrameworkCapability>
    {
        public void Configure(EntityTypeBuilder<FrameworkCapability> builder)
        {
            builder.ToTable("FrameworkCapabilities");

            builder.HasKey(f => new { f.FrameworkId, f.CapabilityId });

            builder.Property(f => f.FrameworkId).HasMaxLength(10);

            builder.HasOne(f => f.Capability)
                .WithMany()
                .HasForeignKey(d => d.CapabilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FrameworkCapabilities_Capability");

            builder.HasOne(f => f.Framework)
                .WithMany()
                .HasForeignKey(d => d.FrameworkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FrameworkCapabilities_Framework");
        }
    }
}