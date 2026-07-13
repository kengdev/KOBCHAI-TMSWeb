using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobAllowance
{
    public long Id { get; set; }

    public DateOnly JobDate { get; set; }

    public string JobDetailCode { get; set; } = null!;

    public string TruckCode { get; set; } = null!;

    public string DriverCode { get; set; } = null!;

    public decimal DriverAllowance { get; set; }

    public string SupplierCode { get; set; } = null!;

    public decimal SupplierAllowance { get; set; }

    public bool IsComplete { get; set; }

    public bool IsWeeklypayment { get; set; }

    public bool IsMonthlypayment { get; set; }

    public virtual TblDriver DriverCodeNavigation { get; set; } = null!;

    public virtual TblJobDetail JobDetailCodeNavigation { get; set; } = null!;
}
