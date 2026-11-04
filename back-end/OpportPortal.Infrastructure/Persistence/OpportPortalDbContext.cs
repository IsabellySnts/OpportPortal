using Microsoft.EntityFrameworkCore;
using OpportPortal.Domain.Entities;
using System.Text.Json;

namespace OpportPortal.Infrastructure.Persistence;

public class OpportPortalDbContext : DbContext
{
    public OpportPortalDbContext(DbContextOptions<OpportPortalDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Curriculum> Curriculums { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<Opportunity> Opportunities { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(o => o.Id)
                .UseIdentityColumn();
            e.Property(u => u.Password);
            e.Property(u => u.Provider)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
            e.Property(u => u.Type).IsRequired().HasConversion<string>().HasMaxLength(50);

            e.OwnsOne(c => c.UserEmail, email =>
            {
                email.Property(p => p.Value)
                    .HasColumnName("UserEmail")
                    .IsRequired()
                    .HasMaxLength(100);

                email.HasIndex(p => p.Value).IsUnique();
            });

            e.HasQueryFilter(u => !u.IsDeleted);
        });

        builder.Entity<Candidate>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(o => o.Id)
                .UseIdentityColumn();
            e.Property(c => c.FullName).IsRequired().HasMaxLength(200);
            e.Property(c => c.UrlLinkedin).HasMaxLength(300);
            e.Property(c => c.CreatedAt).IsRequired();
            e.Property(c => c.UpdatedAt);

            e.OwnsOne(c => c.PhoneNumber, phone =>
            {
                phone.Property(p => p.Value)
                    .HasColumnName("PhoneNumber")
                    .IsRequired()
                    .HasMaxLength(11);
            });

            e.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.ZipCode)
                    .HasColumnName("ZipCode")
                    .IsRequired()
                    .HasMaxLength(9);

                address.Property(a => a.Street)
                    .HasColumnName("Street")
                    .IsRequired()
                    .HasMaxLength(200);

                address.Property(a => a.Number)
                    .HasColumnName("AddressNumber")
                    .IsRequired()
                    .HasMaxLength(10);

