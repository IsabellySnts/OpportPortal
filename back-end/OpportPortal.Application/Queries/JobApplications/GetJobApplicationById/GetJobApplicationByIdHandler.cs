using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Queries.JobApplications.GetJobApplicationById;

public class GetJobApplicationByIdHandler : IHandler<GetJobApplicationByIdQuery, ResultViewModel<JobApplicationResponse>>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public GetJobApplicationByIdHandler(IJobApplicationRepository jobApplicationRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;
    }

    public async Task<ResultViewModel<JobApplicationResponse>> HandleAsync(GetJobApplicationByIdQuery request)
    {
        var jobApplication = await _jobApplicationRepository.GetByIdAsync(request.Id);

        if (jobApplication is null)
            return ResultViewModel<JobApplicationResponse>.Error("Job application not found.");

        return ResultViewModel<JobApplicationResponse>.Success(ToResponse(jobApplication));
    }

    private static JobApplicationResponse ToResponse(Domain.Entities.JobApplication ja) => new(
        ja.Id, ja.CandidateId, ja.Candidate.FullName,
        ja.OpportunityId, ja.Opportunity.Title,
        ja.Status, ja.CreatedAt, ja.UpdatedAt
    );
}