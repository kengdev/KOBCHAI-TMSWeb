using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class ZoneRepository
{
    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;

    public ZoneRepository(DbTMSContext context, CodeRepository codeService)
    {
        db = context;
        _codeService = codeService;
    }

    public async Task<List<ZoneViewModel>> GetZoneDataList()
    {
        return await db.TblZones
            .AsNoTracking()
            .Where(w => w.IsActived == true)
            .OrderBy(o => o.ZoneName)
            .Select(s => new ZoneViewModel
            {
                zone_code = s.ZoneCode,
                zone_name = s.ZoneName,
                tr4 = db.TblFuelRates
                    .Where(w => w.ZoneCode == s.ZoneCode && w.TruckTypegroupCode == "01")
                    .Select(x => x.FuelRateValue)
                    .FirstOrDefault(),
                tr6 = db.TblFuelRates
                    .Where(w => w.ZoneCode == s.ZoneCode && w.TruckTypegroupCode == "02")
                    .Select(x => x.FuelRateValue)
                    .FirstOrDefault(),
                tr10 = db.TblFuelRates
                    .Where(w => w.ZoneCode == s.ZoneCode && w.TruckTypegroupCode == "03")
                    .Select(x => x.FuelRateValue)
                    .FirstOrDefault(),
                is_actived = s.IsActived
            })
            .ToListAsync();
    }

    public async Task<List<TblZone>> GetZone()
    {
        return await db.TblZones
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TblZone> GetZoneByCode(string code)
    {
        return await db.TblZones
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.ZoneCode == code);
    }

    public async Task<List<TblFuelRate>> GetFuelRate()
    {
        return await db.TblFuelRates
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<FuelRateViewModel>> GetFuelRateValue()
    {
        return await db.TblZones
            .AsNoTracking()
            .Where(w => w.ZoneCode != "00")
            .OrderBy(o => o.ZoneName)
            .Select(s => new FuelRateViewModel
            {
                zone_code = s.ZoneCode,
                zone_name = s.ZoneName
            })
            .ToListAsync();
    }

    public async Task<ResponseViewModel> EditRate(string[] zone, int[] tr4, int[] tr6, int[] tr10, string username)
    {
        if (zone == null || tr4 == null || tr6 == null || tr10 == null ||
            zone.Length != tr4.Length || zone.Length != tr6.Length || zone.Length != tr10.Length)
        {
            return new ResponseViewModel
            {
                result = false,
                resultMessage = "ข้อมูลอัตราน้ำมันไม่ถูกต้อง"
            };
        }

        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;
            var zoneCodes = zone.Distinct().ToArray();
            var truckTypes = new[] { "01", "02", "03" };

            var rates = await db.TblFuelRates
                .Where(w => zoneCodes.Contains(w.ZoneCode) &&
                            truckTypes.Contains(w.TruckTypegroupCode))
                .ToListAsync();

            var rateMap = rates.ToDictionary(
                x => $"{x.ZoneCode}|{x.TruckTypegroupCode}",
                x => x);

            for (int i = 0; i < zone.Length; i++)
            {
                UpdateFuelRate(rateMap, zone[i], "01", tr4[i], username, now);
                UpdateFuelRate(rateMap, zone[i], "02", tr6[i], username, now);
                UpdateFuelRate(rateMap, zone[i], "03", tr10[i], username, now);
            }

            var log = new LogRepository(db);
            log.AddSystemLog("-", "Update fuel rate.", username);

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

    public async Task<ResponseViewModel> EditZoneData(TblZone zoneToEdit)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            db.Entry(zoneToEdit).State = EntityState.Modified;

            var log = new LogRepository(db);
            log.AddSystemLog(zoneToEdit.ZoneCode, "Update zone.", zoneToEdit.UpdateBy);

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

    public async Task<ResponseViewModel> AddZoneData(TblZone zoneToAdd, int tr4, int tr6, int tr10)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;

            zoneToAdd.ZoneCode = await _codeService.GetZoneCode();

            var rates = new List<TblFuelRate>
            {
                new TblFuelRate
                {
                    ZoneCode = zoneToAdd.ZoneCode,
                    TruckTypegroupCode = "01",
                    FuelRateValue = tr4,
                    UpdateDate = now,
                    UpdateBy = zoneToAdd.UpdateBy
                },
                new TblFuelRate
                {
                    ZoneCode = zoneToAdd.ZoneCode,
                    TruckTypegroupCode = "02",
                    FuelRateValue = tr6,
                    UpdateDate = now,
                    UpdateBy = zoneToAdd.UpdateBy
                },
                new TblFuelRate
                {
                    ZoneCode = zoneToAdd.ZoneCode,
                    TruckTypegroupCode = "03",
                    FuelRateValue = tr10,
                    UpdateDate = now,
                    UpdateBy = zoneToAdd.UpdateBy
                }
            };

            db.TblZones.Add(zoneToAdd);
            db.TblFuelRates.AddRange(rates);

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
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;

            int affectedRows = await db.TblZones
                .Where(w => w.ZoneCode == code)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.IsActived, false)
                    .SetProperty(s => s.IsEnabled, false)
                    .SetProperty(s => s.UpdateDate, now)
                    .SetProperty(s => s.UpdateBy, username));

            if (affectedRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            await db.TblFuelRates
                .Where(w => w.ZoneCode == code)
                .ExecuteDeleteAsync();

            var log = new LogRepository(db);
            log.AddSystemLog(code, "Remove zone.", username);

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

    public async Task<ResponseViewModel> Active(string code, string username)
    {
        return await UpdateZoneStatusAsync(
            code,
            username,
            isActived: true,
            isEnabled: null,
            logMessage: "Active zone.");
    }

    public async Task<ResponseViewModel> Inactive(string code, string username)
    {
        return await UpdateZoneStatusAsync(
            code,
            username,
            isActived: false,
            isEnabled: null,
            logMessage: "Inactive zone.");
    }

    private async Task<ResponseViewModel> UpdateZoneStatusAsync(
        string code,
        string username,
        bool isActived,
        bool? isEnabled,
        string logMessage)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;
            int affectedRows;

            if (isEnabled.HasValue)
            {
                affectedRows = await db.TblZones
                    .Where(w => w.ZoneCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.IsEnabled, isEnabled.Value)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }
            else
            {
                affectedRows = await db.TblZones
                    .Where(w => w.ZoneCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }

            if (affectedRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound();
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

    private static void UpdateFuelRate(
        Dictionary<string, TblFuelRate> rateMap,
        string zoneCode,
        string truckTypeCode,
        int value,
        string username,
        DateTime now)
    {
        string key = $"{zoneCode}|{truckTypeCode}";

        if (rateMap.TryGetValue(key, out var rate))
        {
            rate.FuelRateValue = value;
            rate.UpdateDate = now;
            rate.UpdateBy = username;
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
            resultMessage = "ไม่พบข้อมูลโซน"
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
