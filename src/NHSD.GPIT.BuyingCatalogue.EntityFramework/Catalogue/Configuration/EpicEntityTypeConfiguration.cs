﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.GPITBuyingCatalogue;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Configuration
{
    internal sealed class EpicEntityTypeConfiguration : IEntityTypeConfiguration<Epic>
    {
        public void Configure(EntityTypeBuilder<Epic> builder)
        {
            builder.ToTable("Epic");

            builder.HasKey(e => e.Id)
                .IsClustered(false);

            builder.Property(e => e.Id).HasMaxLength(10);
            builder.Property(e => e.CompliancyLevel)
                .HasConversion<int>()
                .HasColumnName("CompliancyLevelId");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.Active).IsRequired();
            builder.Property(e => e.SupplierDefined).IsRequired().HasDefaultValue(false);

            builder.HasOne<Capability>()
                .WithMany(c => c.Epics)
                .HasForeignKey(e => e.CapabilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Epic_Capability");
        }
    }
}