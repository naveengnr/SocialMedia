using System;
using System.Collections.Generic;

namespace EFExample.Models;

public partial class Share
{
    public int ShareId { get; set; }

    public int? UserId { get; set; }

    public int? PostId { get; set; }

    public DateTime? DateShared { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
