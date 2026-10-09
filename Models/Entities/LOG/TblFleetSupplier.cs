using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblFleetSupplier
{
    public string FleetSupplierCode { get; set; } = null!;

    public string FleetSupplierName { get; set; } = null!;

    public string FleetSuppplierTypeCode { get; set; } = null!;

    public string? Remark { get; set; }

    public bool IsActived { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public virtual TblFleetSupplierType FleetSuppplierTypeCodeNavigation { get; set; } = null!;

    public virtual ICollection<TblTruckDetail> TblTruckDetails { get; set; } = new List<TblTruckDetail>();
}
