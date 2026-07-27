using OpportPortal.Application.Common;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.JobApplications.DeleteJobApplication;

public class DeleteJobApplicationHandler : IHandler<DeleteJobApplicationCommand, ResultViewModel<long>>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public DeleteJobApplicationHandler(IJobApplicationRepository jobApplicationRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(DeleteJobApplicationCommand request)
    {
        var jobApplication = await _jobApplicationRepository.GetByIdAsync(request.Id);

        if (jobApplication is null)
            return ResultViewModel<long>.Error("Job application not found.");

        if (!jobApplication.IsActive())
            return ResultViewModel<long>.Error("Cannot delete a finalized application.");

        await _jobApplicationRepository.Delete(jobApplication);

        return ResultViewModel<long>.Success(jobApplication.Id, "Application deleted successfully.");
    }
}