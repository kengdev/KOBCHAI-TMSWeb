using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class AdvanceRequest
{
    public long RequestId { get; set; }

    public Guid RequestGuid { get; set; }

    public string RequestCode { get; set; } = null!;

    public long FundId { get; set; }

    public int ExpenseTypeId { get; set; }

    public DateTime RequestDate { get; set; }

    public decimal RequestAmount { get; set; }

    public decimal? ApprovedAmount { get; set; }

    public string? RequestedBy { get; set; }

    public string? PayeeType { get; set; }

    public string? PayeeCode { get; set; }

    public string? PayeeName { get; set; }

    public string Purpose { get; set; } = null!;

    public string? Remark { get; set; }

    public string Status { get; set; } = null!;

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? PaidBy { get; set; }

    public DateTime? PaidDate { get; set; }

    public string? CancelledBy { get; set; }

    public DateTime? CancelledDate { get; set; }

    public string? CancelledReason { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? JobDetailCode { get; set; }

    public virtual AdvanceFundTransaction? AdvanceFundTransaction { get; set; }

    public virtual ICollection<AdvanceRequestReference> AdvanceRequestReferences { get; set; } = new List<AdvanceRequestReference>();

    public virtual AdvanceExpenseType ExpenseType { get; set; } = null!;

    public virtual AdvanceFund Fund { get; set; } = null!;

    public virtual TblJobDetail? JobDetailCodeNavigation { get; set; }
}
