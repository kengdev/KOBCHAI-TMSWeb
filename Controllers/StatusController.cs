using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Wordprocessing;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;
using TMSWeb.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TMSWeb.Controllers;

[Authorize]
public class StatusController : Controller
{
    private readonly OrderRepository _orderService;
    private readonly JobRepository _jobService;
    private readonly TrackingRepository _trackingService;
    private string LoggedInUser => User.Identity.Name;

    public StatusController(OrderRepository orderService, JobRepository jobService, TrackingRepository trackingService)
    {
        _orderService = orderService;
        _jobService = jobService;
        _trackingService = trackingService;
    }

    public async Task<IActionResult> FloorInvoice(int code)
    {
        var model = new OrderStatusViewModel();
        var data = await _orderService.GetOrderById(code);
        if (data != null)
        {
            model.driver_fullname = data.DriverFullname;
            model.truck_license = data.TruckLicense;
            model.job_type = data.JobType;
            model.job_detail_code = data.JobDetailCode;
            model.booking_no = data.BookingNo;
            model.mission_name = data.MissionName;
            model.job_date = data.JobDate.Value;
            model.container_no = data.ContainerNo;
            model.seal_no = data.SealNo;
            model.trailer_license = data.TrailerLicense;
            model.tracking_status = data.TrackingStatus;
        }
        else
        {
            model.driver_fullname = "";
            model.truck_license = "";
            model.job_type = data.JobType;
            model.job_detail_code = data.JobDetailCode;
            model.booking_no = data.BookingNo;
            model.mission_name = data.MissionName;
            model.job_date = data.JobDate.Value;
            model.tracking_status = "-";
        }

        if (model.job_type == "FLOOR")
        {
            var tracking = this.GetTracking(data.JobDetailCode, data.TruckCode, data.DriverCode, data.SupplierCode, model.job_type);
            model.container_no = model.container_no != "-" ? model.container_no : "";
            model.seal_no = model.seal_no != "-" ? model.seal_no : "";
            model.trailer_license = model.trailer_license != "-" ? model.trailer_license : "";

            ViewBag.Tracking = tracking;
            return View(model);
        }
        else
        {
            return RedirectToAction("importinvoice", "status", new { @code = code });
        }
    }

    public async Task<IActionResult> ImportInvoice(int code)
    {
        var model = new OrderStatusViewModel();
        var data = await _orderService.GetOrderById(code);
        model.trade_type = data.TradeType;
        if (data != null)
        {
            model.driver_code = data.DriverCode;
            model.driver_fullname = data.DriverFullname;
            model.truck_code = data.TruckCode;
            model.truck_license = data.TruckLicense;
            model.supplier_code = data.SupplierCode;
            model.job_type = data.JobType;
            model.trade_type = data.TradeType;
            model.job_detail_code = data.JobDetailCode;
            model.booking_no = data.BookingNo;
            model.mission_name = data.MissionName;
            model.job_date = data.JobDate.Value;
            model.container_no = data.ContainerNo;
            model.seal_no = data.SealNo;
            model.trailer_license = data.TrailerLicense;
            model.tracking_status = data.TrackingStatus;
        }
        else
        {
            model.driver_fullname = "";
            model.driver_code = "26000000";
            model.truck_license = "";
            model.truck_code = "T000000";
            model.supplier_code = "SP26001";
            model.job_type = data.JobType;
            model.trade_type = data.TradeType;
            model.job_detail_code = data.JobDetailCode;
            model.booking_no = data.BookingNo;
            model.mission_name = data.MissionName;
            model.job_date = data.JobDate.Value;
            model.tracking_status = "-";
        }

        if (model.trade_type == "IMPORT")
        {
            var tracking = await this.GetTracking(data.JobDetailCode, data.TruckCode, data.DriverCode, data.SupplierCode, model.job_type);
            model.container_no = model.container_no != "-" ? model.container_no : "";
            model.seal_no = model.seal_no != "-" ? model.seal_no : "";
            model.trailer_license = model.trailer_license != "-" ? model.trailer_license : "";

            ViewBag.Tracking = tracking;
            return View(model);
        }
        else
        {
            return RedirectToAction("exportinvoice", "status", new { @code = code });
        }
    }

