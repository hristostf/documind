using DocuMind.Domain.Documents;
using DocuMind.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocuMind.Infrastructure.Persistence.Configuration;

internal sealed class DocumentChunkEmbeddingConfiguration
    : IEntityTypeConfiguration<DocumentChunkEmbedding>
{
    public void Configure(
        EntityTypeBuilder<DocumentChunkEmbedding> builder)
    {
        builder.ToTable("document_chunk_embeddings");

        builder.HasKey(x => x.DocumentChunkId);

        builder.Property(x => x.Embedding)
            .HasColumnType("vector(1536)")
            .IsRequired();

        builder.HasOne<DocumentChunk>()
            .WithOne()
            .HasForeignKey<DocumentChunkEmbedding>(
                x => x.DocumentChunkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}