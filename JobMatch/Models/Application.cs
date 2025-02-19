using System;
using System.Collections.Generic;

namespace JobMatch.Models;

public partial class Application
{
    public int Id { get; set; }

    public int? JobId { get; set; }

    public string? ApplicationStatus { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public int? ResumeId { get; set; }

    public string? CoverLetter { get; set; }

    public virtual Job? Job { get; set; }

    public virtual Resume? Resume { get; set; }
}
