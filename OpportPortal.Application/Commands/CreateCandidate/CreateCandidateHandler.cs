using OpportPortal.Application.Common;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;
using OpportPortal.Domain.ValueObjects;

namespace OpportPortal.Application.Commands.CreateCandidate;

public class CreateCandidateHandler : IHandler<CreateCandidateCommand, ResultViewModel<long>>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IUserRepository _userRepository;

    public CreateCandidateHandler(
        ICandidateRepository candidateRepository,
        IUserRepository userRepository)
    {
        _candidateRepository = candidateRepository;
        _userRepository = userRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(CreateCandidateCommand request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return ResultViewModel<long>.Error("User not found.");

        if (await _candidateRepository.ExistsByUserIdAsync(request.UserId))
            return ResultViewModel<long>.Error("Candidate already exists for this user.");

        var address = new Address(
            request.ZipCode,
            request.Street,
            request.Number,
            request.Neighborhood,
            request.City,
            request.State,
            request.Country,
            request.Complement
        );

        var phone = new Phone(request.PhoneNumber);

        var candidate = new Candidate(
            request.UserId,
            request.FullName,
            request.BirthDate,
            address,
            phone
        );

        if (!string.IsNullOrWhiteSpace(request.UrlLinkedin))
            candidate.SetLinkedin(request.UrlLinkedin);

        var id = await _candidateRepository.Create(candidate);

        return ResultViewModel<long>.Success(id, "Candidate created successfully.");
    }
}
