using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class DashboardRepository
{
    private readonly DbTMSContext db;
    private readonly string connStr;
    private readonly DropListRepository _dropListRepository;
    private readonly CustomerRepository _customerRepository;

    public DashboardRepository(
        DbTMSContext context,
        DropListRepository dropListRepository,
        CustomerRepository customerRepository)
    {
        db = context;
        connStr = db.Database.GetConnectionString()
            ?? throw new InvalidOperationException("Database connection string is not configured.");
        _dropListRepository = dropListRepository;
        _customerRepository = customerRepository;
    }

    public async Task<decimal> GetSumNetValue(DateTime current)
    {
        try
        {
            var (startDate, endDate) = GetMonthRange(current);

            var total = await db.ViewJobSummaries
                .AsNoTracking()
                .Where(w => w.JobDate >= startDate &&
                            w.JobDate < endDate &&
                            w.JobDetailCode != null)
                .SumAsync(s => (decimal?)s.CostValue);

            return total ?? 0;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<decimal> GetSumBillingValue(DateTime current)
    {
        try
        {
            var (startDate, endDate) = GetMonthRange(current);

            var total = await db.ViewJobSummaries
                .AsNoTracking()
                .Where(w => w.JobDate >= startDate &&
                            w.JobDate < endDate &&
                            w.IsBilling == true &&
                            w.JobDetailCode != null)
                .SumAsync(s => (decimal?)s.CostValue);

            return total ?? 0;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<decimal> GetSumUnBillingValue(DateTime current)
    {
        try
        {
            var (startDate, endDate) = GetMonthRange(current);

            var total = await db.ViewJobSummaries
                .AsNoTracking()
                .Where(w => w.JobDate >= startDate &&
                            w.JobDate < endDate &&
                            w.IsBilling == false &&
                            w.IsApprove == true &&
                            w.JobDetailCode != null)
                .SumAsync(s => (decimal?)s.CostValue);

            return total ?? 0;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<decimal> GetSumUnCashingValue(DateTime current)
    {
        try
        {
            var (startDate, endDate) = GetMonthRange(current);

            var total = await db.ViewBillingListActives
                .AsNoTracking()
                .Where(w => w.JobDateValue >= startDate &&
                            w.JobDateValue < endDate &&
                            (w.BillingType == "M" || w.BillingType == "T") &&
                            w.IsClearing == false)
                .SumAsync(s => (decimal?)s.CostValue);

            return total ?? 0;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<List<JobViewModel>> GetListUnCompleteJob()
    {
        return await (from a in db.TblJobMasters.AsNoTracking()
                      join b in db.TblCustomers.AsNoTracking()
                          on a.CustomerCode equals b.CustomerCode
                      select new JobViewModel
                      {
                          job_code = a.JobCode,
                          customer_name = b.CustomerNameTh,
                          booking_no = a.BookingNo,
                          //container_type = a.ContainerType,
                          container_size = a.ContainerSizeName
                      })
            .ToListAsync();
    }

    public async Task<List<JobLoadPlaceViewModel>> GetJob(string truck_license)
    {
        return await db.ViewJobListActives
            .AsNoTracking()
            .Where(w => w.TruckLicense == truck_license &&
                        w.TrackingStatus != "BO")
            .OrderBy(o => o.JobDate)
            .Select(s => new JobLoadPlaceViewModel
            {
                job_date = s.JobDate,
                driver_fullname = s.DriverFullname,
                truck_license = s.TruckLicense,
                job_code = s.JobCode,
                booking_no = s.BookingNo,
                container_no = s.ContainerNo,
                tracking_status = s.TrackingStatus
            })
            .ToListAsync();
    }

    public async Task<List<GraphViewModel>> GetJobCostCustomerBySupplier(string current_month, string current_year)
    {
        var (startDate, endDate) = GetMonthRange(current_year, current_month);

        await using var connection = new SqlConnection(connStr);

        const string sql = @"
;WITH MainCost AS
(
    SELECT
        SUM(d.cost_value) AS id_sum,
        c.customer_name_th AS [name],
        c.customer_code AS code
    FROM tbl_job_detail d
    INNER JOIN tbl_job_master m ON d.job_code = m.job_code
    INNER JOIN tbl_customer c ON m.customer_code = c.customer_code
    WHERE d.is_enabled = 1
      AND m.is_enabled = 1
      AND m.job_date >= @startDate
      AND m.job_date < @endDate
    GROUP BY c.customer_code, c.customer_name_th
),
OtherCost AS
(
    SELECT
        c.customer_code,
        SUM(o.advance_other_value) AS total_other
    FROM tbl_job_detail d
    INNER JOIN tbl_job_master m ON m.job_code = d.job_code
    INNER JOIN tbl_customer c ON m.customer_code = c.customer_code
    INNER JOIN tbl_jobadvance a ON d.job_detail_code = a.job_detail_code
    INNER JOIN tbl_jobadvance_other o ON a.advance_code = o.advance_code
    WHERE d.is_enabled = 1
      AND m.is_enabled = 1
      AND m.job_date >= @startDate
      AND m.job_date < @endDate
    GROUP BY c.customer_code
)
SELECT
    m.id_sum,
    COALESCE(o.total_other, 0) AS other_sum,
    m.[name],
    m.code
FROM MainCost m
LEFT JOIN OtherCost o ON m.code = o.customer_code
ORDER BY m.id_sum DESC;";

        var data = await connection.QueryAsync<GraphViewModel>(sql, new { startDate, endDate });
        return data.AsList();
    }

    public async Task<List<GraphViewModel>> GetYearlyContainerAdvance(string current_year)
    {
        var (startDate, endDate) = GetYearRange(current_year);

        await using var connection = new SqlConnection(connStr);

        const string sql = @"
SELECT
    RIGHT('0' + CAST(m.month_no AS varchar(2)), 2) AS [month],
    COALESCE(p.cost_value, 0) AS cost_value
FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS m(month_no)
LEFT JOIN
(
    SELECT
        SUM(a.advance_totalup) AS cost_value,
        MONTH(j.job_date) AS month_date
    FROM tbl_job_detail d
    INNER JOIN tbl_job_master j ON j.job_code = d.job_code
    INNER JOIN tbl_jobadvance a ON d.job_detail_code = a.job_detail_code
    WHERE d.is_enabled = 1
      AND j.is_enabled = 1
      AND j.job_date >= @startDate
      AND j.job_date < @endDate
    GROUP BY MONTH(j.job_date)
) AS p ON m.month_no = p.month_date
ORDER BY m.month_no;";

        var data = await connection.QueryAsync<GraphViewModel>(sql, new { startDate, endDate });
        return data.AsList();
    }

    public async Task<List<GraphViewModel>> GetYearlyContainerOther(string current_year)
    {
        var (startDate, endDate) = GetYearRange(current_year);

        await using var connection = new SqlConnection(connStr);

        const string sql = @"
SELECT
    RIGHT('0' + CAST(m.month_no AS varchar(2)), 2) AS [month],
    COALESCE(p.cost_value, 0) AS cost_value
FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS m(month_no)
LEFT JOIN
(
    SELECT
        SUM(o.advance_other_valueup) AS cost_value,
        MONTH(j.job_date) AS month_date
    FROM tbl_job_detail d
    INNER JOIN tbl_job_master j ON j.job_code = d.job_code
    INNER JOIN tbl_jobadvance a ON d.job_detail_code = a.job_detail_code
    INNER JOIN tbl_jobadvance_other o ON a.advance_code = o.advance_code
    WHERE d.is_enabled = 1
      AND j.is_enabled = 1
      AND j.job_date >= @startDate
      AND j.job_date < @endDate
    GROUP BY MONTH(j.job_date)
) AS p ON m.month_no = p.month_date
ORDER BY m.month_no;";

        var data = await connection.QueryAsync<GraphViewModel>(sql, new { startDate, endDate });
        return data.AsList();
    }

    public async Task<List<GraphViewModel>> GetYearlyContainerCost(string current_year)
    {
        var (startDate, endDate) = GetYearRange(current_year);

        await using var connection = new SqlConnection(connStr);

        const string sql = @"
SELECT
    RIGHT('0' + CAST(m.month_no AS varchar(2)), 2) AS [month],
    COALESCE(p.cost_value, 0) AS cost_value
FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS m(month_no)
LEFT JOIN
(
    SELECT
        SUM(cost_value) AS cost_value,
        MONTH(job_date) AS month_date
    FROM viewJobSummary
    WHERE job_date >= @startDate
      AND job_date < @endDate
      AND job_detail_code IS NOT NULL
    GROUP BY MONTH(job_date)
) AS p ON m.month_no = p.month_date
ORDER BY m.month_no;";

        var data = await connection.QueryAsync<GraphViewModel>(sql, new { startDate, endDate });
        return data.AsList();
    }

    public async Task<List<GraphViewModel>> GetMonthlyContainerByPlaceData()
    {
        var (startDate, endDate) = GetMonthRange(DateTime.Now);

        await using var connection = new SqlConnection(connStr);

        const string sql = @"
SELECT TOP (10)
    COUNT(d.job_detail_code) AS id_count,
    c.customer_acronym AS [name]
FROM tbl_job_detail d
INNER JOIN tbl_job_master m ON d.job_code = m.job_code
INNER JOIN tbl_customer c ON m.customer_code = c.customer_code
WHERE m.job_date >= @startDate
  AND m.job_date < @endDate
  AND d.job_detail_code IS NOT NULL
GROUP BY c.customer_acronym
ORDER BY id_count DESC;";

        var data = await connection.QueryAsync<GraphViewModel>(sql, new { startDate, endDate });
        return data.AsList();
    }

    public async Task<List<GraphViewModel>> GetYearlyContainerData(string current_year)
    {
        var (startDate, endDate) = GetYearRange(current_year);

        await using var connection = new SqlConnection(connStr);

        const string sql = @"
                            SELECT
                                RIGHT('0' + CAST(m.month_no AS varchar(2)), 2) AS [month],
                                COALESCE(p.id_count, 0) AS id_count
                            FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS m(month_no)
                            LEFT JOIN
                            (
                                SELECT
                                    COUNT(job_code) AS id_count,
                                    MONTH(job_date) AS month_date
                                FROM viewJobSummary
                                WHERE job_date >= @startDate
                                  AND job_date < @endDate
                                  AND job_detail_code IS NOT NULL
                                GROUP BY MONTH(job_date)
                            ) AS p ON m.month_no = p.month_date
                            ORDER BY m.month_no;";

        var data = await connection.QueryAsync<GraphViewModel>(sql, new { startDate, endDate });
        return data.AsList();
    }

    public async Task<List<GraphViewModel>> GetDailyContainerData(int year, int month)
    {
        await using var connection = new SqlConnection(connStr);

        const string sql = @"
;WITH Calendar AS
(
    SELECT CAST(DATEFROMPARTS(@year, @month, 1) AS date) AS CalendarDate
    UNION ALL
    SELECT DATEADD(day, 1, CalendarDate)
    FROM Calendar
    WHERE CalendarDate < EOMONTH(DATEFROMPARTS(@year, @month, 1))
)
SELECT
    DAY(c.CalendarDate) AS [day],
    ISNULL(COUNT(d.container_no), 0) AS id_count
FROM Calendar c
LEFT JOIN tbl_job_master m
    ON m.job_date >= c.CalendarDate
   AND m.job_date < DATEADD(day, 1, c.CalendarDate)
LEFT JOIN tbl_job_detail d ON d.job_code = m.job_code
GROUP BY DAY(c.CalendarDate)
ORDER BY [day]
OPTION (MAXRECURSION 1000);";

        var data = await connection.QueryAsync<GraphViewModel>(sql, new { year, month });
        return data.AsList();
    }

    public async Task<List<TruckWorkingViewModel>> GetTruckWorkingData(DateTime date_val)
    {
        var startDate = date_val.Date;
        var endDate = DateTime.Now.Date.AddDays(1);

        await using var connection = new SqlConnection(connStr);

        const string sql = @"
                        SELECT
                            COALESCE(a.trip, 0) AS trip,
                            d.driver_fullname,
                            t.truck_license,
                            t.truck_code,
                            d.driver_code
                        FROM tbl_truck t
                        INNER JOIN tbl_truck_driver td ON t.truck_code = td.truck_code
                        INNER JOIN tbl_driver d ON td.driver_code = d.driver_code
                        LEFT JOIN
                        (
                            SELECT
                                SUM(dc.driver_allowance) AS trip,
                                dc.driver_code
                            FROM tbl_job_allowance dc
                            INNER JOIN tbl_job_detail jd ON dc.job_detail_code = jd.job_detail_code
                            INNER JOIN tbl_job_master jm ON jd.job_code = jm.job_code
                            WHERE jm.job_date >= @startDate
                              AND jm.job_date < @endDate
                            GROUP BY dc.driver_code
                        ) AS a ON td.driver_code = a.driver_code
                        WHERE d.is_enabled = 1;";

        var data = await connection.QueryAsync<TruckWorkingViewModel>(sql, new { startDate, endDate });
        return data.AsList();
    }

    public async Task<List<JobLoadPlaceViewModel>> GetTotalDetailData(string code, string month, string year)
    {
        var (startDate, endDate) = GetMonthRange(year, month);

        var data = await (from a in db.TblJobMasters.AsNoTracking()
                          join b in db.TblJobDetails.AsNoTracking()
                              on a.JobCode equals b.JobCode
                          join c in db.TblCustomers.AsNoTracking()
                              on a.CustomerCode equals c.CustomerCode
                          join d in db.TblPlaces.AsNoTracking()
                              on b.MissionPlaceCode equals d.PlaceCode
                          where a.CustomerCode == code &&
                                a.JobDate >= startDate &&
                                a.JobDate < endDate &&
                                a.IsEnabled == true &&
                                b.IsEnabled == true
                          orderby a.JobDate, d.PlaceName
                          select new JobLoadPlaceViewModel
                          {
                              job_date = a.JobDate,
                              job_invoice = a.JobInvoice,
                              mission_name = d.PlaceName,
                              booking_no = a.BookingNo,
                              container_no = b.ContainerNo,
                              customer_code = a.CustomerCode,
                              customer_name = c.CustomerNameTh,
                              trip_cost = b.CostValue,
                              is_billing = b.IsBilling,
                              other_list = (from q in db.TblJobadvances.AsNoTracking()
                                            join y in db.TblJobadvanceOthers.AsNoTracking()
                                                on q.AdvanceCode equals y.AdvanceCode
                                            where q.JobDetailCode == b.JobDetailCode
                                            select y).ToList()
                          })
            .ToListAsync();

        return data;
    }

    public async Task<List<WorkingDetailViewModel>> GetWorkingDetailData(string code)
    {
        DateTime startDate = DateTime.Now.Date.AddDays(GetLastSaturdayOffset(DateTime.Now.DayOfWeek));
        DateTime endDate = DateTime.Now.Date.AddDays(1);

        return await (from a in db.TblJobMasters.AsNoTracking()
                      join b in db.TblJobDetails.AsNoTracking()
                          on a.JobCode equals b.JobCode
                      join c in db.TblJobAllowances.AsNoTracking()
                          on b.JobDetailCode equals c.JobDetailCode
                      join d in db.TblDrivers.AsNoTracking()
                          on b.DriverCode equals d.DriverCode
                      join e in db.TblTrucks.AsNoTracking()
                          on b.TruckCode equals e.TruckCode
                      join f in db.TblPlaces.AsNoTracking()
                          on b.MissionPlaceCode equals f.PlaceCode
                      where b.DriverCode == code &&
                            a.JobDate >= startDate &&
                            a.JobDate < endDate
                      select new WorkingDetailViewModel
                      {
                          truck_license = e.TruckLicense,
                          driver_fullname = d.DriverFullname,
                          trip = c.DriverAllowance,
                          fuel = 0,
                          expressway = 0,
                          job_date = a.JobDate,
                          container_no = b.ContainerNo,
                          booking_no = a.BookingNo,
                          job_invoice = a.JobInvoice,
                          place = f.PlaceName
                      })
            .ToListAsync();
    }

    public async Task<List<OtherJobViewModel>> GetAdvanceOtherList(string code, string month, string year)
    {
        var (startDate, endDate) = GetMonthRange(year, month);

        await using var connection = new SqlConnection(connStr);

        const string sql = @"
SELECT
    o.advance_other AS [name],
    o.advance_other_value AS [value],
    d.container_no,
    p.place_name AS place
FROM tbl_jobadvance_other o
INNER JOIN tbl_jobadvance a ON o.advance_code = a.advance_code
INNER JOIN tbl_job_detail d ON a.job_detail_code = d.job_detail_code
INNER JOIN tbl_job_master m ON d.job_code = m.job_code
INNER JOIN tbl_place p ON d.mission_place_code = p.place_code
WHERE m.customer_code = @code
  AND m.job_date >= @startDate
  AND m.job_date < @endDate;";

        var data = await connection.QueryAsync<OtherJobViewModel>(sql, new { code, startDate, endDate });
        return data.AsList();
    }

    public async Task<List<CustomerBillingViewModel>> GetListCustomer(string year)
    {
        var (startDate, endDate) = GetYearRange(year);

        await using var connection = new SqlConnection(connStr);

        const string sql = @"
SELECT
    b.customer_code,
    c.customer_name_th AS customer_name
FROM
(
    SELECT customer_code
    FROM dbo.tbl_billing_master
    WHERE create_date >= @startDate
      AND create_date < @endDate
      AND is_enabled = 1
      AND is_clearing = 0
    GROUP BY customer_code
) AS b
INNER JOIN dbo.tbl_customer c ON b.customer_code = c.customer_code;";

        var data = await connection.QueryAsync<CustomerBillingViewModel>(sql, new { startDate, endDate });
        return data.AsList();
    }

    public async Task<List<PendingCountViewModel>> GetListPending(string customer_code, string year)
    {
        var (startDate, endDate) = GetYearRange(year);

        await using var connection = new SqlConnection(connStr);

        const string sql = @"
SELECT
    RIGHT('0' + CAST(m.month_no AS varchar(2)), 2) AS [month],
    COALESCE(p.count_pending, 0) AS count_pending
FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS m(month_no)
LEFT JOIN
(
    SELECT
        COUNT(billing_code) AS count_pending,
        MONTH(billing_date) AS month_date
    FROM dbo.tbl_billing_master
    WHERE billing_date >= @startDate
      AND billing_date < @endDate
      AND is_clearing = 0
      AND customer_code = @customer_code
      AND is_enabled = 1
    GROUP BY MONTH(billing_date)
) AS p ON m.month_no = p.month_date
ORDER BY m.month_no;";

        var data = await connection.QueryAsync<PendingCountViewModel>(sql, new { customer_code, startDate, endDate });
        return data.AsList();
    }

    public async Task<TblCustomer> GetCustomerByCode(string customer_code)
    {
        return await _customerRepository.GetCustomerByCode(customer_code);
    }

    public List<DropDownViewModel> DropListMonth()
    {
        return _dropListRepository.GetMonth();
    }

    public List<DropDownViewModel> DropListYear()
    {
        return _dropListRepository.GetYear();
    }

    private static (DateTime Start, DateTime End) GetMonthRange(DateTime current)
    {
        var start = new DateTime(current.Year, current.Month, 1);
        return (start, start.AddMonths(1));
    }

    private static (DateTime Start, DateTime End) GetMonthRange(string year, string month)
    {
        int y = int.TryParse(year, out var parsedYear) ? parsedYear : DateTime.Now.Year;
        int m = int.TryParse(month, out var parsedMonth) ? parsedMonth : DateTime.Now.Month;

        var start = new DateTime(y, m, 1);
        return (start, start.AddMonths(1));
    }

    private static (DateTime Start, DateTime End) GetYearRange(string year)
    {
        int y = int.TryParse(year, out var parsedYear) ? parsedYear : DateTime.Now.Year;

        var start = new DateTime(y, 1, 1);
        return (start, start.AddYears(1));
    }

    private static int GetLastSaturdayOffset(DayOfWeek dayOfWeek)
    {
        int diff = DayOfWeek.Saturday - dayOfWeek;
        return diff > 0 ? diff - 7 : diff;
    }
}
