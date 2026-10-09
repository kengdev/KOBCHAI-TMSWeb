using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class ViewTruckListActive
{
    public string TruckCode { get; set; } = null!;

    public string TruckNo { get; set; } = null!;

    public string TruckLicense { get; set; } = null!;

    public string TruckChassisNumber { get; set; } = null!;

    public string TruckYear { get; set; } = null!;

    public string TruckTypeName { get; set; } = null!;

    public string BrandName { get; set; } = null!;

    public string FuelTypeName { get; set; } = null!;

    public string TruckImage { get; set; } = null!;

    public bool IsActived { get; set; }

    public string? DriverCode { get; set; }

    public string? DriverFullname { get; set; }

    public string? DriverPhone { get; set; }

    public string FleetSupplierCode { get; set; } = null!;

    public string FleetSupplierName { get; set; } = null!;

    public string FleetSupplierTypeCode { get; set; } = null!;

    public string FleetSupplierTypeName { get; set; } = null!;
}
