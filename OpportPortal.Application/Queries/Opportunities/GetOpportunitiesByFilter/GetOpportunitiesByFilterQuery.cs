using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Queries.Opportunities.GetOpportunitiesByFilter;

public record GetOpportunitiesByFilterQuery(
    string? FieldOfWork,
    string? State,
    OpportunityType? Type,
    JobFormat? Format
);