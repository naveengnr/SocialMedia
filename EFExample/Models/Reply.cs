using System;
using System.Collections.Generic;

namespace EFExample.Models;

public partial class Reply
{
    public int ReplyId { get; set; }

    public int? UserId { get; set; }

    public int? CommentId { get; set; }

    public string? ReplyContent { get; set; }

    public DateTime? DateReplied { get; set; }

    public bool IsDeleted { get; set; }

    public int? ParentReplyId { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual User? User { get; set; }
}
