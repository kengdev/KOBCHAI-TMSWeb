namespace TMSWeb.Models.ViewModels;

public class TruckViewModel
{
    public string truck_code { get; set; }
    public string truck_license { get; set; }
    public string truck_chassis_number { get; set; }
    public string truck_type_name { get; set; }
    public string fuel_type_name { get; set; }
    public string brand_name { get; set; }
    public string truck_no { get; set; }
    public string driver_code { get; set; }
    public string driver_name { get; set; }
    public string driver_phone { get; set; }
    public string fleet_supplier_code { get; set; }
    public string fleet_supplier_name { get; set; }
    public string fleet_supplier_type_code { get; set; }
    public string fleet_supplier_type_name { get; set; }
    public bool is_actived { get; set; }
}

public class TrailerViewModel
{
    public string trailer_code { get; set; }
    public string trailer_license { get; set; }
    public string trailer_no { get; set; }
    public string trailer_type_name { get; set; }
    public string trailer_floor_name { get; set; }
    public string truck_license { get; set; }
    public bool is_actived { get; set; }
}

public class UpdateTrackingJobModel
{
    public string job_detail_code { get; set; }
    public string job_type { get; set; }
    public string driver_code { get; set; }
    public string truck_code { get; set; }
    public string supplier_code { get; set; }
    public string container_no { get; set; }
    public string seal_no { get; set; }
    public byte[] container_pic { get; set; }
    public string trailer_license { get; set; }
    public byte[] seal_pic { get; set; }
    public decimal tare_weight { get; set; }
    public decimal yard { get; set; }
    public int fuel { get; set; }
    public decimal port { get; set; }
    public decimal express { get; set; }
    public decimal lift1 { get; set; }
    public decimal lift2 { get; set; }
    public decimal lift3 { get; set; }
    public string status { get; set; }
    public string Lat { get; set; }
    public string Lon { get; set; }
    public List<byte[]> floor_images { get; set; }
}