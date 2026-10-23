using OpportPortal.Application.Common;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Enums;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.JobApplications.CreateJobApplication;

public class CreateJobApplicationHandler : IHandler<CreateJobApplicationCommand, ResultViewModel<long>>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IOpportunityRepository _opportunityRepository;

    public CreateJobApplicationHandler(
        IJobApplicationRepository jobApplicationRepository,
        ICandidateRepository candidateRepository,
        IOpportunityRepository opportunityRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _candidateRepository = candidateRepository;
        _opportunityRepository = opportunityRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(CreateJobApplicationCommand request)
    {
        var candidate = await _candidateRepository.GetByUserIdAsync(request.UserId);
        if (candidate is null)
            return ResultViewModel<long>.Error("Candidate not found.");

        var opportunity = await _opportunityRepository.GetByIdAsync(request.OpportunityId);
        if (opportunity is null)
            return ResultViewModel<long>.Error("Opportunity not found.");

        if (opportunity.Status == Domain.Enums.OpportunityStatus.Closed)
            return ResultViewModel<long>.Error("Cannot apply to a closed opportunity.");

        // Já existe candidatura desse candidato pra essa vaga?
        var existing = await _jobApplicationRepository
            .GetByCandidateAndOpportunityAsync(candidate.Id, request.OpportunityId);

        if (existing is not null)
        {
            // Se estiver ativa, é duplicata de verdade → bloqueia
            if (existing.Status != JobApplicationStatus.Withdrawn)
                return ResultViewModel<long>.Error("Candidate already applied to this opportunity.");

            // Se foi cancelada, reativa (respeita o índice único)
            existing.Reapply();
            await _jobApplicationRepository.Update(existing);
            return ResultViewModel<long>.Success(existing.Id, "Application submitted successfully.");
        }

        // Primeira candidatura → cria
        var jobApplication = new JobApplication(candidate.Id, request.OpportunityId);
        var id = await _jobApplicationRepository.Create(jobApplication);
        return ResultViewModel<long>.Success(id, "Application submitted successfully.");
    }
}