                address.Property(a => a.Neighborhood)
                    .HasColumnName("Neighborhood")
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.City)
                    .HasColumnName("City")
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.State)
                    .HasColumnName("State")
                    .IsRequired()
                    .HasMaxLength(2);

                address.Property(a => a.Country)
                    .HasColumnName("Country")
                    .IsRequired()
                    .HasMaxLength(50);

                address.Property(a => a.Complement)
                    .HasColumnName("Complement")
                    .HasMaxLength(100);
            });

            e.HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Candidate>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(c => c.Curriculum)
                .WithOne(cv => cv.Candidate)
                .HasForeignKey<Curriculum>(cv => cv.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasQueryFilter(u => !u.IsDeleted);

        });

        builder.Entity<Company>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(o => o.Id)
                .UseIdentityColumn();
            e.Property(c => c.CompanyName).IsRequired().HasMaxLength(200);
            e.Property(c => c.Sector).IsRequired().HasMaxLength(50);

            e.OwnsOne(c => c.Cnpj, cnpj =>
            {
                cnpj.Property(p => p.Value)
                    .HasColumnName("Cnpj")
                    .IsRequired()
                    .HasMaxLength(14);
                cnpj.HasIndex(p => p.Value).IsUnique();
            });

            e.OwnsOne(c => c.Email, email =>
            {
                email.Property(p => p.Value)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(100);
                email.HasIndex(p => p.Value).IsUnique();
            });

            e.OwnsOne(c => c.PhoneNumber, phone =>
            {
                phone.Property(p => p.Value)
                    .HasColumnName("PhoneNumber")
                    .IsRequired()
                    .HasMaxLength(11);
            });

            e.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.ZipCode)
                    .HasColumnName("ZipCode")
                    .IsRequired()
                    .HasMaxLength(9);

                address.Property(a => a.Street)
                    .HasColumnName("Street")
                    .IsRequired()
                    .HasMaxLength(200);

                address.Property(a => a.Number)
                    .HasColumnName("AddressNumber")
                    .IsRequired()
                    .HasMaxLength(10);

                address.Property(a => a.Neighborhood)
                    .HasColumnName("Neighborhood")
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.City)
                    .HasColumnName("City")
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.State)
                    .HasColumnName("State")
                    .IsRequired()
                    .HasMaxLength(2);

                address.Property(a => a.Country)
                    .HasColumnName("Country")
                    .IsRequired()
                    .HasMaxLength(50);

                address.Property(a => a.Complement)
                    .HasColumnName("Complement")
                    .HasMaxLength(100);
            });

            e.HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Company>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(c => c.Opportunities)
                .WithOne(o => o.Company)
                .HasForeignKey(o => o.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasQueryFilter(u => !u.IsDeleted);
        });

        builder.Entity<Curriculum>(e =>
        {
            e.HasKey(c => c.Id);

            e.Property(o => o.Id)
                .UseIdentityColumn();

            e.Property(cv => cv.CreatedAt).IsRequired();
            e.Property(cv => cv.EducationLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            e.Property(cv => cv.HardSkills)
                .IsRequired()
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)!
                )
                .HasColumnType("nvarchar(max)");

            e.Property(cv => cv.SoftSkills)
                .IsRequired()
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)!
                )
                .HasColumnType("nvarchar(max)");

            e.HasOne(cv => cv.Candidate)
                .WithOne(c => c.Curriculum)
                .HasForeignKey<Curriculum>(cv => cv.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(cv => cv.Experiences)
                .WithOne(ex => ex.Curriculum)
                .HasForeignKey(ex => ex.CurriculumId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasQueryFilter(u => !u.IsDeleted);
        });

        builder.Entity<Experience>(e =>
        {
            e.HasKey(ex => ex.Id);

            e.Property(o => o.Id)
                .UseIdentityColumn();

            e.Property(ex => ex.JobTitle)
                .IsRequired()
                .HasMaxLength(150);

            e.Property(ex => ex.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(ex => ex.StartDate)
                .IsRequired();

            e.Property(ex => ex.EndDate);

            e.Property(ex => ex.Description)
                .HasMaxLength(1000);

            e.Property(ex => ex.CreatedAt)
                .IsRequired();

            e.Property(ex => ex.UpdatedAt);

            e.HasOne(ex => ex.Curriculum)
                .WithMany(cv => cv.Experiences)
                .HasForeignKey(ex => ex.CurriculumId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasQueryFilter(u => !u.IsDeleted);
        });

        builder.Entity<JobApplication>(e =>
        {
            e.HasKey(ja => ja.Id);

            e.Property(o => o.Id)
                .UseIdentityColumn();

            e.Property(ja => ja.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            e.Property(ja => ja.CreatedAt)
                .IsRequired();

            e.Property(ja => ja.UpdatedAt);

            e.HasIndex(ja => new { ja.CandidateId, ja.OpportunityId })
                .IsUnique();

            e.HasOne(ja => ja.Candidate)
                .WithMany(c => c.JobApplications)
                .HasForeignKey(ja => ja.CandidateId)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(ja => ja.Opportunity)
                .WithMany(o => o.JobApplications)
                .HasForeignKey(ja => ja.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasQueryFilter(u => !u.IsDeleted);
        });

        builder.Entity<Opportunity>(e =>
        {
            e.HasKey(o => o.Id);

            e.Property(o => o.Id)
                .UseIdentityColumn();

            e.Property(o => o.Title)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(o => o.Description)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            e.Property(o => o.Requirements)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            e.Property(o => o.FieldOfWork)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(o => o.Salary)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            e.Property(o => o.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            e.Property(o => o.Format)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            e.Property(o => o.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            e.Property(o => o.CreatedAt)
                .IsRequired();

            e.Property(o => o.UpdatedAt); ;

            e.HasOne(o => o.Company)
                .WithMany(c => c.Opportunities)
                .HasForeignKey(o => o.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(o => o.JobApplications)
                .WithOne(ja => ja.Opportunity)
                .HasForeignKey(ja => ja.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasQueryFilter(u => !u.IsDeleted);
        });

        builder.Entity<Notification>(e =>
        {
            e.HasKey(n => n.Id);
            e.Property(o => o.Id).UseIdentityColumn();
            e.Property(n => n.Title).IsRequired().HasMaxLength(150);
            e.Property(n => n.Message).IsRequired().HasMaxLength(500);
            e.Property(n => n.Type).IsRequired().HasConversion<string>().HasMaxLength(30);
            e.Property(n => n.CreatedAt).IsRequired();
            e.Property(n => n.UpdatedAt);
            e.HasOne<User>().WithMany().HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
            e.HasQueryFilter(n => !n.IsDeleted);
        });

        base.OnModelCreating(builder);
    }
}
