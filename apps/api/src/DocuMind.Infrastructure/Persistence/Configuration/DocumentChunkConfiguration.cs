
namespace DocuMind.Infrastructure.Persistence.Configuration;
using DocuMind.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class DocumentChunkConfiguration
    : IEntityTypeConfiguration<DocumentChunk>
{
    public void Configure(EntityTypeBuilder<DocumentChunk> builder)
    {
        builder.ToTable("document_chunks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DocumentId)
            .IsRequired();

        builder.Property(x => x.Index)
            .IsRequired();

        builder.Property(x => x.Content)
            .IsRequired();

        builder.HasOne<Document>()
            .WithMany()
            .HasForeignKey(x => x.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(x => new 
        {
             x.DocumentId,
              x.Index
        })
        .IsUnique();
    }
}