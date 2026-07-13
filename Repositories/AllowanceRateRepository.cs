using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class RateRepository
{
    private readonly DbTMSContext db;

    public RateRepository(DbTMSContext context)
    {
        db = context;
    }

    public async Task<List<DriverRateViewModel>> GetDriverRate()
    {
        return await (
            from a in db.TblDriverRates.AsNoTracking()
            join b in db.TblPlaces.AsNoTracking()
                on a.PlaceCode equals b.PlaceCode
            select new DriverRateViewModel
            {
                name = b.PlaceName,
                rate_value = a.RateValue,
                rate_code = a.PlaceCode
            })
            .OrderBy(o => o.name)
            .ToListAsync();
    }

    public async Task<List<SupplyerRateViewModel>> GetSupplyerRate(string supplier_code)
    {
        return await (
            from b in db.TblSupplierRates.AsNoTracking()
            join a in db.TblSuppliers.AsNoTracking()
                on b.SupplierCode equals a.SupplierCode
            join c in db.TblPlaces.AsNoTracking()
                on b.PlaceCode equals c.PlaceCode
            where b.SupplierCode == supplier_code
            select new SupplyerRateViewModel
            {
                place_name = c.PlaceName,
                supplyer_name = a.SupplierName,
                supplyer_code = a.SupplierCode,
                place_code = b.PlaceCode,
                rate_sup_value = b.CostRate
            })
            .OrderBy(o => o.place_name)
            .ToListAsync();
    }

    public async Task<List<SupplyerRateViewModel>> GetFleetRate(string supplier_code)
    {
        return await (
            from b in db.TblFleetRates.AsNoTracking()
            join a in db.TblFleets.AsNoTracking()
                on b.FleetCode equals a.FleetCode
            join c in db.TblPlaces.AsNoTracking()
                on b.PlaceCode equals c.PlaceCode
            where b.FleetCode == supplier_code &&
                  c.IsActived == true
            select new SupplyerRateViewModel
            {
                place_name = c.PlaceName,
                supplyer_name = a.FleetName,
                supplyer_code = a.FleetCode,
                place_code = b.PlaceCode,
                rate_4W_value = b.Truck4w,
                rate_6W_value = b.Truck6w,
                rate_10W_value = b.Truck10w
            })
            .OrderBy(o => o.place_name)
            .ToListAsync();
    }

    public async Task<ResponseViewModel> EditDriverRate(decimal value, string code, string username)
    {
        try
        {
            int affectedRows = await db.TblDriverRates
                .Where(w => w.PlaceCode == code)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.RateValue, value));

            if (affectedRows == 0)
            {
                return NotFound();
            }

            return Success();
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public async Task<ResponseViewModel> EditSupplyerRate(
        decimal value,
        string supplier_code,
        string place_code,
        string username)
    {
        try
        {
            int affectedRows = await db.TblSupplierRates
                .Where(w => w.SupplierCode == supplier_code &&
                            w.PlaceCode == place_code)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.CostRate, value));

            if (affectedRows == 0)
            {
                return NotFound();
            }

            return Success();
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public async Task<ResponseViewModel> EditFleetRate(
        decimal value,
        string fleet_code,
        string place_code,
        string type,
        string username)
    {
        try
        {
            int affectedRows;

            switch (type?.ToUpper())
            {
                case "4W":
                    affectedRows = await db.TblFleetRates
                        .Where(w => w.FleetCode == fleet_code &&
                                    w.PlaceCode == place_code)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(s => s.Truck4w, value));
                    break;

                case "6W":
                    affectedRows = await db.TblFleetRates
                        .Where(w => w.FleetCode == fleet_code &&
                                    w.PlaceCode == place_code)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(s => s.Truck6w, value));
                    break;

                case "10W":
                    affectedRows = await db.TblFleetRates
                        .Where(w => w.FleetCode == fleet_code &&
                                    w.PlaceCode == place_code)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(s => s.Truck10w, value));
                    break;

                default:
                    return new ResponseViewModel
                    {
                        result = false,
                        resultMessage = "ประเภทรถไม่ถูกต้อง"
                    };
            }

            if (affectedRows == 0)
            {
                return NotFound();
            }

            return Success();
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    private static ResponseViewModel Success()
    {
        return new ResponseViewModel
        {
            result = true,
            resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว"
        };
    }

    private static ResponseViewModel NotFound()
    {
        return new ResponseViewModel
        {
            result = false,
            resultMessage = "ไม่พบข้อมูลเรท"
        };
    }

    private static ResponseViewModel Error(Exception ex)
    {
        return new ResponseViewModel
        {
            result = false,
            resultMessage = "<div>เกิดข้อผิดพลาดระหว่างการทำงาน</div><div style='margin-top:-30px'> กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ</div>",
            errorException = ex
        };
    }
}