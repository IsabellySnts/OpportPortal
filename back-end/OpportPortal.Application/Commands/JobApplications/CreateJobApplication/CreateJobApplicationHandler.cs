using OpportPortal.Application.Common;
using OpportPortal.Application.Services;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Enums;
using OpportPortal.Domain.Interfaces.Repositories;
using OpportPortal.Domain.Interfaces.Services;

namespace OpportPortal.Application.Commands.JobApplications.CreateJobApplication;

public class CreateJobApplicationHandler : IHandler<CreateJobApplicationCommand, ResultViewModel<long>>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;

    public CreateJobApplicationHandler(
        IJobApplicationRepository jobApplicationRepository,
        ICandidateRepository candidateRepository,
        IOpportunityRepository opportunityRepository,
        IUserRepository userRepository,
        IEmailService emailService,
        INotificationService notificationService)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _candidateRepository = candidateRepository;
        _opportunityRepository = opportunityRepository;
        _userRepository = userRepository;
        _emailService = emailService;
        _notificationService = notificationService;
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

        var existing = await _jobApplicationRepository
            .GetByCandidateAndOpportunityAsync(candidate.Id, request.OpportunityId);

        long applicationId;

        if (existing is not null)
        {
            if (existing.Status != JobApplicationStatus.Withdrawn)
                return ResultViewModel<long>.Error("Candidate already applied to this opportunity.");

            existing.Reapply();
            await _jobApplicationRepository.Update(existing);
            applicationId = existing.Id;
        }
        else
        {
            var jobApplication = new JobApplication(candidate.Id, request.OpportunityId);
            applicationId = await _jobApplicationRepository.Create(jobApplication);
        }

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is not null)
        {
            var (subject, html) = EmailTemplates.ApplicationCreated(candidate.FullName, opportunity.Title);
            await _emailService.SendAsync(user.UserEmail.Value, candidate.FullName, subject, html);
            await _notificationService.NotifyAsync(request.UserId, "Candidatura enviada ✅", $"Sua candidatura para '{opportunity.Title}' foi registrada.", NotificationType.ApplicationCreated);
        }

        return ResultViewModel<long>.Success(applicationId, "Application submitted successfully.");
    }
}