using OpportPortal.Application.Common;
using OpportPortal.Application.Services;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Enums;
using OpportPortal.Domain.Interfaces.Repositories;
using OpportPortal.Domain.Interfaces.Services;

namespace OpportPortal.Application.Commands.JobApplications.UpdateJobApplicationStatus;

public class UpdateJobApplicationStatusHandler : IHandler<UpdateJobApplicationStatusCommand, ResultViewModel<long>>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;

    public UpdateJobApplicationStatusHandler(
        IJobApplicationRepository jobApplicationRepository,
        ICandidateRepository candidateRepository,
        IUserRepository userRepository,
        IOpportunityRepository opportunityRepository,
        IEmailService emailService,
        INotificationService notificationService)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _candidateRepository = candidateRepository;
        _userRepository = userRepository;
        _opportunityRepository = opportunityRepository;
        _emailService = emailService;
        _notificationService = notificationService;
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
                case JobApplicationStatus.UnderReview: jobApplication.StartReview(); break;
                case JobApplicationStatus.InterviewScheduled: jobApplication.ScheduleInterview(); break;
                case JobApplicationStatus.FinalProcess: jobApplication.MoveToFinalProcess(); break;
                case JobApplicationStatus.Approved: jobApplication.Approve(); break;
                case JobApplicationStatus.NotAdvanced: jobApplication.Reject(); break;
                case JobApplicationStatus.Withdrawn: jobApplication.Withdraw(); break;
                case JobApplicationStatus.PositionClosed: jobApplication.ClosePosition(); break;
                default: return ResultViewModel<long>.Error("Invalid status.");
            }
        }
        catch (InvalidOperationException ex)
        {
            return ResultViewModel<long>.Error(ex.Message);
        }

        await _jobApplicationRepository.Update(jobApplication);

        await NotifyCandidateAsync(jobApplication, request.NewStatus);

        return ResultViewModel<long>.Success(jobApplication.Id, "Status updated successfully.");
    }

    private async Task NotifyCandidateAsync(JobApplication app, JobApplicationStatus status)
    {
        if (status is not (JobApplicationStatus.InterviewScheduled
            or JobApplicationStatus.Approved
            or JobApplicationStatus.NotAdvanced))
            return;

        try
        {
            var candidate = await _candidateRepository.GetByIdAsync(app.CandidateId);
            if (candidate is null) return;

            var user = await _userRepository.GetByIdAsync(candidate.UserId);
            if (user is null) return;

            var opportunity = await _opportunityRepository.GetByIdAsync(app.OpportunityId);
            var title = opportunity?.Title ?? "a vaga";

            // E-mail
            var email = status switch
            {
                JobApplicationStatus.InterviewScheduled => EmailTemplates.InterviewScheduled(candidate.FullName, title),
                JobApplicationStatus.Approved => EmailTemplates.ApplicationResult(candidate.FullName, title, approved: true),
                JobApplicationStatus.NotAdvanced => EmailTemplates.ApplicationResult(candidate.FullName, title, approved: false),
                _ => default
            };
            await _emailService.SendAsync(user.UserEmail.Value, candidate.FullName, email.Subject, email.Html);

            // Notificação in-app
            var (notifTitle, notifType) = status switch
            {
                JobApplicationStatus.InterviewScheduled => ("Entrevista agendada 📅", NotificationType.InterviewScheduled),
                JobApplicationStatus.Approved => ("Você foi aprovado(a)! 🎉", NotificationType.ApplicationApproved),
                JobApplicationStatus.NotAdvanced => ("Atualização da candidatura", NotificationType.ApplicationRejected),
                _ => ("", NotificationType.General)
            };
            await _notificationService.NotifyAsync(user.Id, notifTitle, $"Vaga: {title}", notifType);
        }
        catch
        {
            // falha de e-mail/notificação nunca derruba a atualização de status
        }
    }
}