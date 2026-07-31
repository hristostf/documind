
using DocuMind.Application.Organizations.CreateOrganization;

namespace DocuMind.Application.Organziations.CreateOrganization;

public sealed record CreateOrganizationResponse
{
    private CreateOrganizationResponse(
        CreateOrganizationResult? organization,
        CreateOrganizationError error)
    {
        Organization = organization;
        Error = error;
    }

    public CreateOrganizationResult? Organization { get; }

    public CreateOrganizationError Error { get; }

    public bool IsSuccess =>
        Error == CreateOrganizationError.None;

    public static CreateOrganizationResponse Success(
        CreateOrganizationResult organization)
    {
        ArgumentNullException.ThrowIfNull(organization);

        return new CreateOrganizationResponse(
            organization,
            CreateOrganizationError.None);
    }

    public static CreateOrganizationResponse Failure(
        CreateOrganizationError error)
    {
        if (error == CreateOrganizationError.None)
        {
            throw new ArgumentException(
                "A failure response must contain an error.",
                nameof(error));
        }

        return new CreateOrganizationResponse(
            organization: null,
            error);
    }
}