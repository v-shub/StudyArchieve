using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class TaskFile
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
