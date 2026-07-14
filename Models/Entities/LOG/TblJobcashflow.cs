using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobcashflow
{
    public string JobDetailCode { get; set; } = null!;

    public string TruckCode { get; set; } = null!;

    public string DriverCode { get; set; } = null!;

    public decimal CashFlow { get; set; }

    public decimal AdvanceLifton { get; set; }

    public decimal AdvanceLiftoff { get; set; }

    public decimal AdvanceYard { get; set; }

    public decimal AdvancePort { get; set; }

    public decimal AdvanceTotal { get; set; }

    public string AdvanceRemark { get; set; } = null!;

    public virtual TblDriver DriverCodeNavigation { get; set; } = null!;

    public virtual TblJobDetail JobDetailCodeNavigation { get; set; } = null!;

    public virtual TblJobcashflowAdvance? TblJobcashflowAdvance { get; set; }

    public virtual ICollection<TblJobcashflowCashing> TblJobcashflowCashings { get; set; } = new List<TblJobcashflowCashing>();

    public virtual TblTruck TruckCodeNavigation { get; set; } = null!;
}