    public async Task<IActionResult> ExportInvoice(int code)
    {
        var model = new OrderStatusViewModel();
        var data = await _orderService.GetOrderById(code);
        if (data != null)
        {
            model.driver_code = data.DriverCode;
            model.driver_fullname = data.DriverFullname;
            model.truck_code = data.TruckCode;
            model.truck_license = data.TruckLicense;
            model.supplier_code = data.SupplierCode;
            model.job_type = data.JobType;
            model.trade_type = data.TradeType;
            model.job_detail_code = data.JobDetailCode;
            model.booking_no = data.BookingNo;
            model.mission_name = data.MissionName;
            model.job_date = data.JobDate.Value;
            model.container_no = data.ContainerNo;
            model.seal_no = data.SealNo;
            model.trailer_license = data.TrailerLicense;
            model.tracking_status = data.TrackingStatus;
        }
        else
        {
            model.driver_fullname = "";
            model.driver_code = "26000000";
            model.truck_license = "";
            model.truck_code = "T000000";
            model.supplier_code = "SP26001";
            model.job_type = data.JobType;
            model.trade_type = data.TradeType;
            model.job_detail_code = data.JobDetailCode;
            model.booking_no = data.BookingNo;
            model.mission_name = data.MissionName;
            model.job_date = data.JobDate.Value;
            model.tracking_status = "-";
        }

        if (model.trade_type == "EXPORT")
        {
            var tracking = await this.GetTracking(data.JobDetailCode, data.TruckCode, data.DriverCode, data.SupplierCode, model.job_type);
            model.container_no = model.container_no != "-" ? model.container_no : "";
            model.seal_no = model.seal_no != "-" ? model.seal_no : "";
            model.trailer_license = model.trailer_license != "-" ? model.trailer_license : "";

            ViewBag.Tracking = tracking;
            return View(model);
        }
        else
        {
            return RedirectToAction("floorinvoice", "status", new { @code = code });
        }
    }


    #region Sharing
    public async Task<IActionResult> UpdateST(UpdateTrackingJobModel model)
    {
        var response = await _trackingService.UpdateSTAsync(model);

        return Json(response);
    }

    public async Task<IActionResult> UpdateWP(UpdateTrackingJobModel model, IFormFile container, IFormFile seal)
    {
        model.container_pic = this.fileToBase64(container);
        model.seal_pic = this.fileToBase64(seal);

        var response = await _trackingService.UpdateWPAsync(model);

        return Json(response);
    }
    #endregion

    #region Import
    public async Task<IActionResult> UpdateImportAR(UpdateTrackingJobModel model)
    {
        var response = await _trackingService.UpdateImportARAsync(model);

        return Json(response);
    }

    public async Task<IActionResult> UpdateImportFN(UpdateTrackingJobModel model)
    {
        var response = await _trackingService.UpdateImportFNAsync(model);

        return Json(response);
    }

    public async Task<IActionResult> UpdateImportDO(UpdateTrackingJobModel model)
    {
        var response = await _trackingService.UpdateImportDOAsync(model);

        return Json(response);
    }
    #endregion

    #region Export
    /*public IActionResult UpdateExportAR(UpdateDriverInvoice model)
    {
        var inv = new JobDriverRepository(_context);
        bool result = inv.UpdateExportAR(model);

        return Json(result);
    }

    public IActionResult UpdateExportDO(UpdateDriverInvoice model)
    {
        var inv = new JobDriverRepository(_context);
        bool result = inv.UpdateExportDO(model);

        return Json(result);
    }
    #endregion

    #region Floor
    public IActionResult UpdateFloorAR(UpdateDriverInvoice model, List<IFormFile> file)
    {
        var inv = new JobDriverRepository(_context);
        List<byte[]> list = new List<byte[]>();

        foreach (var item in file)
        {
            byte[] img = this.fileToBase64(item);
            list.Add(img);
        }

        model.floor_images = list;
        bool result = inv.UpdateFloorAR(model);
        return Json(result);
    }

    public IActionResult UpdateFloorDO(UpdateDriverInvoice model)
    {
        var inv = new JobDriverRepository(_context);
        bool result = inv.UpdateFloorDO(model);

        return Json(result);
    }*/
    #endregion

