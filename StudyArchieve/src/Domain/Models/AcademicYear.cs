using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class AcademicYear
{
    public int Id { get; set; }

    public string YearLabel { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
