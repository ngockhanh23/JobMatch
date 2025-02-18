using System;
using System.Collections.Generic;

namespace JobMatch.Models;

public partial class Company
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyDescription { get; set; }

    public string? CompanyLocation { get; set; }

    public string? CompanySize { get; set; }

    public string? Website { get; set; }

    public string? ContactEmail { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual User? User { get; set; }
}
