using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.Candidates.CreateCandidate;

namespace OpportPortal.API.Mappers;
internal static class CreateCandidateRequestToCommand
{
    public static CreateCandidateCommand MapToCommand (this CreateCandidateRequest request, long userId)
    {
        return new CreateCandidateCommand(
            userId,
            request.FullName,
            request.BirthDate,
            request.ZipCode,
            request.Street,
            request.Number,
            request.Neighborhood,
            request.City,
            request.State,
            request.Country,
            request.Complement,
            request.PhoneNumber,
            request.UrlLinkedin
        );
    }
}
