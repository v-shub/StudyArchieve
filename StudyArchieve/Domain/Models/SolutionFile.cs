using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class SolutionFile
{
    public int Id { get; set; }

    public int SolutionId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public virtual Solution Solution { get; set; } = null!;
}
