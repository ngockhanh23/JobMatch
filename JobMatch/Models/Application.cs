using System;
using System.Collections.Generic;

namespace JobMatch.Models;

public partial class Application
{
    public int Id { get; set; }

    public int? JobId { get; set; }

    public int? CandidateId { get; set; }

    public string? ApplicationStatus { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public virtual User? Candidate { get; set; }

    public virtual Job? Job { get; set; }
}
