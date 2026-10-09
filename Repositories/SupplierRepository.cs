using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class SupplierRepository
{
    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;

    private const string InternalFleetTypeCode = "F1";
    private const string ExternalFleetTypeCode = "F2";
    private const string DefaultFleetCode = "FL001";
    private const string DefaultInternalFleetTypeCode = "A1";

    private const string SuccessMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
    private const string ErrorMessage = "<div>เกิดข้อผิดพลาดระหว่างการทำงาน</div><div style='margin-top:-30px'> กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ</div>";

    public SupplierRepository(DbTMSContext context, CodeRepository codeService)
    {
        db = context;
        _codeService = codeService;
    }

    public async Task<TblSupplier?> GetSupplierData(string code)
    {
        return await db.TblSuppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.SupplierCode == code);
    }

    public async Task<List<SupplierViewModel>> GetSupplierDataList()
    {
        return await db.TblSuppliers
            .AsNoTracking()
            .Where(w => w.IsEnabled == true && w.SupplierCode != "SP18001")
            .OrderBy(o => o.SupplierName)
            .Select(s => new SupplierViewModel
            {
                supplier_code = s.SupplierCode,
                supplier_name = s.SupplierName,
                supplier_phone = s.SupplierPhone,
                supplier_address = s.SupplierAddress,
                is_actived = s.IsActived
            })
            .ToListAsync();
    }

    public async Task<ResponseViewModel> Add(TblSupplier model)
    {
        using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            model.SupplierCode = await _codeService.GetSupplierCode();

            AddFleetByType(model.SupplierCode, model.FleetTypeCode, model);

            if (model.FleetTypeCode == ExternalFleetTypeCode)
            {
                var supplierRates = await BuildDefaultSupplierRatesAsync(model.SupplierCode);

                foreach (var rate in supplierRates)
                {
                    model.TblSupplierRates.Add(rate);
                }
            }

            await db.TblSuppliers.AddAsync(model);
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

    public async Task<ResponseViewModel> Edit(TblSupplier model)
    {
        using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            string supplierCode = model.SupplierCode;

            // ลบ fleet เดิมแบบไม่ต้องดึงข้อมูลขึ้น memory
            /*await db.TblFleetInternals
                .Where(w => w.SupplierCode == supplierCode)
                .ExecuteDeleteAsync();*/

            // เพิ่ม fleet ใหม่ตามประเภท
            if (model.FleetTypeCode == InternalFleetTypeCode)
            {
                /*await db.TblFleetInternals.AddAsync(new TblFleetInternal
                {
                    SupplierCode = supplierCode,
                    FleetCode = DefaultFleetCode,
                    FleetTypeCode = DefaultInternalFleetTypeCode
                });*/
            }

            if (model.FleetTypeCode == ExternalFleetTypeCode)
            {
                bool hasSupplierRate = await db.TblSupplierRates
                    .AsNoTracking()
                    .AnyAsync(w => w.SupplierCode == supplierCode);

                if (!hasSupplierRate)
                {
                    var supplierRates = await BuildDefaultSupplierRatesAsync(supplierCode);
                    await db.TblSupplierRates.AddRangeAsync(supplierRates);
                }
            }
            else
            {
                // ไม่ใช่รถร่วมแล้ว ลบ rate เก่าแบบไม่ต้อง ToListAsync
                await db.TblSupplierRates
                    .Where(w => w.SupplierCode == supplierCode)
                    .ExecuteDeleteAsync();
            }

            db.TblSuppliers.Update(model);

            var log = new LogRepository(db);
            log.AddSystemLog(model.SupplierCode, "Update supplier.", model.UpdateBy);

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

    public async Task<ResponseViewModel> Remove(string code, string username)
    {
        return await UpdateSupplierStatusAsync(
            code,
            username,
            isActived: false,
            isEnabled: false,
            logMessage: "Remove supplier.");
    }

    public async Task<ResponseViewModel> Active(string code, string username)
    {
        return await UpdateSupplierStatusAsync(
            code,
            username,
            isActived: true,
            isEnabled: null,
            logMessage: "Active supplier.");
    }

    public async Task<ResponseViewModel> Inactive(string code, string username)
    {
        return await UpdateSupplierStatusAsync(
            code,
            username,
            isActived: false,
            isEnabled: null,
            logMessage: "Inactive supplier.");
    }

    private void AddFleetByType(string supplierCode, string? fleetTypeCode, TblSupplier model)
    {
        if (fleetTypeCode == InternalFleetTypeCode)
        {
            /*model.TblFleetInternals.Add(new TblFleetInternal
            {
                SupplierCode = supplierCode,
                FleetCode = DefaultFleetCode,
                FleetTypeCode = DefaultInternalFleetTypeCode
            });*/
        }
        /*else if (fleetTypeCode == ExternalFleetTypeCode)
        {
            model.TblFleetExternals.Add(new TblFleetExternal
            {
                SupplierCode = supplierCode,
                FleetTypeCode = ExternalFleetTypeCode
            });
        }*/
    }

    private async Task<List<TblSupplierRate>> BuildDefaultSupplierRatesAsync(string supplierCode)
    {
        var missionCodes = await db.TblCustomerCosts
            .AsNoTracking()
            .Where(w => w.IsActived == true && w.MissionCode != null)
            .Select(s => s.MissionCode)
            .Distinct()
            .ToListAsync();

        return missionCodes
            .Select(missionCode => new TblSupplierRate
            {
                SupplierCode = supplierCode,
                PlaceCode = missionCode,
                CostRate = 0
            })
            .ToList();
    }

    private async Task<ResponseViewModel> UpdateSupplierStatusAsync(
        string code,
        string username,
        bool isActived,
        bool? isEnabled,
        string logMessage)
    {
        using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            int affectedRows;
            DateTime now = DateTime.Now;

            if (isEnabled.HasValue)
            {
                affectedRows = await db.TblSuppliers
                    .Where(w => w.SupplierCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.IsEnabled, isEnabled.Value)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }
            else
            {
                affectedRows = await db.TblSuppliers
                    .Where(w => w.SupplierCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }

            if (affectedRows == 0)
            {
                await tr.RollbackAsync();
                return new ResponseViewModel
                {
                    result = false,
                    resultMessage = "ไม่พบข้อมูลที่ต้องการแก้ไข"
                };
            }

            var log = new LogRepository(db);
            log.AddSystemLog(code, logMessage, username);

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
            resultMessage = SuccessMessage
        };
    }

    private static ResponseViewModel Error(Exception ex)
    {
        return new ResponseViewModel
        {
            result = false,
            resultMessage = ErrorMessage,
            errorException = ex
        };
    }
}
