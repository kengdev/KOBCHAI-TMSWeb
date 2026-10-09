using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblTruckType
{
    public string TruckTypeCode { get; set; } = null!;

    public string TruckTypeName { get; set; } = null!;

    public virtual ICollection<TblJobMaster> TblJobMasters { get; set; } = new List<TblJobMaster>();

    public virtual ICollection<TblTruckDetail> TblTruckDetails { get; set; } = new List<TblTruckDetail>();
}
