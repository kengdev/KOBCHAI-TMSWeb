using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblTruck
{
    public string TruckCode { get; set; } = null!;

    public string TruckLicense { get; set; } = null!;

    public bool IsActived { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public virtual ICollection<TblJobAllowance> TblJobAllowances { get; set; } = new List<TblJobAllowance>();

    public virtual ICollection<TblJobDetail> TblJobDetails { get; set; } = new List<TblJobDetail>();

    public virtual ICollection<TblJobTracking> TblJobTrackings { get; set; } = new List<TblJobTracking>();

    public virtual ICollection<TblJobTruck> TblJobTrucks { get; set; } = new List<TblJobTruck>();

    public virtual ICollection<TblJobcashflowAdvance> TblJobcashflowAdvances { get; set; } = new List<TblJobcashflowAdvance>();

    public virtual ICollection<TblJobcashflowCashing> TblJobcashflowCashings { get; set; } = new List<TblJobcashflowCashing>();

    public virtual ICollection<TblJobcashflow> TblJobcashflows { get; set; } = new List<TblJobcashflow>();

    public virtual TblTruckDetail? TblTruckDetail { get; set; }

    public virtual ICollection<TblTruckDriver> TblTruckDrivers { get; set; } = new List<TblTruckDriver>();

    public virtual ICollection<TblTruckTrailer> TblTruckTrailers { get; set; } = new List<TblTruckTrailer>();

    public virtual ICollection<TblFleetpayment> FleetpaymentCodes { get; set; } = new List<TblFleetpayment>();
}
