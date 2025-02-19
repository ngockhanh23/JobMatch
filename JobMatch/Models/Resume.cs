using System;
using System.Collections.Generic;

namespace JobMatch.Models;

public partial class Resume
{
    public int Id { get; set; }

    public int? CandidateId { get; set; }

    public byte[]? ResumeFile { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ResumeTitle { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual User? Candidate { get; set; }
}
