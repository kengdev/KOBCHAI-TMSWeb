using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class AdvanceFundTransaction
{
    public long TransactionId { get; set; }

    public Guid TransactionGuid { get; set; }

    public string FundCode { get; set; } = null!;

    public string RequestCode { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal Amount { get; set; }

    public long? ReferenceTransactionId { get; set; }

    public string? ReferenceNo { get; set; }

    public long? CashflowId { get; set; }

    public string? Remark { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual AdvanceFund FundCodeNavigation { get; set; } = null!;

    public virtual ICollection<AdvanceFundTransaction> InverseReferenceTransaction { get; set; } = new List<AdvanceFundTransaction>();

    public virtual AdvanceFundTransaction? ReferenceTransaction { get; set; }

    public virtual AdvanceRequest RequestCodeNavigation { get; set; } = null!;
}
