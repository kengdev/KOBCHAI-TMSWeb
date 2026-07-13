using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblJobMaster
{
    public string JobCode { get; set; } = null!;

    public string? JobType { get; set; }

    public string TradeType { get; set; } = null!;

    public string TruckTypegroupCode { get; set; } = null!;

    public string CustomerCode { get; set; } = null!;

    public string BookingNo { get; set; } = null!;

    public string ContainerSizeCode { get; set; } = null!;

    public string ContainerSizeName { get; set; } = null!;

    public string AgentCode { get; set; } = null!;

    public string JobIncoterm { get; set; } = null!;

    public string JobInvoice { get; set; } = null!;

    public string? OtherAddress { get; set; }

    public string JobRemark { get; set; } = null!;

    public string? CustomerRemark { get; set; }

    public string BookingPeriod { get; set; } = null!;

    public DateTime JobDate { get; set; }

    public DateTime LoadingDate { get; set; }

    public string ClosingTime { get; set; } = null!;

    public int ContainerAmount { get; set; }

    public int LiftedAmount { get; set; }

    public string CostCode { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    public string UpdateBy { get; set; } = null!;

    public virtual TblContainerSize ContainerSizeCodeNavigation { get; set; } = null!;

    public virtual TblCustomerCost CostCodeNavigation { get; set; } = null!;

    public virtual TblCustomer CustomerCodeNavigation { get; set; } = null!;

    public virtual ICollection<TblBindingGenerate> TblBindingGenerates { get; set; } = new List<TblBindingGenerate>();

    public virtual ICollection<TblJobDetail> TblJobDetails { get; set; } = new List<TblJobDetail>();

    public virtual ICollection<TblJobOcean> TblJobOceans { get; set; } = new List<TblJobOcean>();

    public virtual ICollection<TblJobPlace> TblJobPlaces { get; set; } = new List<TblJobPlace>();

    public virtual TblTruckTypegroup TruckTypegroupCodeNavigation { get; set; } = null!;
}
