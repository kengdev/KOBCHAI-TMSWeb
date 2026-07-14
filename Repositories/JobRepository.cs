using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;
using TMSWeb.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TMSWeb.Repositories;

public class JobRepository
{
    private readonly DbTMSContext db;
    private readonly BudgetRepository _budgetService;

    public JobRepository(DbTMSContext context, BudgetRepository budgetService)
    {
        db = context;
        _budgetService = budgetService;
    }

    public async Task<ViewJobListActive?> GetOrderByCode(int id)
    {
        return await db.ViewJobListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<ResponseViewModel> RemoveJob(string code, string username)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            DateTime now = DateTime.Now;

            var detail = await db.TblJobDetails.Where(w => w.JobDetailCode == code).AsNoTracking().FirstOrDefaultAsync();
            if (detail == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงาน");
            }

            var jobCode = detail.JobCode;
            var job = await db.TblJobMasters.FindAsync(jobCode);
            if (job != null)
            {
                job.LiftedAmount = job.LiftedAmount - 1;
            }

            var advance = await db.TblJobadvances
                .FirstOrDefaultAsync(w => w.JobDetailCode == code);

            if (advance != null)
            {
                await db.TblJobadvanceOthers
                    .Where(w => w.AdvanceCode == advance.AdvanceCode)
                    .ExecuteDeleteAsync();

                await db.TblJobadvances
                    .Where(w => w.AdvanceCode == advance.AdvanceCode)
                    .ExecuteDeleteAsync();
            }

            await db.TblJobImgs
                .Where(w => w.JobDetailCode == code)
                .ExecuteDeleteAsync();

            var supplierCost = await db.TblSupplierAllowances
                .FirstOrDefaultAsync(w => w.JobDetailCode == code);

            if (supplierCost != null)
            {
                db.TblSupplierAllowances.Remove(supplierCost);
            }

            var transactionCodes = await db.LogBudgetTransactions
                .AsNoTracking()
                .Where(w => w.JobDetailCode == code)
                .Select(s => s.TransactionCode)
                .Distinct()
                .ToListAsync();

            decimal amount = 0;
            foreach (var transactionCode in transactionCodes)
            {
                var budgetTransaction = await db.TblBudgetTransactions
                    .FirstOrDefaultAsync(w => w.TransactionCode == transactionCode);

                if (budgetTransaction == null)
                {
                    continue;
                }

                if (budgetTransaction.PaymentType == "MIN")
                {
                    amount += budgetTransaction.Amount;
                }
                else
                {
                    if (budgetTransaction.Amount < 0)
                    {
                        amount = budgetTransaction.Amount + Math.Abs(amount);
                    }
                    else
                    {
                        amount = budgetTransaction.Amount - Math.Abs(amount);
                    }
                }

                budgetTransaction.IsEnabled = false;
                budgetTransaction.UpdateDate = now;
                budgetTransaction.UpdateBy = username;
            }

            var budget = await db.TblBudgets
                .FirstOrDefaultAsync(w => w.BudgetType == "A");

            if (budget != null)
            {
                budget.Balance += amount;
            }

            var i_binding = await db.TblBindingGenerates.FindAsync(job.JobCode, code);
            i_binding.IsEnabled = false;

            await db.TblJobAllowances
                .Where(w => w.JobDetailCode == code)
                .ExecuteDeleteAsync();

            await db.TblJobTrackings
                .Where(w => w.JobDetailCode == code)
                .ExecuteDeleteAsync();

            await db.TblJobDetails
                .Where(w => w.JobDetailCode == code)
                .ExecuteDeleteAsync();

            log.AddSystemLog(code, "Remove job.", username);

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

    public async Task<ResponseViewModel> EditJob(TblJobDetail jobToEdit, int job_allowance_id)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            DateTime now = DateTime.Now;

            int fuel = 0;
            var jobDetail = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == jobToEdit.JobDetailCode);

            if (jobDetail == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงาน");
            }

