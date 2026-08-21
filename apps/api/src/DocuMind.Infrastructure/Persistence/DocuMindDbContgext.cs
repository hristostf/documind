using DocuMind.Domain.Organizations;
using DocuMind.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using DocuMind.Domain.Documents;
using DocuMind.Infrastructure.Persistence.Entities;

namespace DocuMind.Infrastructure.Persistence;

public sealed class DocuMindDbContext(
    DbContextOptions<DocuMindDbContext> options)
    : DbContext(options)
{
    public DbSet<Organization> Organizations =>
        Set<Organization>();

    public DbSet<Workspace> Workspaces =>
        Set<Workspace>();
  
    public DbSet<Document> Documents => 
        Set<Document>();

    public DbSet<DocumentChunk> DocumentChunks =>
        Set<DocumentChunk>();

    internal DbSet<DocumentChunkEmbedding> DocumentChunkEmbeddings =>
        Set<DocumentChunkEmbedding>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(DocuMindDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}