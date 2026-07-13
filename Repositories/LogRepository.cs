using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;

namespace TMSWeb.Repositories;
public class LogRepository
{
    private DbTMSContext db;

    public LogRepository(DbTMSContext context)
    {
        db = context;
    }

    public void AddSystemLog(string code, string action, string username)
    {
        LogSystem log = new LogSystem();
        log.LogReferenceCode = code;
        log.LogAction = action;
        log.CreateDate = DateTime.Now;
        log.CreateBy = username;

        db.LogSystems.Add(log);
    }

    public void AddCoverSheetDetailLog(TblCoversheetDetail model, string username, string action)
    {
        LogCoversheetDetail log = new LogCoversheetDetail();
        log.BillingCode = model.BillingCode;
        log.CoversheetValue = model.CoversheetValue;
        log.CoversheetCode = model.CoversheetCode;
        log.Action = action;
        log.CreateDate = DateTime.Now;
        log.CreateBy = username;

        db.LogCoversheetDetails.Add(log);
    }

    public void AddApproveLog(string job_detail_code, string message, decimal customer_cost, decimal supplier_cost, string username)
    {
        LogApproved log = new LogApproved();
        log.JobDetailCode = job_detail_code;
        log.Message = message;
        log.CustomerCost =  customer_cost;
        log.SupplierCost = supplier_cost;
        log.CreateDate = DateTime.Now;
        log.CreateBy = username;
        
        db.LogApproveds.Add(log);
    }

    public void AddDriverAllowanceLog(TblAllowanceDetail model, string username, string action)
    {
        LogAllowanceDetail log = new LogAllowanceDetail();
        log.AllowanceDate = model.AllowanceDate;
        log.TripValue = model.TripValue;
        log.FuelValue = model.FuelValue;
        log.WasteValue = model.WasteValue;
        log.OtherValue = model.OtherValue;
        log.JobDetailCode = model.JobDetailCode;
        log.AllowanceCode = model.AllowanceCode;
        log.Action = action;
        log.CreateDate = DateTime.Now;
        log.CreateBy = username;

        db.LogAllowanceDetails.Add(log);
    }

    public void AddReceiptLog(TblReceiptDetail model, string username, string action)
    {
        LogReceiptDetail log = new LogReceiptDetail();
        log.BillingCode = model.BillingCode;
        log.ReceiptAdvanceValue = model.ReceiptAdvanceValue;
        log.ReceiptTransportValue = model.ReceiptTransportValue;
        log.ReceiptValue = model.ReceiptValue;
        log.ReceiptCode = model.ReceiptCode;
        log.CreateDate = DateTime.Now;
        log.CreateBy = username;
        log.Action = action;

        db.LogReceiptDetails.Add(log);
    }

    public void AddSupplierPaymentLog(TblSupplierpaymentDetail model, string username, string action)
    {
        LogSupplyerpaymentDetail log = new LogSupplyerpaymentDetail();
        log.JobDetailCode = model.JobDetailCode;
        log.SupplyerpaymentValue = model.SupplierpaymentValue;
        log.SupplyerpaymentCode = model.SupplierpaymentCode;
        log.CreateDate = DateTime.Now;
        log.CreateBy = username;
        log.Action = action;

        db.LogSupplyerpaymentDetails.Add(log);
    }

    public void AddBillingDetailLog(TblBillingDetail model, string username, string action)
    {
        LogBillingDetail log = new LogBillingDetail();
        log.JobDetailCode = model.JobDetailCode;
        log.BillingValue = model.BillingValue;
        log.AdvanceValue = model.AdvanceValue;
        log.BillingCode = model.BillingCode;
        log.Action = action;
        log.CreateDate = DateTime.Now;
        log.CreateBy = username;
        db.LogBillingDetails.Add(log);
    }

    public void AddBillingOtherLog(TblBillingOther model, string username, string action)
    {
        LogBillingOther log = new LogBillingOther();
        log.AdvanceCode = model.AdvanceCode;
        log.AdvanceId = model.AdvanceId;
        log.BillingValue = model.BillingValue;
        log.BillingCode = model.BillingCode;
        log.Action = action;
        log.CreateDate = DateTime.Now;
        log.CreateBy = username;
        db.LogBillingOthers.Add(log);
    }

    public void AddBudgetTransactionLog(string job_detail_code, string transaction_code, string transaction_name, string type, decimal amount, string username)
    {
        LogBudgetTransaction log = new LogBudgetTransaction();
        log.JobDetailCode = job_detail_code;
        log.TransactionCode = transaction_code;
        log.TransactionName = transaction_name;
        log.PaymentType = type;
        log.Amount = amount;
        log.CreateDate = DateTime.Now;
        log.CreateBy = username;
        db.LogBudgetTransactions.Add(log);
    }
}