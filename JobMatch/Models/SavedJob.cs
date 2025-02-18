using System;
using System.Collections.Generic;

namespace JobMatch.Models;

public partial class SavedJob
{
    public int Id { get; set; }

    public int? JobId { get; set; }

    public int? UserId { get; set; }

    public virtual Job? Job { get; set; }

    public virtual User? User { get; set; }
}
