using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobcashing
{
    public long Id { get; set; }

    public string Cashing { get; set; } = null!;

    public decimal CashingValue { get; set; }

    public string JobDetailCode { get; set; } = null!;

    public virtual TblJobDetail JobDetailCodeNavigation { get; set; } = null!;
}
