using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.Transactions;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;
using TMSWeb.Repositories;
using TMSWeb.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TMSWeb.Repositories;

public class JobRepository
{
    private readonly DbTMSContext db;
    private readonly IConfiguration _configuration;
    private readonly AdvanceFundRepository _fundService;
    private readonly TripAllowanceService _allowanceService;

    public JobRepository(DbTMSContext context, IConfiguration configuration, AdvanceFundRepository funcService, TripAllowanceService allowanceService)
    {
        db = context;
        _configuration = configuration;
        _fundService = funcService;
        _allowanceService = allowanceService;
    }

    public async Task<ViewJobListActive?> GetOrderByCode(int id)
    {
        return await db.ViewJobListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<ResponseViewModel> EditJob(TblJobDetail jobToEdit, ChangeDriverModel model)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            DateTime now = DateTime.Now;

            int fuel = 0;
            var jobDetail = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == jobToEdit.JobDetailCode);

            var supplierData = await db.TblSuppliers.Where(w => w.SupplierCode == model.new_supplier_code).FirstOrDefaultAsync();

            if (jobDetail == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงาน");
            }

            var change = new ChangeDriverModel
            {
                cashflow = model.cashflow,
                job_allowance_id = model.job_allowance_id,
                job_detail_code = jobDetail.JobDetailCode,
                mission_code = jobDetail.MissionPlaceCode,
                supplier_code = jobDetail.SupplierCode,
                driver_code = jobDetail.DriverCode,
                truck_code = jobDetail.TruckCode,
                new_supplier_code = model.new_supplier_code,
                new_driver_code = supplierData.FleetTypeCode == "F1" ? model.new_driver_code : "26000000",
                new_truck_code = supplierData.FleetTypeCode == "F1" ? model.new_truck_code : "T000000",
                round_trip = model.round_trip,
                create_by = model.create_by
            };

