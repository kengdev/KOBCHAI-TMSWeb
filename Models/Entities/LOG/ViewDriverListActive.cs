using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class ViewDriverListActive
{
    public string DriverCode { get; set; } = null!;

    public string? TruckCode { get; set; }

    public string? TruckLicense { get; set; }

    public string? TruckNo { get; set; }

    public string? TruckTypegroupCode { get; set; }

    public string? TruckTypegroupName { get; set; }

    public string? TruckTypeName { get; set; }

    public string DriverFullname { get; set; } = null!;

    public string DriverPhone { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public string? FleetSupplierTypeName { get; set; }

    public string? FleetSupplierTypeCode { get; set; }
}
