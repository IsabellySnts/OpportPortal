using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Commands.Opportunities.UpdateOpportunity;

public record UpdateOpportunityCommand(
    long Id,
    string Title,
    string Description,
    OpportunityType Type,
    JobFormat Format,
    string Requirements,
    string FieldOfWork,
    string State,
    decimal Salary
);