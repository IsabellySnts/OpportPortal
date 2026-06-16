using OpportPortal.Domain.Enums;

namespace OpportPortal.Domain.Entities;

public class Curriculum : BaseEntity
{
    public long CandidateId { get; private set; }
    public Candidate Candidate { get; private set; }
    public EducationLevel EducationLevel { get; private set; }
    public List<string> HardSkills { get; private set; }
    public List<string> SoftSkills { get; private set; }
    public ICollection<Experience> Experiences { get; private set; } = [];

    private Curriculum(){}
    public Curriculum(long idCandidate, EducationLevel education, List<string> hardSkills, List<string> softSkills)
    {
        if (idCandidate <= 0)
            throw new ArgumentException("Invalid candidate");

        if (hardSkills == null || hardSkills.Count == 0)
            throw new ArgumentException("At least one hard skill is required.");

        if (softSkills == null || softSkills.Count == 0)
            throw new ArgumentException("At least one soft skill is required.");

        CandidateId = idCandidate;
        EducationLevel = education;
        HardSkills = hardSkills;
        SoftSkills = softSkills;
    }
}
