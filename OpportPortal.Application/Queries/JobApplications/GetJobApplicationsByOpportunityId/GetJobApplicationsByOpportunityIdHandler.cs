using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.JobApplications.GetJobApplicationsByOpportunityId;

public class GetJobApplicationsByOpportunityIdHandler : IHandler<GetJobApplicationsByOpportunityIdQuery, ResultViewModel<IEnumerable<JobApplicationResponse>>>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public GetJobApplicationsByOpportunityIdHandler(IJobApplicationRepository jobApplicationRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;
    }

    public async Task<ResultViewModel<IEnumerable<JobApplicationResponse>>> HandleAsync(GetJobApplicationsByOpportunityIdQuery request)
    {
        var jobApplications = await _jobApplicationRepository.GetByOpportunityIdAsync(request.OpportunityId);

        var response = jobApplications.Select(ja => new JobApplicationResponse(
            ja.Id, ja.CandidateId, ja.Candidate.FullName,
            ja.OpportunityId, ja.Opportunity.Title,
            ja.Status, ja.CreatedAt, ja.UpdatedAt
        ));

        return ResultViewModel<IEnumerable<JobApplicationResponse>>.Success(response);
    }
}