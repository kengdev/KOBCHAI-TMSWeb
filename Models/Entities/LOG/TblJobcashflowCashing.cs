using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobcashflowCashing
{
    public long Id { get; set; }

    public string Cashing { get; set; } = null!;

    public decimal CashingValue { get; set; }

    public string JobDetailCode { get; set; } = null!;

    public string TruckCode { get; set; } = null!;

    public string DriverCode { get; set; } = null!;

    public virtual TblJobcashflow TblJobcashflow { get; set; } = null!;
}
