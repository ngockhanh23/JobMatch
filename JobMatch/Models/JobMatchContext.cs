using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JobMatch.Models;

public partial class JobMatchContext : DbContext
{
    public JobMatchContext()
    {
    }

    public JobMatchContext(DbContextOptions<JobMatchContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobCategory> JobCategories { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Resume> Resumes { get; set; }

    public virtual DbSet<SavedJob> SavedJobs { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=JobMatch;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC07A440F021");

            entity.Property(e => e.ApplicationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ApplicationStatus).HasMaxLength(100);

            entity.HasOne(d => d.Job).WithMany(p => p.Applications)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK__Applicati__JobId__2F10007B");

            entity.HasOne(d => d.Resume).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("FK_Applications_Resumes");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Companie__3214EC07DDD84190");

            entity.Property(e => e.CompanyLocation).HasMaxLength(800);
            entity.Property(e => e.CompanyName).HasMaxLength(800);
            entity.Property(e => e.CompanySize).HasMaxLength(100);
            entity.Property(e => e.ContactEmail).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Website).HasMaxLength(1000);

            entity.HasOne(d => d.User).WithMany(p => p.Companies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Companies__UserI__286302EC");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Jobs__3214EC07B6BC1BD5");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Experience).HasMaxLength(100);
            entity.Property(e => e.JobAddressDetail).HasMaxLength(255);
            entity.Property(e => e.JobType).HasMaxLength(100);
            entity.Property(e => e.SalaryRange).HasMaxLength(100);
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.WorkAt).HasMaxLength(800);

            entity.HasOne(d => d.Company).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Jobs__CompanyId__2B3F6F97");

            entity.HasMany(d => d.Categories).WithMany(p => p.Jobs)
                .UsingEntity<Dictionary<string, object>>(
                    "JobCategoryLink",
                    r => r.HasOne<JobCategory>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__JobCatego__Categ__3F466844"),
                    l => l.HasOne<Job>().WithMany()
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__JobCatego__JobId__3E52440B"),
                    j =>
                    {
                        j.HasKey("JobId", "CategoryId").HasName("PK__JobCateg__A4F603629A4AED7A");
                        j.ToTable("JobCategoryLinks");
                    });
        });

        modelBuilder.Entity<JobCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobCateg__3214EC07FB04E306");

            entity.Property(e => e.CategoryName).HasMaxLength(1000);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07656C6E1A");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__UserI__47DBAE45");
        });

        modelBuilder.Entity<Resume>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Resumes__3214EC0790E21D5B");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ResumeTitle).HasMaxLength(1000);

            entity.HasOne(d => d.Candidate).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.CandidateId)
                .HasConstraintName("FK__Resumes__Candida__33D4B598");
        });

        modelBuilder.Entity<SavedJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SavedJob__3214EC07C68CD1BB");

            entity.HasOne(d => d.Job).WithMany(p => p.SavedJobs)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK__SavedJobs__JobId__4222D4EF");

            entity.HasOne(d => d.User).WithMany(p => p.SavedJobs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SavedJobs__UserI__4316F928");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Skills__3214EC07FB96594F");

            entity.Property(e => e.SkillName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07DDB695A2");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534B3DCD759").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName).HasMaxLength(255);
            entity.Property(e => e.UserType).HasMaxLength(50);

            entity.HasMany(d => d.Skills).WithMany(p => p.Candidates)
                .UsingEntity<Dictionary<string, object>>(
                    "CandidateSkill",
                    r => r.HasOne<Skill>().WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Candidate__Skill__398D8EEE"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Candidate__Candi__38996AB5"),
                    j =>
                    {
                        j.HasKey("CandidateId", "SkillId").HasName("PK__Candidat__B2A99284F330C896");
                        j.ToTable("CandidateSkills");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
