using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Queries.Opportunities;

public record OpportunityResponse(
    long Id,
    long CompanyId,
    string CompanyName,
    string Title,
    string Description,
    OpportunityType Type,
    JobFormat Format,
    string Requirements,
    string FieldOfWork,
    string State,
    decimal Salary,
    OpportunityStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);