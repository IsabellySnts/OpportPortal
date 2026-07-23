using OpportPortal.Application.Common;
using OpportPortal.Domain.Entities;
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
        if (await _candidateRepository.GetByIdAsync(request.CandidateId) is null)
            return ResultViewModel<long>.Error("Candidate not found.");

        var opportunity = await _opportunityRepository.GetByIdAsync(request.OpportunityId);

        if (opportunity is null)
            return ResultViewModel<long>.Error("Opportunity not found.");

        if (opportunity.Status == Domain.Enums.OpportunityStatus.Closed)
            return ResultViewModel<long>.Error("Cannot apply to a closed opportunity.");

        if (await _jobApplicationRepository.ExistsAsync(request.CandidateId, request.OpportunityId))
            return ResultViewModel<long>.Error("Candidate already applied to this opportunity.");

        var jobApplication = new JobApplication(request.CandidateId, request.OpportunityId);

        var id = await _jobApplicationRepository.Create(jobApplication);

        return ResultViewModel<long>.Success(id, "Application submitted successfully.");
    }
}