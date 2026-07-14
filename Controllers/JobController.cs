using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;
using TMSWeb.Repositories;

namespace TMSWeb.Controllers;
[Authorize]
public class JobController : Controller
{
    private readonly JobRepository _jobService;
    private readonly DropListRepository _dropListService;
    private readonly BillingRepository _billingService;
    private string LoggedInUser => User.Identity.Name;

    public JobController(JobRepository jobService, DropListRepository dropListService, BillingRepository billingService)
    {
        _jobService = jobService;
        _dropListService = dropListService;
        _billingService = billingService;
    }

    public async Task<IActionResult> Index()
    {
        string month = DateTime.Now.ToString("MM");
        string year = DateTime.Now.ToString("yyyy");

        ViewData["container"] = from a in await _dropListService.GetContainerSize() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        ViewData["month"] = from a in _dropListService.GetMonth() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == month };
        ViewData["year"] = from a in _dropListService.GetYear() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == year };

        return View();
    }

    [Authorize(Roles = "Administrator, Accounting, Super")]
    public async Task<IActionResult> Finish(int code)
    {
        var data = await _jobService.GetOrderByCode(code);
        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> Finish(FinishJobModel model, TblJobadvance jobAdvance, TblJobcashing jobCashing)
    {
        string username = LoggedInUser;
        model.username = username;

        ResponseViewModel result = await _jobService.FinishJob(model, jobAdvance, jobCashing);
        return Json(result);
    }

    [Authorize(Roles = "Administrator, Transport, Super")]
    public async Task<IActionResult> UpdateAdvance(int code)
    {
        var data = await _jobService.GetTblJobByIdAsync(code);
        var transaction = await _jobService.GetTrBudget();

        ViewBag.Transaction = transaction;
        return View(data);
    }

    public async Task<IActionResult> UpdateAdvanceData(FinishJobModel model, TblJobadvance jobAdvance, TblJobcashing jobCashing)
    {
        var _notif = new LineNotifRepository();
        model.username = this.LoggedInUser;

        ResponseViewModel response = await _jobService.UpdateAdvanceData(model, jobAdvance, jobCashing);
        if (response.result == true && response.notifMessage != null)
            await _notif.NotifGroup(response.notifMessage);

        return Json(response);
    }

    public async Task<IActionResult> GetJobDataList(jQueryDataTableParamModel param, iDataTableParamModel iFilter)
    {
        var query = await _jobService.GetJobDataList(iFilter.filterDateRangeStart, iFilter.filterDateRangeEnd);
        var data = query.Where(w => (iFilter.filterCustomer == null || w.customer_name.ToUpper().Contains(iFilter.filterCustomer.ToUpper())) &&
        (iFilter.filterMission == null || w.mission_name.ToUpper().Contains(iFilter.filterMission.ToUpper())) &&
        (iFilter.filterBookingNo == null || w.booking_no.ToUpper().Contains(iFilter.filterBookingNo.ToUpper())) &&
        (iFilter.filterContainerNo == null || w.container_no.ToUpper().Contains(iFilter.filterContainerNo.ToUpper())) &&
        (iFilter.filterSupplier == null || w.supplier_name.Contains(iFilter.filterSupplier.ToUpper())) &&
        (iFilter.filterLicense == null || w.truck_license.Contains(iFilter.filterLicense)));

        Func<JobViewModel, string> orderingFunction = (c => param.iSortCol_0 == 2 ? c.customer_name :
                                                                param.iSortCol_0 == 3 ? c.mission_name :
                                                                param.iSortCol_0 == 4 ? c.booking_no :
                                                                param.iSortCol_0 == 5 ? c.container_no :
                                                                param.iSortCol_0 == 6 ? c.container_size :
                                                                param.iSortCol_0 == 7 ? c.truck_license :
                                                                param.iSortCol_0 == 8 ? c.supplier_name : c.mission_name);

        IEnumerable<JobViewModel> listQuery;
        if (param.iSortCol_0 == 2)
        {
            if (param.sSortDir_0 == "asc")
            {
                listQuery = data.OrderBy(o => o.job_date_value)
                                .Skip(param.iDisplayStart)
                                .Take(param.iDisplayLength);
            }
            else
            {
                listQuery = data.OrderByDescending(o => o.job_date_value)
                                .Skip(param.iDisplayStart)
                                .Take(param.iDisplayLength);
            }
        }
        else
        {
            if (param.sSortDir_0 == "asc")
            {
                listQuery = data.OrderBy(orderingFunction)
                                .Skip(param.iDisplayStart)
                                .Take(param.iDisplayLength);
            }
            else
            {
                listQuery = data.OrderByDescending(orderingFunction)
                                .Skip(param.iDisplayStart)
                                .Take(param.iDisplayLength);
            }
        }

        return Json(new
        {
            sEcho = param.sEcho,
            iTotalRecords = data.Count(),
            iTotalDisplayRecords = data.Count(),
            aaData = listQuery
        });
    }

    public async Task<IActionResult> StatusUpdate(int code)
    {
        var model = new UpdateStatusModel();
        var data = await _jobService.GetOrderByCode(code);
        var tracking = this.GetTracking(data.JobDetailCode, data.JobType);
        model.job_detail_code = data.JobDetailCode;
        model.status = this.NextStatus(data.TrackingStatus);
        model.status_stamp = DateTime.Now;

        ViewBag.Data = data;
        ViewBag.Tracking = tracking;
        return PartialView(model);
    }

    public async Task<IActionResult> ReverseStatus(string code, string status)
    {
        ResponseViewModel response = await _jobService.ReverseStatus(code, status);

        return Json(response);
    }

    [Authorize(Roles = "Administrator, Transport, Super")]
    public async Task<IActionResult> Update(int code)
    {
        var data = await _jobService.GetOrderByCode(code);
        ViewData["new_driver_code"] = from a in await _dropListService.GetDriver() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.DriverCode };
        ViewData["truck_code"] = from a in await _dropListService.GetTruck() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.TruckCode };
        return PartialView(data);
    }

    public async Task<IActionResult> GetDriverCodeByTruck(string code)
    {
        var data = await _jobService.GetDriverCodeByTruck(code);

        return Json(data);
    }

    public async Task<IActionResult> UpdateData(UpdateDataModel dataToUpdate)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _jobService.UpdateData(dataToUpdate, username);
        return Json(result);
    }

    public async Task<IActionResult> Detail(int code)
    {
        var data = await _jobService.GetOrderByCode(code);
        var image = await _jobService.GetOrderImagesByCode(data.JobDetailCode);
        var tracking = this.GetTracking(data.JobDetailCode, data.JobType);

        ViewBag.Data = data;
        ViewBag.Image = image;
        ViewBag.Tracking = tracking;
        return View();
    }

    public async Task<IActionResult> RemoveImage(int id)
    {
        ResponseViewModel result = await _jobService.RemoveImage(id, this.LoggedInUser);
        return Json(result);
    }

    [Authorize(Roles = "Administrator, Transport, Super")]
    public async Task<IActionResult> ChangeDriver(int code)
    {
        var model = new ChangeDriverModel();
        var data = await _jobService.GetOrderByCode(code);
        var allowance = await _jobService.GetJobAllowanceListAsync(data.JobDetailCode);

        model.driver_code = data.DriverCode;
        model.truck_code = data.TruckCode;
        model.supplier_code = data.SupplierCode;
        model.trailer_license = data.TrailerLicense;
        model.job_detail_code = data.JobDetailCode;
        //model.driver_cost = 150;

        if (data.FleetTypeCode == "F1")
        {
            ViewData["new_truck_code"] = from a in await _dropListService.GetTruck() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.TruckCode };
            ViewData["new_driver_code"] = from a in await _dropListService.GetDriver() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.DriverCode };
        }
        else
        {
            ViewData["new_truck_code"] = from a in await _dropListService.GetTruck() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
            ViewData["new_driver_code"] = from a in await _dropListService.GetDriver() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        }

        ViewData["new_supplier_code"] = from a in await _dropListService.GetSupplierTransport() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.SupplierCode };
        ViewBag.Data = data;
        ViewBag.Allowance = allowance;
        return View(model);
    }

    public async Task<IActionResult> ChangeDriverData(ChangeDriverModel model)
    {
        string username = LoggedInUser;
        var supplierData = await _jobService.GetTblSupplier(model.new_supplier_code);

        model.new_driver_code = supplierData.FleetTypeCode != "F1" ? "DV000000" : model.new_driver_code;
        model.new_truck_code = supplierData.FleetTypeCode != "F1" ? "T000000" : model.new_truck_code;
        ResponseViewModel result = await _jobService.ChangeDriver(model, username);
        return Json(result);
    }

    [Authorize(Roles = "Administrator, Transport, Super")]
    public async Task<IActionResult> CancelJob(int code)
    {
        var model = new CancelJobModel();
        var data = await _jobService.GetOrderByCode(code);
        var allowance = await _jobService.GetJobAllowanceListAsync(data.JobDetailCode);

        model.job_detail_code = data.JobDetailCode;
        ViewBag.Data = data;
        ViewBag.Allowance = allowance;
        return View(model);
    }

    public async Task<IActionResult> CancelJobData(CancelJobModel model)
    {
        string username = LoggedInUser;
        model.create_by = username;

        ResponseViewModel result = await _jobService.CancelJob(model);
        return Json(result);
    }

    [Authorize(Roles = "Administrator, Transport, Super")]
    public async Task<IActionResult> Edit(int code)
    {
        var data = await _jobService.GetOrderByCode(code);
        var detail = await _jobService.GetDetailDataByCode(data.JobDetailCode);
        var allowance = await _jobService.GetJobAllowanceListAsync(data.JobDetailCode);

        ViewData["new_supplier_code"] = from a in await _dropListService.GetSupplierTransport() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.SupplierCode };
        ViewData["new_truck_code"] = from a in await _dropListService.GetTruck() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.TruckCode };
        ViewData["new_driver_code"] = from a in await _dropListService.GetDriver() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.DriverCode };
        ViewBag.Data = data;
        ViewBag.Allowance = allowance;
        return View(detail);
    }

    public async Task<IActionResult> EditOrderTruck(TblJobDetail jobToEdit, ChangeDriverModel model)
    {
        string username = LoggedInUser;
        jobToEdit.SupplierCode = model.new_supplier_code;
        jobToEdit.TruckCode = model.new_truck_code;
        jobToEdit.DriverCode = model.new_driver_code;
        jobToEdit.UpdateDate = DateTime.Now;
        jobToEdit.UpdateBy = username;

        ResponseViewModel result = await _jobService.EditJob(jobToEdit, model.job_allowance_id);
        return Json(result);
    }

    [Authorize(Roles = "Administrator, Transport, Super")]
    public async Task<IActionResult> RemoveJob(string job_detail_code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _jobService.RemoveJob(job_detail_code, username);
        return Json(result);
    }

    public IActionResult Search()
    {
        string month = DateTime.Now.ToString("MM");
        string year = DateTime.Now.ToString("yyyy");
        ViewData["month"] = from a in _dropListService.GetMonth() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == month };
        ViewData["year"] = from a in _dropListService.GetYear() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == year };
        return View();
    }

    public async Task<IActionResult> SearchResult(jQueryDataTableParamModel param, string code)
    {
        code = code == null || code == "" ? "!" : code;
        var data = await _jobService.GetOrderBySearch(code);
        return Json(new
        {
            sEcho = param.sEcho,
            iTotalRecords = data.Count(),
            iTotalDisplayRecords = data.Count(),
            aaData = data
        });
    }

    public async Task<IActionResult> SearchDetail(string code)
    {
        var data = await _jobService.GetOrderBySearchDetail(code);
        var billing = await _billingService.GetBillingDetailByJobDetailCode(data.JobDetailCode);

        ViewBag.Billing = billing;
        return View(data);
    }

    public async Task<IActionResult> GetYardPort(string code)
    {
        var data = await _jobService.GetYardPortDataByCode(code);

        return Json(data);
    }

    private async Task<List<StepViewModel>> GetTracking(string code, string type)
    {
        var list = new List<StepViewModel>();
        var tracking = await _jobService.GetListTrackingByJob(code);
        var count_tracking = tracking.Count;
        if (type != "FLOOR")
        {
            for (int i = 0; i < 6; i++)
            {
                var item = new StepViewModel();
                var i_track = new TrackingViewModel();
                switch (i)
                {
                    case 0:
                        i_track = tracking.Where(w => w.tracking_status == "PN").FirstOrDefault();
                        item.action = "งานเข้าระบบ";
                        item.action_time = "";
                        item.active = "done";
                        item.index = "first";
                        item.icon = "fad fa-desktop";
                        item.column = 2;
                        break;
                    case 1:
                        i_track = tracking.Where(w => w.tracking_status == "ST").FirstOrDefault();
                        item.action = "พนังานขับรถรับงาน";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = (i_track != null ? "done" : "");
                        item.icon = "fad fa-user-hard-hat";
                        item.column = 2;
                        break;
                    case 2:
                        i_track = tracking.Where(w => w.tracking_status == "WP").FirstOrDefault();
                        item.action = "รับตู้";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = (i_track != null ? "done" : "");
                        item.icon = "fad fa-container-storage";
                        item.column = 2;
                        break;
                    case 3:
                        i_track = tracking.Where(w => w.tracking_status == "AR").FirstOrDefault();
                        item.action = "รอเปิด/รอบรรจุ";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = (i_track != null ? "done" : "");
                        item.icon = "fad fa-truck-loading";
                        item.column = 2;
                        break;
                    case 4:
                        i_track = tracking.Where(w => w.tracking_status == "FN").FirstOrDefault();
                        item.action = "เปิด/บรรจุเรียบร้อย";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = (i_track != null ? "done" : "");
                        item.icon = "fad fa-clipboard-list-check";
                        item.column = 2;
                        break;
                    case 5:
                        i_track = tracking.Where(w => w.tracking_status == "DO").FirstOrDefault();
                        item.action = "คืนตู้";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = (i_track != null ? "done" : "");
                        item.icon = "fad fa-trailer";
                        item.column = 2;
                        item.index = "last";
                        break;
                    /*case 5:
                        i_track = tracking.Where(w => w.tracking_status == "BO").FirstOrDefault();
                        item.action = "รับเอกสาร";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = (i_track != null ? "done" : "");
                        item.index = "last";
                        item.icon = "fad fa-file-certificate";
                        item.column = 2;
                        break;*/
                    default:
                        item.action_time = "";
                        item.active = "";
                        break;
                }

                list.Add(item);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                var item = new StepViewModel();
                var i_track = new TrackingViewModel();
                switch (i)
                {
                    case 0:
                        i_track = tracking.Where(w => w.tracking_status == "PN").FirstOrDefault();
                        item.action = "งานเข้าระบบ";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = "done";
                        item.index = "first";
                        item.icon = "fad fa-desktop";
                        item.column = 3;
                        break;
                    case 1:
                        i_track = tracking.Where(w => w.tracking_status == "ST").FirstOrDefault();
                        item.action = "พนังานขับรถรับงาน";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = (i_track != null ? "done" : "");
                        item.icon = "fad fa-user-hard-hat";
                        item.column = 2;
                        break;
                    case 2:
                        i_track = tracking.Where(w => w.tracking_status == "AR").FirstOrDefault();
                        item.action = "รับสินค้า/ลงสินค้า";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = (i_track != null ? "done" : "");
                        item.icon = "fad fa-truck-loading";
                        item.column = 2;
                        break;
                    case 3:
                        i_track = tracking.Where(w => w.tracking_status == "DO").FirstOrDefault();
                        item.action = "จบงาน";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = (i_track != null ? "done" : "");
                        item.icon = "fad fa-garage";
                        item.column = 2;
                        break;
                    case 4:
                        i_track = tracking.Where(w => w.tracking_status == "BO").FirstOrDefault();
                        item.action = "รับเอกสาร";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MMM-yyyy HH:mm") : "");
                        item.active = (i_track != null ? "done" : "");
                        item.index = "last";
                        item.icon = "fad fa-file-certificate";
                        item.column = 2;
                        break;
                    default:
                        item.action_time = "";
                        item.active = "";
                        break;
                }

                list.Add(item);
            }
        }

        return list;
    }

    private string NextStatus(string current)
    {
        string next_status = "";
        switch (current)
        {
            case "PN":
                next_status = "ST";
                break;
            case "ST":
                next_status = "WP";
                break;
            case "WP":
                next_status = "AR";
                break;
            case "AR":
                next_status = "FN";
                break;
            case "FN":
                next_status = "DO";
                break;
            case "DO":
                next_status = "BO";
                break;
        }

        return next_status;
    }
}