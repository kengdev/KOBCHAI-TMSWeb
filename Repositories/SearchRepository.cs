using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;
public class SearchRepository
{
    private DbTMSContext db;

    public SearchRepository(DbTMSContext context)
    {
        db = context;
    }

    public async Task<List<YardPortViewModel>> GetYardPortDataList()
    {
        var data = await (from a in db.TblPlaces
                    where a.IsActived == true
                    select new YardPortViewModel
                    {
                        yardport_code = a.PlaceCode,
                        yardport_name = a.PlaceName,
                        yardport_type = a.PlaceType
                    }).ToListAsync();

        return data;
    }

    public async Task<List<YardPortViewModel>> GetPortDataList()
    {
        var data = await (from a in db.TblYardports
                    join b in db.TblPlaces on a.YardportCode equals b.PlaceCode
                    where b.IsActived == true && b.PlaceType == "PORT"
                    select new YardPortViewModel
                    {
                        yardport_code = a.YardportCode,
                        yardport_name = b.PlaceName,
                        yardport_type = b.PlaceType,
                        import_fee = a.ImportFee,
                        export_fee = a.ExportFee
                    }).ToListAsync();
        return data;
    }

    public async Task<List<YardPortViewModel>> GetYardDataList()
    {
        var data = await (from a in db.TblYardports
                    join b in db.TblPlaces on a.YardportCode equals b.PlaceCode
                    where b.IsActived == true && b.PlaceType == "YARD"
                    select new YardPortViewModel
                    {
                        yardport_code = a.YardportCode,
                        yardport_name = b.PlaceName,
                        yardport_type = b.PlaceType,
                        import_fee = a.ImportFee,
                        export_fee = a.ExportFee
                    }).ToListAsync();
        return data;
    }

    public async Task<List<PlaceViewModel>> GetPlaceDataList()
    {
        var data = await (from a in db.TblIndustries
                    join b in db.TblPlaces on a.IndustryCode equals b.PlaceCode
                    where b.IsEnabled == true
                    select new PlaceViewModel
                    {
                        place_code = b.PlaceCode,
                        place_port_code = a.PortCode,
                        place_name = b.PlaceName
                    }).ToListAsync();
        return data;
    }

    public async Task<List<CostViewModel>> GetCustomerCostDataList()
    {
        var data = await (from a in db.ViewCostListActives
                    where a.IsActived == true
                    select new CostViewModel
                    {
                        customer_code = a.CustomerCode,
                        customer_acronym = a.CustomerAcronym,
                        job_name = a.JobName,
                        cost_code = a.CostCode,
                        cost_remark = a.CostRemark,
                        mission_code = a.MissionCode,
                        mission_name = a.MissionName
                    }).ToListAsync();
        return data;
    }

    public async Task<List<CustomerViewModel>> GetCustomerDataList()
    {
        var data = await (from a in db.TblCustomers
                    join b in db.TblCustomerTypes on a.CustomerTypeCode equals b.CustomerTypeCode
                    where b.IsEnabled == true
                    select new CustomerViewModel
                    {
                        customer_code = a.CustomerCode,
                        customer_name_th = a.CustomerNameTh,
                        customer_name_en = a.CustomerNameEn,
                        customer_address = a.CustomerAddress
                    }).ToListAsync();
        return data;
    }

    public async Task<List<SupplierViewModel>> GetSupplierDataList()
    {
        var data = await (from a in db.TblSuppliers
                    where a.IsActived == true
                    select new SupplierViewModel
                    {
                        supplier_code = a.SupplierCode,
                        supplier_name = a.SupplierName,
                        supplier_type = a.FleetTypeCode
                    }).ToListAsync();
        return data;
    }

    public async Task<List<ZoneViewModel>> GetZoneDataList()
    {
        var data = await (from a in db.TblZones
                    where a.IsActived == true
                    select new ZoneViewModel
                    {
                        zone_code = a.ZoneCode,
                        zone_name = a.ZoneName
                    }).ToListAsync();

        return data;
    }
}