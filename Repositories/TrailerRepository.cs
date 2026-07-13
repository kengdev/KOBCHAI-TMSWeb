using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class TrailerRepository
{
    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;

    public TrailerRepository(DbTMSContext context)
    {
        db = context;
        _codeService = new CodeRepository(context);
    }

    public async Task<TblTrailer?> GetTrailerDataByCode(string trailer_code)
    {
        return await db.TblTrailers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.TrailerCode == trailer_code);
    }

    public async Task<List<TrailerViewModel>> GetTruckDataList()
    {
        return await db.ViewTrailerListActives
            .AsNoTracking()
            .Select(s => new TrailerViewModel
            {
                trailer_code = s.TrailerCode,
                trailer_license = s.TrailerLicense,
                trailer_type_name = s.TrailerTypeName,
                trailer_floor_name = s.TrailerFloorName,
                truck_license = s.TruckLicense,
                trailer_no = s.TrailerNo,
                is_actived = s.IsActived
            })
            .OrderBy(o => o.trailer_license)
            .ToListAsync();
    }

    public async Task<ViewTrailerListActive?> GetTrailerByLicense(string license)
    {
        return await db.ViewTrailerListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.TrailerLicense == license);
    }

    public async Task<List<ViewTrailerInsuranceList>> GetInsuranceByLicense(string license)
    {
        return await db.ViewTrailerInsuranceLists
            .AsNoTracking()
            .Where(w => w.TrailerLicense == license &&
                        w.IsEnabled == true)
            .ToListAsync();
    }

    public async Task<TblInsurance?> GetInsuranceName(string insurance_code)
    {
        return await db.TblInsurances
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.InsuranceCode == insurance_code);
    }

    public async Task<TblInsuranceType?> GetInsuranceTypeName(string insurance_type_code)
    {
        return await db.TblInsuranceTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.InsuranceTypeCode == insurance_type_code);
    }

    public async Task<TblTruckTrailer?> GetTruckByTrailer(string trailer_code)
    {
        return await db.TblTruckTrailers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.TrailerCode == trailer_code);
    }

    public async Task<ResponseViewModel> AddTrailer(TblTrailer trailerToAdd, string truck_code)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            trailerToAdd.TrailerCode = _codeService.GetTrailerCode();

            db.TblTrailers.Add(trailerToAdd);

            if (!string.IsNullOrWhiteSpace(truck_code))
            {
                db.TblTruckTrailers.Add(new TblTruckTrailer
                {
                    TruckCode = truck_code,
                    TrailerCode = trailerToAdd.TrailerCode
                });
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

    public async Task<ResponseViewModel> EditTrailerInsurance(TblTrailer truckInsuranceToEdit, string username)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;

            await db.TblTrailerInsurances
                .Where(w => w.TrailerCode == truckInsuranceToEdit.TrailerCode &&
                            w.IsEnabled == true)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.IsEnabled, false)
                    .SetProperty(s => s.UpdateDate, now)
                    .SetProperty(s => s.UpdateBy, username));

            var newInsurances = truckInsuranceToEdit.TblTrailerInsurances
                .Select(item => new TblTrailerInsurance
                {
                    TrailerCode = truckInsuranceToEdit.TrailerCode,
                    InsuranceCode = string.IsNullOrWhiteSpace(item.InsuranceCode)
                        ? item.InsuranceTypeCode
                        : item.InsuranceCode,
                    InsuranceTypeCode = item.InsuranceTypeCode,
                    InsuranceBudget = item.InsuranceBudget,
                    InsuranceIncrease = item.InsuranceIncrease,
                    InsuranceDetail = "-",
                    StartedDate = item.StartedDate,
                    ExpiredDate = item.ExpiredDate,
                    IsEnabled = true,
                    CreateDate = now,
                    CreateBy = username,
                    UpdateDate = now,
                    UpdateBy = username
                })
                .ToList();

            if (newInsurances.Count > 0)
            {
                await db.TblTrailerInsurances.AddRangeAsync(newInsurances);
            }

            var log = new LogRepository(db);
            log.AddSystemLog(truckInsuranceToEdit.TrailerLicense, "Update trailer insurance.", username);

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

    public async Task<ResponseViewModel> EditTrailer(TblTrailer trailerToEdit, string truck_code)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            await db.TblTruckTrailers
                .Where(w => w.TrailerCode == trailerToEdit.TrailerCode)
                .ExecuteDeleteAsync();

            if (!string.IsNullOrWhiteSpace(truck_code))
            {
                db.TblTruckTrailers.Add(new TblTruckTrailer
                {
                    TruckCode = truck_code,
                    TrailerCode = trailerToEdit.TrailerCode
                });
            }

            db.Entry(trailerToEdit).State = EntityState.Modified;

            var log = new LogRepository(db);
            log.AddSystemLog(trailerToEdit.TrailerLicense, "Update trailer.", trailerToEdit.UpdateBy);

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

    public async Task<ResponseViewModel> Remove(string code, string update_by)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var trailer = await db.TblTrailers
                .AsNoTracking()
                .Where(w => w.TrailerCode == code)
                .Select(s => new
                {
                    s.TrailerCode,
                    s.TrailerLicense
                })
                .FirstOrDefaultAsync();

            if (trailer == null)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            await db.TblTruckTrailers
                .Where(w => w.TrailerCode == code)
                .ExecuteDeleteAsync();

            DateTime now = DateTime.Now;

            int affectedRows = await db.TblTrailers
                .Where(w => w.TrailerCode == code)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.IsActived, false)
                    .SetProperty(s => s.IsEnabled, false)
                    .SetProperty(s => s.UpdateDate, now)
                    .SetProperty(s => s.UpdateBy, update_by));

            if (affectedRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            var log = new LogRepository(db);
            log.AddSystemLog(trailer.TrailerLicense, "Remove trailer", update_by);

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

    public async Task<ResponseViewModel> Active(string code, string update_by)
    {
        return await UpdateTrailerStatusAsync(
            code,
            update_by,
            isActived: true,
            isEnabled: null,
            logAction: "Active trailer"
        );
    }

    public async Task<ResponseViewModel> Inactive(string code, string update_by)
    {
        return await UpdateTrailerStatusAsync(
            code,
            update_by,
            isActived: false,
            isEnabled: null,
            logAction: "Inactive trailer"
        );
    }

    private async Task<ResponseViewModel> UpdateTrailerStatusAsync(
        string code,
        string username,
        bool isActived,
        bool? isEnabled,
        string logAction)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var trailer = await db.TblTrailers
                .AsNoTracking()
                .Where(w => w.TrailerCode == code)
                .Select(s => new
                {
                    s.TrailerCode,
                    s.TrailerLicense
                })
                .FirstOrDefaultAsync();

            if (trailer == null)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            DateTime now = DateTime.Now;
            int affectedRows;

            if (isEnabled.HasValue)
            {
                affectedRows = await db.TblTrailers
                    .Where(w => w.TrailerCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.IsEnabled, isEnabled.Value)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }
            else
            {
                affectedRows = await db.TblTrailers
                    .Where(w => w.TrailerCode == code)
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
            log.AddSystemLog(trailer.TrailerLicense, logAction, username);

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
            resultMessage = "ไม่พบข้อมูล Trailer"
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