    private byte[] fileToBase64(IFormFile file)
    {
        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            return fileBytes;
        }
    }

    private async Task<List<StepWizardViewModels>> GetTracking(string code, string truck_code, string driver_code, string supplier_code, string type)
    {
        var list = new List<StepWizardViewModels>();
        var tracking = await _jobService.GetListTrackingByJob(code, truck_code, driver_code, supplier_code);
        var count_tracking = tracking.Count;
        if (type != "FLOOR")
        {
            for (int i = 0; i < 5; i++)
            {
                var item = new StepWizardViewModels();
                var i_track = new TrackingViewModel();
                switch (i)
                {
                    case 0:
                        i_track = tracking.Where(w => w.tracking_status == "ST").FirstOrDefault();
                        item.action = "รับงาน";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MM-yyyy HH:mm") : "");
                        item.progress = (i_track != null ? "progress-bar" : "progress-bar-active");
                        item.wizard = (i_track != null ? "bs-wizard-dot-active" : "bs-wizard-dot");
                        item.icon = "fad fa-user-hard-hat";
                        list.Add(item);
                        break;
                    case 1:
                        i_track = tracking.Where(w => w.tracking_status == "WP").FirstOrDefault();
                        item.action = "รับตู้";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MM-yyyy HH:mm") : "");
                        item.progress = (i_track != null ? "progress-bar" : "progress-bar-active");
                        item.wizard = (i_track != null ? "bs-wizard-dot-active" : "bs-wizard-dot");
                        item.icon = "fad fa-container-storage";
                        list.Add(item);
                        break;
                    case 2:
                        i_track = tracking.Where(w => w.tracking_status == "AR").FirstOrDefault();
                        item.action = "รอบรรจุ/รอเปิด";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MM-yyyy HH:mm") : "");
                        item.progress = (i_track != null ? "progress-bar" : "progress-bar-active");
                        item.wizard = (i_track != null ? "bs-wizard-dot-active" : "bs-wizard-dot");
                        item.icon = "fad fa-truck-loading";
                        list.Add(item);
                        break;
                    case 3:
                        i_track = tracking.Where(w => w.tracking_status == "FN").FirstOrDefault();
                        item.action = "เปิดตู้ / บรรจุเรียบร้อย";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MM-yyyy HH:mm") : "");
                        item.progress = (i_track != null ? "progress-bar" : "progress-bar-active");
                        item.wizard = (i_track != null ? "bs-wizard-dot-active" : "bs-wizard-dot");
                        item.icon = "fad fa-truck-loading";
                        list.Add(item);
                        break;
                    case 4:
                        i_track = tracking.Where(w => w.tracking_status == "DO").FirstOrDefault();
                        item.action = "คืนตู้";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MM-yyyy HH:mm") : "");
                        item.progress = (i_track != null ? "progress-bar" : "progress-bar-active");
                        item.wizard = (i_track != null ? "bs-wizard-dot-active" : "bs-wizard-dot");
                        item.icon = "fad fa-truck-container";
                        list.Add(item);
                        break;
                    default:
                        item.action_time = "";
                        item.progress = "";
                        item.wizard = "";
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                var item = new StepWizardViewModels();
                var i_track = new TrackingViewModel();
                switch (i)
                {
                    case 0:
                        i_track = tracking.Where(w => w.tracking_status == "ST").FirstOrDefault();
                        item.action = "รับงาน";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MM-yyyy HH:mm") : "");
                        item.progress = (i_track != null ? "progress-bar" : "progress-bar-active");
                        item.wizard = (i_track != null ? "bs-wizard-dot-active" : "bs-wizard-dot");
                        item.icon = "fad fa-user-hard-hat";
                        list.Add(item);
                        break;
                    case 1:
                        i_track = tracking.Where(w => w.tracking_status == "AR").FirstOrDefault();
                        item.action = "รับสินค้า/ลงสินค้า";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MM-yyyy HH:mm") : "");
                        item.progress = (i_track != null ? "progress-bar" : "progress-bar-active");
                        item.wizard = (i_track != null ? "bs-wizard-dot-active" : "bs-wizard-dot");
                        item.icon = "fad fa-truck-loading";
                        list.Add(item);
                        break;
                    case 2:
                        i_track = tracking.Where(w => w.tracking_status == "DO").FirstOrDefault();
                        item.action = "จบงาน";
                        item.action_time = (i_track != null ? i_track.tracking_date.ToString("dd-MM-yyyy HH:mm") : "");
                        item.progress = (i_track != null ? "progress-bar" : "progress-bar-active");
                        item.wizard = (i_track != null ? "bs-wizard-dot-active" : "bs-wizard-dot");
                        item.icon = "fad fa-truck-container";
                        list.Add(item);
                        break;
                    default:
                        item.action_time = "";
                        item.progress = "";
                        item.wizard = "";
                        break;
                }
            }
        }

        return list;
    }
}