            var change = new ChangeDriverModel
            {
                job_allowance_id = job_allowance_id,
                job_detail_code = jobDetail.JobDetailCode,
                mission_code = jobDetail.MissionPlaceCode,
                supplier_code = jobDetail.SupplierCode,
                driver_code = jobDetail.DriverCode,
                truck_code = jobDetail.TruckCode,
                new_supplier_code = jobToEdit.SupplierCode,
                new_driver_code = jobToEdit.DriverCode,
                new_truck_code = jobToEdit.TruckCode
            };

            var truck = await db.TblTruckDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.TruckCode == jobToEdit.TruckCode);

            var cashing = await db.TblJobcashings
                .FirstOrDefaultAsync(w => w.JobDetailCode == jobToEdit.JobDetailCode);

            string? truckType = truck != null ? truck.TruckTypegroupCode : null;

            await EditDriverDataAsync(change);


            jobToEdit.DriverCode = jobToEdit.DriverCode == null ? "DV000000" : jobToEdit.DriverCode;
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
                        from allowance in db.TblJobAllowances

                        join cashflowData in db.TblJobcashflows
                            on new
                            {
                                allowance.JobDetailCode,
                                allowance.TruckCode,
                                allowance.DriverCode
                            }
                            equals new
                            {
                                cashflowData.JobDetailCode,
                                cashflowData.TruckCode,
                                cashflowData.DriverCode
                            }
                            into cashflowGroup

                        from cashflow in cashflowGroup.DefaultIfEmpty()

                        join truck in db.TblTrucks
                            on allowance.TruckCode equals truck.TruckCode

                        join driver in db.TblDrivers
                            on allowance.DriverCode equals driver.DriverCode

                        where allowance.JobDetailCode == job_detail_code

                        select new CashflowViewModel
                        {
                            job_detail_code = allowance.JobDetailCode,
                            job_date = allowance.JobDate,
                            driver = driver.DriverFullname,
                            license = truck.TruckLicense,

                            cashflow = cashflow != null
                                ? cashflow.CashFlow
                                : 0
                        })
                        .AsNoTracking()
                        .ToListAsync();

        return query;
    }

    public async Task<TblJobDetail?> GetDetailDataByCode(string code)
    {
        return await db.TblJobDetails
            .Include(i => i.TblJobcashflows)
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.JobDetailCode == code);
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
            join i in db.TblJobcashflows.AsNoTracking()
                on new
                {
                    g.JobDetailCode,
                    g.TruckCode,
                    g.DriverCode
                }
                equals new
                {
                    i.JobDetailCode,
                    i.TruckCode,
                    i.DriverCode
                }
                into cashflowGroup
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
                tracking_status = b.TrackingStatus,
                is_weeklypayment = g != null ? g.IsWeeklypayment : false,
                is_advance_paid = cashflowGroup.Any(x => x.CashFlow > 0)
            })
            .ToListAsync();
    }

    public async Task<TblJobadvance?> GetAdvanceByJob(string code)
    {
        return await db.TblJobadvances
            .Include(i => i.TblJobcashflowAdvances)
            .Include(i => i.TblJobadvanceOthers)
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.JobDetailCode == code && w.IsEnabled == true);
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

    public async Task<List<OtherJobViewModel>> GetAdvanceOtherByJob(string code)
    {
        return await db.TblJobadvanceOthers
            .AsNoTracking()
            .Where(w => w.AdvanceCode == code)
            .Select(s => new OtherJobViewModel
            {
                name = s.AdvanceOther,
                value = s.AdvanceOtherValue,
                supplier_value = s.SupplierOtherValue
            })
            .ToListAsync();
    }

    public async Task<List<TblJobcashing>> GetCashingByJob(string code)
    {
        return await db.TblJobcashings
            .Where(w => w.JobDetailCode == code)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ResponseViewModel> FinishJob(FinishJobModel jobToFinish, TblJobadvance jobAdvance, TblJobcashing jobCashing)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            DateTime now = DateTime.Now;

            var allowance = await db.TblJobAllowances
                .FirstOrDefaultAsync(w => w.Id == jobToFinish.allowance_id);

            if (allowance == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลเงินสด");
            }
            allowance.IsComplete = true;

            decimal totalOtherCashing = 0;
            if (jobToFinish.valCashing != null)
            {
                await db.TblJobcashings
                    .Where(w => w.JobDetailCode == jobToFinish.job_detail_code)
                    .ExecuteDeleteAsync();

                await db.TblJobcashflowCashings
                    .Where(w => w.JobDetailCode == jobToFinish.job_detail_code &&
                            w.TruckCode == allowance.TruckCode &&
                            w.DriverCode == allowance.DriverCode)
                    .ExecuteDeleteAsync();


                for (int i = 0; i < jobToFinish.valCashing.Count(); i++)
                {
                    if (jobToFinish.nameCashing[i] != "" && jobToFinish.valCashing[i] != 0)
                    {
                        totalOtherCashing += jobToFinish.valCashing[i];
                        db.TblJobcashings.Add(new TblJobcashing
                        {
                            Cashing = jobToFinish.nameCashing[i],
                            CashingValue = jobToFinish.valCashing[i],
                            JobDetailCode = jobToFinish.job_detail_code
                        });

                        db.TblJobcashflowCashings.Add(new TblJobcashflowCashing
                        {
                            Cashing = jobToFinish.nameCashing[i],
                            CashingValue = jobToFinish.valCashing[i],
                            JobDetailCode = jobToFinish.job_detail_code,
                            TruckCode = allowance.TruckCode,
                            DriverCode = allowance.DriverCode
                        });
                    }
                }
            }

            var advance = await db.TblJobadvances
                .FirstOrDefaultAsync(w => w.JobDetailCode == jobToFinish.job_detail_code);

            if (advance == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลเงินสำรอง");
            }

            advance.AdvanceLiftoff = jobAdvance.AdvanceLiftoff;
            advance.AdvanceLifton = jobAdvance.AdvanceLifton;
            advance.AdvancePort = jobAdvance.AdvancePort;
            advance.AdvanceYard = jobAdvance.AdvanceYard;
            advance.AdvanceTotal = jobAdvance.AdvanceLiftoff + jobAdvance.AdvanceLifton + jobAdvance.AdvancePort + jobAdvance.AdvanceYard;

            var cashflowAdvance = await db.TblJobcashflowAdvances.FirstOrDefaultAsync(w => w.AdvanceCode == advance.AdvanceCode);
            if (cashflowAdvance == null)
            {
                db.TblJobcashflowAdvances.Add(new TblJobcashflowAdvance
                {
                    JobDetailCode = jobToFinish.job_detail_code,
                    TruckCode = allowance.TruckCode,
                    DriverCode = allowance.DriverCode,
                    AdvanceLifton = advance.AdvanceLifton,
                    AdvanceLiftoff = advance.AdvanceLiftoff,
                    AdvanceYard = advance.AdvanceYard,
                    AdvancePort = advance.AdvancePort,
                    AdvanceTotal = advance.AdvanceTotal,
                    AdvanceCode = advance.AdvanceCode
                });
            }
            else
            {
                cashflowAdvance.AdvanceLifton = advance.AdvanceLifton;
                cashflowAdvance.AdvanceLiftoff = advance.AdvanceLiftoff;
                cashflowAdvance.AdvanceYard = advance.AdvanceYard;
                cashflowAdvance.AdvancePort = advance.AdvancePort;
                cashflowAdvance.AdvanceTotal = advance.AdvanceTotal;
                cashflowAdvance.AdvanceCode = advance.AdvanceCode;
            }

            decimal totalOtherAdvance = 0;
            if (jobToFinish.valAdvance != null)
            {
                await db.TblJobadvanceOthers
                    .Where(w => w.AdvanceCode == advance.AdvanceCode)
                    .ExecuteDeleteAsync();

                await db.TblJobcashflowAdvanceOthers
                    .Where(w => w.AdvanceCode == advance.AdvanceCode)
                    .ExecuteDeleteAsync();

                for (int i = 0; i < jobToFinish.valAdvance.Count(); i++)
                {
                    if (jobToFinish.nameAdvance[i] != "" && jobToFinish.valAdvance[i] != 0)
                    {
                        totalOtherAdvance += jobToFinish.valAdvance[i];
                        db.TblJobadvanceOthers.Add(new TblJobadvanceOther
                        {
                            AdvanceOther = jobToFinish.nameAdvance[i],
                            AdvanceOtherValue = jobToFinish.valAdvance[i],
                            SupplierOtherValue = 0,
                            AdvanceCode = advance.AdvanceCode
                        });

                        db.TblJobcashflowAdvanceOthers.Add(new TblJobcashflowAdvanceOther
                        {
                            AdvanceOther = jobToFinish.nameAdvance[i],
                            AdvanceOtherValue = jobToFinish.valAdvance[i],
                            JobDetailCode = jobToFinish.job_detail_code,
                            TruckCode = allowance.TruckCode,
                            DriverCode = allowance.DriverCode,
                            AdvanceCode = advance.AdvanceCode
                        });
                    }
                }
            }

            var itemDetail = await db.TblJobDetails.FindAsync(jobToFinish.job_detail_code);
            if (itemDetail == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงาน");
            }

            itemDetail.TrackingStatus = "BO";
            itemDetail.ContainerNo = jobToFinish.container_no.ToUpper();
            itemDetail.SealNo = jobToFinish.seal_no.ToUpper();
            itemDetail.TareWeight = jobToFinish.tare_weight;
            itemDetail.SupplierInvoice = itemDetail.SupplierCode != "SP26001" ? jobToFinish.supplier_invoice : "-";
            itemDetail.UpdateDate = now;
            itemDetail.UpdateBy = jobToFinish.username;

            if (itemDetail.SupplierCode == "SP26001")
            {
                var driver = await db.TblJobAllowances
                    .FirstOrDefaultAsync(w => w.JobDetailCode == jobToFinish.job_detail_code);

                if (driver == null)
                {
                    var master = await db.TblJobMasters
                        .FirstOrDefaultAsync(w => w.JobCode == itemDetail.JobCode);

                    if (master != null)
                    {
                        int fuel = await GetFuelRateAsync(itemDetail.MissionPlaceCode, master.TruckTypegroupCode);
                        var jobPlace = await db.TblJobPlaces
                            .FirstOrDefaultAsync(w => w.JobCode == master.JobCode);
                    }
                }
            }

            db.TblJobTrackings.Add(new TblJobTracking
            {
                JobDetailCode = jobToFinish.job_detail_code,
                TrackingStatus = "BO",
                TrackingDate = now,
                Lattitude = "-",
                Longitude = "-",
                Speed = "0"
            });

            log.AddSystemLog(jobToFinish.job_detail_code, "Finished job.", jobToFinish.username);

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

    public async Task<List<TrackingViewModel>> GetListTrackingByJob(string code)
    {
        return await db.TblJobTrackings
            .AsNoTracking()
            .Where(w => w.JobDetailCode == code)
            .Select(s => new TrackingViewModel
            {
                job_detail_code = s.JobDetailCode,
                tracking_status = s.TrackingStatus,
                tracking_date = s.TrackingDate
            })
            .ToListAsync();
    }

    public async Task<ResponseViewModel> UpdateAdvanceData(FinishJobModel model, TblJobadvance jobAdvance, TblJobcashing jobCashing)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var response = new ResponseViewModel();
            decimal total = 0;
            decimal totalOtherCashing = 0;
            var log = new LogRepository(db);
            var codeService = new CodeRepository(db);
            var transactionCode = codeService.GetBudgetTransactionCode();
            DateTime now = DateTime.Now;

            var order = await db.ViewJobListActives
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == model.allowance_id);

            if (order == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงาน");
            }

            var jobBudget = await (
                from a in db.TblBudgetTransactions.AsNoTracking()
                join b in db.TblJobBudgetTransactions.AsNoTracking()
                    on a.TransactionCode equals b.TransactionCode
                where b.JobDetailCode == model.job_detail_code
                orderby a.TransactionDate descending
                select a)
                .FirstOrDefaultAsync();

            var budget = await db.TblBudgets
                .FirstOrDefaultAsync(w => w.BudgetCode == "01");

            if (budget == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงบประมาณ");
            }

            if (jobBudget == null)
            {
                db.TblBudgetTransactions.Add(new TblBudgetTransaction
                {
                    TransactionCode = transactionCode,
                    TransactionType = "JOB",
                    BudgetCode = "01",
                    TransactionDate = now,
                    Balance = budget.Balance,
                    Amount = model.cash_flow,
                    AmountDiff = 0,
                    PaymentType = "MIN",
                    IsEnabled = true,
                    CreateDate = now,
                    CreateBy = model.username,
                    UpdateDate = now,
                    UpdateBy = model.username
                });

                budget.Balance -= model.cash_flow;

                db.TblJobBudgetTransactions.Add(new TblJobBudgetTransaction
                {
                    JobDetailCode = order.JobDetailCode,
                    TransactionCode = transactionCode
                });

                string missionName = order.MissionName.Replace("&", " ");
                response.notifMessage = "จ่ายเงินสำรองจำนวน : " + model.cash_flow.ToString("#,##0.00") + " บ." + "\n";
                response.notifMessage += "งานเลขที่ : " + model.job_detail_code + "\n";
                response.notifMessage += "ลูกค้า : " + order.CustomerAcronym + "\n";
                response.notifMessage += "สถานที่ : " + order.StartName + " - " + missionName + "\n";
                response.notifMessage += "ยอดคงเหลือ : " + budget.Balance.ToString("#,##0.00") + " บ.";
            }
            else
            {
                var diff = jobBudget.Amount - model.cash_flow;

                db.TblBudgetTransactions.Add(new TblBudgetTransaction
                {
                    TransactionCode = transactionCode,
                    TransactionType = "JOB",
                    BudgetCode = "01",
                    TransactionDate = now,
                    Balance = budget.Balance,
                    Amount = model.cash_flow,
                    AmountDiff = diff,
                    PaymentType = "UPDATE",
                    IsEnabled = true,
                    CreateDate = now,
                    CreateBy = model.username,
                    UpdateDate = now,
                    UpdateBy = model.username
                });

                budget.Balance += diff;

                db.TblJobBudgetTransactions.Add(new TblJobBudgetTransaction
                {
                    JobDetailCode = order.JobDetailCode,
                    TransactionCode = transactionCode
                });

                string missionName = order.MissionName.Replace("&", " ");
                response.notifMessage = "แก้ไขการจ่ายเงินสำรองของงานเลขที่ " + model.job_detail_code + "\n";
                response.notifMessage += "ลูกค้า : " + order.CustomerAcronym + "\n";
                response.notifMessage += "สถานที่ : " + order.StartName + " - " + missionName + "\n";
                response.notifMessage += "จากเดิมจ่าย : " + jobBudget.Amount.ToString("#,##0.00") + " บ." + "\n";
                response.notifMessage += "ยอดจ่ายใหม่ : " + model.cash_flow.ToString("#,##0.00") + " บ." + "\n";
                response.notifMessage += "ยอดคงเหลือ : " + budget.Balance.ToString("#,##0.00") + " บ.";
            }

            var jobAllowance = await db.TblJobAllowances.Where(w => w.Id == model.allowance_id).FirstOrDefaultAsync();
            var jobCashflow = await db.TblJobcashflows.FirstOrDefaultAsync(w => w.JobDetailCode == jobAllowance.JobDetailCode &&
                                                                w.TruckCode == jobAllowance.TruckCode &&
                                                                w.DriverCode == jobAllowance.DriverCode);

            if (jobCashflow != null)
            {
                jobCashflow.CashFlow = model.cash_flow;
            }
            else
            {
                db.TblJobcashflows.Add(new TblJobcashflow
                {
                    JobDetailCode = jobAllowance.JobDetailCode,
                    TruckCode = jobAllowance.TruckCode,
                    DriverCode = jobAllowance.DriverCode,
                    CashFlow = model.cash_flow,
                    AdvanceLifton = 0,
                    AdvanceLiftoff = 0,
                    AdvanceYard = 0,
                    AdvancePort = 0,
                    AdvanceTotal = 0,
                    AdvanceRemark = "-"
                });
            }

            log.AddSystemLog(order.JobDetailCode, "Update advance payment data.", model.username + " (paid)");

            await db.SaveChangesAsync();
            await tr.CommitAsync();

            response.result = true;
            response.resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
            return response;
        }
        catch (Exception ex)
        {
            await tr.RollbackAsync();
            return Error(ex);
        }
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

    public async Task<ResponseViewModel> ChangeDriver(ChangeDriverModel model, string username)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            DateTime now = DateTime.Now;

            var jobDetail = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (jobDetail == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงาน");
            }

            var truck = await (
                from a in db.TblTrucks.AsNoTracking()
                join b in db.TblTruckDetails.AsNoTracking()
                    on a.TruckCode equals b.TruckCode
                where a.TruckCode == model.truck_code
                select new
                {
                    a.SupplierCode,
                    b.TruckTypegroupCode
                })
                .FirstOrDefaultAsync();

            string supplierCode = jobDetail.SupplierCode;
            string driverCode = jobDetail.DriverCode;

            await ChangeDriverDataAsync(model);

            var job = await db.TblJobDetails.FindAsync(model.job_detail_code);
            if (job != null)
            {
                job.SupplierCode = model.new_supplier_code;
                job.DriverCode = model.new_driver_code;
                job.TruckCode = model.new_truck_code;
                job.TrailerLicense = model.trailer_license != null ? model.trailer_license : "-";
                job.UpdateBy = username;
                job.UpdateDate = now;
            }

            log.AddSystemLog(model.job_detail_code, "Change driver from " + driverCode + " to " + model.new_driver_code + ".", username);

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

    public async Task<BudgetViewModel> GetTrBudget()
    {
        var budgetData = await _budgetService.GetTrBudget();
        return budgetData;
    }

    public async Task<TblSupplier> GetTblSupplier(string supplierCode)
    {
        var data = await db.TblSuppliers.Where(w => w.SupplierCode == supplierCode).AsNoTracking().FirstOrDefaultAsync();
        return data;
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
        var jobDetail = await db.TblJobDetails.FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);
        var jobAllowance = await db.TblJobAllowances.FirstOrDefaultAsync(w => w.Id == model.job_allowance_id);
        var jobMaster = await db.TblJobMasters.Where(w => w.JobCode == jobDetail.JobCode).AsNoTracking().FirstOrDefaultAsync();

        var newSupplier = await db.TblSuppliers.Where(w => w.SupplierCode == model.new_supplier_code).FirstOrDefaultAsync();
        var oldSupplier = await db.TblSuppliers.Where(w => w.SupplierCode == model.supplier_code).FirstOrDefaultAsync();

        if (oldSupplier.FleetTypeCode == "F1" && newSupplier.FleetTypeCode == "F1")
        {
            decimal driverAllowance = await GetDriverCostAsync(model.mission_code);
            decimal supplierAllowance = await GetSupplierCostAsync(newSupplier.SupplierCode, jobMaster.TruckTypegroupCode, model.mission_code);
            if (jobAllowance != null)
            {
                jobAllowance.TruckCode = model.new_truck_code;
                jobAllowance.DriverAllowance = driverAllowance;
                jobAllowance.DriverCode = model.new_driver_code;
                jobAllowance.SupplierCode = model.new_supplier_code;
                jobAllowance.SupplierAllowance = supplierAllowance;
            }
            else
            {
                db.TblJobAllowances.Add(new TblJobAllowance
                {
                    JobDetailCode = model.job_detail_code,
                    TruckCode = model.new_truck_code,
                    DriverCode = model.new_driver_code,
                    DriverAllowance = driverAllowance,
                    SupplierCode = model.new_supplier_code,
                    SupplierAllowance = supplierAllowance,
                    IsComplete = false,
                    IsWeeklypayment = false
                });
            }
        }
        else if (oldSupplier.FleetTypeCode != "F1" && newSupplier.FleetTypeCode == "F1")
        {
            decimal driverAllowance = await GetDriverCostAsync(model.mission_code);
            decimal supplierAllowance = await GetSupplierCostAsync(model.new_supplier_code, jobMaster.TruckTypegroupCode, model.mission_code);
            if (jobAllowance != null)
            {
                jobAllowance.TruckCode = model.new_truck_code;
                jobAllowance.DriverAllowance = driverAllowance;
                jobAllowance.DriverCode = model.new_driver_code;
                jobAllowance.SupplierCode = model.new_supplier_code;
                jobAllowance.SupplierAllowance = supplierAllowance;
            }
            else
            {
                db.TblJobAllowances.Add(new TblJobAllowance
                {
                    JobDetailCode = model.job_detail_code,
                    TruckCode = model.new_truck_code,
                    DriverCode = model.new_driver_code,
                    DriverAllowance = driverAllowance,
                    SupplierCode = model.new_supplier_code,
                    SupplierAllowance = supplierAllowance,
                    IsComplete = false,
                    IsWeeklypayment = false
                });
            }
        }
        else
        {
            var supplierJobAllowance = await db.TblSupplierAllowances
                                    .Where(w => w.SupplierCode == model.supplier_code &&
                                            w.JobDetailCode == model.job_detail_code)
                                    .FirstOrDefaultAsync();

            decimal driverAllowance = await GetDriverCostAsync(model.mission_code);
            decimal supplierAllowance = await GetSupplierCostAsync(model.new_supplier_code, jobMaster.TruckTypegroupCode, model.mission_code);
            if (supplierJobAllowance != null)
            {
                jobAllowance.TruckCode = "T000000";
                jobAllowance.DriverAllowance = 0;
                jobAllowance.DriverCode = "26000000";
                supplierJobAllowance.SupplierCode = model.new_supplier_code;
                supplierJobAllowance.SupplierAllowance = supplierAllowance;
            }
            else
            {
                db.TblJobAllowances.Add(new TblJobAllowance
                {
                    JobDetailCode = model.job_detail_code,
                    JobDate = DateOnly.FromDateTime(DateTime.Now),
                    TruckCode = "T000000",
                    DriverCode = "26000000",
                    DriverAllowance = 0,
                    SupplierCode = model.new_supplier_code,
                    SupplierAllowance = await GetSupplierCostAsync(model.new_supplier_code, jobMaster.TruckTypegroupCode, jobDetail.MissionPlaceCode),
                    IsComplete = false,
                    IsWeeklypayment = false
                });
            }
        }
    }

    private async Task ChangeDriverDataAsync(ChangeDriverModel model)
    {
        var oldSupplier = await db.TblSuppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.SupplierCode == model.supplier_code);
        var newSupplier = await db.TblSuppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.SupplierCode == model.new_supplier_code);

        var jobAllowance = await db.TblJobAllowances
            .Where(w => w.JobDetailCode == model.job_detail_code)
            .OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        if (oldSupplier.FleetTypeCode == "F1" && newSupplier.FleetTypeCode == "F1")
        {
            if (jobAllowance != null)
            {
                jobAllowance.DriverAllowance = model.driver_allowance;
                jobAllowance.SupplierAllowance = model.supplier_allowance;
            }

            db.TblJobAllowances.Add(new TblJobAllowance
            {
                JobDetailCode = model.job_detail_code,
                JobDate = DateOnly.FromDateTime(DateTime.Now),
                TruckCode = model.new_truck_code,
                DriverCode = model.new_driver_code,
                DriverAllowance = model.new_supplier_allowance,
                SupplierCode = model.new_supplier_code,
                SupplierAllowance = model.new_supplier_allowance,
                IsComplete = false,
                IsWeeklypayment = false
            });
        }
        else if (oldSupplier.FleetTypeCode != "F1" && newSupplier.FleetTypeCode == "F1")
        {
            decimal driverAllowance = await GetDriverCostAsync(model.mission_code);
            decimal supplierAllowance = await GetSupplierCostAsync(model.new_supplier_code, model.new_truck_code, model.mission_code);

            db.TblJobAllowances.Add(new TblJobAllowance
            {
                JobDetailCode = model.job_detail_code,
                JobDate = DateOnly.FromDateTime(DateTime.Now),
                TruckCode = model.new_truck_code,
                DriverCode = model.new_driver_code,
                DriverAllowance = model.new_supplier_allowance,
                SupplierCode = model.new_supplier_code,
                SupplierAllowance = model.new_supplier_allowance,
                IsComplete = false,
                IsWeeklypayment = false
            });
        }
        else
        {
            decimal driverAllowance = await GetDriverCostAsync(model.mission_code);
            decimal supplierAllowance = await GetSupplierCostAsync(model.new_supplier_code, model.new_truck_code, model.mission_code);
            if (jobAllowance != null)
            {
                jobAllowance.DriverAllowance = model.driver_allowance;
                jobAllowance.SupplierAllowance = model.supplier_allowance;
            }
            else
            {
                db.TblJobAllowances.Add(new TblJobAllowance
                {
                    JobDetailCode = model.job_detail_code,
                    JobDate = DateOnly.FromDateTime(DateTime.Now),
                    TruckCode = "T000000",
                    DriverCode = "26000000",
                    DriverAllowance = 0,
                    SupplierCode = model.new_supplier_code,
                    SupplierAllowance = model.new_supplier_allowance,
                    IsComplete = false,
                    IsWeeklypayment = false
                });
            }
        }
    }

    private async Task<int> GetFuelRateAsync(string placeCode, string? truckType)
    {
        if (string.IsNullOrEmpty(truckType))
        {
            return 0;
        }

        bool hasPlace = await db.TblIndustries
            .AsNoTracking()
            .AnyAsync(w => w.IndustryCode == placeCode);

        if (!hasPlace)
        {
            return 0;
        }

        return await db.TblFuelRates
            .AsNoTracking()
            .Where(w => w.TruckTypegroupCode == truckType)
            .Select(s => (int?)s.FuelRateValue)
            .FirstOrDefaultAsync() ?? 0;
    }

    private async Task<decimal> GetSupplierCostAsync(string? supplierCode, string? truckType, string placeCode)
    {
        if (string.IsNullOrEmpty(supplierCode))
        {
            return 0;
        }

        var supplier = await db.TblSuppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.SupplierCode == supplierCode);

        if (supplier == null)
        {
            return 0;
        }

        if (supplier.FleetTypeCode == "F1")
        {
            var fleet = await (
                from a in db.TblFleetInternals.AsNoTracking()
                join b in db.TblFleetRates.AsNoTracking()
                    on a.FleetCode equals b.FleetCode
                where a.SupplierCode == supplierCode &&
                      b.PlaceCode == placeCode
                select b)
                .FirstOrDefaultAsync();

            if (fleet == null)
            {
                return 0;
            }

            return truckType switch
            {
                "01" => fleet.Truck4w,
                "02" => fleet.Truck6w,
                "03" => fleet.Truck10w,
                _ => 0
            };
        }

        return await db.TblSupplierRates
            .AsNoTracking()
            .Where(w => w.SupplierCode == supplierCode &&
                        w.PlaceCode == placeCode)
            .Select(s => (decimal?)s.CostRate)
            .FirstOrDefaultAsync() ?? 0;
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
