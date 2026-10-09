using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class AutoCompleteRepository
{
    private readonly DbTMSContext db;
    private const int MaxResults = 30;

    public AutoCompleteRepository(DbTMSContext context)
    {
        db = context;
    }

    public async Task<List<ViewDriverListActive>> GetDriver(
        string term, CancellationToken cancellationToken = default)
    {
        term = term?.Trim() ?? string.Empty;
        return await db.ViewDriverListActives
            .AsNoTracking()
            .Where(x => x.DriverCode.Contains(term) || x.DriverFullname.Contains(term))
            .Take(MaxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TruckViewModel>> GetTruck(
        string term, CancellationToken cancellationToken = default)
    {
        term = term?.Trim() ?? string.Empty;
        return await (
            from truck in db.TblTrucks.AsNoTracking()
            join detail in db.TblTruckDetails.AsNoTracking()
                on truck.TruckCode equals detail.TruckCode into truckDetails
            from detail in truckDetails.DefaultIfEmpty()
            where truck.TruckLicense.Contains(term)
                || (detail != null && detail.TruckNo != null && detail.TruckNo.Contains(term))
            select new TruckViewModel
            {
                truck_code = truck.TruckCode,
                truck_license = truck.TruckLicense,
                truck_no = detail != null && detail.TruckNo != null
                    ? truck.TruckLicense + " [" + detail.TruckNo + "]"
                    : truck.TruckLicense
            })
            .Take(MaxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TblTrailer>> GetTrailer(
        string term, CancellationToken cancellationToken = default)
    {
        term = term?.Trim() ?? string.Empty;
        return await db.TblTrailers
            .AsNoTracking()
            .Where(x => x.TrailerLicense.Contains(term) || x.TrailerNo.Contains(term))
            .Take(MaxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TblZone>> GetZone(
        string term, CancellationToken cancellationToken = default)
    {
        term = term?.Trim() ?? string.Empty;
        return await db.TblZones
            .AsNoTracking()
            .Where(x => x.ZoneCode.Contains(term) || x.ZoneName.Contains(term))
            .Take(MaxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TblPlace>> GetPlace(
        string term, CancellationToken cancellationToken = default)
    {
        term = term?.Trim() ?? string.Empty;
        return await db.TblPlaces
            .AsNoTracking()
            .Where(x => x.PlaceCode.Contains(term) || x.PlaceName.Contains(term))
            .Take(MaxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CostModel>> GetCustomerCost(
        string term, CancellationToken cancellationToken = default)
    {
        term = term?.Trim() ?? string.Empty;
        return await db.ViewCostListActives
            .AsNoTracking()
            .Where(x => x.IsActived == true &&
                (x.CostCode.Contains(term) || x.CustomerNameTh.Contains(term) || x.MissionName.Contains(term)))
            .Select(x => new CostModel
            {
                cost_code = x.CostCode,
                cost_name = x.MissionName,
                cost_remark = x.CostRemark,
                customer_code = x.CustomerCode,
                cost_value = x.CostValue.Value,
                customer_name_th = x.CustomerNameTh
            })
            .Take(MaxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TblSupplier>> GetSupplier(
        string term, CancellationToken cancellationToken = default)
    {
        term = term?.Trim() ?? string.Empty;
        return await db.TblSuppliers
            .AsNoTracking()
            .Where(x => x.SupplierCode.Contains(term) || x.SupplierName.Contains(term) || x.SupplierFullname.Contains(term))
            .Take(MaxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CustomerModel>> GetCustomer(
        string term, CancellationToken cancellationToken = default)
    {
        term = term?.Trim() ?? string.Empty;
        return await db.TblCustomers
            .AsNoTracking()
            .Where(x => x.IsActived == true &&
                (x.CustomerCode.Contains(term) || x.CustomerAcronym.Contains(term) || x.CustomerNameEn.Contains(term)))
            .Select(x => new CustomerModel
            {
                customer_code = x.CustomerCode,
                customer_name_th = x.CustomerNameTh
            })
            .Take(MaxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<string>> GetBooking(
        string term, CancellationToken cancellationToken = default)
    {
        term = term?.Trim() ?? string.Empty;
        return await db.TblJobMasters
            .AsNoTracking()
            .Where(x => x.BookingNo != null && x.BookingNo.Contains(term))
            .Select(x => x.BookingNo!)
            .Take(MaxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TblPlace>> GetPlaceByType(
        string term, string type, CancellationToken cancellationToken = default)
    {
        term = term?.Trim() ?? string.Empty;
        return await db.TblPlaces
            .AsNoTracking()
            .Where(x => x.PlaceType == type &&
                (x.PlaceCode.Contains(term) || x.PlaceName.Contains(term)))
            .Take(MaxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<DriverViewModel>> GetDriver(
        CancellationToken cancellationToken = default)
    {
        return await db.ViewDriverListActives
            .AsNoTracking()
            .Select(x => new DriverViewModel
            {
                driver_code = x.DriverCode,
                fullname = x.DriverFullname
            })
            .ToListAsync(cancellationToken);
    }
}
