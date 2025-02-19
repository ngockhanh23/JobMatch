using System;
using System.Collections.Generic;

namespace JobMatch.Models;

public partial class Job
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public string? Title { get; set; }

    public string? JobDescription { get; set; }

    public string? WorkAt { get; set; }

    public string? SalaryRange { get; set; }

    public string? Experience { get; set; }

    public string? Benefits { get; set; }

    public string? JobType { get; set; }

    public string? Requirements { get; set; }

    public DateTime UploadDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? JobAddressDetail { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Company? Company { get; set; }

    public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();

    public virtual ICollection<JobCategory> Categories { get; set; } = new List<JobCategory>();
}
