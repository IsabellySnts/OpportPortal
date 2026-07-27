// UpdateJobApplicationStatusHandler.cs
using OpportPortal.Application.Common;
using OpportPortal.Domain.Enums;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Commands.JobApplications.UpdateJobApplicationStatus;

public class UpdateJobApplicationStatusHandler : IHandler<UpdateJobApplicationStatusCommand, ResultViewModel<long>>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public UpdateJobApplicationStatusHandler(IJobApplicationRepository jobApplicationRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;
    }

    public async Task<ResultViewModel<long>> HandleAsync(UpdateJobApplicationStatusCommand request)
    {
        var jobApplication = await _jobApplicationRepository.GetByIdAsync(request.Id);

        if (jobApplication is null)
            return ResultViewModel<long>.Error("Job application not found.");

        try
        {
            switch (request.NewStatus)
            {
                case JobApplicationStatus.UnderReview:
                    jobApplication.StartReview();
                    break;
                case JobApplicationStatus.InterviewScheduled:
                    jobApplication.ScheduleInterview();
                    break;
                case JobApplicationStatus.FinalProcess:
                    jobApplication.MoveToFinalProcess();
                    break;
                case JobApplicationStatus.Approved:
                    jobApplication.Approve();
                    break;
                case JobApplicationStatus.NotAdvanced:
                    jobApplication.Reject();
                    break;
                case JobApplicationStatus.Withdrawn:
                    jobApplication.Withdraw();
                    break;
                case JobApplicationStatus.PositionClosed:
                    jobApplication.ClosePosition();
                    break;
                default:
                    return ResultViewModel<long>.Error("Invalid status.");
            }
        }
        catch (InvalidOperationException ex)
        {
            return ResultViewModel<long>.Error(ex.Message);
        }

        await _jobApplicationRepository.Update(jobApplication);

        return ResultViewModel<long>.Success(jobApplication.Id, "Status updated successfully.");
    }
}