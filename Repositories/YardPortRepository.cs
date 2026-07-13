using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class YardPortRepository
{
    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;

    private const string PortPlaceType = "PORT";
    private const string ExcludedPlaceCode = "000000000";
    private const string SaveSuccessMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
    private const string ErrorMessage = "<div>เกิดข้อผิดพลาดระหว่างการทำงาน</div><div style='margin-top:-30px'> กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ</div>";

    public YardPortRepository(DbTMSContext context, CodeRepository codeService)
    {
        db = context;
        _codeService = codeService;
    }

    public async Task<List<YardPortViewModel>> GetYardPortDataList()
    {
        return await db.TblYardports
            .AsNoTracking()
            .Join(
                db.TblPlaces.AsNoTracking(),
                yardPort => yardPort.YardportCode,
                place => place.PlaceCode,
                (yardPort, place) => new { yardPort, place })
            .Where(x => x.place.IsEnabled == true && x.place.PlaceCode != ExcludedPlaceCode)
            .Select(x => new YardPortViewModel
            {
                yardport_code = x.yardPort.YardportCode,
                yardport_name = x.place.PlaceName,
                yardport_type = x.place.PlaceType,
                is_actived = x.place.IsActived
            })
            .ToListAsync();
    }

    public async Task<PortModel?> GetPortData(string yardport_code)
    {
        return await db.TblPlaces
            .AsNoTracking()
            .Join(
                db.TblYardports.AsNoTracking(),
                place => place.PlaceCode,
                yardPort => yardPort.YardportCode,
                (place, yardPort) => new { place, yardPort })
            .Where(x => x.place.PlaceCode == yardport_code)
            .Select(x => new PortModel
            {
                place_code = x.place.PlaceCode,
                place_name = x.place.PlaceName,
                place_type = x.place.PlaceType,
                place_remark = x.place.PlaceRemark,
                import_fee = x.yardPort.ImportFee,
                export_fee = x.yardPort.ExportFee
            })
            .FirstOrDefaultAsync();
    }

    public async Task<YardModel?> GetYardData(string yardport_code)
    {
        return await db.TblPlaces
            .AsNoTracking()
            .Join(
                db.TblYardports.AsNoTracking(),
                place => place.PlaceCode,
                yardPort => yardPort.YardportCode,
                (place, yardPort) => new { place, yardPort })
            .Where(x => x.place.PlaceCode == yardport_code)
            .Select(x => new YardModel
            {
                place_code = x.place.PlaceCode,
                place_name = x.place.PlaceName,
                place_type = x.place.PlaceType,
                place_remark = x.place.PlaceRemark,
                import_fee = x.yardPort.ImportFee,
                export_fee = x.yardPort.ExportFee
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TblYardCashing?> GetYardCashingData(string yardport_code)
    {
        return await db.TblYardCashings
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.YardportCode == yardport_code);
    }

    public async Task<List<PortViewModel>> GetPortCashingData(string yardport_code)
    {
        return await db.TblPortCashings
            .AsNoTracking()
            .Join(
                db.TblContainerSizes.AsNoTracking(),
                cashing => cashing.ContainerSizeCode,
                size => size.ContainerSizeCode,
                (cashing, size) => new { cashing, size })
            .Where(x => x.cashing.YardportCode == yardport_code)
            .OrderByDescending(x => x.size.ContainerSizeGroup)
            .Select(x => new PortViewModel
            {
                yardport_code = x.cashing.YardportCode,
                container_kind_code = x.size.ContainerSizeCode,
                container_size = x.size.ContainerSizeName,
                container_fee = x.cashing.ContainerFee,
                container_group = x.size.ContainerSizeGroup
            })
            .ToListAsync();
    }

    public async Task<List<TblContainerSize>> GetContainerData()
    {
        return await db.TblContainerSizes
            .AsNoTracking()
            .Where(w => w.IsEnabled == true)
            .OrderByDescending(o => o.ContainerSizeGroup)
            .ToListAsync();
    }

    public async Task<List<PortCashingModel>> GetListPortCashing(string code)
    {
        return await db.TblContainerSizes
            .AsNoTracking()
            .GroupJoin(
                db.TblPortCashings
                    .AsNoTracking()
                    .Where(w => w.YardportCode == code),
                size => size.ContainerSizeCode,
                cashing => cashing.ContainerSizeCode,
                (size, cashings) => new { size, cashings })
            .SelectMany(
                x => x.cashings.DefaultIfEmpty(),
                (x, cashing) => new PortCashingModel
                {
                    container_kind_code = x.size.ContainerSizeCode,
                    container_kind_name = x.size.ContainerSizeName,
                    container_group = x.size.ContainerSizeGroup,
                    container_fee = cashing != null ? cashing.ContainerFee : 0
                })
            .OrderByDescending(o => o.container_group)
            .ToListAsync();
    }

    public async Task<ResponseViewModel> AddPortData(PortModel portToAdd)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;
            string placeCode = await _codeService.GetPortCode();

            var placeModel = new TblPlace
            {
                PlaceCode = placeCode,
                PlaceName = portToAdd.place_name.ToUpper(),
                PlaceType = PortPlaceType,
                PlaceRemark = portToAdd.place_remark,
                IsActived = true,
                IsEnabled = true,
                CreateDate = now,
                CreateBy = portToAdd.create_by,
                UpdateDate = now,
                UpdateBy = portToAdd.create_by
            };

            var portModel = new TblYardport
            {
                YardportCode = placeCode,
                ImportFee = portToAdd.import_fee,
                ExportFee = portToAdd.export_fee,
                TblPortCashings = BuildPortCashings(placeCode, portToAdd)
            };

            db.TblPlaces.Add(placeModel);
            db.TblYardports.Add(portModel);

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

    public async Task<ResponseViewModel> AddYardData(YardModel yardToAdd)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;
            string placeCode = await _codeService.GetYardCode();

            var placeModel = new TblPlace
            {
                PlaceCode = placeCode,
                PlaceName = yardToAdd.place_name.ToUpper(),
                PlaceType = yardToAdd.place_type,
                PlaceRemark = yardToAdd.place_remark,
                IsActived = true,
                IsEnabled = true,
                CreateDate = now,
                CreateBy = yardToAdd.create_by,
                UpdateDate = now,
                UpdateBy = yardToAdd.create_by,
                TblYardport = new TblYardport
                {
                    YardportCode = placeCode,
                    ImportFee = yardToAdd.import_fee,
                    ExportFee = yardToAdd.export_fee
                }
            };

            db.TblPlaces.Add(placeModel);
            db.TblYardCashings.Add(new TblYardCashing
            {
                YardportCode = placeCode,
                ShortliftonFee = yardToAdd.shortlifton,
                LongliftonFee = yardToAdd.longlifton,
                ShortliftoffFee = yardToAdd.shortliftoff,
                LongliftoffFee = yardToAdd.longliftoff
            });

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

    public async Task<ResponseViewModel> EditPortData(PortModel portToEdit)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;
            string placeCode = portToEdit.place_code;

            int placeRows = await db.TblPlaces
                .Where(w => w.PlaceCode == placeCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.PlaceName, portToEdit.place_name.ToUpper())
                    .SetProperty(s => s.PlaceRemark, portToEdit.place_remark)
                    .SetProperty(s => s.UpdateDate, now)
                    .SetProperty(s => s.UpdateBy, portToEdit.update_by));

            int yardPortRows = await db.TblYardports
                .Where(w => w.YardportCode == placeCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.ImportFee, portToEdit.import_fee)
                    .SetProperty(s => s.ExportFee, portToEdit.export_fee));

            if (placeRows == 0 || yardPortRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            await db.TblPortCashings
                .Where(w => w.YardportCode == placeCode)
                .ExecuteDeleteAsync();

            db.TblPortCashings.AddRange(
                BuildPortCashings(placeCode, portToEdit));

            var log = new LogRepository(db);
            log.AddSystemLog(placeCode, "Update port.", portToEdit.update_by);

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

    public async Task<ResponseViewModel> EditYardData(YardModel yardToEdit)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;
            string placeCode = yardToEdit.place_code;

            int placeRows = await db.TblPlaces
                .Where(w => w.PlaceCode == placeCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.PlaceName, yardToEdit.place_name.ToUpper())
                    .SetProperty(s => s.PlaceRemark, yardToEdit.place_remark)
                    .SetProperty(s => s.UpdateDate, now)
                    .SetProperty(s => s.UpdateBy, yardToEdit.update_by));

            int yardPortRows = await db.TblYardports
                .Where(w => w.YardportCode == placeCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.ImportFee, yardToEdit.import_fee)
                    .SetProperty(s => s.ExportFee, yardToEdit.export_fee));

            int yardCashingRows = await db.TblYardCashings
                .Where(w => w.YardportCode == placeCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.ShortliftonFee, yardToEdit.shortlifton)
                    .SetProperty(s => s.LongliftonFee, yardToEdit.longlifton)
                    .SetProperty(s => s.ShortliftoffFee, yardToEdit.shortliftoff)
                    .SetProperty(s => s.LongliftoffFee, yardToEdit.longliftoff));

            if (placeRows == 0 || yardPortRows == 0 || yardCashingRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            var log = new LogRepository(db);
            log.AddSystemLog(placeCode, "Update yard.", yardToEdit.update_by);

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
            logMessage: "Remove place.");
    }

    public async Task<ResponseViewModel> Active(string code, string username)
    {
        return await UpdatePlaceStatusAsync(
            code,
            username,
            isActived: true,
            isEnabled: null,
            logMessage: "Active place.");
    }

    public async Task<ResponseViewModel> Inactive(string code, string username)
    {
        return await UpdatePlaceStatusAsync(
            code,
            username,
            isActived: false,
            isEnabled: null,
            logMessage: "Inactive place.");
    }

    private async Task<ResponseViewModel> UpdatePlaceStatusAsync(
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

    private static List<TblPortCashing> BuildPortCashings(string yardportCode, PortModel model)
    {
        int count = Math.Min(model.container_size.Count(), model.container_fee.Count());
        var list = new List<TblPortCashing>(count);

        for (int i = 0; i < count; i++)
        {
            list.Add(new TblPortCashing
            {
                YardportCode = yardportCode,
                ContainerSizeCode = model.container_size[i],
                ContainerFee = model.container_fee[i]
            });
        }

        return list;
    }

    private static ResponseViewModel Success()
    {
        return new ResponseViewModel
        {
            result = true,
            resultMessage = SaveSuccessMessage
        };
    }

    private static ResponseViewModel NotFound()
    {
        return new ResponseViewModel
        {
            result = false,
            resultMessage = "ไม่พบข้อมูล"
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
