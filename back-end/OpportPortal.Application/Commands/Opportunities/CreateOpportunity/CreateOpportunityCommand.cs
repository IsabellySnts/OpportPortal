using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Commands.Opportunities.CreateOpportunity;

public record CreateOpportunityCommand(
    long UserId,
    string Title,
    string Description,
    OpportunityType Type,
    JobFormat Format,
    string Requirements,
    string FieldOfWork,
    string State,
    decimal Salary
);