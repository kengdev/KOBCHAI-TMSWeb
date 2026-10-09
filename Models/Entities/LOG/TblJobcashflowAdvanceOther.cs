using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobcashflowAdvanceOther
{
    public long Id { get; set; }

    public string AdvanceOther { get; set; } = null!;

    public decimal AdvanceOtherValue { get; set; }

    public long CashflowAdvanceId { get; set; }

    public virtual TblJobcashflowAdvance CashflowAdvance { get; set; } = null!;
}
