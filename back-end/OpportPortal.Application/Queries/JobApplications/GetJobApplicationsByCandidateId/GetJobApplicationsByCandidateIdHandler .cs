using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.JobApplications.GetJobApplicationsByCandidateId;

public class GetJobApplicationsByCandidateIdHandler : IHandler<GetJobApplicationsByCandidateIdQuery, ResultViewModel<IEnumerable<JobApplicationResponse>>>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public GetJobApplicationsByCandidateIdHandler(IJobApplicationRepository jobApplicationRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;
    }

    public async Task<ResultViewModel<IEnumerable<JobApplicationResponse>>> HandleAsync(GetJobApplicationsByCandidateIdQuery request)
    {
        var jobApplications = await _jobApplicationRepository.GetByCandidateIdAsync(request.CandidateId);

        var response = jobApplications.Select(ja => new JobApplicationResponse(
            ja.Id, ja.CandidateId, ja.Candidate.FullName,
            ja.OpportunityId, ja.Opportunity.Title,
            ja.Status, ja.CreatedAt, ja.UpdatedAt
        ));

        return ResultViewModel<IEnumerable<JobApplicationResponse>>.Success(response);
    }
}