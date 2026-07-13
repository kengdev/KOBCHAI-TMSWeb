using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMSWeb.Repositories;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace Dummy.Controllers;
[Authorize]
public class DashboardController : Controller
{
    private readonly DbTMSContext _context;
    private readonly DashboardRepository _dashboardService;
    private string LoggedInUser => User.Identity.Name;

    public DashboardController(DbTMSContext context,DashboardRepository dashboardService)
    {
        _context = context;
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> SummaryDetail()
    {
        var model = new List<SummaryDetailViewModel>();
        var item = new SummaryDetailViewModel();
        string current = DateTime.Now.Year.ToString();
        string lastyear = DateTime.Now.AddYears(-1).Year.ToString();
        string last2year = DateTime.Now.AddYears(-2).Year.ToString();

        var current_data = await _dashboardService.GetYearlyContainerCost(current);
        var lastyear_data = await _dashboardService.GetYearlyContainerCost(lastyear);
        var last2year_data = await _dashboardService.GetYearlyContainerCost(last2year);

        var current_other = await _dashboardService.GetYearlyContainerOther(current);
        var lastyear_other = await _dashboardService.GetYearlyContainerOther(lastyear);
        var last2year_other = await _dashboardService.GetYearlyContainerOther(last2year);

        var current_advance = await _dashboardService.GetYearlyContainerAdvance(current);
        var lastyear_advance = await _dashboardService.GetYearlyContainerAdvance(lastyear);
        var last2year_advance = await _dashboardService.GetYearlyContainerAdvance(last2year);

        item.year = "current";
        item.year_name = current;
        item.data = current_data;
        item.other = current_other;
        item.advance = current_advance;
        model.Add(item);

        item = new SummaryDetailViewModel();
        item.year = "lastyear";
        item.year_name = lastyear;
        item.data = lastyear_data;
        item.other = lastyear_other;
        item.advance = lastyear_advance;
        model.Add(item);

        item = new SummaryDetailViewModel();
        item.year = "last2year";
        item.year_name = last2year;
        item.data = last2year_data;
        item.other = last2year_other;
        item.advance = last2year_advance;
        model.Add(item);
        return View(model);
    }

    public async Task<IActionResult> GetHistory()
    {
        var dashview = new DashboardHistoryViewModel();
        DateTime second_month = DateTime.Now.AddMonths(-1);
        DateTime third_month = DateTime.Now.AddMonths(-2);
        DateTime forth_month = DateTime.Now.AddMonths(-3);
        DateTime fifth_month = DateTime.Now.AddMonths(-4);
        DateTime sixth_month = DateTime.Now.AddMonths(-5);
        dashview.month_2 = second_month.ToString("yyyy MMM");
        dashview.month_3 = third_month.ToString("yyyy MMM");
        dashview.month_4 = forth_month.ToString("yyyy MMM");
        dashview.month_5 = fifth_month.ToString("yyyy MMM");
        dashview.month_6 = sixth_month.ToString("yyyy MMM");

        dashview.net_value2 = await _dashboardService.GetSumNetValue(second_month);
        dashview.net_value3 = await _dashboardService.GetSumNetValue(third_month);
        dashview.net_value4 = await _dashboardService.GetSumNetValue(forth_month);
        dashview.net_value5 = await _dashboardService.GetSumNetValue(fifth_month);
        dashview.net_value6 = await _dashboardService.GetSumNetValue(sixth_month);

        dashview.bill_value2 = await _dashboardService.GetSumBillingValue(second_month);
        dashview.bill_value3 = await _dashboardService.GetSumBillingValue(third_month);
        dashview.bill_value4 = await _dashboardService.GetSumBillingValue(forth_month);
        dashview.bill_value5 = await _dashboardService.GetSumBillingValue(fifth_month);
        dashview.bill_value6 = await _dashboardService.GetSumBillingValue(sixth_month);

        dashview.unbill_value2 = await _dashboardService.GetSumUnBillingValue(second_month);
        dashview.unbill_value3 = await _dashboardService.GetSumUnBillingValue(third_month);
        dashview.unbill_value4 = await _dashboardService.GetSumUnBillingValue(forth_month);
        dashview.unbill_value5 = await _dashboardService.GetSumUnBillingValue(fifth_month);
        dashview.unbill_value6 = await _dashboardService.GetSumUnBillingValue(sixth_month);

        dashview.unpay_value2 = await _dashboardService.GetSumUnCashingValue(second_month);
        dashview.unpay_value3 = await _dashboardService.GetSumUnCashingValue(third_month);
        dashview.unpay_value4 = await _dashboardService.GetSumUnCashingValue(forth_month);
        dashview.unpay_value5 = await _dashboardService.GetSumUnCashingValue(fifth_month);
        dashview.unpay_value6 = await _dashboardService.GetSumUnCashingValue(sixth_month);
        return Json(dashview);
    }

    public IActionResult HistoryDetail()
    {
        return View();
    }

    public async Task<IActionResult> GetSum()
    {
        var dashview = new DashboardCounterViewModel();
        DateTime current = DateTime.Now;
        dashview.sum_netvalue = await _dashboardService.GetSumNetValue(current);
        dashview.sum_billingvalue = await _dashboardService.GetSumBillingValue(current);
        dashview.sum_unbillingvalue = await _dashboardService.GetSumUnBillingValue(current);
        dashview.sum_uncashingvalue = await _dashboardService.GetSumUnCashingValue(current);

        return Json(dashview);
    }

    public async Task<JsonResult> _GraphDailyContainer()
    {
        System.Globalization.CultureInfo _cultureEnInfo = new System.Globalization.CultureInfo("en-US");
        var fc = new FusionMSLine();
        var list = new List<FCData>();
        var data = new FCData();
        var list_ds = new List<FCDataset>();
        DateTime current_month = DateTime.Now;

        var currentmonthData = await _dashboardService.GetDailyContainerData(current_month.Year, current_month.Month);
        var dsValues = new List<FCDatasetValue>();
        var ds = new FCDataset();
        foreach (var item in currentmonthData)
        {
            var ds_value = new FCDatasetValue();
            ds_value.value = item.id_count.ToString();
            dsValues.Add(ds_value);
        }
        ds.seriesname = current_month.ToString("MMM yyyy");
        ds.data = dsValues.ToArray();
        list_ds.Add(ds);


        var chart = new FCMSLineAttribute();
        chart.theme = "fint";
        chart.caption = "";
        chart.subCaption = "";
        chart.xAxisName = "";
        chart.yAxisName = "";
        chart.showXAxisLine = "0";
        chart.xAxisLineColor = "#cccccc";

        int daysInMonth = DateTime.DaysInMonth(current_month.Year, current_month.Month);
        var list_label = new List<CategoryValue>();
        for (int i = 1; i <= daysInMonth; i++)
        {
            var label = new CategoryValue();
            label.label = i.ToString();
            list_label.Add(label);
        }

        var category = new FCCategory();
        category.category = list_label.ToArray();

        fc.chart = chart;
        fc.categories = category;
        fc.dataset = list_ds.ToArray();
        return Json(fc);
    }

    public async Task<JsonResult> _GraphYearlyContainer()
    {
        System.Globalization.CultureInfo _cultureEnInfo = new System.Globalization.CultureInfo("en-US");
        var fc = new FusionMSLine();
        var list = new List<FCData>();
        var data = new FCData();
        string startyear = DateTime.Now.Year.ToString();
        string endyear = DateTime.Now.AddYears(-2).Year.ToString();
        int e = int.Parse(startyear);
        int s = int.Parse(endyear);

        var list_ds = new List<FCDataset>();
        for (int i = s; i <= e; i++)
        {
            string year = i.ToString();
            var query = await _dashboardService.GetYearlyContainerData(year);
            var dsValues = new List<FCDatasetValue>();
            var ds = new FCDataset();
            foreach (var item in query)
            {
                var ds_value = new FCDatasetValue();
                ds_value.value = item.id_count.ToString();
                dsValues.Add(ds_value);
            }
            ds.seriesname = year;
            ds.data = dsValues.ToArray();
            list_ds.Add(ds);
        }


        var chart = new FCMSLineAttribute();
        chart.theme = "fint";
        chart.caption = "";
        chart.subCaption = "";
        chart.xAxisName = "";
        chart.yAxisName = "";
        chart.showXAxisLine = "0";
        chart.xAxisLineColor = "#cccccc";

        var list_label = new List<CategoryValue>();
        for (int i = 1; i <= 12; i++)
        {
            DateTime dt = new DateTime(e, i, 1);
            string sMonth = dt.ToString("MMM", _cultureEnInfo).ToUpper();
            var label = new CategoryValue();
            label.label = sMonth;
            list_label.Add(label);
        }

        var category = new FCCategory();
        category.category = list_label.ToArray();

        fc.chart = chart;
        fc.categories = category;
        fc.dataset = list_ds.ToArray();
        return Json(fc);
    }

    public async Task<IActionResult> _GraphMonthlyContainerByPlace()
    {
        var fc = new FusionBarChart();
        var list = new List<FCData>();
        var query = await _dashboardService.GetMonthlyContainerByPlaceData();
        foreach (var item in query)
        {
            var data = new FCData();
            data.label = item.name;
            data.value = item.id_count.ToString();
            list.Add(data);
        }

        var chart = new FCBarAttribute();
        chart.plotgradientcolor = "";
        chart.showalternatehgridcolor = "0";
        chart.showShadow = "0";
        chart.showplotborder = "0";
        chart.showvalues = "0";
        chart.theme = "fint";
        chart.divlinecolor = "CCCCCC";
        chart.palettecolors = "#b2d9f9,#f7c018,#94bf13,#ff9049,#069191,#d74a4a,#914b91,#5c882b,#0c93d8";
        chart.toolTipBorderColor = "7d7d7d";
        chart.toolTipBgColor = "7d7d7d";
        chart.toolTipColor = "ffffff";
        chart.showcanvasborder = "0";
        chart.canvasborderalpha = "0";
        chart.numberSuffix = " trip";
        chart.anchorRadius = "4";
        chart.anchorBorderThickness = "2";
        chart.yaxisvaluespadding = "10";
        chart.showborder = "0";
        chart.outcnvbasefontsize = "12";
        chart.showXAxisLine = "0";

        fc.chart = chart;
        fc.data = list.ToArray();
        return Json(fc);
    }

    public async Task<IActionResult> TotalDetail(string code, string month, string year)
    {
        string current_month = DateTime.Now.ToString("MM");
        string current_year = DateTime.Now.ToString("yyyy");

        month = (month != null ? month : current_month);
        year = (year != null ? year : current_year);

        var model = await _dashboardService.GetTotalDetailData(code, month, year);
        var customer = await _dashboardService.GetCustomerByCode(code);
        ViewBag.Customer = customer;
        ViewData["month"] = from a in _dashboardService.DropListMonth() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == month };
        ViewData["year"] = from a in _dashboardService.DropListYear() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == year };
        return View(model);
    }

    public async Task<IActionResult> OtherDetail(string month, string year)
    {
        int d = 1;
        int m = Convert.ToInt32(month);
        int y = Convert.ToInt32(year);

        DateTime date = new DateTime(y, m, d);

        var model = await _dashboardService.GetJobCostCustomerBySupplier(month, year);
        ViewBag.Date = date;
        return View(model);
    }

    public async Task<IActionResult> ListOtherDetail(string code, int month, int year)
    {
        int d = 1;
        int m = Convert.ToInt32(month);
        int y = Convert.ToInt32(year);

        DateTime date = new DateTime(y, m, d);

        var customer = await _dashboardService.GetCustomerByCode(code);
        var data = _dashboardService.GetAdvanceOtherList(code, month.ToString(), year.ToString());

        ViewBag.Date = date;
        ViewBag.Customer = customer;
        return View(data);
    }

    public async Task<IActionResult> WorkingDetail(string code)
    {
        var model = await _dashboardService.GetWorkingDetailData(code);
        return View(model);
    }

    public IActionResult _PendingPayment()
    {
        string year = DateTime.Now.ToString("yyyy");
        ViewData["year"] = from a in _dashboardService.DropListYear() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == year };

        return View();
    }

    public async Task<IActionResult> _PendingData(string year)
    {
        var customer = await _dashboardService.GetListCustomer(year);
        List<PendingBillViewModel> list = new List<PendingBillViewModel>();
        foreach (var cs in customer)
        {
            var item = new PendingBillViewModel();
            var data = await _dashboardService.GetListPending(cs.customer_code, year);
            item.customer_code = cs.customer_code;
            item.customer_name = cs.customer_name;
            foreach (var pd in data)
            {
                switch (pd.month)
                {
                    case "01":
                        item.month_01 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "02":
                        item.month_02 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "03":
                        item.month_03 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "04":
                        item.month_04 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "05":
                        item.month_05 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "06":
                        item.month_06 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "07":
                        item.month_07 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "08":
                        item.month_08 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "09":
                        item.month_09 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "10":
                        item.month_10 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "11":
                        item.month_11 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                    case "12":
                        item.month_12 = (pd.count_pending == null ? 0 : pd.count_pending.Value);
                        break;
                }
            }

            list.Add(item);
        }
        return PartialView(list);
    }
}