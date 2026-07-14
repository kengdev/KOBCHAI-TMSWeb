using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using TMSWeb.Models.ViewModels;
using TMSWeb.Repositories;

namespace TMSWeb.Controllers
{
    public class DocController : Controller
    {
        private readonly DocRepository _docService;

        public DocController(DocRepository docService)
        {
            _docService = docService;
        }

        public async Task<IActionResult> CashFlow(string code)
        {
            var model = new OrderFormViewModel();
            var data = await _docService.GetJobOrderData(code);
            var truck = await _docService.GetTruckByJobCode(code);

            model.truck_license = truck.truck_license;
            model.trailer_license = truck.trailer_license;
            model.truck_typegroup_code = data.truck_typegroup_code;
            model.customer_name = data.customer_name;
            model.job_name = data.job_name;
            model.other_address = data.receipt_address;
            model.port_no = data.port_no;
            model.industry = data.industry;
            model.job_detail_code = data.job_detail_code;
            model.container_size_code = data.container_size_code;
            model.container_size = data.container_size;
            model.job_type = data.job_type;
            model.mission_name = data.mission_name;
            model.receipt_name = data.receipt_name;
            model.receipt_tax = data.receipt_tax;
            model.receipt_address = data.receipt_address;
            model.customer_name = data.customer_name;
            model.job_name = data.job_name;
            model.booking_no = data.booking_no;
            model.agent_name = data.agent_name;
            model.start_name = data.start_name;
            model.finish_name = data.finish_name;
            model.mission_name = data.mission_name;
            model.container_amount = data.container_amount;
            model.job_date = data.job_date;
            model.loading_date = data.loading_date;
            model.job_invoice = data.job_invoice;
            model.job_remark = data.job_remark;
            model.container_amount = data.container_amount;
            //model.truck_typegroup_name = truck.truck_typegroup_name;
            model.truck_license = data.truck_license;
            model.driver_fullname = data.driver_fullname;
            model.cash_flow = data.cash_flow;

            return new ViewAsPdf("_CashFlow", model)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 },
                CustomSwitches = "--disable-smart-shrinking"
            };
        }

        public async Task<IActionResult> OrderAssignment(string code)
        {
            var model = new OrderFormViewModel();
            var data = await _docService.GetJobOrderByCode(code);
            var cashing = await _docService.GetJobcashingBycode(code);
            var truck = await _docService.GetTruckByJobCode(code);
            string booking = "";
            string delivery = "";
            delivery = data.JobDate.ToString("HH:mm");
            booking = data.BookingNo;

            model.container_amount = data.ContainerAmount;
            model.other_address = data.OtherAddress;
            model.fleet_type = truck.fleet_type;
            model.truck_license = truck.truck_license;
            model.trailer_license = truck.trailer_license;
            model.truck_typegroup_code = data.TruckTypegroupCode;
            model.driver_fullname = truck.fullname;
            model.port_no = data.PortCode;
            model.industry = data.IndustryNameTh;
            model.job_detail_code = data.JobDetailCode;
            model.container_size_code = data.ContainerSizeCode;
            model.container_size = data.ContainerSizeName;
            model.job_type = data.JobType;
            model.mission_name = data.MissionName;
            model.receipt_name = data.ReceiptName;
            model.receipt_tax = data.ReceiptTax;
            model.receipt_address = data.ReceiptAddress;
            model.booking_no = booking;
            model.agent_code = data.AgentCode;
            model.start_name = data.LiftonName;
            model.finish_name = data.LiftoffName;
            model.closing_time = data.ClosingTime;
            model.job_date = data.JobDate;
            model.loading_date = data.LoadingDate;
            model.delivery_date = delivery;
            model.job_invoice = data.JobInvoice;
            model.job_remark = data.JobRemark;
            model.is2short = data.Is2short;
            model.fuel_rate = 0;
            model.lift = data.LiftonValue;
            model.port = data.PortValue;
            model.yard = data.YardValue;
            //model.express_way = data.ExpresswayValue;
            //model.truck_typegroup_name = truck.truck_typegroup_name;

            //model.barcode = qr.JobCodeURL(data.HashCode);

            return new ViewAsPdf("_OrderAssignment", model)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 },
                CustomSwitches = "--disable-smart-shrinking"
            };
        }
    }

}