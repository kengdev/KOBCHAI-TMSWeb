using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace TMSWeb.Repositories;

public class DocRepository
{
    private readonly DbTMSContext db;

    public DocRepository(DbTMSContext context)
    {
        db = context;
    }

    public async Task<ViewOrderListActive> GetJobOrderByCode(string code)
    {
        var data = await db.ViewOrderListActives.Where(w => w.JobDetailCode == code).AsNoTracking().FirstOrDefaultAsync();
        return data;
    }

    public async Task<TblJobcashing> GetJobcashingBycode(string code)
    {
        var data = await db.TblJobcashings.Where(w => w.JobDetailCode == code).AsNoTracking().FirstOrDefaultAsync();
        return data;
    }

    public async Task<OrderFormViewModel> GetJobOrderData(string job_detail_code)
    {
        var data = await (from a in db.ViewJobListActives
                          where a.JobDetailCode == job_detail_code
                          select new OrderFormViewModel
                          {
                              job_code = a.JobCode,
                              job_detail_code = a.JobDetailCode,
                              truck_license = a.TruckLicense,
                              trailer_license = a.TrailerLicense,
                              driver_fullname = a.DriverFullname,
                              agent_name = a.AgentName,
                              customer_code = a.CustomerCode,
                              customer_name = a.CustomerNameTh,
                              job_name = a.JobName,
                              job_date = a.JobDate,
                              receipt_name = a.ReceiptName,
                              receipt_address = a.ReceiptAddress,
                              receipt_tax = a.ReceiptTax,
                              container_size = a.ContainerSizeName,
                              container_amount = a.ContainerAmount,
                              job_type = a.JobType,
                              start_name = a.StartName,
                              mission_name = a.MissionName,
                              closing_time = a.ClosingTime,
                              job_invoice = a.JobInvoice,
                              finish_name = a.FinishName,
                              booking_no = a.BookingNo,
                              agent_code = a.AgentCode,
                              container_no = a.ContainerNo,
                              seal_no = a.SealNo,
                              cash_flow = a.CashFlow.Value,
                              job_remark = a.JobRemark
                          }).AsNoTracking().FirstOrDefaultAsync();
        return data;
    }

    public async Task<DriverViewModel> GetTruckByJobCode(string code)
    {
        var data = await (from a in db.TblTrucks
                          join b in db.TblJobDetails on a.TruckCode equals b.TruckCode
                          join c in db.TblDrivers on b.DriverCode equals c.DriverCode
                          join d in db.TblTruckDetails on a.TruckCode equals d.TruckCode
                          join f in db.TblTruckTypes on d.TruckTypeCode equals f.TruckTypeCode
                          where b.JobDetailCode == code
                          select new DriverViewModel
                          {
                              truck_license = b.TruckCode != "T000000" ? a.TruckLicense : "",
                              fullname = b.DriverCode != "DV000000" ? c.DriverFullname : "",
                              truck_type_code = d.TruckTypeCode
                              //truck_typegroup_name = f.TruckTypegroupName,
                          }).AsNoTracking().FirstOrDefaultAsync();
        if (data != null)
        {
            return data;
        }
        else
        {
            var model = new DriverViewModel();
            model.truck_license = "";
            model.fullname = "";
            model.fleet_type = "F2";

            return model;
        }
    }
}