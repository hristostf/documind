using DocuMind.Domain.Organizations;
using DocuMind.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocuMind.Infrastructure.Persistence.Configurations;

internal sealed class WorkspaceConfiguration
    : IEntityTypeConfiguration<Workspace>
{
    public void Configure(
        EntityTypeBuilder<Workspace> builder)
    {
        builder.ToTable("workspaces");

        builder.HasKey(x => x.Id);

        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.OrganizationId)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(x => x.OrganizationId);
    }
}