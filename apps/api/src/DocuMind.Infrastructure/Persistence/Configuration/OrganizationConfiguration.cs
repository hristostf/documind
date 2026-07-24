using DocuMind.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocuMind.Infrastructure.Persistence.Configurations;

internal sealed class OrganizationConfiguration
    : IEntityTypeConfiguration<Organization>
{
    public void Configure(
        EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("organizations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();
    }
}