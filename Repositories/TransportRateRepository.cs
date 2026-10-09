using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class TransportRateRepository
{
    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;

    public TransportRateRepository(DbTMSContext context, CodeRepository codeService)
    {
        db = context;
        _codeService = codeService;
    }

    public async Task<ResponseViewModel> AddCost(TblCustomerCost model, decimal fleet_cost, decimal driver_cost)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;
            string missionCode = model.MissionCode;

            model.CostCode = _codeService.GetCostCode();

            db.TblCustomerCosts.Add(model);

            bool hasDriverRate = await db.TblDriverRates
                .AsNoTracking()
                .AnyAsync(w => w.PlaceCode == missionCode);

            if (!hasDriverRate)
            {
                db.TblDriverRates.Add(new TblDriverRate
                {
                    PlaceCode = missionCode,
                    RateValue = driver_cost
                });
            }

            var supplierCodes = await db.TblSuppliers
                .AsNoTracking()
                .Where(w => w.FleetTypeCode == "F2")
                .Select(s => s.SupplierCode)
                .ToListAsync();

            if (supplierCodes.Count > 0)
            {
                var existingSupplierCodes = await db.TblSupplierRates
                    .AsNoTracking()
                    .Where(w => w.PlaceCode == missionCode)
                    .Select(s => s.SupplierCode)
                    .ToListAsync();

                var existingSupplierSet = existingSupplierCodes.ToHashSet();

                var supplierRatesToAdd = supplierCodes
                    .Where(supplierCode => !existingSupplierSet.Contains(supplierCode))
                    .Select(supplierCode => new TblSupplierRate
                    {
                        SupplierCode = supplierCode,
                        PlaceCode = missionCode,
                        CostRate = 0
                    })
                    .ToList();

                if (supplierRatesToAdd.Count > 0)
                {
                    await db.TblSupplierRates.AddRangeAsync(supplierRatesToAdd);
                }
            }

            var fleetCodes = await db.TblFleetSupplierTypes
                .AsNoTracking()
                .Select(s => s.FleetSupplierTypeCode)
                .ToListAsync();

            if (fleetCodes.Count > 0)
            {
                var existingFleetCodes = await db.TblFleetRates
                    .AsNoTracking()
                    .Where(w => w.PlaceCode == missionCode)
                    .Select(s => s.FleetSupplierTypeCode)
                    .ToListAsync();

                var existingFleetSet = existingFleetCodes.ToHashSet();

                var fleetRatesToAdd = fleetCodes
                    .Where(fleetCode => !existingFleetSet.Contains(fleetCode))
                    .Select(fleetCode => new TblFleetRate
                    {
                        FleetSupplierTypeCode = fleetCode,
                        PlaceCode = missionCode,
                        Truck4w = fleet_cost,
                        Truck6w = fleet_cost,
                        Truck10w = fleet_cost,
                        Trailer6w = fleet_cost,
                        Trailer10w = fleet_cost,
                        Trailer18w = fleet_cost,
                        TrailerExtra = fleet_cost
                    })
                    .ToList();

                if (fleetRatesToAdd.Count > 0)
                {
                    await db.TblFleetRates.AddRangeAsync(fleetRatesToAdd);
                }
            }

            await db.SaveChangesAsync();
            await tr.CommitAsync();

            return Success();
        }
        catch (Exception ex)
        {
            await tr.RollbackAsync();
            return Error(ex);
        }
    }

    public async Task<List<CostViewModel>> GetCostDataList()
    {
        return await db.ViewCostListActives
            .AsNoTracking()
            .Where(w => w.CostCode != null)
            .Select(s => new CostViewModel
            {
                job_name = s.JobName,
                customer_code = s.CustomerCode,
                customer_name_th = s.CustomerNameTh,
                cost_code = s.CostCode,
                cost_value = s.CostValue ?? 0,
                mission_name = s.MissionName,
                is_actived = s.IsActived ?? false
            })
            .ToListAsync();
    }

    public async Task<TblCustomerCost?> GetCustomerCostDataByCode(string code)
    {
        return await db.TblCustomerCosts
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.CostCode == code);
    }

    public async Task<CostViewModel?> GetCustomerDataByCode(string code)
    {
        return await (
            from a in db.TblCustomers.AsNoTracking()
            join b in db.TblCustomerCosts.AsNoTracking() on a.CustomerCode equals b.CustomerCode
            join c in db.TblPlaces.AsNoTracking() on b.MissionCode equals c.PlaceCode
            join d in db.TblIndustries.AsNoTracking() on c.PlaceCode equals d.IndustryCode
            where b.CostCode == code
            select new CostViewModel
            {
                mission_name = c.PlaceName,
                customer_name_th = a.CustomerNameTh,
                customer_address = a.CustomerAddress,
                customer_tax = a.CustomerVatNo,
                mission_address = d.IndustryAddress,
                mission_tax = d.IndustryTax
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CostViewModel?> GetCustomerCostByCode(string code)
    {
        return await db.ViewCostListActives
            .AsNoTracking()
            .Where(w => w.CostCode == code)
            .Select(s => new CostViewModel
            {
                customer_code = s.CustomerCode,
                customer_name_th = s.CustomerNameTh,
                cost_code = s.CostCode,
                cost_value = s.CostValue ?? 0
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ResponseViewModel> EditCost(TblCustomerCost costToEdit)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);

            db.Entry(costToEdit).State = EntityState.Modified;

            log.AddSystemLog(costToEdit.CostCode, "Update cost.", costToEdit.UpdateBy);

            await db.SaveChangesAsync();
            await tr.CommitAsync();

            return Success();
        }
        catch (Exception ex)
        {
            await tr.RollbackAsync();
            return Error(ex);
        }
    }

    public async Task<bool> IsDuplicate(string customer_code, string place_code)
    {
        return await db.TblCustomerCosts
            .AsNoTracking()
            .AnyAsync(w => w.CustomerCode == customer_code &&
                           w.MissionCode == place_code &&
                           w.IsEnabled == true);
    }

    public async Task<ResponseViewModel> Remove(string code, string update_by)
    {
        return await UpdateCostStatusAsync(
            code,
            update_by,
            isActived: false,
            isEnabled: false,
            logAction: "Remove cost."
        );
    }

    public async Task<ResponseViewModel> Active(string code, string update_by)
    {
        return await UpdateCostStatusAsync(
            code,
            update_by,
            isActived: true,
            isEnabled: null,
            logAction: "Active customer cost."
        );
    }

    public async Task<ResponseViewModel> Inactive(string code, string update_by)
    {
        return await UpdateCostStatusAsync(
            code,
            update_by,
            isActived: false,
            isEnabled: null,
            logAction: "Inactive customer cost."
        );
    }

    private async Task<ResponseViewModel> UpdateCostStatusAsync(
        string code,
        string updateBy,
        bool isActived,
        bool? isEnabled,
        string logAction)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;
            int affectedRows;

            if (isEnabled.HasValue)
            {
                affectedRows = await db.TblCustomerCosts
                    .Where(w => w.CostCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.IsEnabled, isEnabled.Value)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, updateBy));
            }
            else
            {
                affectedRows = await db.TblCustomerCosts
                    .Where(w => w.CostCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, updateBy));
            }

            if (affectedRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            var log = new LogRepository(db);
            log.AddSystemLog(code, logAction, updateBy);

            await db.SaveChangesAsync();
            await tr.CommitAsync();

            return Success();
        }
        catch (Exception ex)
        {
            await tr.RollbackAsync();
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
            resultMessage = "ไม่พบข้อมูล Cost"
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
