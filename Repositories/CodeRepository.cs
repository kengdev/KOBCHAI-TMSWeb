using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using Microsoft.EntityFrameworkCore;

namespace TMSWeb.Repositories;

public class CodeRepository
{
    private static readonly CultureInfo EnCulture = new("en-US", false);
    private readonly DbTMSContext db;

    public CodeRepository(DbTMSContext context)
    {
        db = context;
    }

    private static string DateText(DateTime date, string format)
    {
        return date.ToString(format, EnCulture);
    }

    private static string FormatRunning(int runningNo, int runningLength)
    {
        return runningNo.ToString("D" + runningLength, EnCulture);
    }

    private static int ParseRunning(string runningText)
    {
        return int.TryParse(runningText, NumberStyles.None, EnCulture, out int runningNo)
            ? runningNo
            : 0;
    }

    private static string FirstCode(string prefix, int runningLength)
    {
        return prefix + FormatRunning(1, runningLength);
    }

    private static string NextCodeByPrefix(string? lastCode, string prefix, int runningLength)
    {
        if (string.IsNullOrWhiteSpace(lastCode) ||
            !lastCode.StartsWith(prefix) ||
            lastCode.Length < prefix.Length + runningLength)
        {
            return FirstCode(prefix, runningLength);
        }

        string runningText = lastCode.Substring(prefix.Length, runningLength);
        int runningNo = ParseRunning(runningText) + 1;

        return prefix + FormatRunning(runningNo, runningLength);
    }

    private static string NextCodeFromSegment(string? lastCode, string outputPrefix, int runningStartIndex, int runningLength)
    {
        if (string.IsNullOrWhiteSpace(lastCode) ||
            lastCode.Length < runningStartIndex + runningLength)
        {
            return FirstCode(outputPrefix, runningLength);
        }

        string runningText = lastCode.Substring(runningStartIndex, runningLength);
        int runningNo = ParseRunning(runningText) + 1;

        return outputPrefix + FormatRunning(runningNo, runningLength);
    }

    private static string NextCodeByYear(string? lastCode, string outputPrefix, string currentYear, int yearStartIndex, int runningStartIndex, int runningLength)
    {
        if (string.IsNullOrWhiteSpace(lastCode) ||
            lastCode.Length < runningStartIndex + runningLength ||
            lastCode.Length < yearStartIndex + currentYear.Length)
        {
            return FirstCode(outputPrefix, runningLength);
        }

        string yearInLastCode = lastCode.Substring(yearStartIndex, currentYear.Length);

        if (yearInLastCode != currentYear)
        {
            return FirstCode(outputPrefix, runningLength);
        }

        return NextCodeFromSegment(lastCode, outputPrefix, runningStartIndex, runningLength);
    }

    private string SaveCodeGenerate(string code, string codeType)
    {
        db.TblCodeGenerates.Add(new TblCodeGenerate
        {
            CodeName = code,
            CodeType = codeType,
            CreateDate = DateTime.Now
        });

        db.SaveChanges();

        return code;
    }

    public async Task<string> GetSupplierCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "SP" + DateText(now, "yy");

        string? lastCode = await db.TblSuppliers
            .AsNoTracking()
            .Where(w => w.SupplierCode != null && w.SupplierCode.StartsWith(prefix))
            .Select(s => s.SupplierCode)
            .OrderByDescending(code => code)
            .FirstOrDefaultAsync();

