using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblFleetRate
{
    public string FleetSupplierTypeCode { get; set; } = null!;

    public string PlaceCode { get; set; } = null!;

    public decimal Truck4w { get; set; }

    public decimal Truck6w { get; set; }

    public decimal Truck10w { get; set; }

    public decimal Trailer6w { get; set; }

    public decimal Trailer10w { get; set; }

    public decimal Trailer18w { get; set; }

    public decimal TrailerExtra { get; set; }

    public virtual TblFleetSupplierType FleetSupplierTypeCodeNavigation { get; set; } = null!;

    public virtual TblPlace PlaceCodeNavigation { get; set; } = null!;
}
