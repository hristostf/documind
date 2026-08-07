using System.Net;
using System.Net.Http.Json;
using DocuMind.Application.Organizations.CreateOrganization;
using DocuMind.IntegrationTests.Infrastructure;

namespace DocuMind.IntegrationTests.Organizations;

public sealed class CreateOrganizationTests :
    IClassFixture<DocuMindApiFactory>
{
    private readonly HttpClient _client;

    public CreateOrganizationTests(
        DocuMindApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrganization_WithValidName_ReturnsCreated()
    {
        var request = new
        {
            Name = "Acme Corporation"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/organizations",
            request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var organization =
            await response.Content
                .ReadFromJsonAsync<CreateOrganizationResponse>();

        Assert.NotNull(organization);
        Assert.NotEqual(Guid.Empty, organization.Id);
        Assert.Equal("Acme Corporation", organization.Name);
        Assert.NotEqual(default, organization.CreatedAtUtc);

        Assert.NotNull(response.Headers.Location);

        Assert.Equal(
            $"/api/organizations/{organization.Id}",
            response.Headers.Location.OriginalString);
    }

    [Fact]
    public async Task CreateOrganization_WithWhitespaceName_ReturnsBadRequest()
    {
        var request = new
        {
            Name = "   "
        };

        var response = await _client.PostAsJsonAsync(
            "/api/organizations",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateOrganization_WithNameLongerThanMaximum_ReturnsBadRequest()
    {
        var request = new
        {
            Name = new string('A', 101)
        };

        var response = await _client.PostAsJsonAsync(
            "/api/organizations",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateOrganization_WithSurroundingWhitespace_TrimsName()
    {
        var request = new
        {
            Name = "   Acme Corporation   "
        };

        var response = await _client.PostAsJsonAsync(
            "/api/organizations",
            request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var organization =
            await response.Content
                .ReadFromJsonAsync<CreateOrganizationResponse>();

        Assert.NotNull(organization);

        Assert.Equal(
            "Acme Corporation",
            organization.Name);
    }
}