using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PM.BL.Domain.contact;
using PM.BL.Domain.elements;
using PM.BL.Domain.flows;
using PM.BL.Domain.projects;
using PM.BL.Domain.questions;
using PM.BL.Domain.subthemes;

namespace PM.DAL.EF;

public class PhygitalDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<ResponseOption> ResponseOptions { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public DbSet<Subtheme> Subthemes { get; set; }
    public DbSet<Flow> Flows { get; set; }
    public DbSet<Element> Elements { get; set; }
    public DbSet<UserOpinion> UserOpinions { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    public PhygitalDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Phygital.db");
        }

        optionsBuilder.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
    }

    public bool CreateDatabase(bool dropDatabase)
    {
        if (dropDatabase)
        {
            Database.EnsureDeleted();
        }

        return Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasMany(project => project.Flows)
            .WithOne(flow => flow.Project)
            .HasForeignKey(flow => flow.ProjectId);

        modelBuilder.Entity<Flow>()
            .HasMany(f => f.Subthemes)
            .WithOne(s => s.Flow)
            .HasForeignKey(s => s.FlowId);

        modelBuilder.Entity<Subtheme>()
            .HasMany(s => s.Elements)
            .WithOne(e => e.Subtheme)
            .HasForeignKey(e => e.SubthemeId);
        
        modelBuilder.Entity<Question>()
            .HasMany(q => q.ResponseOptions)
            .WithOne(ro => ro.Question)
            .HasForeignKey(ro => ro.QuestionId);

        modelBuilder.Entity<Question>()
            .HasMany(q => q.Answers)
            .WithOne(ua => ua.Question)
            .HasForeignKey(ua => ua.QuestionId);
    }
}