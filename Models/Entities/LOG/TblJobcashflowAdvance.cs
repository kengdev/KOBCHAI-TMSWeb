using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobcashflowAdvance
{
    public string JobDetailCode { get; set; } = null!;

    public string TruckCode { get; set; } = null!;

    public string DriverCode { get; set; } = null!;

    public decimal AdvanceLifton { get; set; }

    public decimal AdvanceLiftoff { get; set; }

    public decimal AdvanceYard { get; set; }

    public decimal AdvancePort { get; set; }

    public decimal AdvanceTotal { get; set; }

    public string AdvanceCode { get; set; } = null!;

    public virtual TblJobadvance AdvanceCodeNavigation { get; set; } = null!;

    public virtual TblJobcashflow TblJobcashflow { get; set; } = null!;
}
