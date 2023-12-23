using System;
using System.Collections.Generic;

namespace EFExample.Models;

public partial class Friend
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FollowersId { get; set; }

    public string RequestStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
