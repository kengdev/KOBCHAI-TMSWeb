using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSWeb.Models.ViewModels;
public class DriverViewModel
{
    public string driver_code { get; set; }
    public string fullname { get; set; }
    public string supplyer_code { get; set; }
    public string supplyer_name { get; set; }
    public string truck_license { get; set; }
    public string truck_no { get; set; }
    public string truck_code { get; set; }
    public string truck_type_code { get; set; }
    public string trailer_license { get; set; }
    public string fleet_type { get; set; }
    public string start_date { get; set; }
    public string status { get; set; }
    public bool is_actived { get; set; }
}
