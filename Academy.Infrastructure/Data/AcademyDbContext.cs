using Academy.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Academy.Infrastructure.Data;

public class AcademyDbContext : DbContext
{
    public AcademyDbContext(DbContextOptions<AcademyDbContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Course configuration
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Level).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Duration).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt);
            
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Level);
            entity.HasIndex(e => e.IsActive);
        });

        // ContactMessage configuration
        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Subject).HasMaxLength(100);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsRead).IsRequired();
            
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.IsRead);
            entity.HasIndex(e => e.CreatedAt);
        });

        // BlogPost configuration
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Summary).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ImageUrl).HasMaxLength(200);
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt);
            entity.Property(e => e.PublishedAt);
            entity.Property(e => e.ViewCount).IsRequired();
            entity.Property(e => e.MetaTitle).HasMaxLength(200);
            entity.Property(e => e.MetaDescription).HasMaxLength(300);
            entity.Property(e => e.MetaKeywords).HasMaxLength(500);
            entity.Property(e => e.TagsJson).HasMaxLength(500);
            
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.IsPublished);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.PublishedAt);
        });
    }
} 