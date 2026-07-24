namespace DocuMind.Domain.Organizations;

public sealed class Organization
{
    private Organization()
    {
    }

    public Organization(Guid id, string name)
    {
        Id = id;
        Name = name;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }
}