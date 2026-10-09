using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobTracking
{
    public int Id { get; set; }

    public string JobDetailCode { get; set; } = null!;

    public string TruckCode { get; set; } = null!;

    public string DriverCode { get; set; } = null!;

    public string SupplierCode { get; set; } = null!;

    public string TrackingStatus { get; set; } = null!;

    public DateTime TrackingDate { get; set; }

    public string? Lattitude { get; set; }

    public string? Longitude { get; set; }

    public string? Speed { get; set; }

    public virtual TblDriver DriverCodeNavigation { get; set; } = null!;

    public virtual TblJobDetail JobDetailCodeNavigation { get; set; } = null!;

    public virtual TblSupplier SupplierCodeNavigation { get; set; } = null!;

    public virtual TblTruck TruckCodeNavigation { get; set; } = null!;
}
