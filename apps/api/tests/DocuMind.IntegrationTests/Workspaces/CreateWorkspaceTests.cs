using System.Net;
using System.Net.Http.Json;
using DocuMind.IntegrationTests.Infrastructure;

namespace DocuMind.IntegrationTests.Workspaces;

public sealed class CreateWorkspaceTests :
    IClassFixture<DocuMindApiFactory>
{
    private readonly HttpClient _client;

    public CreateWorkspaceTests(
        DocuMindApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateWorkspace_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var organization = await CreateOrganizationAsync();

        var request = new
        {
            Name = "Engineering"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/organizations/{organization.Id}/workspaces",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var workspace = await response.Content
            .ReadFromJsonAsync<CreatedWorkspaceResponse>();

        Assert.NotNull(workspace);

        Assert.NotEqual(
            Guid.Empty,
            workspace.Id);

        Assert.Equal(
            organization.Id,
            workspace.OrganizationId);

        Assert.Equal(
            "Engineering",
            workspace.Name);

        Assert.NotEqual(
            default,
            workspace.CreatedAtUtc);

        Assert.NotNull(response.Headers.Location);

        Assert.Equal(
            $"/api/organizations/{organization.Id}/workspaces/{workspace.Id}",
            response.Headers.Location.OriginalString);
    }

    [Fact]
    public async Task CreateWorkspace_WithMissingOrganization_ReturnsNotFound()
    {
        // Arrange
        var missingOrganizationId = Guid.NewGuid();

        var request = new
        {
            Name = "Engineering"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/organizations/{missingOrganizationId}/workspaces",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateWorkspace_WithWhitespaceName_ReturnsBadRequest()
    {
        // Arrange
        var organization = await CreateOrganizationAsync();

        var request = new
        {
            Name = "   "
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/organizations/{organization.Id}/workspaces",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateWorkspace_WithNameLongerThanMaximum_ReturnsBadRequest()
    {
        // Arrange
        var organization = await CreateOrganizationAsync();

        var request = new
        {
            Name = new string('A', 101)
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/organizations/{organization.Id}/workspaces",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateWorkspace_WithSurroundingWhitespace_TrimsName()
    {
        // Arrange
        var organization = await CreateOrganizationAsync();

        var request = new
        {
            Name = "   Engineering   "
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/organizations/{organization.Id}/workspaces",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var workspace = await response.Content
            .ReadFromJsonAsync<CreatedWorkspaceResponse>();

        Assert.NotNull(workspace);

        Assert.Equal(
            "Engineering",
            workspace.Name);
    }

    private async Task<CreatedOrganizationResponse>
        CreateOrganizationAsync()
    {
        var request = new
        {
            Name = $"Test Organization {Guid.NewGuid()}"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/organizations",
            request);

        response.EnsureSuccessStatusCode();

        var organization = await response.Content
            .ReadFromJsonAsync<CreatedOrganizationResponse>();

        Assert.NotNull(organization);

        return organization;
    }

    private sealed record CreatedOrganizationResponse(
        Guid Id,
        string Name,
        DateTime CreatedAtUtc);

    private sealed record CreatedWorkspaceResponse(
        Guid Id,
        Guid OrganizationId,
        string Name,
        DateTime CreatedAtUtc);
}