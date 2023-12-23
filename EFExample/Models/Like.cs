using System;
using System.Collections.Generic;

namespace EFExample.Models;

public partial class Like
{
    public int LikeId { get; set; }

    public int? UserId { get; set; }

    public string LikeType { get; set; } = null!;

    public int LikeTypeId { get; set; }

    public DateTime? DateLiked { get; set; }

    public bool UnLike { get; set; }

    public virtual User? User { get; set; }
}
