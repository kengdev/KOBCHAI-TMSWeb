using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class PlaceRepository
{
    private const string PlaceTypeIndustry = "INDT";
    private const string SuccessMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
    private const string ErrorMessage = "<div>เกิดข้อผิดพลาดระหว่างการทำงาน</div><div style='margin-top:-30px'> กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ</div>";

    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;

    public PlaceRepository(DbTMSContext context, CodeRepository codeService)
    {
        db = context;
        _codeService = codeService;
    }

    public async Task<PlaceModel> GetPlaceByCode(string place_code)
    {
        var query = await (
            from a in db.TblPlaces.AsNoTracking()
            join b in db.TblIndustries.AsNoTracking()
                on a.PlaceCode equals b.IndustryCode
            join c in db.TblZones.AsNoTracking()
                on b.ZoneCode equals c.ZoneCode
            where a.PlaceCode == place_code
            select new PlaceModel
            {
                place_code = a.PlaceCode,
                place_port_code = b.PortCode,
                place_tax = b.IndustryTax,
                place_fullname = b.IndustryFullname,
                place_name = a.PlaceName,
                place_name_th = b.IndustryNameTh,
                place_address = b.IndustryAddress,
                province_code = b.ProvinceCode,
                place_lat = b.IndustryLat,
                place_lon = b.IndustryLon,
                place_remark = a.PlaceRemark,
                expressway_value = a.ExpresswayValue,
                is_actived = a.IsActived,
                is_enabled = a.IsEnabled,
                zone_code = c.ZoneCode,
                zone_name = c.ZoneName
            }).FirstOrDefaultAsync();

        return query;
    }

    public async Task<List<PlaceViewModel>> GetPlaceDataList()
    {
        return await (
            from a in db.TblIndustries.AsNoTracking()
            join b in db.TblPlaces.AsNoTracking()
                on a.IndustryCode equals b.PlaceCode
            where b.IsEnabled == true
            select new PlaceViewModel
            {
                place_code = b.PlaceCode,
                place_port_code = a.PortCode,
                place_name = b.PlaceName,
                is_actived = b.IsActived
            }).ToListAsync();
    }

    public async Task<TblZone> GetZoneDataByCode(string code)
    {
        return await db.TblZones
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.ZoneCode == code);
    }

    public async Task<PlaceViewModel> GetPlaceDataByCode(string code)
    {
        var data = await (
            from a in db.TblPlaces.AsNoTracking()
            join b in db.TblIndustries.AsNoTracking()
                on a.PlaceCode equals b.IndustryCode
            where a.PlaceCode == code
            select new PlaceViewModel
            {
                place_code = a.PlaceCode,
                place_name = b.IndustryFullname,
                place_address = b.IndustryAddress,
                place_tax = b.IndustryTax
            }).FirstOrDefaultAsync();

        return data;
    }

    public async Task<ResponseViewModel> AddPlace(PlaceModel model)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;
            string placeCode = await _codeService.GetPlaceCode(model.province_code);

            var placeModel = new TblPlace
            {
                PlaceCode = placeCode,
                PlaceName = model.place_name.ToUpper(),
                PlaceType = PlaceTypeIndustry,
                ExpresswayValue = model.expressway_value,
                PlaceRemark = model.place_remark,
                IsActived = true,
                IsEnabled = true,
                CreateDate = now,
                CreateBy = model.create_by,
                UpdateDate = now,
                UpdateBy = model.create_by,
                TblIndustry = new TblIndustry
                {
                    IndustryCode = placeCode,
                    PortCode = model.place_port_code,
                    IndustryNameTh = model.place_name_th,
                    IndustryFullname = model.place_fullname,
                    IndustryAddress = model.place_address,
                    IndustryTax = model.place_tax,
                    IndustryLat = model.place_lat,
                    IndustryLon = model.place_lon,
                    ProvinceCode = model.province_code,
                    ZoneCode = model.zone_code
                }
            };

            db.TblPlaces.Add(placeModel);

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

    public async Task<ResponseViewModel> EditPlace(PlaceModel placeToEdit)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;
            string placeCode = placeToEdit.place_code;
            string placeName = placeToEdit.place_name.ToUpper();

            int placeRows = await db.TblPlaces
                .Where(w => w.PlaceCode == placeCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.PlaceName, placeName)
                    .SetProperty(s => s.ExpresswayValue, placeToEdit.expressway_value)
                    .SetProperty(s => s.PlaceRemark, placeToEdit.place_remark)
                    .SetProperty(s => s.IsEnabled, true)
                    .SetProperty(s => s.UpdateDate, now)
                    .SetProperty(s => s.UpdateBy, placeToEdit.update_by));

            int industryRows = await db.TblIndustries
                .Where(w => w.IndustryCode == placeCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.PortCode, placeToEdit.place_port_code)
                    .SetProperty(s => s.IndustryNameTh, placeToEdit.place_name_th)
                    .SetProperty(s => s.IndustryFullname, placeToEdit.place_fullname)
                    .SetProperty(s => s.IndustryAddress, placeToEdit.place_address)
                    .SetProperty(s => s.IndustryTax, placeToEdit.place_tax)
                    .SetProperty(s => s.IndustryLat, placeToEdit.place_lat)
                    .SetProperty(s => s.IndustryLon, placeToEdit.place_lon)
                    .SetProperty(s => s.ProvinceCode, placeToEdit.province_code));

            if (placeRows == 0 || industryRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลสถานที่");
            }

            var log = new LogRepository(db);
            log.AddSystemLog(placeCode, "Update place.", placeToEdit.update_by);

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
        return await UpdatePlaceStatusAsync(
            code,
            username,
            isActived: false,
            isEnabled: false,
            action: "Remove place.");
    }

    public async Task<ResponseViewModel> Active(string code, string username)
    {
        return await UpdatePlaceStatusAsync(
            code,
            username,
            isActived: true,
            isEnabled: null,
            action: "Active place.");
    }

    public async Task<ResponseViewModel> Inactive(string code, string username)
    {
        return await UpdatePlaceStatusAsync(
            code,
            username,
            isActived: false,
            isEnabled: null,
            action: "Inactive place.");
    }

    private async Task<ResponseViewModel> UpdatePlaceStatusAsync(
        string code,
        string username,
        bool isActived,
        bool? isEnabled,
        string action)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;

            int affectedRows;

            if (isEnabled.HasValue)
            {
                affectedRows = await db.TblPlaces
                    .Where(w => w.PlaceCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.IsEnabled, isEnabled.Value)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }
            else
            {
                affectedRows = await db.TblPlaces
                    .Where(w => w.PlaceCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }

            if (affectedRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลสถานที่");
            }

            var log = new LogRepository(db);
            log.AddSystemLog(code, action, username);

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

    private static ResponseViewModel NotFound(string message)
    {
        return new ResponseViewModel
        {
            result = false,
            resultMessage = message
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
