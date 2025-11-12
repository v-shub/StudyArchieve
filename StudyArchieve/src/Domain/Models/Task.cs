using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Task
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? ConditionText { get; set; }

    public int SubjectId { get; set; }

    public int? AcademicYearId { get; set; }

    public int TypeId { get; set; }

    public DateTime DateAdded { get; set; }

    public int? UserAddedId { get; set; }

    public virtual AcademicYear? AcademicYear { get; set; }

    public virtual ICollection<Solution> Solutions { get; set; } = new List<Solution>();

    public virtual Subject Subject { get; set; } = null!;

    public virtual ICollection<TaskFile> TaskFiles { get; set; } = new List<TaskFile>();

    public virtual TaskType Type { get; set; } = null!;

    public virtual User? UserAdded { get; set; }

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
