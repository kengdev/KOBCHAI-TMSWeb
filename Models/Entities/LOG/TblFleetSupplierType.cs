using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblFleetSupplierType
{
    public string FleetSupplierTypeCode { get; set; } = null!;

    public string FleetSupplierTypeName { get; set; } = null!;

    public virtual ICollection<TblFleetRate> TblFleetRates { get; set; } = new List<TblFleetRate>();

    public virtual ICollection<TblFleetSupplier> TblFleetSuppliers { get; set; } = new List<TblFleetSupplier>();
}
