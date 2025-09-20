using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Solution
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string SolutionText { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public bool IsOriginal { get; set; }

    public virtual ICollection<SolutionFile> SolutionFiles { get; set; } = new List<SolutionFile>();

    public virtual Task Task { get; set; } = null!;
}
