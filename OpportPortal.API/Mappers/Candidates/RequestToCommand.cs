using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.Candidates.CreateCandidate;
using OpportPortal.Application.Commands.Candidates.UpdateCandidate;

namespace OpportPortal.API.Mappers.Candidates;
internal static class RequestToCommand
{
    public static CreateCandidateCommand CreateCandidateRequestToCommand(this CreateCandidateRequest request, long userId)
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

    public static UpdateCandidateCommand UpdateCandidateRequestToCommand(this UpdateCandidateRequest request, long userId)
    {
        return new UpdateCandidateCommand(
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
