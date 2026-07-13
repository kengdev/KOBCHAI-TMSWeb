using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblSupplierAllowance
{
    public long Id { get; set; }

    public DateOnly JobDate { get; set; }

    public string SupplierCode { get; set; } = null!;

    public string JobDetailCode { get; set; } = null!;

    public decimal SupplierAllowance { get; set; }

    public bool IsComplete { get; set; }

    public bool IsPayment { get; set; }

    public virtual TblJobDetail JobDetailCodeNavigation { get; set; } = null!;

    public virtual TblSupplier SupplierCodeNavigation { get; set; } = null!;
}
