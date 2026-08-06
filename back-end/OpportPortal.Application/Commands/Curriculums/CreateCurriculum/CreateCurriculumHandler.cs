using OpportPortal.Application.Common;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.Curriculums.CreateCurriculum;

public class CreateCurriculumHandler : IHandler<CreateCurriculumCommand, ResultViewModel<long>>
{
    private readonly ICurriculumRepository _curriculumRepository;
    private readonly ICandidateRepository _candidateRepository;

    public CreateCurriculumHandler(
        ICurriculumRepository curriculumRepository,
        ICandidateRepository candidateRepository)
    {
        _curriculumRepository = curriculumRepository;
        _candidateRepository = candidateRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(CreateCurriculumCommand request)
    {
        var candidate = await _candidateRepository.GetByUserIdAsync(request.UserId);

        if (candidate is null)
            return ResultViewModel<long>.Error("Candidate not found.");

        if (await _curriculumRepository.ExistsByCandidateIdAsync(candidate.Id))
            return ResultViewModel<long>.Error("Curriculum already exists for this candidate.");

        var curriculum = new Curriculum(
            candidate.Id,
            request.EducationLevel,
            request.HardSkills,
            request.SoftSkills
        );

        var id = await _curriculumRepository.Create(curriculum);

        return ResultViewModel<long>.Success(id, "Curriculum created successfully.");
    }
}
