using System;
using System.Collections.Generic;

namespace EFExample.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? CommentUserId { get; set; }

    public int? PostId { get; set; }

    public string? Content { get; set; }

    public DateTime? DateCommented { get; set; }

    public bool IsDeleted { get; set; }

    public virtual User? CommentUser { get; set; }

    public virtual Post? Post { get; set; }

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
}
