using System;
using System.Collections.Generic;

namespace TMSWeb.Models.Entities.LOG;

public partial class ViewJobListActive
{
    public string? JobCode { get; set; }

    public string? JobDetailCode { get; set; }

    public string? JobType { get; set; }

    public string? TradeType { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerNameTh { get; set; }

    public string? CustomerAcronym { get; set; }

    public string? CustomerTypeCode { get; set; }

    public string? BookingNo { get; set; }

    public string? ContainerSizeCode { get; set; }

    public string? ContainerSizeName { get; set; }

    public int? ContainerAmount { get; set; }

    public int? LiftedAmount { get; set; }

    public string? AgentCode { get; set; }

    public DateTime? JobDate { get; set; }

    public DateTime? LoadingDate { get; set; }

    public string? JobStatus { get; set; }

    public string? ContainerNo { get; set; }

    public string? SealNo { get; set; }

    public string? TruckCode { get; set; }

    public string? TruckLicense { get; set; }

    public string? TruckNo { get; set; }

    public string? TrailerLicense { get; set; }

    public string? SupplierCode { get; set; }

    public string? SupplierName { get; set; }

    public string? FleetTypeCode { get; set; }

    public string? DriverCode { get; set; }

    public string? DriverFullname { get; set; }

    public string? DriverPhone { get; set; }

    public decimal? CostValue { get; set; }

    public string? JobRemark { get; set; }

    public string? TrackingStatus { get; set; }

    public string? ClosingTime { get; set; }

    public string? JobInvoice { get; set; }

    public decimal? TareWeight { get; set; }

    public bool? IsApprove { get; set; }

    public bool? IsDocument { get; set; }

    public bool? IsBilling { get; set; }

    public bool? IsCanceled { get; set; }

    public string? StartCode { get; set; }

    public string? StartName { get; set; }

    public string? MissionCode { get; set; }

    public string? MissionName { get; set; }

    public string? FinishCode { get; set; }

    public string? FinishName { get; set; }

    public bool? TripcostRule { get; set; }

    public bool? IsLiftCashing { get; set; }

    public string? SupplierInvoice { get; set; }

    public string? HashCode { get; set; }

    public bool? IsWeeklypayment { get; set; }

    public string? JobName { get; set; }

    public string? ReceiptName { get; set; }

    public string? ReceiptAddress { get; set; }

    public string? ReceiptTax { get; set; }

    public string? AgentName { get; set; }

    public long? Id { get; set; }

    public decimal? CashFlow { get; set; }

    public string? RoundTrip { get; set; }

    public decimal? DriverAllowance { get; set; }

    public decimal? SupplierAllowance { get; set; }

    public string? TruckTypeCode { get; set; }

    public string? TruckTypeName { get; set; }
}