            var truck = await db.TblTruckDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.TruckCode == jobToEdit.TruckCode);

            var cashing = await db.TblJobcashings
                .FirstOrDefaultAsync(w => w.JobDetailCode == jobToEdit.JobDetailCode);

            string? truckType = truck != null ? truck.TruckTypegroupCode : null;

            await EditDriverDataAsync(change);

            jobToEdit.DriverCode = jobToEdit.DriverCode == null ? "26000000" : jobToEdit.DriverCode;
            jobToEdit.TruckCode = jobToEdit.TruckCode == null ? "T000000" : jobToEdit.TruckCode;
            jobToEdit.UpdateDate = now;

            db.Entry(jobDetail).CurrentValues.SetValues(jobToEdit);

            log.AddSystemLog(jobToEdit.JobDetailCode, "Update job.", jobToEdit.UpdateBy);

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

    public async Task<ViewJobListActive> GetTblJobByIdAsync(int id)
    {
        var query = await db.ViewJobListActives.AsNoTracking().Where(w => w.Id == id).FirstOrDefaultAsync();
        return query;
    }

    public async Task<List<CashflowViewModel>> GetJobAllowanceListAsync(string job_detail_code)
    {
        var query = await (
                         from cashflow in db.TblJobcashflows
                         join truck in db.TblTrucks
                             on cashflow.TruckCode equals truck.TruckCode

                         join allowance in db.TblJobAllowances
                             on truck.TruckCode equals allowance.TruckCode

                         join driver in db.TblDrivers
                             on allowance.DriverCode equals driver.DriverCode

                         where allowance.JobDetailCode == job_detail_code &&
                                cashflow.JobDetailCode == job_detail_code &&
                                allowance.RoundTrip != "C" &&
                                allowance.DriverCode == driver.DriverCode
                         select new CashflowViewModel
                         {
                             job_detail_code = allowance.JobDetailCode,
                             job_date = allowance.JobDate,
                             driver = driver.DriverFullname,
                             license = truck.TruckLicense,
                             cashflow = cashflow.CashFlow
                         })
                        .AsNoTracking()
                        .ToListAsync();

        return query;
    }

    public async Task<TblJobDetail?> GetDetailDataByCode(string code)
    {
        return await db.TblJobDetails
            .Include(i => i.TblJobcashflows)
            .Include(i => i.SupplierCodeNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.JobDetailCode == code);
    }

    public async Task<List<DriverViewModel>> GetDriver(string driver_code)
    {
        return await db.TblDrivers
            .Where(w => w.IsEnabled == true && w.DriverCode != driver_code)
            .AsNoTracking()
            .OrderBy(o => o.DriverFullname)
            .Select(s => new DriverViewModel
            {
                driver_code = s.DriverCode,
                fullname = s.DriverFullname
            })
            .ToListAsync();
    }

    public async Task<List<JobViewModel>> GetJobDataList(DateTime start, DateTime end)
    {
        return await (
            from a in db.TblJobMasters.AsNoTracking()
            join b in db.TblJobDetails.AsNoTracking()
                on a.JobCode equals b.JobCode
            join d in db.TblCustomers.AsNoTracking()
                on a.CustomerCode equals d.CustomerCode
            join e in db.TblPlaces.AsNoTracking()
                on b.MissionPlaceCode equals e.PlaceCode
            join g in db.TblJobAllowances.AsNoTracking()
                on b.JobDetailCode equals g.JobDetailCode
            join c in db.TblTrucks.AsNoTracking()
                on g.TruckCode equals c.TruckCode
            join h in db.TblDrivers.AsNoTracking()
                on g.DriverCode equals h.DriverCode
            join f in db.TblSuppliers.AsNoTracking()
                on g.SupplierCode equals f.SupplierCode
            where g.IsComplete == false &&
                  b.IsCanceled == false &&
                  b.JobDetailCode != null &&
                  a.JobDate >= start &&
                  a.JobDate <= end &&
                  a.IsEnabled == true &&
                  b.IsEnabled == true
            select new JobViewModel
            {
                job_code = a.JobCode,
                job_detail_code = b.JobDetailCode,
                job_allowance_id = g.Id,
                job_date = a.LoadingDate.ToString("yyyy-MM-dd HH:mm"),
                job_date_value = a.LoadingDate,
                job_type = a.JobType,
                customer_name = d.CustomerAcronym,
                mission_name = e.PlaceName,
                booking_no = a.BookingNo,
                container_no = b.ContainerNo,
                container_size = a.ContainerSizeName,
                truck_license = c.TruckLicense,
                supplier_name = f.SupplierName,
                driver_fullname = h.DriverFullname,
                is_document = b.IsDocument,
                is_billing = b.IsBilling,
                is_weeklypayment = g != null ? g.IsWeeklypayment : false,
                tracking_status = b.TrackingStatus,
                round_status = g.RoundTrip
            })
            .ToListAsync();
    }

    public async Task<TblJobcashflowAdvance?> GetAdvanceByJob(string code)
    {
        return await db.TblJobcashflowAdvances
            .Where(w => w.JobDetailCode == code)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsPayment(string job_detail_code, string driver_code)
    {
        var isWeeklyPayment = await db.TblJobAllowances
            .AsNoTracking()
            .Where(w => w.JobDetailCode == job_detail_code &&
                        w.DriverCode == driver_code)
            .Select(s => (bool?)s.IsWeeklypayment)
            .FirstOrDefaultAsync();

        return isWeeklyPayment.HasValue && isWeeklyPayment.Value == false;
    }

    public async Task<List<OtherJobViewModel>> GetAdvanceOtherByJob(long code)
    {
        return await db.TblJobcashflowAdvanceOthers
            .AsNoTracking()
            .Where(w => w.CashflowAdvanceId == code)
            .Select(s => new OtherJobViewModel
            {
                name = s.AdvanceOther,
                value = s.AdvanceOtherValue,
            })
            .ToListAsync();
    }

    public async Task<List<TblJobcashflowCashing>> GetCashingByJob(string code)
    {
        return await db.TblJobcashflowCashings
            .Where(w => w.JobDetailCode == code)

            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ResponseViewModel> FinishJob(CashflowClearingModel model)
    {
        var log = new LogRepository(db);
        var response = new ResponseViewModel();
        try
        {
            decimal total = 0;
            decimal refund = 0;
            var allowanceData = await db.TblJobAllowances.FirstOrDefaultAsync(w => w.Id == model.allowance_id);
            allowanceData.IsComplete = true;

            TblJobcashflowAdvance advance = new TblJobcashflowAdvance();
            advance.JobDetailCode = model.job_detail_code;
            advance.TruckCode = allowanceData.TruckCode;
            advance.DriverCode = allowanceData.DriverCode;
            advance.SupplierCode = allowanceData.SupplierCode;
            advance.AdvanceLifton = model.AdvanceLifton;
            advance.AdvanceLiftoff = model.AdvanceLiftoff;
            advance.AdvanceYard = model.AdvanceYard;
            advance.AdvancePort = model.AdvancePort;
            advance.AdvanceTotal = advance.AdvanceLifton + advance.AdvanceLiftoff + advance.AdvanceYard + advance.AdvancePort;

            List<TblJobcashflowAdvanceOther> listAdvance = new List<TblJobcashflowAdvanceOther>();
            for (int i = 0; i < model.valAdvance.Count(); i++)
            {
                if (model.valAdvance[i] > 0)
                {
                    listAdvance.Add(new TblJobcashflowAdvanceOther
                    {
                        AdvanceOther = model.nameAdvance[i],
                        AdvanceOtherValue = model.valAdvance[i],
                    });

                    total = total + model.valAdvance[i];
                }
            }

            for (int i = 0; i < model.valCashing.Count(); i++)
            {
                if (model.valCashing[i] > 0)
                {
                    db.TblJobcashflowCashings.Add(new TblJobcashflowCashing
                    {
                        Cashing = model.nameCashing[i],
                        CashingValue = model.valCashing[i],
                        JobDetailCode = model.job_detail_code,
                        TruckCode = advance.TruckCode,
                        DriverCode = advance.DriverCode,
                        SupplierCode = advance.SupplierCode
                    });

                    total = total + model.valCashing[i];
                }
            }

            var supplierData = await db.TblSuppliers.Where(w => w.SupplierCode == allowanceData.SupplierCode).AsNoTracking().FirstOrDefaultAsync();
            if (supplierData.FleetTypeCode == "F1")
            {
                var cashflowData = await db.TblJobcashflows.FirstOrDefaultAsync(w => w.JobDetailCode == allowanceData.JobDetailCode &&
                                                                                    w.TruckCode == allowanceData.TruckCode && w.DriverCode == allowanceData.DriverCode);
                cashflowData.IsClearing = true;
                cashflowData.ClearingDate = DateTime.Now;

                refund = cashflowData.CashFlow - total;
                AdvanceFundResult fundResult = await _fundService.ReturnAsync(model.job_detail_code, cashflowData.Id, refund, model.create_by, cashflowData.DriverCode, null, false);
            }

            advance.AdvanceTotal = advance.AdvanceTotal + listAdvance.Sum(s => s.AdvanceOtherValue);
            advance.TblJobcashflowAdvanceOthers = listAdvance;

            db.TblJobcashflowAdvances.Add(advance);

            var jobDetail = await db.TblJobDetails.FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);
            jobDetail.TrackingStatus = "BO";
            jobDetail.ContainerNo = model.container_no.ToUpper();
            jobDetail.SealNo = model.seal_no.ToUpper();
            jobDetail.TareWeight = model.tare_weight;
            jobDetail.IsDocument = true;
            jobDetail.UpdateDate = DateTime.Now;
            jobDetail.UpdateBy = model.create_by;

            log.AddSystemLog(model.job_detail_code, "Cashflow clearing.", model.create_by);
            await db.SaveChangesAsync();

            return Success();
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public async Task<ResponseViewModel> RemoveImage(int id, string username)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            var data = await db.TblJobImgs.FindAsync(id);

            if (data == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบรูปภาพ");
            }

            db.TblJobImgs.Remove(data);
            log.AddSystemLog(data.JobDetailCode, "Remove image.", username);

            await db.SaveChangesAsync();
            await tr.CommitAsync();

            return new ResponseViewModel
            {
                result = true,
                resultMessage = "จัดการข้อมูลเรียบร้อยแล้ว"
            };
        }
        catch (Exception ex)
        {
            await tr.RollbackAsync();
            return Error(ex);
        }
    }

    public async Task<ResponseViewModel> ReverseStatus(string code, string status)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var job = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == code);

            if (job == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงาน");
            }

            if (status == "BO")
            {
                var tracking = await db.TblJobTrackings
                    .FirstOrDefaultAsync(w => w.JobDetailCode == code && w.TrackingStatus == "DO");

                if (tracking != null)
                {
                    db.TblJobTrackings.Remove(tracking);
                }

                job.TrackingStatus = "AR";
            }
            else if (status == "DO")
            {
                var tracking = await db.TblJobTrackings
                    .FirstOrDefaultAsync(w => w.JobDetailCode == code && w.TrackingStatus == "AR");

                if (tracking != null)
                {
                    db.TblJobTrackings.Remove(tracking);
                }

                job.TrackingStatus = "WP";
            }
            else if (status == "AR")
            {
                var tracking = await db.TblJobTrackings
                    .FirstOrDefaultAsync(w => w.JobDetailCode == code && w.TrackingStatus == "WP");

                if (tracking != null)
                {
                    db.TblJobTrackings.Remove(tracking);
                }

                job.TrackingStatus = "ST";
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

    public async Task<List<TrackingViewModel>> GetListTrackingByJob(string code, string truck_code, string driver_code, string supplier_code)
    {
        return await db.TblJobTrackings
            .AsNoTracking()
            .Where(w => w.JobDetailCode == code && w.TruckCode == truck_code && w.DriverCode == driver_code && w.SupplierCode == supplier_code)
            .Select(s => new TrackingViewModel
            {
                job_detail_code = s.JobDetailCode,
                tracking_status = s.TrackingStatus,
                tracking_date = s.TrackingDate
            })
            .ToListAsync();
    }

    public async Task<ResponseViewModel> UpdateData(UpdateDataModel dataToUpdate, string username)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            DateTime now = DateTime.Now;

            var detail = await db.TblJobDetails.FindAsync(dataToUpdate.job_detail_code);
            if (detail == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงาน");
            }

            var jobMaster = await db.TblJobMasters
                .FirstOrDefaultAsync(w => w.JobCode == detail.JobCode);

            if (jobMaster != null)
            {
                jobMaster.JobDate = dataToUpdate.job_date;
            }

            var truck = await db.TblTruckDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.TruckCode == detail.TruckCode);

            detail.TrailerLicense = dataToUpdate.trailer_license;
            detail.ContainerNo = dataToUpdate.container_no.ToUpper();
            detail.SealNo = dataToUpdate.seal_no.ToUpper();
            detail.TareWeight = dataToUpdate.tare_weight;
            detail.FinishPlaceCode = dataToUpdate.job_type != "FLOOR" ? dataToUpdate.finish_place_code : "000000000";
            detail.UpdateDate = now;
            detail.UpdateBy = username;

            var container = await db.TblContainerSizes
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.ContainerSizeCode == dataToUpdate.container_size_code);

            var advance = await db.TblJobadvances
                .FirstOrDefaultAsync(w => w.JobDetailCode == dataToUpdate.job_detail_code);

            if (dataToUpdate.job_type != "FLOOR" &&
                dataToUpdate.finish_place_code != "000000000" &&
                dataToUpdate.job_type == "IMPORT" &&
                advance != null &&
                container != null)
            {
                var finishYardport = await GetYardPortAsync(dataToUpdate.finish_place_code);
                var lift = await GetYardLiftAsync(dataToUpdate.finish_place_code);

                advance.AdvanceLiftoff = 0;
                advance.AdvanceLifton = container.ContainerSizeGroup == "S" ? lift?.ShortliftoffFee ?? 0 : lift?.LongliftoffFee ?? 0;
                advance.AdvanceYard = finishYardport?.ImportFee ?? 0;
            }

            log.AddSystemLog(dataToUpdate.job_detail_code, "Update job data.", username);

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

    public async Task<ResponseViewModel> WaitingDriver(ChangeDriverModel model, string username)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            var jobData = await db.TblJobDetails.FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);
            var oldJobData = jobData;

            jobData.TruckCode = "T000000";
            jobData.DriverCode = "26000000";

            var allowanceData = await db.TblJobAllowances.FirstOrDefaultAsync(w => w.Id == model.job_allowance_id);
            allowanceData.RoundTrip = "B";

            db.TblJobAllowances.Add(new TblJobAllowance
            {
                RoundTrip = "C",
                JobDetailCode = model.job_detail_code,
                JobDate = allowanceData.JobDate,
                TruckCode = "T000000",
                DriverCode = "26000000",
                DriverAllowance = 0,
                SupplierCode = jobData.SupplierCode,
                SupplierAllowance = 0,
                IsComplete = false,
                IsWeeklypayment = false,
                IsMonthlypayment = false,
            });

            log.AddSystemLog(model.job_detail_code, "Waiting driver from " + oldJobData.DriverCode + " to new driver.", username);

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

    public async Task<ResponseViewModel> CancelJob(CancelJobModel model)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            DateTime now = DateTime.Now;

            var detail = await db.TblJobDetails.FindAsync(model.job_detail_code);
            if (detail == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงาน");
            }

            detail.CostValue = model.trip_cost;
            detail.IsCanceled = true;
            detail.UpdateDate = now;
            detail.UpdateBy = model.create_by;

            var job = await db.TblJobMasters.FindAsync(detail.JobCode);
            if (job != null)
            {
                job.LiftedAmount = job.LiftedAmount - 1;
            }

            var advance = await db.TblJobadvances
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (advance != null)
            {
                advance.IsEnabled = false;
            }

            if (job != null)
            {
                await CancelDriverCostAsync(job, detail, model.driver_cost);
            }

            log.AddSystemLog(model.job_detail_code, "Cancel job.", model.create_by);

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

    public async Task<ResponseViewModel> CashflowClearing(CashflowClearingModel model)
    {
        var log = new LogRepository(db);
        var response = new ResponseViewModel();
        try
        {
            decimal total = 0;
            decimal refund = 0;
            var allowanceData = await db.TblJobAllowances.FirstOrDefaultAsync(w => w.Id == model.allowance_id);
            allowanceData.IsComplete = true;

            TblJobcashflowAdvance advance = new TblJobcashflowAdvance();
            advance.JobDetailCode = model.job_detail_code;
            advance.TruckCode = allowanceData.TruckCode;
            advance.DriverCode = allowanceData.DriverCode;
            advance.SupplierCode = allowanceData.SupplierCode;
            advance.AdvanceLifton = model.AdvanceLifton;
            advance.AdvanceLiftoff = model.AdvanceLiftoff;
            advance.AdvanceYard = model.AdvanceYard;
            advance.AdvancePort = model.AdvancePort;
            advance.AdvanceTotal = advance.AdvanceLifton + advance.AdvanceLiftoff + advance.AdvanceYard + advance.AdvancePort;

            List<TblJobcashflowAdvanceOther> listAdvance = new List<TblJobcashflowAdvanceOther>();
            for (int i = 0; i < model.valAdvance.Count(); i++)
            {
                if (model.valAdvance[i] > 0)
                {
                    listAdvance.Add(new TblJobcashflowAdvanceOther
                    {
                        AdvanceOther = model.nameAdvance[i],
                        AdvanceOtherValue = model.valAdvance[i],
                    });

                    total = total + model.valAdvance[i];
                }
            }

            for (int i = 0; i < model.valCashing.Count(); i++)
            {
                if (model.valCashing[i] > 0)
                {
                    db.TblJobcashflowCashings.Add(new TblJobcashflowCashing
                    {
                        Cashing = model.nameCashing[i],
                        CashingValue = model.valCashing[i],
                        JobDetailCode = model.job_detail_code,
                        TruckCode = advance.TruckCode,
                        DriverCode = advance.DriverCode,
                        SupplierCode = advance.SupplierCode
                    });

                    total = total + model.valCashing[i];
                }
            }

            var supplierData = await db.TblSuppliers.Where(w => w.SupplierCode == allowanceData.SupplierCode).AsNoTracking().FirstOrDefaultAsync();
            if (supplierData.FleetTypeCode == "F1")
            {
                var cashflowData = await db.TblJobcashflows.FirstOrDefaultAsync(w => w.JobDetailCode == allowanceData.JobDetailCode &&
                                                                                    w.TruckCode == allowanceData.TruckCode && 
                                                                                    w.DriverCode == allowanceData.DriverCode);
                cashflowData.IsClearing = true;
                cashflowData.ClearingDate = DateTime.Now;

                refund = cashflowData.CashFlow - (total + advance.AdvanceTotal);
                AdvanceFundResult fundResult = await _fundService.ReturnAsync(model.job_detail_code, cashflowData.Id, refund, model.create_by, cashflowData.DriverCode, null, false);
            }

            advance.AdvanceTotal = advance.AdvanceTotal + listAdvance.Sum(s => s.AdvanceOtherValue);
            advance.TblJobcashflowAdvanceOthers = listAdvance;

            db.TblJobcashflowAdvances.Add(advance);

            log.AddSystemLog(model.job_detail_code, "Cashflow clearing.", model.create_by);
            await db.SaveChangesAsync();

            return Success();
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public async Task<List<JobViewModel>> GetOrderBySearch(string code)
    {
        string keyword = code.ToUpper();

        return await db.ViewJobSummaries
            .AsNoTracking()
            .Where(w => w.JobDetailCode.Contains(keyword) ||
                        w.BookingNo.Contains(keyword) ||
                        w.JobInvoice.Contains(keyword) ||
                        w.ContainerNo.Contains(keyword))
            .Select(s => new JobViewModel
            {
                job_type = s.JobType,
                booking_no = s.BookingNo,
                job_code = s.JobCode,
                job_invoice = s.JobInvoice,
                container_no = s.ContainerNo,
                job_detail_code = s.JobDetailCode,
                mission_name = s.MissionName,
                customer_name = s.CustomerAcronym,
                job_date = s.JobDate.ToString("dd-MM-yyyy"),
                supplier_name = s.SupplierName,
                tracking_status = s.TrackingStatus
            })
            .ToListAsync();
    }

    public async Task<ViewJobListActive?> GetOrderBySearchDetail(string code)
    {
        return await db.ViewJobListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.JobDetailCode == code);
    }

    public async Task<OrderPlaceViewModel?> GetPlaceByCode(string code)
    {
        return await db.TblPlaces
            .AsNoTracking()
            .Where(w => w.PlaceCode == code)
            .Select(s => new OrderPlaceViewModel
            {
                place_code = s.PlaceCode,
                place_name = s.PlaceName,
                place_type = s.PlaceType
            })
            .FirstOrDefaultAsync();
    }

    public async Task<OrderPlaceViewModel?> GetYardPortDataByCode(string code)
    {
        return await GetPlaceByCode(code);
    }

    public async Task<List<TblJobImg>> GetOrderImagesByCode(string code)
    {
        return await db.TblJobImgs
            .AsNoTracking()
            .Where(w => w.JobDetailCode == code)
            .ToListAsync();
    }

    public async Task<ViewDriverListActive?> GetDriverCodeByTruck(string code)
    {
        return await db.ViewDriverListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.TruckCode == code);
    }

    public async Task<TblSupplier> GetTblSupplier(string supplierCode)
    {
        var data = await db.TblSuppliers.Where(w => w.SupplierCode == supplierCode).AsNoTracking().FirstOrDefaultAsync();
        return data;
    }

    public async Task<ResponseViewModel> RemoveJob(string code, string username)
    {
        await using var tr = await db.Database.BeginTransactionAsync();
        ResponseViewModel response = new ResponseViewModel();

        try
        {
            var log = new LogRepository(db);
            var detail = await db.TblJobDetails.FirstOrDefaultAsync(w => w.JobDetailCode == code);
            var job_code = detail.JobCode;
            detail.IsEnabled = false;

            var job = await db.TblJobMasters.FirstOrDefaultAsync(w => w.JobCode == job_code);
            job.LiftedAmount = job.LiftedAmount - 1;

            var cashflowData = await db.TblJobcashflows.Where(w => w.JobDetailCode == code).FirstOrDefaultAsync();
            var cashflowAdvance = await db.TblJobcashflowAdvances.Where(w => w.JobDetailCode == code).FirstOrDefaultAsync();
            var cashflowCashing = await db.TblJobcashflowCashings.Where(w => w.JobDetailCode == code).ToListAsync();

            if (cashflowAdvance != null)
            {
                var cashflowAdvanceOther = await db.TblJobcashflowAdvanceOthers.Where(w => w.CashflowAdvanceId == cashflowAdvance.Id).ToListAsync();
                if (cashflowAdvanceOther != null)
                    db.TblJobcashflowAdvances.Remove(cashflowAdvance);

                db.TblJobcashflowAdvanceOthers.RemoveRange(cashflowAdvanceOther);
            }

            if (cashflowCashing != null)
                db.TblJobcashflowCashings.RemoveRange(cashflowCashing);

            var images = await db.TblJobImgs.Where(w => w.JobDetailCode == code).ToListAsync();
            db.TblJobImgs.RemoveRange(images);

            var allowanceData = await db.TblJobAllowances.FirstOrDefaultAsync(w => w.JobDetailCode == code);
            db.TblJobAllowances.Remove(allowanceData);

            if (cashflowData != null)
            {
                AdvanceFundResult fundResult = await _fundService.ReturnAsync(code, cashflowData.Id, cashflowData.CashFlow, username, cashflowData.DriverCode, null, false);
                db.TblJobcashflows.Remove(cashflowData);
            }

            var i_binding = db.TblBindingGenerates.Find(job.JobCode, code);
            i_binding.IsEnabled = false;

            var tracking = db.TblJobTrackings.Where(w => w.JobDetailCode == code).ToList();
            db.TblJobTrackings.RemoveRange(tracking);

            log.AddSystemLog(code, "Remove job.", username);
            await db.SaveChangesAsync();
            await tr.CommitAsync();
            response.result = true;
            response.resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
        }
        catch (Exception ex)
        {
            await tr.DisposeAsync();
            await db.DisposeAsync();
            response.result = false;
            response.resultMessage = "<div>เกิดข้อผิดพลาดระหว่างการทำงาน</div><div style='margin-top:-30px'> กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ</div>";
            response.errorException = ex;
        }

        return response;
    }


    private async Task<decimal> GetDriverCostAsync(string placeCode)
    {
        return await db.TblDriverRates
            .AsNoTracking()
            .Where(w => w.PlaceCode == placeCode)
            .Select(s => (decimal?)s.RateValue)
            .FirstOrDefaultAsync() ?? 0;
    }

    private async Task<TblYardCashing?> GetYardLiftAsync(string yardportCode)
    {
        return await db.TblYardCashings
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.YardportCode == yardportCode);
    }

    private async Task<TblYardport?> GetYardPortAsync(string yardportCode)
    {
        return await db.TblYardports
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.YardportCode == yardportCode);
    }

    private async Task CancelDriverCostAsync(TblJobMaster jobMaster, TblJobDetail jobDetail, decimal driverCost)
    {
        string oldSupplier = jobDetail.SupplierCode;
        var holiday = 0;

        if (oldSupplier == "SP26001")
        {
            var driver = await db.TblJobAllowances
                .FirstOrDefaultAsync(w => w.DriverCode == jobDetail.DriverCode &&
                                          w.JobDetailCode == jobDetail.JobDetailCode);

            if (driver != null)
            {
                driver.DriverAllowance = driverCost + holiday;
            }
        }
    }

    private async Task EditDriverDataAsync(ChangeDriverModel model)
    {
        var deductDriver = _configuration.GetValue<decimal>("Deduction:driver");
        var deductSupplier = _configuration.GetValue<decimal>("Deduction:supplier");
        var driverContainer = _configuration.GetValue<decimal>("Allowance:driver_container_pickup");
        var supplierContainer = _configuration.GetValue<decimal>("Allowance:supplier_container_pickup");

        var jobDetail = await db.TblJobDetails.FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);
        var jobMaster = await db.TblJobMasters.Where(w => w.JobCode == jobDetail.JobCode).AsNoTracking().FirstOrDefaultAsync();

        var oldJobAllowance = await db.TblJobAllowances.FirstOrDefaultAsync(w => w.Id == model.job_allowance_id);

        var newSupplier = await db.TblSuppliers.Where(w => w.SupplierCode == model.new_supplier_code).FirstOrDefaultAsync();
        var oldSupplier = await db.TblSuppliers.Where(w => w.SupplierCode == model.supplier_code).FirstOrDefaultAsync();

        var oldCashflow = await db.TblJobcashflows.FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code && w.DriverCode == oldJobAllowance.DriverCode);

        var newTruck = await db.TblTruckDetails.Where(w => w.TruckCode == model.new_truck_code).AsNoTracking().FirstOrDefaultAsync();
        var oldTruck = await db.TblTruckDetails.Where(w => w.TruckCode == jobDetail.TruckCode).AsNoTracking().FirstOrDefaultAsync();

        var driverAllowance = await _allowanceService.GetDriverCostAsync(oldSupplier, newSupplier, jobDetail.MissionPlaceCode);
        var truckAllowance = await _allowanceService.GetTruckCostAsync(oldSupplier, newSupplier, newTruck, jobDetail.MissionPlaceCode);

        if (jobDetail.TrackingStatus != "PN")
        {
            //จัดรถใหม่หลังจากมีการทอยตู้เสร็จ
            if (model.round_trip == "C")
            {
                jobDetail.TruckCode = model.new_truck_code;
                jobDetail.DriverCode = model.new_driver_code;
                jobDetail.SupplierCode = model.new_supplier_code;

                oldJobAllowance.TruckCode = model.new_truck_code;
                oldJobAllowance.DriverCode = model.new_driver_code;
                oldJobAllowance.SupplierCode = model.new_supplier_code;
                oldJobAllowance.DriverAllowance = driverAllowance.oldDriver;
                oldJobAllowance.SupplierAllowance = driverAllowance.oldTruck;
                oldJobAllowance.RoundTrip = "A";

                if (newSupplier.FleetTypeCode == "F1")
                {
                    var jobCashflow = new TblJobcashflow
                    {
                        JobDetailCode = model.job_detail_code,
                        TruckCode = model.new_truck_code,
                        DriverCode = model.new_driver_code,
                        CashFlow = model.cashflow,
                        AdvanceRemark = "มีการเปลี่ยนแปลงพนักงานขับรถ",
                        IsClearing = false
                    };

                    db.TblJobcashflows.Add(jobCashflow);

                    await db.SaveChangesAsync();

                    AdvanceFundResult fundResult = await _fundService.PayAsync(model.job_detail_code, jobCashflow.Id, 2, model.cashflow, model.new_driver_code, model.create_by);
                }
            }
            else
            {
                if (newSupplier.FleetTypeCode == "F1")
                {
                    //จ่ายค่าทอยตู้ให้คนเก่า
                    if (oldSupplier.FleetTypeCode == "F1")
                    {
                        oldJobAllowance.RoundTrip = "B";
                        oldJobAllowance.DriverAllowance = driverAllowance.oldDriver;
                        oldJobAllowance.SupplierAllowance = truckAllowance.oldTruck;
                    }

                    //จ่ายค่าเที่ยวให้คนใหม่
                    db.TblJobAllowances.Add(new TblJobAllowance
                    {
                        RoundTrip = "A",
                        JobDate = DateOnly.FromDateTime(jobMaster.JobDate),
                        JobDetailCode = model.job_detail_code,
                        TruckCode = model.new_truck_code,
                        DriverCode = model.new_driver_code,
                        DriverAllowance = driverAllowance.newDriver,
                        SupplierCode = model.new_supplier_code,
                        SupplierAllowance = truckAllowance.newTruck,
                        IsComplete = false,
                        IsWeeklypayment = false,
                        IsMonthlypayment = false,
                    });

                    var jobCashflow = new TblJobcashflow
                    {
                        JobDetailCode = model.job_detail_code,
                        TruckCode = model.new_truck_code,
                        DriverCode = model.new_driver_code,
                        CashFlow = model.cashflow,
                        AdvanceRemark = "มีการเปลี่ยนแปลงพนักงานขับรถ",
                        IsClearing = false
                    };

                    db.TblJobcashflows.Add(jobCashflow);

                    await db.SaveChangesAsync();

                    AdvanceFundResult fundResult = await _fundService.PayAsync(model.job_detail_code, jobCashflow.Id, 2, model.cashflow, model.new_driver_code, model.create_by);
                }
                else
                {
                    if (oldSupplier.FleetTypeCode == "F1")
                    {
                        oldJobAllowance.RoundTrip = "B";
                        oldJobAllowance.DriverAllowance = driverAllowance.oldDriver;
                        oldJobAllowance.SupplierAllowance = truckAllowance.oldTruck;
                    }

                    db.TblJobAllowances.Add(new TblJobAllowance
                    {
                        RoundTrip = "A",
                        JobDate = DateOnly.FromDateTime(jobMaster.JobDate),
                        JobDetailCode = model.job_detail_code,
                        TruckCode = "T000000",
                        DriverCode = "26000000",
                        DriverAllowance = 0,
                        SupplierCode = model.new_supplier_code,
                        SupplierAllowance = truckAllowance.newTruck,
                        IsComplete = false,
                        IsWeeklypayment = false,
                        IsMonthlypayment = false
                    });
                }
            }
        }
        else
        {
            oldJobAllowance.DriverCode = model.new_driver_code;
            oldJobAllowance.TruckCode = model.new_truck_code;
            oldJobAllowance.SupplierCode = model.new_supplier_code;

            long cashflowId;
            if (newSupplier.FleetTypeCode == "F1")
            {
                if (oldSupplier.FleetTypeCode == "F1")
                {
                    cashflowId = oldCashflow.Id;
                    oldCashflow.TruckCode = model.new_truck_code;
                    oldCashflow.DriverCode = model.new_driver_code;
                    oldCashflow.CashFlow = model.cashflow;
                    oldCashflow.AdvanceRemark = "มีการเปลี่ยนแปลงพนักงานขับรถ";

                    AdvanceFundResult returnFundResult = await _fundService.ReturnAsync(model.job_detail_code, oldCashflow.Id, oldCashflow.CashFlow, model.driver_code, model.create_by);
                }
                else
                {
                    //เปลี่ยนจากรถนอกเป็นรถภายใน
                    oldJobAllowance.DriverAllowance = driverAllowance.driverValue;
                    oldJobAllowance.SupplierAllowance = truckAllowance.truckValue;

                    var jobCashflow = new TblJobcashflow
                    {
                        JobDetailCode = model.job_detail_code,
                        TruckCode = model.new_truck_code,
                        DriverCode = model.new_driver_code,
                        CashFlow = model.cashflow,
                        AdvanceRemark = "มีการเปลี่ยนแปลงพนักงานขับรถ",
                        IsClearing = false
                    };

                    db.TblJobcashflows.Add(jobCashflow);
                    await db.SaveChangesAsync();

                    cashflowId = jobCashflow.Id;
                }

                AdvanceFundResult payFundResult = await _fundService.PayAsync(model.job_detail_code, cashflowId, 2, model.cashflow, model.new_driver_code, model.create_by);
            }
            else
            {
                oldJobAllowance.DriverAllowance = 0;
                oldJobAllowance.SupplierAllowance = truckAllowance.oldTruck;

                //เปลี่ยนจากรถภายในเป็นภายนอก
                if (oldSupplier.FleetTypeCode == "F1")
                {
                    if (oldCashflow != null)
                    {
                        AdvanceFundResult fundResult = await _fundService.ReturnAsync(model.job_detail_code, oldCashflow.Id, oldCashflow.CashFlow, model.create_by, oldCashflow.DriverCode, null, false);
                        oldCashflow.IsClearing = true;
                        oldCashflow.ClearingDate = DateTime.Now;
                    }
                }
            }
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
            resultMessage = "<div>เกิดข้อผิดพลาดระหว่างการทำงาน</div><div style='margin-top:-30px'> กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ</div>",
            errorException = ex
        };
    }
}