        return NextCodeByPrefix(lastCode, prefix, 3);
    }

    public string GetQuotationCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "QT" + DateText(now, "yy") + "AVA";

        string? lastCode = db.TblQuotations
            .AsNoTracking()
            .Where(w => w.QuotationCode != null && w.QuotationCode.StartsWith(prefix))
            .Select(s => s.QuotationCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 4);
    }

    public string GetBudgetTransactionCode()
    {
        DateTime now = DateTime.Now;
        string prefix = DateText(now, "yyMM");

        string? lastCode = db.TblBudgetTransactions
            .AsNoTracking()
            .Where(w => w.TransactionCode != null && w.TransactionCode.StartsWith(prefix))
            .Select(s => s.TransactionCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 4);
    }

    /*
    public string GetRouteCode()
    {
        DateTime now = DateTime.Now;
        string prefix = DateText(now, "yyMM");

        string? lastCode = db.TblRoutes
            .AsNoTracking()
            .Where(w => w.RouteCode != null && w.RouteCode.StartsWith(prefix))
            .Select(s => s.RouteCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 4);
    }
    */

    public string GetJobCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "AVA" + DateText(now, "yyMM");

        string? lastCode = db.TblCodeGenerates
            .AsNoTracking()
            .Where(w => w.CodeType == "job_master" &&
                        w.CodeName != null &&
                        w.CodeName.StartsWith(prefix))
            .Select(s => s.CodeName)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        string code = NextCodeByPrefix(lastCode, prefix, 5);

        return SaveCodeGenerate(code, "job_master");
    }

    public string GetDailyPaymentCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "AVA" + DateText(now, "yyMM") + "D";

        string? lastCode = db.TblDailypayments
            .AsNoTracking()
            .Where(w => w.DailypaymentCode != null && w.DailypaymentCode.StartsWith(prefix))
            .Select(s => s.DailypaymentCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 3);
    }

    public string GetMonthlyPaymentCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "AVA" + DateText(now, "yyMM") + "M";

        string? lastCode = db.TblMonthlypayments
            .AsNoTracking()
            .Where(w => w.MonthlyPaymentCode != null && w.MonthlyPaymentCode.StartsWith(prefix))
            .Select(s => s.MonthlyPaymentCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 3);
    }

    public string GetReceivePaymentCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "AVA" + DateText(now, "yyMM") + "I";

        string? lastCode = db.TblReceivePayments
            .AsNoTracking()
            .Where(w => w.ReceivePaymentCode != null && w.ReceivePaymentCode.StartsWith(prefix))
            .Select(s => s.ReceivePaymentCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 3);
    }

    public string GetJobDetailCode(string customer_acronym)
    {
        DateTime now = DateTime.Now;
        string prefixYear = DateText(now, "yy");
        string prefix = customer_acronym + DateText(now, "yy/MM") + "-";

        string? lastCode = db.TblCodeGenerates
            .AsNoTracking()
            .Where(w => w.CodeType == "job_detail")
            .OrderByDescending(o => o.Id)
            .Select(s => s.CodeName)
            .FirstOrDefault();

        string code = NextCodeByYear(
            lastCode: lastCode,
            outputPrefix: prefix,
            currentYear: prefixYear,
            yearStartIndex: 3,
            runningStartIndex: 9,
            runningLength: 5);

        return SaveCodeGenerate(code, "job_detail");
    }

    public async Task<string> GetCustomerCode(string customer_type_code)
    {
        DateTime now = DateTime.Now;
        string prefix = "C" + DateText(now, "yy");
        string suffix = "T" + customer_type_code;
        string codePrefix = prefix;
        string fullPrefixForSearch = prefix;
        string fullSuffixForSearch = suffix;

        string? lastCode = await db.TblCustomers
            .AsNoTracking()
            .Where(w => w.CustomerTypeCode == customer_type_code &&
                        w.CustomerCode != null &&
                        w.CustomerCode.StartsWith(fullPrefixForSearch) &&
                        w.CustomerCode.EndsWith(fullSuffixForSearch))
            .Select(s => s.CustomerCode)
            .OrderByDescending(code => code)
            .FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(lastCode) ||
            lastCode.Length < 9 ||
            !lastCode.StartsWith(prefix) ||
            !lastCode.EndsWith(suffix))
        {
            return prefix + "001" + suffix;
        }

        string runningText = lastCode.Substring(3, 3);
        int runningNo = ParseRunning(runningText) + 1;

        return codePrefix + FormatRunning(runningNo, 3) + suffix;
    }

    public async Task<string> GetPlaceCode(string sy)
    {
        DateTime now = DateTime.Now;
        string prefix = sy + "-" + DateText(now, "yy");

        string? lastCode = await db.TblIndustries
            .AsNoTracking()
            .Where(w => w.IndustryCode != null && w.IndustryCode.StartsWith(prefix))
            .Select(s => s.IndustryCode)
            .OrderByDescending(code => code)
            .FirstOrDefaultAsync();

        return NextCodeByPrefix(lastCode, prefix, 3);
    }

    public string GetJobAdvanceCode()
    {
        DateTime now = DateTime.Now;
        string prefixYear = DateText(now, "yy");
        string yearPrefix = "AVA" + prefixYear;
        string outputPrefix = "AVA" + DateText(now, "yyMM") + "A-";

        string? lastCode = db.TblCodeGenerates
            .AsNoTracking()
            .Where(w => w.CodeType == "job_advance" &&
                        w.CodeName != null &&
                        w.CodeName.StartsWith(yearPrefix))
            .Select(s => s.CodeName)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        string code = NextCodeByYear(
            lastCode: lastCode,
            outputPrefix: outputPrefix,
            currentYear: prefixYear,
            yearStartIndex: 3,
            runningStartIndex: 9,
            runningLength: 5);

        return SaveCodeGenerate(code, "job_advance");
    }

    public string GetJobCashingCode()
    {
        DateTime now = DateTime.Now;
        string prefixYear = DateText(now, "yy");
        string yearPrefix = "AVA" + prefixYear;
        string outputPrefix = "AVA" + DateText(now, "yyMM") + "C-";

        string? lastCode = db.TblCodeGenerates
            .AsNoTracking()
            .Where(w => w.CodeType == "job_cashing" &&
                        w.CodeName != null &&
                        w.CodeName.StartsWith(yearPrefix))
            .Select(s => s.CodeName)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        string code = NextCodeByYear(
            lastCode: lastCode,
            outputPrefix: outputPrefix,
            currentYear: prefixYear,
            yearStartIndex: 3,
            runningStartIndex: 9,
            runningLength: 5);

        return SaveCodeGenerate(code, "job_cashing");
    }

    public async Task<string> GetTruckCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "T" + DateText(now, "yy");

        string? lastCode = await db.TblTrucks
            .AsNoTracking()
            .Where(w => w.TruckCode != null && w.TruckCode.StartsWith(prefix))
            .Select(s => s.TruckCode)
            .OrderByDescending(code => code)
            .FirstOrDefaultAsync();

        return NextCodeByPrefix(lastCode, prefix, 4);
    }

    public string GetTrailerCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "R" + DateText(now, "yy");

        string? lastCode = db.TblTrailers
            .AsNoTracking()
            .Where(w => w.TrailerCode != null && w.TrailerCode.StartsWith(prefix))
            .Select(s => s.TrailerCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 4);
    }

    public string GetCostCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "COS" + DateText(now, "yy");

        string? lastCode = db.TblCustomerCosts
            .AsNoTracking()
            .Where(w => w.CostCode != null && w.CostCode.StartsWith(prefix))
            .Select(s => s.CostCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 4);
    }

    public string GetBillingCode()
    {
        DateTime now = DateTime.Now;
        string yearPrefix = "AVA" + DateText(now, "yy") + "/";
        string outputPrefix = yearPrefix + DateText(now, "MM") + "-";

        string? lastCode = db.TblCodeGenerates
            .AsNoTracking()
            .Where(w => w.CodeType == "billing" &&
                        w.CodeName != null &&
                        w.CodeName.StartsWith(yearPrefix))
            .Select(s => s.CodeName)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        string code = NextCodeFromSegment(lastCode, outputPrefix, 9, 5);

        return SaveCodeGenerate(code, "billing");
    }

    public string GetSupplyerPaymentCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "AVA" + DateText(now, "yy/MM") + "SP";

        string? lastCode = db.TblSupplierpayments
            .AsNoTracking()
            .Where(w => w.SupplierpaymentCode != null && w.SupplierpaymentCode.StartsWith(prefix))
            .Select(s => s.SupplierpaymentCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 4);
    }

    public string GetFleetPaymentCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "AVA" + DateText(now, "yy/MM") + "FL";

        string? lastCode = db.TblFleetpayments
            .AsNoTracking()
            .Where(w => w.FleetpaymentCode != null && w.FleetpaymentCode.StartsWith(prefix))
            .Select(s => s.FleetpaymentCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 4);
    }

    public string GetCoverSheetCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "AVA/Cov" + DateText(now, "yy") + "-";

        string? lastCode = db.TblCoversheets
            .AsNoTracking()
            .Where(w => w.CoversheetCode != null && w.CoversheetCode.StartsWith(prefix))
            .Select(s => s.CoversheetCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 5);
    }

    public string GetReceiptCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "AVA" + DateText(now, "yy");

        string? lastCode = db.TblReceipts
            .AsNoTracking()
            .Where(w => w.ReceiptCode != null && w.ReceiptCode.StartsWith(prefix))
            .Select(s => s.ReceiptCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 5);
    }

    public string GetOtherReceipt()
    {
        DateTime now = DateTime.Now;
        string prefix = "R" + DateText(now, "yy/MM") + "-";

        string? lastCode = db.TblReceiptOthers
            .AsNoTracking()
            .Where(w => w.ReceiptOtherCode != null && w.ReceiptOtherCode.StartsWith(prefix))
            .Select(s => s.ReceiptOtherCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 3);
    }

    public string GetPayeeReceipt()
    {
        DateTime now = DateTime.Now;
        string prefix = "A" + DateText(now, "yyMM") + "RE";

        string? lastCode = db.TblReceivePaymentReceipts
            .AsNoTracking()
            .Where(w => w.ReceiptCode != null && w.ReceiptCode.StartsWith(prefix))
            .Select(s => s.ReceiptCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 3);
    }

    public string GetAllownceCode()
    {
        DateTime now = DateTime.Now;
        string prefix = DateText(now, "yyyyMMdd");

        string? lastCode = db.TblCodeGenerates
            .AsNoTracking()
            .Where(w => w.CodeType == "allowance" &&
                        w.CodeName != null &&
                        w.CodeName.StartsWith(prefix))
            .Select(s => s.CodeName)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        string code = NextCodeByPrefix(lastCode, prefix, 3);

        return SaveCodeGenerate(code, "allowance");
    }

    public async Task<string> GetZoneCode()
    {
        DateTime now = DateTime.Now;
        string prefix = DateText(now, "yyMM");

        string? lastCode = await db.TblZones
            .AsNoTracking()
            .Where(w => w.ZoneCode != null && w.ZoneCode.StartsWith(prefix))
            .Select(s => s.ZoneCode)
            .OrderByDescending(code => code)
            .FirstOrDefaultAsync();

        return NextCodeByPrefix(lastCode, prefix, 2);
    }

    public async Task<string> GetPortCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "POR" + DateText(now, "yy") + "-";

        string? lastCode = await db.TblPlaces
            .AsNoTracking()
            .Where(w => w.PlaceType == "PORT" &&
                        w.PlaceCode != null &&
                        w.PlaceCode.StartsWith(prefix))
            .Select(s => s.PlaceCode)
            .OrderByDescending(code => code)
            .FirstOrDefaultAsync();

        return NextCodeByPrefix(lastCode, prefix, 3);
    }

    public async Task<string> GetYardCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "YRD" + DateText(now, "yy") + "-";

        string? lastCode = await db.TblPlaces
            .AsNoTracking()
            .Where(w => w.PlaceType == "YARD" &&
                        w.PlaceCode != null &&
                        w.PlaceCode.StartsWith(prefix))
            .Select(s => s.PlaceCode)
            .OrderByDescending(code => code)
            .FirstOrDefaultAsync();

        return NextCodeByPrefix(lastCode, prefix, 3);
    }

    public string GetPaymentCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "P" + DateText(now, "yyMM");

        string? lastCode = db.TblCodeGenerates
            .AsNoTracking()
            .Where(w => w.CodeType == "payment" &&
                        w.CodeName != null &&
                        w.CodeName.StartsWith(prefix))
            .Select(s => s.CodeName)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        string code = NextCodeByPrefix(lastCode, prefix, 3);

        return SaveCodeGenerate(code, "payment");
    }

    public string GetAgentCode()
    {
        DateTime now = DateTime.Now;
        string prefix = DateText(now, "yy");

        string? lastCode = db.TblAgents
            .AsNoTracking()
            .Where(w => w.AgentCode != null && w.AgentCode.StartsWith(prefix))
            .Select(s => s.AgentCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 2);
    }

    public string GetTaxPaymentCode()
    {
        DateTime now = DateTime.Now;
        string prefix = "AVA" + DateText(now, "yy/MM") + "SP";

        string? lastCode = db.TblWithholdingTaxes
            .AsNoTracking()
            .Where(w => w.WithholdingTaxCode != null && w.WithholdingTaxCode.StartsWith(prefix))
            .Select(s => s.WithholdingTaxCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 4);
    }

    public string GetShippingPaymentCode()
    {
        DateTime now = DateTime.Now;
        string prefix = DateText(now, "yyMM");

        string? lastCode = db.TblShippingpayments
            .AsNoTracking()
            .Where(w => w.PaymentCode != null && w.PaymentCode.StartsWith(prefix))
            .Select(s => s.PaymentCode)
            .OrderByDescending(code => code)
            .FirstOrDefault();

        return NextCodeByPrefix(lastCode, prefix, 3);
    }
}
