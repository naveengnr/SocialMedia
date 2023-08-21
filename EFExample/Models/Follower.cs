using System;
using System.Collections.Generic;

namespace EFExample.Models;

public partial class Follower
{
    public int FollowerId { get; set; }

    public int? UserId { get; set; }

    public int? FollowingUserId { get; set; }

    public int? FollowerUserId { get; set; }

    public DateTime? DateFollowed { get; set; }

    public virtual User? FollowerUser { get; set; }

    public virtual User? FollowingUser { get; set; }

    public virtual User? User { get; set; }
}
