using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblCustomer
{
    public string CustomerCode { get; set; } = null!;

    public string CustomerVatNo { get; set; } = null!;

    public string? CustomerExten { get; set; }

    public string CustomerNameTh { get; set; } = null!;

    public string CustomerNameEn { get; set; } = null!;

    public string CustomerAcronym { get; set; } = null!;

    public string CustomerTypeCode { get; set; } = null!;

    public string CustomerAddress { get; set; } = null!;

    public string VendorCode { get; set; } = null!;

    public string DocType { get; set; } = null!;

    public bool IsActived { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    public string UpdateBy { get; set; } = null!;

    public virtual TblCustomerType CustomerTypeCodeNavigation { get; set; } = null!;

    public virtual TblCustomerAddress? TblCustomerAddress { get; set; }

    public virtual ICollection<TblCustomerCost> TblCustomerCosts { get; set; } = new List<TblCustomerCost>();

    public virtual TblCustomerRule? TblCustomerRule { get; set; }

    public virtual ICollection<TblJobMaster> TblJobMasters { get; set; } = new List<TblJobMaster>();

    public virtual ICollection<TblReceipt> TblReceipts { get; set; } = new List<TblReceipt>();
}
