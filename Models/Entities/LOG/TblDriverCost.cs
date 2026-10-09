using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblDriverCost
{
    public long Id { get; set; }

    public string DriverCode { get; set; } = null!;

    public string JobDetailCode { get; set; } = null!;

    public string TripRound { get; set; } = null!;

    public decimal TripValue { get; set; }

    public bool IsWeeklypayment { get; set; }

    public virtual TblDriver DriverCodeNavigation { get; set; } = null!;

    public virtual TblJobDetail JobDetailCodeNavigation { get; set; } = null!;
}
