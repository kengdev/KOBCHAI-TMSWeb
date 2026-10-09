using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobadvance
{
    public string AdvanceCode { get; set; } = null!;

    public string JobDetailCode { get; set; } = null!;

    public decimal AdvanceLifton { get; set; }

    public decimal AdvanceLiftoff { get; set; }

    public decimal AdvanceYard { get; set; }

    public decimal AdvancePort { get; set; }

    public decimal AdvanceTotal { get; set; }

    public string AdvanceRemark { get; set; } = null!;

    public bool IsBilling { get; set; }

    public bool IsClearing { get; set; }

    public DateOnly? ClearingDate { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    public string UpdateBy { get; set; } = null!;

    public virtual TblJobDetail JobDetailCodeNavigation { get; set; } = null!;

    public virtual ICollection<TblJobadvanceOther> TblJobadvanceOthers { get; set; } = new List<TblJobadvanceOther>();
}
