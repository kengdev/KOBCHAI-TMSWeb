using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class BudgetRepository
{
    private readonly DbTMSContext db;

    public BudgetRepository(DbTMSContext context)
    {
        db = context;
    }

    #region TransportBudget

    public async Task<BudgetViewModel> GetTrBudget()
    {
        var budget = await GetTrBudgetData();

        var model = new BudgetViewModel();

        if (budget != null)
        {
            model.balance = budget.Balance;
            model.update_date = budget.UpdateDate;
        }

        model.transport_transactions = await db.ViewTransportBudgetTransactions
            .AsNoTracking()
            .Where(w => w.BudgetType == "TR")
            .OrderByDescending(o => o.CreateDate)
            .Take(5)
            .Select(s => new TransportBudgetTransactionViewModel
            {
                amount = s.Amount,
                amount_diff = s.AmountDiff,
                transaction_date = s.TransactionDate.ToString("dd-MM-yyyy"),
                create_date = s.CreateDate,
                job_detail_code = s.JobDetailCode,
                customer_name = s.CustomerAcronym,
                start = s.Start,
                mission = s.Mission,
                transaction_type = s.TransactionType,
                payment_type = s.PaymentType,
                remark = s.Remark
            })
            .ToListAsync();

        return model;
    }

    public async Task<ViewTransportBudgetTransaction?> GetBudgetTransactionDataByCode(string code)
    {
        return await db.ViewTransportBudgetTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.TransactionCode == code);
    }

    public async Task<TblBudget?> GetTrBudgetData()
    {
        return await db.TblBudgets
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.BudgetCode == "01");
    }

    public async Task<List<LogBudgetTransaction>> GetBudgetTransactionDetail(string code)
    {
        return await db.LogBudgetTransactions
            .AsNoTracking()
            .Where(w => w.TransactionCode == code)
            .OrderBy(o => o.PaymentType)
            .ToListAsync();
    }

    public async Task<ResponseViewModel> UpdateBalance(
        decimal balance,
        DateTime budget_date,
        string remark,
        string username)
    {
        return await AddBudgetTransactionAsync(
            budgetCode: "01",
            balance: balance,
            budgetDate: budget_date,
            remark: remark,
            username: username,
            transactionType: "GET",
            paymentType: "ADD",
            isIncrease: true,
            notificationPrefix: "รับยอดจำนวน "
        );
    }

    public async Task<ResponseViewModel> AddPayment(
        decimal balance,
        DateTime budget_date,
        string remark,
        string username)
    {
        return await AddBudgetTransactionAsync(
            budgetCode: "01",
            balance: balance,
            budgetDate: budget_date,
            remark: remark,
            username: username,
            transactionType: "OTHER",
            paymentType: "MIN",
            isIncrease: false,
            notificationPrefix: "จ่าย" + remark + " จำนวน "
        );
    }

    public async Task<List<TransportBudgetTransactionViewModel>> GetTransportBudgetTransaction(
        DateTime start,
        DateTime end)
    {
        return await db.ViewTransportBudgetTransactions
            .AsNoTracking()
            .Where(w => w.TransactionDate >= start &&
                        w.TransactionDate <= end)
            .OrderByDescending(o => o.CreateDate)
            .Select(s => new TransportBudgetTransactionViewModel
            {
                transaction_code = s.TransactionCode,
                transaction_date = s.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                job_detail_code = s.JobDetailCode,
                booking = s.BookingNo,
                container = s.ContainerNo,
                customer_name = s.CustomerAcronym,
                start = s.Start,
                mission = s.Mission,
                transaction_type = s.TransactionType,
                payment_type = s.PaymentType,
                job_type = s.JobType,
                amount = s.Amount,
                amount_diff = s.AmountDiff,
                remark = s.Remark
            })
            .ToListAsync();
    }

    #endregion

    #region GeneralBudget

    public async Task<BudgetViewModel> GetGeBudget()
    {
        var budget = await GetGeBudgetData();

        var model = new BudgetViewModel();

        if (budget != null)
        {
            model.balance = budget.Balance;
            model.update_date = budget.UpdateDate;
        }

        model.general_transactions = await db.ViewGeneralBudgetTransactions
            .AsNoTracking()
            .Where(w => w.BudgetType == "GE")
            .OrderByDescending(o => o.TransactionCode)
            .Take(5)
            .Select(s => new GeneralBudgetTransactionViewModel
            {
                dailypayment_code = s.DailypaymentCode,
                amount = s.Amount,
                amount_diff = s.AmountDiff,
                transaction_date = s.TransactionDate.ToString("dd-MM-yyyy"),
                create_date = s.CreateDate,
                transaction_by = s.CreateBy,
                payment_type = s.PaymentType,
                remark = s.Remark
            })
            .OrderByDescending(o => o.create_date)
            .ToListAsync();

        return model;
    }

    public async Task<TblBudget?> GetGeBudgetData()
    {
        return await db.TblBudgets
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.BudgetCode == "02");
    }

    public async Task<ResponseViewModel> UpdateGeneralBalance(
        decimal balance,
        DateTime budget_date,
        string remark,
        string username)
    {
        return await AddBudgetTransactionAsync(
            budgetCode: "02",
            balance: balance,
            budgetDate: budget_date,
            remark: remark,
            username: username,
            transactionType: "GET",
            paymentType: "ADD",
            isIncrease: true,
            notificationPrefix: "รับยอดจำนวน "
        );
    }

    public async Task<ResponseViewModel> AddGeneralPayment(
        decimal balance,
        DateTime budget_date,
        string remark,
        string username)
    {
        return await AddBudgetTransactionAsync(
            budgetCode: "02",
            balance: balance,
            budgetDate: budget_date,
            remark: remark,
            username: username,
            transactionType: "OTHER",
            paymentType: "MIN",
            isIncrease: false,
            notificationPrefix: "จ่าย" + remark + " จำนวน "
        );
    }

    #endregion

    private async Task<ResponseViewModel> AddBudgetTransactionAsync(
        string budgetCode,
        decimal balance,
        DateTime budgetDate,
        string remark,
        string username,
        string transactionType,
        string paymentType,
        bool isIncrease,
        string notificationPrefix)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var budget = await db.TblBudgets
                .FirstOrDefaultAsync(w => w.BudgetCode == budgetCode);

            if (budget == null)
            {
                await tr.RollbackAsync();
                return NotFoundBudget();
            }

            var codeService = new CodeRepository(db);
            DateTime now = DateTime.Now;
            decimal currentBalance = budget.Balance;

            budget.Balance = isIncrease
                ? budget.Balance + balance
                : budget.Balance - balance;

            budget.UpdateDate = now;
            budget.UpdateBy = username;

            var transaction = new TblBudgetTransaction
            {
                TransactionCode = codeService.GetBudgetTransactionCode(),
                TransactionDate = budgetDate,
                TransactionType = transactionType,
                Balance = currentBalance,
                Amount = balance,
                AmountDiff = 0,
                PaymentType = paymentType,
                Remark = remark,
                BudgetCode = budgetCode,
                IsEnabled = true,
                CreateDate = now,
                CreateBy = username,
                UpdateDate = now,
                UpdateBy = username
            };

            db.TblBudgetTransactions.Add(transaction);

            await db.SaveChangesAsync();
            await tr.CommitAsync();

            return Success(
                notificationPrefix +
                balance.ToString("#,##0.00") +
                " บ." +
                "\n" +
                (isIncrease ? "รับยอด ณ. วันที่ " : "ณ. วันที่ ") +
                budgetDate.ToString("dd-MM-yyyy") +
                "\n" +
                "ยอดคงเหลือ : " +
                budget.Balance.ToString("#,##0.00") +
                " บ."
            );
        }
        catch (Exception ex)
        {
            await tr.RollbackAsync();
            return Error(ex);
        }
    }

    private static ResponseViewModel Success(string notificationMessage)
    {
        return new ResponseViewModel
        {
            result = true,
            notifMessage = notificationMessage,
            resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว"
        };
    }

    private static ResponseViewModel NotFoundBudget()
    {
        return new ResponseViewModel
        {
            result = false,
            resultMessage = "ไม่พบข้อมูลงบประมาณ"
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
