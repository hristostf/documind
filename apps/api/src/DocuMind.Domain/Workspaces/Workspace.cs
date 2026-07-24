namespace DocuMind.Domain.Workspaces;

public sealed class Workspace
{
    private Workspace()
    {
    }

    public Workspace(Guid id, Guid organizationId, string name)
    {
        Id = id;
        OrganizationId = organizationId;
        Name = name;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }
}