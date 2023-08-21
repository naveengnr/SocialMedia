using System;
using System.Collections.Generic;

namespace EFExample.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int? PostUserId { get; set; }

    public string? Content { get; set; }

    public DateTime? DatePosted { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User? PostUser { get; set; }

    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();
}
