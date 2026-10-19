using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class AdvanceFund
{
    public string FundCode { get; set; } = null!;

    public Guid FundGuid { get; set; }

    public string FundName { get; set; } = null!;

    public string? HolderCode { get; set; }

    public string? Remark { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal CurrentBalance { get; set; }

    public virtual ICollection<AdvanceFundTransaction> AdvanceFundTransactions { get; set; } = new List<AdvanceFundTransaction>();

    public virtual ICollection<AdvanceRequest> AdvanceRequests { get; set; } = new List<AdvanceRequest>();
}
