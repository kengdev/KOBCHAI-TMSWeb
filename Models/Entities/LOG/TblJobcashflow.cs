using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobcashflow
{
    public long Id { get; set; }

    public string JobDetailCode { get; set; } = null!;

    public string TruckCode { get; set; } = null!;

    public string DriverCode { get; set; } = null!;

    public decimal CashFlow { get; set; }

    public string AdvanceRemark { get; set; } = null!;

    public bool IsClearing { get; set; }

    public DateTime? ClearingDate { get; set; }

    public virtual TblDriver DriverCodeNavigation { get; set; } = null!;

    public virtual TblJobDetail JobDetailCodeNavigation { get; set; } = null!;

    public virtual TblTruck TruckCodeNavigation { get; set; } = null!;
}
