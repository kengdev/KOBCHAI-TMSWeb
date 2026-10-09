using Microsoft.EntityFrameworkCore;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class DropListRepository
{
    private DbTMSContext db;

    public DropListRepository(DbTMSContext context)
    {
        db = context;
    }

    public async Task<List<DropDownViewModel>> GetSupplierType()
    {
        var query = await (from a in db.TblSupplierTypes
                    select new DropDownViewModel
                    {
                        key = a.SupplierTypeCode,
                        label = a.SupplierTypeName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetZone()
    {
        var query = await (from a in db.TblZones
                           select new DropDownViewModel
                           {
                               key = a.ZoneCode,
                               label = a.ZoneName
                           }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetFleet()
    {
        var query = await (from a in db.TblFleetSupplierTypes
                    select new DropDownViewModel
                    {
                        key = a.FleetSupplierTypeCode,
                        label = a.FleetSupplierTypeName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetCustomerType()
    {
        var query = await (from a in db.TblCustomerTypes
                           select new DropDownViewModel
                           {
                               key = a.CustomerTypeCode,
                               label = a.CustomerTypeName
                           }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetSupplierTransport()
    {
        var query = await (from a in db.TblSuppliers
                    where (a.IsActived == true && a.FleetTypeCode == "F2") || a.SupplierCode == "SP26001"
                    orderby a.SupplierName ascending
                    select new DropDownViewModel
                    {
                        key = a.SupplierCode,
                        label = a.SupplierName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public List<DropDownViewModel> GetSupplierSupport()
    {
        var query = from a in db.TblSuppliers
                    where a.FleetTypeCode == "F3" && a.IsActived == true
                    select new DropDownViewModel
                    {
                        key = a.SupplierCode,
                        label = a.SupplierName
                    };
        return query.ToList();
    }

    public async Task<List<DropDownViewModel>> GetTruckBrand()
    {
        var query = await (from a in db.TblBrands
                    select new DropDownViewModel
                    {
                        key = a.BrandCode,
                        label = a.BrandName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetTruckType()
    {
        var query = await (from a in db.TblTruckTypes
                    select new DropDownViewModel
                    {
                        key = a.TruckTypeCode,
                        label = a.TruckTypeName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetFuelType()
    {
        var query = await (from a in db.TblFuelTypes
                    select new DropDownViewModel
                    {
                        key = a.FuelTypeCode,
                        label = a.FuelTypeName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public List<DropDownViewModel> GetTruckYear()
    {
        string y = DateTime.Now.Year.ToString();
        int current = int.Parse(y);
        List<DropDownViewModel> list = new List<DropDownViewModel>();
        for (int i = 2005; i <= current; i++)
        {
            DropDownViewModel item = new DropDownViewModel();
            item.key = i.ToString();
            item.label = i.ToString();
            list.Add(item);
        }

        return list;
    }

    public async Task<List<DropDownViewModel>> GetProvince()
    {
        var query = await (from a in db.LcProvinces
                           orderby a.ProvinceNameTha ascending
                           select new DropDownViewModel
                           {
                               key = a.ProvinceCode,
                               label = a.ProvinceNameTha
                           }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetInsurance()
    {
        var query = await (from a in db.TblInsurances
                    select new DropDownViewModel
                    {
                        key = a.InsuranceCode,
                        label = a.InsuranceName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetInsuranceType()
    {
        var query = await (from a in db.TblInsuranceTypes
                    select new DropDownViewModel
                    {
                        key = a.InsuranceTypeCode,
                        label = a.InsuranceTypeName
                    }).AsNoTracking().ToListAsync();
        
        return query;
    }

    public async Task<List<DropDownViewModel>> GetTrailerType()
    {
        var query = await (from a in db.TblTrailerTypes
                    select new DropDownViewModel
                    {
                        key = a.TrailerTypeCode,
                        label = a.TrailerTypeName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetTrailerFloor()
    {
        var query = await (from a in db.TblTrailerFloors
                    select new DropDownViewModel
                    {
                        key = a.TrailerFloorCode,
                        label = a.TrailerFloorName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetDriver()
    {
        var query = await (from a in db.TblDrivers
                    where a.IsEnabled == true
                    select new DropDownViewModel
                    {
                        key = a.DriverCode,
                        label = a.DriverFullname
                    }).AsNoTracking().OrderBy(o => o.label).ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetTruck()
    {
        var query = await (from a in db.TblTrucks
                     join b in db.TblTruckDetails on a.TruckCode equals b.TruckCode
                     join c in db.TblTruckTypes on b.TruckTypeCode equals c.TruckTypeCode
                     where a.IsActived == true
                     orderby b.TruckNo
                     select new DropDownViewModel
                     {
                         key = a.TruckCode,
                         label = a.TruckLicense + " [" + c.TruckTypeName + "]"
                     }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetChangeDriver(string driver_code)
    {
        var query = await (from a in db.TblDrivers
                           where a.IsEnabled == true && a.DriverCode != driver_code
                           select new DropDownViewModel
                           {
                               key = a.DriverCode,
                               label = a.DriverFullname
                           }).AsNoTracking().OrderBy(o => o.label).ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetChangeTruck(string truck_code)
    {
        var query = await (from a in db.TblTrucks
                           join b in db.TblTruckDetails on a.TruckCode equals b.TruckCode
                           join c in db.TblTruckTypes on b.TruckTypeCode equals c.TruckTypeCode
                           where a.IsActived == true && a.TruckCode != truck_code
                           orderby b.TruckNo
                           select new DropDownViewModel
                           {
                               key = a.TruckCode,
                               label = a.TruckLicense + " [" + c.TruckTypeName + "]"
                           }).AsNoTracking().ToListAsync();

        return query;
    }

    public List<DropDownViewModel> GetContainerTruck()
    {
        var query = from a in db.TblTrucks
                    join b in db.TblTruckDetails on a.TruckCode equals b.TruckCode
                    where a.IsActived == true && b.TruckTypeCode != "03"
                    orderby b.TruckNo
                    select new DropDownViewModel
                    {
                        key = a.TruckCode,
                        label = a.TruckLicense + " [" + b.TruckNo + "]"
                    };
        return query.ToList();
    }

    public List<DropDownViewModel> Get4WTruck()
    {
        var query = from a in db.TblTrucks
                    join b in db.TblTruckDetails on a.TruckCode equals b.TruckCode
                    where a.IsActived == true && b.TruckTypeCode == "03" && b.TruckCode != "T000000"
                    orderby b.TruckNo
                    select new DropDownViewModel
                    {
                        key = a.TruckCode,
                        label = a.TruckLicense + " [" + b.TruckNo + "]"
                    };
        return query.ToList();
    }

    public List<DropDownViewModel> GetTrailer()
    {
        var query = from a in db.ViewTrailerListActives
                    where a.IsActived == true
                    orderby a.TrailerLicense
                    select new DropDownViewModel
                    {
                        key = a.TrailerCode,
                        label = a.TrailerLicense + " [" + a.TrailerNo + "]"
                    };
        return query.ToList();
    }

    public List<DropDownViewModel> GetMonth()
    {
        List<DropDownViewModel> list = new List<DropDownViewModel>();
        for (int i = 1; i <= 12; i++)
        {
            DropDownViewModel item = new DropDownViewModel();
            switch (i)
            {
                case 1:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "มกราคม";
                    break;
                case 2:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "กุมภาพันธ์";
                    break;
                case 3:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "มีนาคม";
                    break;
                case 4:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "เมษายน";
                    break;
                case 5:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "พฤษภาคม";
                    break;
                case 6:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "มิถุนายน";
                    break;
                case 7:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "กรกฎาคม";
                    break;
                case 8:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "สิงหาคม";
                    break;
                case 9:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "กันยายน";
                    break;
                case 10:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "ตุลาคม";
                    break;
                case 11:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "พฤศจิกายน";
                    break;
                case 12:
                    item.key = i.ToString().PadLeft(2, '0');
                    item.label = "ธันวาคม";
                    break;
            }

            list.Add(item);
        }

        return list;
    }

    public List<DropDownViewModel> GetBranchType()
    {
        List<DropDownViewModel> list = new List<DropDownViewModel>();
        DropDownViewModel item = new DropDownViewModel();
        item.key = "1";
        item.label = "สำนักงานใหญ่";
        list.Add(item);

        item = new DropDownViewModel();
        item.key = "2";
        item.label = "สาขา";
        list.Add(item);

        return list;
    }

    public List<DropDownViewModel> GetReceptType()
    {
        List<DropDownViewModel> list = new List<DropDownViewModel>();
        var item = new DropDownViewModel();
        item.key = "01";
        item.label = "เงินสด";
        list.Add(item);

        item = new DropDownViewModel();
        item.key = "02";
        item.label = "โอนเงิน";
        list.Add(item);

        item = new DropDownViewModel();
        item.key = "03";
        item.label = "เช็คธนาคาร";
        list.Add(item);

        return list;
    }

    public List<DropDownViewModel> GetYardPortType()
    {
        List<DropDownViewModel> list = new List<DropDownViewModel>();
        var item = new DropDownViewModel();
        item.key = "PORT";
        item.label = "ท่าเรือ";
        list.Add(item);

        item = new DropDownViewModel();
        item.key = "YARD";
        item.label = "ลานตู้";
        list.Add(item);
        return list;
    }

    public async Task<List<DropDownViewModel>> GetAgent()
    {
        var query = await (from a in db.TblAgents
                    where a.IsActived == true
                    orderby a.AgentName
                    select new DropDownViewModel
                    {
                        key = a.AgentName,
                        label = a.AgentName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public List<DropDownViewModel> GetCostType()
    {
        List<DropDownViewModel> list = new List<DropDownViewModel>();
        var item = new DropDownViewModel();

        item.key = "A";
        item.label = "คิดเป็นยอดเงิน";
        list.Add(item);

        item = new DropDownViewModel();
        item.key = "B";
        item.label = "คิดเป็นเปอร์เซนต์";
        list.Add(item);

        return list;
    }

    public async Task<List<DropDownViewModel>> GetContainerSize()
    {
        var query = await (from a in db.TblContainerSizes
                    orderby a.ContainerSizeName
                    select new DropDownViewModel
                    {
                        key = a.ContainerSizeCode,
                        label = a.ContainerSizeName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetBank()
    {
        var query = await (from a in db.TblBanks
                    select new DropDownViewModel
                    {
                        key = a.BankCode,
                        label = a.BankName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public List<DropDownViewModel> GetYear()
    {
        List<DropDownViewModel> list = new List<DropDownViewModel>();
        DropDownViewModel item = new DropDownViewModel();
        string year = DateTime.Now.ToString("yyyy");
        int current_year = int.Parse(year);

        string last = DateTime.Now.AddYears(-1).ToString("yyyy");
        int last_year = int.Parse(last);

        item.key = current_year.ToString();
        item.label = current_year.ToString();
        list.Add(item);

        item = new DropDownViewModel();
        item.key = last_year.ToString();
        item.label = last_year.ToString();
        list.Add(item);

        string last_2 = DateTime.Now.AddYears(-2).ToString("yyyy");
        int last_2_year = int.Parse(last_2);

        item = new DropDownViewModel();
        item.key = last_2_year.ToString();
        item.label = last_2_year.ToString();
        list.Add(item);

        return list;
    }

    public List<DropDownViewModel> GetMaintenanceStatus()
    {
        List<DropDownViewModel> list = new List<DropDownViewModel>();
        var item = new DropDownViewModel();
        item.key = "S";
        item.label = "หยุดงาน";
        list.Add(item);

        item = new DropDownViewModel();
        item.key = "R";
        item.label = "ปฏิบัติงานตามปกติ";
        list.Add(item);

        return list;
    }

    /*public List<DropDownViewModel> GetSpareparts()
    {
        var query = from a in db.ViewSparepartsStockeds
                    select new DropDownViewModel
                    {
                        key = a.SparepartCode,
                        label = a.SparepartName
                    };
        return query.ToList();
    }*/

    public async Task<List<DropDownViewModel>> GetShipper()
    {
        var query = await (from a in db.TblShippers
                    select new DropDownViewModel
                    {
                        key = a.ShipperName,
                        label = a.ShipperName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public async Task<List<DropDownViewModel>> GetRequestBy()
    {
        var query = await (from a in db.TblRequests
                    select new DropDownViewModel
                    {
                        key = a.RequestName,
                        label = a.RequestName
                    }).AsNoTracking().ToListAsync();

        return query;
    }

    public List<DropDownViewModel> GetTruckTrailer()
    {
        List<DropDownViewModel> list = new List<DropDownViewModel>();
        DropDownViewModel item = new DropDownViewModel();
        item.key = "1";
        item.label = "หัวลาก";
        list.Add(item);

        item = new DropDownViewModel();
        item.key = "2";
        item.label = "หางลาก";
        list.Add(item);

        return list;
    }

    public List<DropDownViewModel> GetTyreChangeType()
    {
        List<DropDownViewModel> list = new List<DropDownViewModel>();
        DropDownViewModel item = new DropDownViewModel();
        item.key = "1";
        item.label = "ปกติ";
        list.Add(item);

        item = new DropDownViewModel();
        item.key = "2";
        item.label = "หล่อ";
        list.Add(item);

        return list;
    }
}