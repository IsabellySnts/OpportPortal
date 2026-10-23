using OpportPortal.Domain.Enums;

namespace OpportPortal.API.Requests;

public record CreateOpportunityRequest
(
    string Title,
    string Description,
    OpportunityType Type,
    JobFormat Format,
    string Requirements,
    string FieldOfWork,
    string State,
    decimal Salary
);
