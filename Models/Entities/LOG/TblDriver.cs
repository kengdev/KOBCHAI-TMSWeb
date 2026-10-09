using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class TblDriver
{
    public string DriverCode { get; set; } = null!;

    public string DriverFullname { get; set; } = null!;

    public string DriverPhone { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public virtual ICollection<TblAllowance> TblAllowances { get; set; } = new List<TblAllowance>();

    public virtual ICollection<TblDriverCost> TblDriverCosts { get; set; } = new List<TblDriverCost>();

    public virtual ICollection<TblDriverSpecialvalue> TblDriverSpecialvalues { get; set; } = new List<TblDriverSpecialvalue>();

    public virtual ICollection<TblJobAllowance> TblJobAllowances { get; set; } = new List<TblJobAllowance>();

    public virtual ICollection<TblJobDetail> TblJobDetails { get; set; } = new List<TblJobDetail>();

    public virtual ICollection<TblJobTracking> TblJobTrackings { get; set; } = new List<TblJobTracking>();

    public virtual ICollection<TblJobTruck> TblJobTrucks { get; set; } = new List<TblJobTruck>();

    public virtual ICollection<TblJobcashflowAdvance> TblJobcashflowAdvances { get; set; } = new List<TblJobcashflowAdvance>();

    public virtual ICollection<TblJobcashflowCashing> TblJobcashflowCashings { get; set; } = new List<TblJobcashflowCashing>();

    public virtual ICollection<TblJobcashflow> TblJobcashflows { get; set; } = new List<TblJobcashflow>();

    public virtual ICollection<TblTruckDriver> TblTruckDrivers { get; set; } = new List<TblTruckDriver>();
}
