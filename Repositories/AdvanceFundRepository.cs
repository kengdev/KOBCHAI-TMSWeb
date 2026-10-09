using Microsoft.EntityFrameworkCore;
using System.Data;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;

namespace TMSWeb.Repositories;

public class AdvanceFundRepository
{
    private readonly DbTMSContext _db;

    public AdvanceFundRepository(DbTMSContext db)
    {
        _db = db;
    }

    // ============================================================
    // BALANCE
    // ============================================================

    public async Task<decimal> GetBalanceAsync(long fundId)
    {
        return await GetBalanceInternalAsync(fundId);
    }


    // ============================================================
    // PAY
    // จ่ายเงินออกตามเที่ยวงาน
    // ============================================================

    public async Task<AdvanceFundResult> PayAsync(
    string jobDetailCode,
    long cashflowId,
    long fundId,
    decimal amount,
    string driverCode,
    string username)
    {
        if (string.IsNullOrWhiteSpace(jobDetailCode))
        {
            return AdvanceFundResult.Fail(
                "ไม่พบรหัสเที่ยวงาน");
        }

        if (fundId <= 0)
        {
            return AdvanceFundResult.Fail(
                "ไม่พบกองเงินสำรอง");
        }

        if (amount <= 0)
        {
            return AdvanceFundResult.Fail(
                "จำนวนเงินที่จ่ายต้องมากกว่า 0");
        }

        if (string.IsNullOrWhiteSpace(username))
        {
            return AdvanceFundResult.Fail(
                "ไม่พบผู้ทำรายการ");
        }

        jobDetailCode = jobDetailCode.Trim();
        username = username.Trim();

        try
        {
            var driverData = await _db.TblDrivers.AsNoTracking().Where(w => w.DriverCode == driverCode).FirstOrDefaultAsync(); ;

            // =====================================================
            // 3. ตรวจ Expense Type
            // =====================================================

            var expenseType =
                await _db.AdvanceExpenseTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.ExpenseTypeCode == "DRIVER_ADVANCE" &&
                        x.IsActive);

            if (expenseType == null)
            {
                return AdvanceFundResult.Fail(
                    "ไม่พบประเภทค่าใช้จ่าย DRIVER_ADVANCE");
            }


            // =====================================================
            // 4. หัก CurrentBalance แบบ Atomic
            //
            // สำคัญ:
            // ถ้าเงินไม่พอ affected = 0
            // =====================================================

            var affected =
                await _db.AdvanceFunds
                    .Where(x =>
                        x.FundId == fundId &&
                        x.IsActive &&
                        x.CurrentBalance >= amount)
                    .ExecuteUpdateAsync(s =>
                        s.SetProperty(
                            x => x.CurrentBalance,
                            x => x.CurrentBalance - amount));

            if (affected == 0)
            {
                var balance = await GetBalanceAsync(fundId);

                return AdvanceFundResult.Fail(
                    $"เงินสำรองไม่เพียงพอ " +
                    $"คงเหลือ {balance:N2} บาท " +
                    $"ต้องการจ่าย {amount:N2} บาท");
            }


            var now = DateTime.Now;


            // =====================================================
            // 5. สร้าง Advance Request
            // =====================================================

            var request = new AdvanceRequest
            {
                RequestGuid = Guid.NewGuid(),

                RequestCode =
                    $"ADV{now:yyMMddHHmmssfff}",

                FundId = fundId,

                ExpenseTypeId =
                    expenseType.ExpenseTypeId,

                JobDetailCode =
                    jobDetailCode,

                RequestDate = now,

                RequestAmount = amount,

                ApprovedAmount = amount,

                RequestedBy = username,

                Purpose =
                    $"เงินสำรองเที่ยว {jobDetailCode}",

                Status = "PAID",

                PaidBy = username,
                PaidDate = now,

                PayeeCode = driverCode,
                PayeeName = driverData.DriverFullname,

                CreatedBy = username,
                CreatedDate = now,

                UpdatedBy = username,
                UpdatedDate = now
            };

            _db.AdvanceRequests.Add(request);

            // ต้องการ RequestId
            await _db.SaveChangesAsync();


            // =====================================================
            // 6. สร้าง PAYMENT Transaction
            // =====================================================

            var payment =
                new AdvanceFundTransaction
                {
                    TransactionGuid = Guid.NewGuid(),

                    FundId = fundId,

                    RequestId = request.RequestId,

                    TransactionDate = now,

                    TransactionType = "PAYMENT",

                    Amount = amount,

                    ReferenceTransactionId = null,

                    ReferenceNo = jobDetailCode,

                    CashflowId = cashflowId,

                    Remark =
                        $"จ่ายเงินสำรองเที่ยว {jobDetailCode}",

                    CreatedBy = username,
                    CreatedDate = now
                };

            _db.AdvanceFundTransactions.Add(payment);

            await _db.SaveChangesAsync();


            // =====================================================
            // 7. อ่านยอดล่าสุด
            // =====================================================

            var currentBalance =
                await GetBalanceAsync(fundId);


            return AdvanceFundResult.Ok(
                $"จ่ายเงินเที่ยว {jobDetailCode} " +
                $"{amount:N2} บาทเรียบร้อย",
                payment.TransactionId,
                currentBalance);
        }
        catch (DbUpdateException)
        {
            return AdvanceFundResult.Fail(
                $"ไม่สามารถจ่ายเงินเที่ยว {jobDetailCode} ได้ " +
                "รายการอาจถูกสร้างหรือจ่ายเงินไปแล้ว");
        }
    }


    // ============================================================
    // RETURN
    // รับเงินคืนจากเที่ยวงาน
    // ============================================================

    public async Task<AdvanceFundResult> ReturnAsync(
        string jobDetailCode,
        long cashflowId,
        decimal returnAmount,
        string username,
        string driverCode,
        string? remark = null,
        bool cancelTrip = false)
    {
        if (string.IsNullOrWhiteSpace(jobDetailCode))
        {
            return AdvanceFundResult.Fail(
                "ไม่พบรหัสเที่ยวงาน");
        }

        if (returnAmount <= 0)
        {
            return AdvanceFundResult.Fail(
                "จำนวนเงินคืนต้องมากกว่า 0");
        }

        jobDetailCode = jobDetailCode.Trim();

        try
        {
            var driverData = await _db.TblDrivers.AsNoTracking().Where(w => w.DriverCode == driverCode).FirstOrDefaultAsync();

            // ---------------------------------------------
            // หา PAYMENT ต้นทาง
            // ---------------------------------------------

            var payment =
                await _db.AdvanceFundTransactions
                    .FirstOrDefaultAsync(x =>
                        x.CashflowId == cashflowId &&
                        x.TransactionType == "PAYMENT");

            if (payment == null)
            {
                return AdvanceFundResult.Fail(
                    $"เที่ยว {jobDetailCode} ยังไม่มีรายการจ่ายเงิน");
            }

            // ---------------------------------------------
            // หา Advance Request ของเที่ยว
            // ---------------------------------------------

            var request = await _db.AdvanceRequests
                .FirstOrDefaultAsync(x =>
                    x.RequestId == payment.RequestId);

            if (request == null)
            {
                return AdvanceFundResult.Fail(
                    $"ไม่พบรายการเงินสำรองของเที่ยว {jobDetailCode}");
            }

            // ---------------------------------------------
            // รวมยอดที่คืนไปแล้ว
            // ---------------------------------------------

            /*var alreadyReturned =
                await _db.AdvanceFundTransactions
                    .Where(x =>
                        x.ReferenceTransactionId ==
                            payment.TransactionId &&
                        x.TransactionType == "RETURN")
                    .Select(x => (decimal?)x.Amount)
                    .SumAsync()
                ?? 0m;

            var remainingAmount =
                payment.Amount - alreadyReturned;

            if (remainingAmount <= 0)
            {
                return AdvanceFundResult.Fail(
                    $"เที่ยว {jobDetailCode} คืนเงินครบแล้ว");
            }

            if (returnAmount > remainingAmount)
            {
                return AdvanceFundResult.Fail(
                    $"รับคืนเกินยอดคงค้าง " +
                    $"ยอดที่ยังรับคืนได้ {remainingAmount:N2} บาท");
            }*/

            var now = DateTime.Now;

            // ---------------------------------------------
            // สร้าง RETURN
            // ต้องคืนเข้ากองต้นทางเท่านั้น
            // ---------------------------------------------

            var returnTransaction =
                new AdvanceFundTransaction
                {
                    TransactionGuid = Guid.NewGuid(),

                    FundId = payment.FundId,

                    RequestId = request.RequestId,

                    TransactionDate = now,

                    TransactionType = "RETURN",

                    Amount = returnAmount,

                    // ชี้กลับ PAYMENT ต้นทาง
                    ReferenceTransactionId =
                        payment.TransactionId,

                    ReferenceNo = jobDetailCode,
                    CashflowId = cashflowId,

                    Remark =
                        !string.IsNullOrWhiteSpace(remark)
                            ? remark
                            : $"รับคืนเงินเที่ยว {jobDetailCode}",

                    CreatedBy = username,

                    CreatedDate = now,
                };

            _db.AdvanceFundTransactions.Add(returnTransaction);

            /*var totalReturned =
                alreadyReturned + returnAmount;*/

            // ---------------------------------------------
            // Status
            // ---------------------------------------------

            /*if (cancelTrip)
            {
                if (totalReturned >= payment.Amount)
                {
                    // เที่ยวถูกยกเลิกและได้เงินคืนครบแล้ว
                    request.Status = "CANCELLED";

                    request.CancelledBy = username;
                    request.CancelledDate = now;

                    if (!string.IsNullOrWhiteSpace(remark))
                    {
                        request.CancelledReason = remark;
                    }
                }
                else
                {
                    // เที่ยวยกเลิก แต่เงินยังคืนไม่ครบ
                    request.Status = "WAITING_RETURN";
                }
            }
            else
            {
                /*
                 * กรณีไม่ใช่ Cancel
                 *
                 * เช่น จ่าย 5,000
                 * ใช้จริง 3,700
                 * คืน 1,300
                 *
                 * Flow การ SETTLED เราจะจัดการตอนทำ
                 * Settlement module ต่อไป
                 *
                if (totalReturned >= payment.Amount)
                {
                    request.Status = "SETTLED";
                }
            }*/

            request.PayeeCode = driverCode;
            request.PayeeName = driverData.DriverFullname;
            request.UpdatedBy = username;
            request.UpdatedDate = now;

            var fund = await _db.AdvanceFunds.FirstOrDefaultAsync(x => x.FundId == payment.FundId);

            if (fund == null)
            {
                return AdvanceFundResult.Fail("ไม่พบกองเงินต้นทาง");
            }

            fund.CurrentBalance += returnAmount;
            fund.UpdatedBy = username;
            fund.UpdatedDate = now;

            await _db.SaveChangesAsync();

            var balance =
                await GetBalanceInternalAsync(
                    payment.FundId);

            return AdvanceFundResult.Ok(
                $"รับคืนเงินเที่ยว {jobDetailCode} " +
                $"{returnAmount:N2} บาทเรียบร้อย",
                returnTransaction.TransactionId,
                balance);
        }
        catch
        {
            throw;
        }
    }


    // ============================================================
    // CANCEL TRIP AFTER PAYMENT
    //
    // ใช้ตอน "ยกเลิกเที่ยว" แต่คนขับยังไม่ได้คืนเงินจริง
    // ตรงนี้ห้ามเพิ่มเงินกลับกอง
    // ============================================================

    public async Task<AdvanceFundResult> MarkWaitingReturnAsync(
        string jobDetailCode,
        string username,
        string? reason = null)
    {
        if (string.IsNullOrWhiteSpace(jobDetailCode))
        {
            return AdvanceFundResult.Fail(
                "ไม่พบรหัสเที่ยวงาน");
        }

        jobDetailCode = jobDetailCode.Trim();

        var request = await _db.AdvanceRequests
            .FirstOrDefaultAsync(x =>
                x.JobDetailCode == jobDetailCode);

        if (request == null)
        {
            return AdvanceFundResult.Fail(
                $"ไม่พบรายการเงินสำรองของเที่ยว {jobDetailCode}");
        }

        var payment =
            await _db.AdvanceFundTransactions
                .AnyAsync(x =>
                    x.RequestId == request.RequestId &&
                    x.TransactionType == "PAYMENT");

        /*
         * ยังไม่เคยจ่ายเงินจริง
         * Cancel ได้เลย ไม่มีเงินต้องคืน
         */
        if (!payment)
        {
            request.Status = "CANCELLED";

            request.CancelledBy = username;
            request.CancelledDate = DateTime.Now;
            request.CancelledReason = reason;

            request.UpdatedBy = username;
            request.UpdatedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return AdvanceFundResult.Ok(
                $"ยกเลิกเที่ยว {jobDetailCode} เรียบร้อย");
        }

        /*
         * จ่ายเงินจริงแล้ว
         * ห้ามคืนยอดเข้ากองจนกว่าจะรับเงินจริงกลับมา
         */
        request.Status = "WAITING_RETURN";

        request.CancelledReason = reason;

        request.UpdatedBy = username;
        request.UpdatedDate = DateTime.Now;

        await _db.SaveChangesAsync();

        return AdvanceFundResult.Ok(
            $"เที่ยว {jobDetailCode} ถูกยกเลิก " +
            "และกำลังรอรับเงินคืน");
    }


    // ============================================================
    // GET REMAINING RETURN
    // เอาไว้แสดงบนหน้าจอรับคืนเงิน
    // ============================================================

    public async Task<decimal> GetRemainingReturnAsync(
        string jobDetailCode)
    {
        var request = await _db.AdvanceRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.JobDetailCode == jobDetailCode);

        if (request == null)
        {
            return 0m;
        }

        var payment =
            await _db.AdvanceFundTransactions
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.RequestId == request.RequestId &&
                    x.TransactionType == "PAYMENT");

        if (payment == null)
        {
            return 0m;
        }

        var returned =
            await _db.AdvanceFundTransactions
                .AsNoTracking()
                .Where(x =>
                    x.ReferenceTransactionId ==
                        payment.TransactionId &&
                    x.TransactionType == "RETURN")
                .Select(x => (decimal?)x.Amount)
                .SumAsync()
            ?? 0m;

        return Math.Max(
            payment.Amount - returned,
            0m);
    }


    // ============================================================
    // INTERNAL BALANCE
    // ============================================================

    private async Task<decimal> GetBalanceInternalAsync(
        long fundId)
    {
        var balance =
            await _db.AdvanceFundTransactions
                .Where(x => x.FundId == fundId)
                .Select(x =>

                    x.TransactionType == "TOPUP" ||
                    x.TransactionType == "RETURN" ||
                    x.TransactionType == "ADJUST_IN"
                        ? (decimal?)x.Amount

                    : x.TransactionType == "PAYMENT" ||
                      x.TransactionType == "ADJUST_OUT"
                        ? (decimal?)-x.Amount

                    : 0m
                )
                .SumAsync();

        return balance ?? 0m;
    }
}