using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Task = Domain.Models.Task;

namespace DataAccess;

public partial class StudyArchieveContext : DbContext
{
    public StudyArchieveContext()
    {
    }

    public StudyArchieveContext(DbContextOptions<StudyArchieveContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicYear> AcademicYears { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Solution> Solutions { get; set; }

    public virtual DbSet<SolutionFile> SolutionFiles { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskFile> TaskFiles { get; set; }

    public virtual DbSet<TaskType> TaskTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicYear>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__academic__3213E83F1B6CA0E6");

            entity.ToTable("academic_years");

            entity.HasIndex(e => e.YearLabel, "UQ__academic__588C243AB9AC4808").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.YearLabel)
                .HasMaxLength(9)
                .HasColumnName("year_label");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__authors__3213E83F90580668");

            entity.ToTable("authors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F0B07FD31");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Solution>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__solution__3213E83F48212A39");

            entity.ToTable("solutions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("date_added");
            entity.Property(e => e.SolutionText)
                .HasColumnType("text")
                .HasColumnName("solution_text");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UserAddedId).HasColumnName("user_added_id");

            entity.HasOne(d => d.Task).WithMany(p => p.Solutions)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_Solutions_Task");

            entity.HasOne(d => d.UserAdded).WithMany(p => p.Solutions)
                .HasForeignKey(d => d.UserAddedId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_users_solutions");
        });

        modelBuilder.Entity<SolutionFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__solution__3213E83FA52D0F06");

            entity.ToTable("solution_files");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("file_name");
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .HasColumnName("file_path");
            entity.Property(e => e.SolutionId).HasColumnName("solution_id");

            entity.HasOne(d => d.Solution).WithMany(p => p.SolutionFiles)
                .HasForeignKey(d => d.SolutionId)
                .HasConstraintName("FK_SolutionFiles_Solution");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__subjects__3213E83F4FAD094E");

            entity.ToTable("subjects");

            entity.HasIndex(e => e.Name, "UQ__subjects__72E12F1B80E3E474").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tags__3213E83FCA339F7A");

            entity.ToTable("tags");

            entity.HasIndex(e => e.Name, "UQ__tags__72E12F1B5021F2F2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tasks__3213E83F35905B03");

            entity.ToTable("tasks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicYearId).HasColumnName("academic_year_id");
            entity.Property(e => e.ConditionText).HasColumnName("condition_text");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("date_added");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasColumnName("title");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.UserAddedId).HasColumnName("user_added_id");

            entity.HasOne(d => d.AcademicYear).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.AcademicYearId)
                .HasConstraintName("FK_Tasks_AcademicYear");

            entity.HasOne(d => d.Subject).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Subject");

            entity.HasOne(d => d.Type).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Type");

            entity.HasOne(d => d.UserAdded).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserAddedId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_users_tasks");

            entity.HasMany(d => d.Authors).WithMany(p => p.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TaskAuthors_Author"),
                    l => l.HasOne<Task>().WithMany()
                        .HasForeignKey("TaskId")
                        .HasConstraintName("FK_TaskAuthors_Task"),
                    j =>
                    {
                        j.HasKey("TaskId", "AuthorId").HasName("PK_TaskAuthors");
                        j.ToTable("task_authors");
                        j.IndexerProperty<int>("TaskId").HasColumnName("task_id");
                        j.IndexerProperty<int>("AuthorId").HasColumnName("author_id");
                    });

            entity.HasMany(d => d.Tags).WithMany(p => p.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TaskTags_Tag"),
                    l => l.HasOne<Task>().WithMany()
                        .HasForeignKey("TaskId")
                        .HasConstraintName("FK_TaskTags_Task"),
                    j =>
                    {
                        j.HasKey("TaskId", "TagId").HasName("PK_TaskTags");
                        j.ToTable("task_tags");
                        j.IndexerProperty<int>("TaskId").HasColumnName("task_id");
                        j.IndexerProperty<int>("TagId").HasColumnName("tag_id");
                    });
        });

        modelBuilder.Entity<TaskFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__task_fil__3213E83F73F69B58");

            entity.ToTable("task_files");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("file_name");
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .HasColumnName("file_path");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskFiles)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_TaskFiles_Task");
        });

        modelBuilder.Entity<TaskType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__task_typ__3213E83F1AADA2B1");

            entity.ToTable("task_types");

            entity.HasIndex(e => e.Name, "UQ__task_typ__72E12F1B80BCA519").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FB69E9D25");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
