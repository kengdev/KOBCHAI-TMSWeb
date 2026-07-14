using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobcashflowAdvanceOther
{
    public long Id { get; set; }

    public string AdvanceOther { get; set; } = null!;

    public decimal AdvanceOtherValue { get; set; }

    public string JobDetailCode { get; set; } = null!;

    public string TruckCode { get; set; } = null!;

    public string DriverCode { get; set; } = null!;

    public string AdvanceCode { get; set; } = null!;
}
