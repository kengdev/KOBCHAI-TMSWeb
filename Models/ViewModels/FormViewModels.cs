using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMSWeb.Models.Entities.LOG;

namespace TMSWeb.Models.ViewModels;

public class QuotationFormViewModel
{
    public string quotation_code { get; set; }
    public decimal start_rate { get; set; }
    public DateTime quotation_date { get; set; }
    public DateTime quotation_expire { get; set; }
    public string customer_name { get; set; }
    public string contact { get; set; }
    public string contact_line { get; set; }
    public DateOnly start { get; set; }
    public DateOnly expire { get; set; }
    public string route { get; set; }
    public string scope { get; set; }
    public string commodity { get; set; }
    public string container1 { get; set; }
    public string container2 { get; set; }
    public string container3 { get; set; }
    public string condition_hour { get; set; }
    public string condition_charged { get; set; }
    public decimal minimum_rate { get; set; }
    public decimal price_increase { get; set; }
    public decimal price_length { get; set; }
    public decimal charge_cutchassis { get; set; }
    public decimal insurance_rate { get; set; }
    public string remark { get; set; }
    public List<TblQuotationDetail> detail { get; set; }
    public List<TblQuotationCondition> condition { get; set; }
}

public class OrderFormViewModel
{
    public string customer_code { get; set; }
    public string customer_name { get; set; }
    public string customer_address { get; set; }
    public string other_address { get; set; }
    public string customer_tax { get; set; }
    public string job_name { get; set; }
    public string truck_license { get; set; }
    public string trailer_license { get; set; }
    public string truck_typegroup_code { get; set; }
    public string truck_typegroup_name { get; set; }
    public string driver_code { get; set; }
    public string driver_fullname { get; set; }
    public string job_code { get; set; }
    public string job_detail_code { get; set; }
    public string job_type { get; set; }
    public string container_size_code { get; set; }
    public string container_size { get; set; }
    public string port_no { get; set; }
    public string industry { get; set; }
    public int container_amount { get; set; }
    public decimal tare { get; set; }
    public decimal cash_flow { get; set; }
    public string mission_code { get; set; }
    public string mission_name { get; set; }
    public string zone_name { get; set; }
    public string receipt_name { get; set; }
    public string receipt_address { get; set; }
    public string receipt_tax { get; set; }

    public string mission_vat { get; set; }
    public string mission_address { get; set; }
    public string booking_no { get; set; }
    public string agent_code { get; set; }
    public string agent_name { get; set; }
    public string start_name { get; set; }
    public string finish_name { get; set; }
    public string container_no { get; set; }
    public string seal_no { get; set; }
    public DateTime job_date { get; set; }
    public DateTime loading_date { get; set; }
    public string closing_time { get; set; }
    public string job_invoice { get; set; }
    public string job_remark { get; set; }
    public int fuel_rate { get; set; }
    public decimal lift { get; set; }
    public decimal port { get; set; }
    public decimal yard { get; set; }
    public decimal express_way { get; set; }
    public string barcode { get; set; }
    public bool is2short { get; set; }
    public string supplyer_code { get; set; }
    public string tracking_status { get; set; }
    public string fleet_type { get; set; }
    public string delivery_date { get; set; }
}

