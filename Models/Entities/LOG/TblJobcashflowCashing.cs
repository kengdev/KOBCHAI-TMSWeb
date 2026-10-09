using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobcashflowCashing
{
    public long Id { get; set; }

    public string JobDetailCode { get; set; } = null!;

    public string TruckCode { get; set; } = null!;

    public string DriverCode { get; set; } = null!;

    public string SupplierCode { get; set; } = null!;

    public string Cashing { get; set; } = null!;

    public decimal CashingValue { get; set; }

    public virtual TblDriver DriverCodeNavigation { get; set; } = null!;

    public virtual TblJobDetail JobDetailCodeNavigation { get; set; } = null!;

    public virtual TblSupplier SupplierCodeNavigation { get; set; } = null!;

    public virtual TblTruck TruckCodeNavigation { get; set; } = null!;
}
