using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobcashflowAdvance
{
    public long Id { get; set; }

    public string JobDetailCode { get; set; } = null!;

    public string TruckCode { get; set; } = null!;

    public string DriverCode { get; set; } = null!;

    public string SupplierCode { get; set; } = null!;

    public decimal AdvanceLifton { get; set; }

    public decimal AdvanceLiftoff { get; set; }

    public decimal AdvanceYard { get; set; }

    public decimal AdvancePort { get; set; }

    public decimal AdvanceTotal { get; set; }

    public virtual TblDriver DriverCodeNavigation { get; set; } = null!;

    public virtual TblJobDetail JobDetailCodeNavigation { get; set; } = null!;

    public virtual TblSupplier SupplierCodeNavigation { get; set; } = null!;

    public virtual ICollection<TblJobcashflowAdvanceOther> TblJobcashflowAdvanceOthers { get; set; } = new List<TblJobcashflowAdvanceOther>();

    public virtual TblTruck TruckCodeNavigation { get; set; } = null!;
}
