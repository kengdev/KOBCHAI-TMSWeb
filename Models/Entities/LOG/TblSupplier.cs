using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblSupplier
{
    public string SupplierCode { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public string SupplierFullname { get; set; } = null!;

    public string SupplierPhone { get; set; } = null!;

    public string SupplierTypeCode { get; set; } = null!;

    public string FleetTypeCode { get; set; } = null!;

    public string SupplierAddress { get; set; } = null!;

    public string SupplierVatNo { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string BankId { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public bool IsSpecialVat { get; set; }

    public bool IsActived { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    public string UpdateBy { get; set; } = null!;

    public virtual ICollection<TblJobAllowance> TblJobAllowances { get; set; } = new List<TblJobAllowance>();

    public virtual ICollection<TblJobDetail> TblJobDetails { get; set; } = new List<TblJobDetail>();

    public virtual ICollection<TblJobTracking> TblJobTrackings { get; set; } = new List<TblJobTracking>();

    public virtual ICollection<TblJobcashflowAdvance> TblJobcashflowAdvances { get; set; } = new List<TblJobcashflowAdvance>();

    public virtual ICollection<TblJobcashflowCashing> TblJobcashflowCashings { get; set; } = new List<TblJobcashflowCashing>();

    public virtual ICollection<TblSupplierAllowance> TblSupplierAllowances { get; set; } = new List<TblSupplierAllowance>();

    public virtual ICollection<TblSupplierRate> TblSupplierRates { get; set; } = new List<TblSupplierRate>();

    public virtual ICollection<TblSupplierpayment> TblSupplierpayments { get; set; } = new List<TblSupplierpayment>();
}
