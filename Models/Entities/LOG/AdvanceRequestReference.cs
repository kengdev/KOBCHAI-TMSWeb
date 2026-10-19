using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class AdvanceRequestReference
{
    public long ReferenceId { get; set; }

    public string RequestCode { get; set; } = null!;

    public string ReferenceType { get; set; } = null!;

    public string ReferenceCode { get; set; } = null!;

    public string? ReferenceName { get; set; }

    public bool IsPrimary { get; set; }

    public string? Remark { get; set; }

    public DateTime CreatedDate { get; set; }
}
