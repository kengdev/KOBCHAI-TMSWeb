using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class TruckRepository
{
    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;

    public TruckRepository(DbTMSContext context, CodeRepository codeService)
    {
        db = context;
        _codeService = codeService;
    }

    public async Task<List<TruckViewModel>> GetDriverListByTruck(string code)
    {
        return await (
            from a in db.TblTruckDrivers.AsNoTracking()
            join b in db.TblDrivers.AsNoTracking()
                on a.DriverCode equals b.DriverCode
            where a.TruckCode == code
            select new TruckViewModel
            {
                truck_code = a.TruckCode,
                driver_code = a.DriverCode,
                driver_name = b.DriverFullname,
                driver_phone = b.DriverPhone
            })
            .OrderBy(o => o.driver_name)
            .ToListAsync();
    }

    public async Task<ViewTruckListActive?> GetTruckByLicense(string code)
    {
        return await db.ViewTruckListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.TruckCode == code);
    }

    public async Task<TruckModel?> GetTruckDataByCode(string truck_code)
    {
        return await (
            from a in db.TblTrucks.AsNoTracking()
            join b in db.TblTruckDetails.AsNoTracking()
                on a.TruckCode equals b.TruckCode
            where a.TruckCode == truck_code
            select new TruckModel
            {
                truck_code = a.TruckCode,
                truck_license = a.TruckLicense,
                truck_no = b.TruckNo,
                truck_chassis_number = b.TruckChassisNumber,
                truck_year = b.TruckYear,
                brand_code = b.BrandCode,
                truck_type_code = b.TruckTypeCode,
                fuel_type_code = b.FuelTypeCode,
                truck_img = b.TruckImage
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ResponseViewModel> EditTruckDriver(TblTruck model, string username)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            DateTime now = DateTime.Now;

            await db.TblTruckDrivers
                .Where(w => w.TruckCode == model.TruckCode)
                .ExecuteDeleteAsync();

            var driverRates = model.TblTruckDrivers
                .Select(s => s.DriverCode)
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Distinct()
                .Select(driverCode => new TblTruckDriver
                {
                    TruckCode = model.TruckCode,
                    DriverCode = driverCode,
                    CreateDate = now,
                    CreateBy = username
                })
                .ToList();

            if (driverRates.Count > 0)
            {
                await db.TblTruckDrivers.AddRangeAsync(driverRates);
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

    public async Task<ResponseViewModel> AddTruck(TruckModel truckToAdd, string username)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            string? truckTypeGroupCode = await db.TblTruckTypes
                .AsNoTracking()
                .Where(w => w.TruckTypeCode == truckToAdd.truck_type_code)
                .Select(s => s.TruckTypeCode)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(truckTypeGroupCode))
            {
                await tr.RollbackAsync();
                return new ResponseViewModel
                {
                    result = false,
                    resultMessage = "ไม่พบประเภทรถ"
                };
            }

            DateTime now = DateTime.Now;

            var model = new TblTruck
            {
                TruckCode = await _codeService.GetTruckCode(),
                TruckLicense = truckToAdd.truck_license,
                IsActived = true,
                IsEnabled = true,
                CreateDate = now,
                CreateBy = username,
                TblTruckDetail = new TblTruckDetail
                {
                    TruckCode = truckToAdd.truck_code,
                    //FleetSupplierCode = truckToAdd.supplier_code,
                    TruckNo = truckToAdd.truck_no?.ToUpper(),
                    TruckChassisNumber = truckToAdd.truck_chassis_number?.ToUpper(),
                    TruckYear = truckToAdd.truck_year,
                    BrandCode = truckToAdd.brand_code,
                    TruckTypeCode = truckToAdd.truck_type_code,
                    FuelTypeCode = truckToAdd.fuel_type_code,
                    TruckTypegroupCode = truckTypeGroupCode,
                    TruckImage = truckToAdd.truck_img
                }
            };

            db.TblTrucks.Add(model);

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

    public async Task<List<TruckViewModel>> GetTruckDataList()
    {
        return await db.ViewTruckListActives
            .AsNoTracking()
            .OrderBy(o => o.FleetSupplierTypeCode)
            .Select(s => new TruckViewModel
            {
                fleet_supplier_code = s.FleetSupplierCode,
                fleet_supplier_name = s.FleetSupplierName,
                fleet_supplier_type_code = s.FleetSupplierTypeCode,
                fleet_supplier_type_name = s.FleetSupplierTypeName,
                truck_code = s.TruckCode,
                truck_license = s.TruckLicense,
                truck_chassis_number = s.TruckChassisNumber,
                truck_no = s.TruckNo,
                truck_type_name = s.TruckTypeName,
                brand_name = s.BrandName,
                fuel_type_name = s.FuelTypeName,
                driver_code = s.DriverCode,
                driver_name = s.DriverFullname,
                driver_phone = s.DriverPhone,
                is_actived = s.IsActived
            })
            .ToListAsync();
    }

    public async Task<ViewDriverListActive?> GetDriverByDriverCode(string driver_code)
    {
        return await db.ViewDriverListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.DriverCode == driver_code);
    }

    /*public ViewTruckInsuranceList GetInsuranceByInsuranceCode(string insurance_code)
    {
        var query = db.ViewTruckInsuranceLists.Where(w => w.InsuranceCode == insurance_code).FirstOrDefault();
        return query;
    }*/

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

    public async Task<ResponseViewModel> EditTruck(TruckModel truckToEdit, string username)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            string? truckTypeGroupCode = await db.TblTruckTypes
                .AsNoTracking()
                .Where(w => w.TruckTypeCode == truckToEdit.truck_type_code)
                .Select(s => s.TruckTypeCode)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(truckTypeGroupCode))
            {
                await tr.RollbackAsync();
                return new ResponseViewModel
                {
                    result = false,
                    resultMessage = "ไม่พบประเภทรถ"
                };
            }

            DateTime now = DateTime.Now;

            int truckRows = await db.TblTrucks
                .Where(w => w.TruckCode == truckToEdit.truck_code)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.TruckLicense, truckToEdit.truck_license));

            if (truckRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            int detailRows = await db.TblTruckDetails
                .Where(w => w.TruckCode == truckToEdit.truck_code)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.TruckNo, truckToEdit.truck_no == null ? null : truckToEdit.truck_no.ToUpper())
                    .SetProperty(s => s.TruckChassisNumber, truckToEdit.truck_chassis_number == null ? null : truckToEdit.truck_chassis_number.ToUpper())
                    .SetProperty(s => s.TruckYear, truckToEdit.truck_year)
                    .SetProperty(s => s.BrandCode, truckToEdit.brand_code)
                    .SetProperty(s => s.TruckTypeCode, truckToEdit.truck_type_code)
                    .SetProperty(s => s.TruckTypegroupCode, truckTypeGroupCode)
                    .SetProperty(s => s.FuelTypeCode, truckToEdit.fuel_type_code)
                    .SetProperty(s => s.TruckImage, truckToEdit.truck_img));

            if (detailRows == 0)
            {
                db.TblTruckDetails.Add(new TblTruckDetail
                {
                    TruckCode = truckToEdit.truck_code,
                    TruckNo = truckToEdit.truck_no?.ToUpper(),
                    TruckChassisNumber = truckToEdit.truck_chassis_number?.ToUpper(),
                    TruckYear = truckToEdit.truck_year,
                    BrandCode = truckToEdit.brand_code,
                    TruckTypeCode = truckToEdit.truck_type_code,
                    TruckTypegroupCode = truckTypeGroupCode,
                    FuelTypeCode = truckToEdit.fuel_type_code,
                    TruckImage = truckToEdit.truck_img
                });
            }

            var log = new LogRepository(db);
            log.AddSystemLog(truckToEdit.truck_code, "Update truck.", username);

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
            await db.TblTruckDrivers
                .Where(w => w.TruckCode == code)
                .ExecuteDeleteAsync();

            await db.TblTruckTrailers
                .Where(w => w.TruckCode == code)
                .ExecuteDeleteAsync();

            DateTime now = DateTime.Now;

            int affectedRows = await db.TblTrucks
                .Where(w => w.TruckCode == code)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.IsActived, false)
                    .SetProperty(s => s.IsEnabled, false));

            if (affectedRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            var log = new LogRepository(db);
            log.AddSystemLog(code, "Remove truck.", update_by);

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
        return await UpdateTruckStatusAsync(
            code,
            update_by,
            isActived: true,
            isEnabled: null,
            logAction: "Active truck.");
    }

    public async Task<ResponseViewModel> Inactive(string code, string update_by)
    {
        return await UpdateTruckStatusAsync(
            code,
            update_by,
            isActived: false,
            isEnabled: null,
            logAction: "Inactive truck.");
    }

    private async Task<ResponseViewModel> UpdateTruckStatusAsync(
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
                affectedRows = await db.TblTrucks
                    .Where(w => w.TruckCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.IsEnabled, isEnabled.Value));
            }
            else
            {
                affectedRows = await db.TblTrucks
                    .Where(w => w.TruckCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived));
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
            resultMessage = "ไม่พบข้อมูลรถ"
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
