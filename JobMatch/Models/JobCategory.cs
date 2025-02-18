using System;
using System.Collections.Generic;

namespace JobMatch.Models;

public partial class JobCategory
{
    public int Id { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
