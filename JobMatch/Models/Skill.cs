using System;
using System.Collections.Generic;

namespace JobMatch.Models;

public partial class Skill
{
    public int Id { get; set; }

    public string? SkillName { get; set; }

    public virtual ICollection<User> Candidates { get; set; } = new List<User>();
}
