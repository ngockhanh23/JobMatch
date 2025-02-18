using System;
using System.Collections.Generic;

namespace JobMatch.Models;

public partial class Resume
{
    public int Id { get; set; }

    public int? CandidateId { get; set; }

    public byte[]? ResumeFile { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? Candidate { get; set; }
}
