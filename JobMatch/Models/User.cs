using System;
using System.Collections.Generic;

namespace JobMatch.Models;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? UserAvatar { get; set; }

    public string? Email { get; set; }

    public string? UserPassword { get; set; }

    public string? Phone { get; set; }

    public string? UserType { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();

    public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
