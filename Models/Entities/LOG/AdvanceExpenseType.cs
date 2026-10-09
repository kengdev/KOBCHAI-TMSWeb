using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class AdvanceExpenseType
{
    public int ExpenseTypeId { get; set; }

    public string ExpenseTypeCode { get; set; } = null!;

    public string ExpenseTypeName { get; set; } = null!;

    public bool IsTripRelated { get; set; }

    public bool RequireSettlement { get; set; }

    public bool RequireDocument { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<AdvanceRequest> AdvanceRequests { get; set; } = new List<AdvanceRequest>();
}
