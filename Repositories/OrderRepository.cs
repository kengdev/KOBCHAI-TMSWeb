
using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class OrderRepository
{
    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;
    private readonly AdvanceFundRepository _fundService;

    public OrderRepository(DbTMSContext context)
    {
        db = context;
        _codeService = new CodeRepository(context);
        _fundService = new AdvanceFundRepository(context);
    }

    public async Task<ViewJobListActive?> GetOrderById(int code)
    {
        return await db.ViewJobListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == code);
    }

    public async Task<ViewJobListActive?> GetOrderByCode(string code)
    {
        return await db.ViewJobListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.JobDetailCode == code);
    }

    public async Task<ViewJobListActive?> GetOrderBySearchDetail(string code)
    {
        return await db.ViewJobListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.JobDetailCode == code);
    }

    public async Task<TblJobMaster?> GetOrderDataByCode(string code)
    {
        return await db.TblJobMasters
            .AsNoTracking()
            .Include(i => i.CostCodeNavigation)
            .Include(i => i.TruckTypeCodeNavigation)
            .FirstOrDefaultAsync(w => w.JobCode == code);
    }

    public async Task<TblJobPlace?> GetOrderPlaceDataByCode(string code)
    {
        return await db.TblJobPlaces
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.JobCode == code);
    }

    public async Task<TblJobPlace?> GetJobPlaceDataByCode(string code)
    {
        return await GetOrderPlaceDataByCode(code);
    }

    public async Task<OrderPlaceViewModel?> GetPlaceByCode(string code)
    {
        return await db.TblPlaces
            .AsNoTracking()
            .Where(w => w.PlaceCode == code)
            .Select(s => new OrderPlaceViewModel
            {
                place_code = s.PlaceCode,
                place_name = s.PlaceName,
                place_type = s.PlaceType
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<OrderModel>> GetOrderBookingList()
    {
        var rows = await (
            from a in db.TblJobPlaces.AsNoTracking()
            join b in db.TblPlaces.AsNoTracking()
                on a.MissionCode equals b.PlaceCode
            join c in db.TblJobMasters.AsNoTracking()
                on a.JobCode equals c.JobCode
            join d in db.TblCustomers.AsNoTracking()
                on c.CustomerCode equals d.CustomerCode
            join e in db.TblBindingGenerates.AsNoTracking()
                on c.JobCode equals e.JobCode
            join f in db.TblCustomerCosts.AsNoTracking()
                on c.CostCode equals f.CostCode
            where e.IsEnabled == false
            select new
            {
                c.JobCode,
                f.JobName,
                e.JobDetailCode,
                c.BookingNo,
                c.JobInvoice,
                d.CustomerAcronym,
                c.ContainerSizeName,
                //c.ContainerType,
                c.ContainerAmount,
                c.JobDate,
                MissionName = b.PlaceName,
                c.TradeType
            })
            .OrderBy(o => o.JobDate)
            .ToListAsync();

        return rows.Select(s => new OrderModel
        {
            job_code = s.JobCode,
            job_detail_code = s.JobDetailCode,
            booking_no = s.BookingNo,
            job_invoice = s.JobInvoice,
            job_name = s.JobName,
            customer_name = s.CustomerAcronym,
            container_size = s.ContainerSizeName,
            //container_type = s.ContainerType,
            container_amount = s.ContainerAmount,
            job_date = s.JobDate.ToString("yyyy-MM-dd HH:mm"),
            job_date_value = s.JobDate,
            mission_name = s.MissionName,
            trade_type = s.TradeType
        }).ToList();
    }

    public async Task<CostModel?> GetCustomerCostByCode(string cost_code)
    {
        return await db.ViewCostListActives
            .AsNoTracking()
            .Where(w => w.CostCode == cost_code)
            .Select(s => new CostModel
            {
                cost_code = s.CostCode,
                cost_value = s.CostValue ?? 0,
                cost_remark = s.CostRemark,
                customer_code = s.CustomerCode,
                customer_name_th = s.CustomerNameTh,
                customer_acronym = s.CustomerAcronym,
                mission_code = s.MissionCode,
                mission_name = s.MissionName,
                mission_type = s.PlaceType,
                expressway_value = s.ExpresswayValue ?? 0
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<TblJobOcean>> GetOceanJob(string code)
    {
        return await db.TblJobOceans
            .AsNoTracking()
            .Where(w => w.JobCode == code)
            .ToListAsync();
    }

    public async Task<OrderPlaceViewModel?> GetYardPortDataByCode(string code)
    {
        return await GetPlaceByCode(code);
    }

    public async Task<List<DriverViewModel>> GetDriver()
    {
        return await db.TblDrivers
            .Where(w => w.IsEnabled == true)
            .AsNoTracking()
            .OrderBy(o => o.DriverFullname)
            .Select(s => new DriverViewModel
            {
                driver_code = s.DriverCode,
                fullname = s.DriverFullname
            })
            .ToListAsync();
    }

    public async Task<DriverViewModel?> GetTruckDriverByTruckCode(string code)
    {
        return await db.ViewDriverListActives
            .AsNoTracking()
            .Where(w => w.TruckCode == code)
            .Select(s => new DriverViewModel
            {
                driver_code = s.DriverCode,
                fullname = s.DriverFullname,
                truck_license = s.TruckLicense,
                truck_code = s.TruckCode
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<TblJobImg>> GetOrderImagesByCode(string code)
    {
        return await db.TblJobImgs
            .AsNoTracking()
            .Where(w => w.JobDetailCode == code)
            .ToListAsync();
    }

    public async Task<ResponseViewModel> OrderTruck(TblJobDetail model, decimal cashflow)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            var now = DateTime.Now;

            string advanceCode = _codeService.GetJobAdvanceCode();
            string cashingCode = _codeService.GetJobCashingCode();

            var supplierData = await db.TblSuppliers
                                .Where(w => w.SupplierCode == model.SupplierCode)
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

            var truck = await (
                from a in db.TblTrucks.AsNoTracking()
                join b in db.TblTruckDetails.AsNoTracking()
                    on a.TruckCode equals b.TruckCode
                where a.TruckCode == model.TruckCode
                select new
                {
                    truck_type = b.TruckTypegroupCode
                })
                .FirstOrDefaultAsync();

            var jobMaster = await db.TblJobMasters
                .FirstOrDefaultAsync(w => w.JobCode == model.JobCode);

            if (jobMaster == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลงาน");
            }

            var jobPlace = await db.TblJobPlaces
                .FirstOrDefaultAsync(w => w.JobCode == model.JobCode);

            if (jobPlace == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลสถานที่ของงาน");
            }

            jobMaster.LiftedAmount = jobMaster.LiftedAmount + 1;
            jobMaster.TruckTypeCode = truck != null ? truck.truck_type : "04";

            model.SupplierInvoice = supplierData.FleetTypeCode != "F1" ? "-" : model.JobDetailCode;
            model.ContainerNo = "-";
            model.SealNo = "-";
            model.TareWeight = 0;
            model.JobStatus = "W";
            model.TrackingStatus = "PN";
            model.TrailerLicense = "-";
            model.IsCanceled = false;
            model.IsDocument = false;
            model.IsBilling = false;
            model.IsPayment = false;
            db.TblJobDetails.Add(model);

            db.TblJobTrackings.Add(new TblJobTracking
            {
                JobDetailCode = model.JobDetailCode,
                TruckCode = model.TruckCode,
                DriverCode = model.DriverCode,
                SupplierCode = model.SupplierCode,
                TrackingStatus = "PN",
                TrackingDate = DateTime.Now,
                Lattitude = "0",
                Longitude = "0",
                Speed = "0"
            });

            string supplierCode;

            db.TblJobAllowances.Add(new TblJobAllowance
            {
                JobDetailCode = model.JobDetailCode,
                JobDate = DateOnly.FromDateTime(DateTime.Now),
                TruckCode = model.TruckCode,
                DriverCode = supplierData.FleetTypeCode == "F1" ? model.DriverCode : "26000000",
                DriverAllowance = supplierData.FleetTypeCode == "F1" ? await GetDriverCostAsync(jobPlace.MissionCode) : 0,
                SupplierCode = model.SupplierCode,
                SupplierAllowance = supplierData.FleetTypeCode == "F1" ? await GetFleetCostAsync(model.TruckCode, truck.truck_type,jobPlace.MissionCode) : await GetSupplierCostAsync(model.SupplierCode, jobPlace.MissionCode),
                RoundTrip = "A",
                IsComplete = false,
                IsWeeklypayment = false
            });

            var binding = await db.TblBindingGenerates.FindAsync(model.JobCode, model.JobDetailCode);

            if (binding != null)
            {
                binding.IsEnabled = true;
                binding.UpdateDate = now;
                binding.UpdateBy = model.CreateBy;
            }

            var cashflowData = new TblJobcashflow
            {
                JobDetailCode = model.JobDetailCode,
                TruckCode = model.TruckCode,
                DriverCode = model.DriverCode,
                CashFlow = cashflow,
                AdvanceRemark = "-",
                IsClearing = false
            };

            db.TblJobcashflows.Add(cashflowData);
            await db.SaveChangesAsync();

            AdvanceFundResult fundResult = await _fundService.PayAsync(model.JobDetailCode, cashflowData.Id, 2, cashflow, model.DriverCode,model.CreateBy);

            log.AddSystemLog(model.JobDetailCode, $"Add truck to job. [{model.JobCode}]", model.UpdateBy);

            await db.SaveChangesAsync();
            await tr.CommitAsync();

            return Success();
        }
        catch (Exception ex)
        {
            await tr.RollbackAsync();
            return Error(ex);
        }
    }

    public async Task<ResponseViewModel> AddContainerOrder(
        TblJobMaster orderToAdd,
        OrderPlaceModel placeJob,
        string txtOther)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var now = DateTime.Now;

            var container = await db.TblContainerSizes
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.ContainerSizeCode == orderToAdd.ContainerSizeCode);

            var customerCost = await db.TblCustomerCosts
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.CostCode == orderToAdd.CostCode);

            if (customerCost == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลค่าเที่ยวลูกค้า");
            }

            var payment = await db.TblCustomerRules
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.CustomerCode == customerCost.CustomerCode);

            orderToAdd.JobCode = _codeService.GetJobCode();
            orderToAdd.CustomerCode = customerCost.CustomerCode;
            orderToAdd.ContainerSizeName = GetContainerSizeName(orderToAdd.ContainerSizeCode, txtOther, container?.ContainerSizeName);
            orderToAdd.JobRemark = string.IsNullOrWhiteSpace(orderToAdd.JobRemark) ? "-" : orderToAdd.JobRemark;

            orderToAdd.JobInvoice = NormalizeUpper(orderToAdd.JobInvoice);
            orderToAdd.BookingNo = NormalizeUpper(orderToAdd.BookingNo);

            var place = await BuildContainerPlaceAsync(orderToAdd, placeJob, customerCost, payment, container);

            var bindings = BuildBindingGenerates(
                orderToAdd.JobCode,
                orderToAdd.ContainerAmount,
                orderToAdd.ContainerSizeCode,
                placeJob.acronym,
                orderToAdd.CreateBy,
                now);

            db.TblJobMasters.Add(orderToAdd);
            db.TblJobPlaces.Add(place);
            db.TblBindingGenerates.AddRange(bindings);

            await db.SaveChangesAsync();
            await tr.CommitAsync();

            return Success();
        }
        catch (Exception ex)
        {
            await tr.RollbackAsync();
            return Error(ex);
        }
    }

    public async Task<ResponseViewModel> EditContainerOrder(
        TblJobMaster orderToEdit,
        OrderPlaceModel placeJob,
        string txtOther)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            var now = DateTime.Now;
            string jobCode = orderToEdit.JobCode;

            var container = await db.TblContainerSizes
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.ContainerSizeCode == orderToEdit.ContainerSizeCode);

            var customerCost = await db.TblCustomerCosts
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.CostCode == orderToEdit.CostCode);

            if (customerCost == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลค่าเที่ยวลูกค้า");
            }

            var payment = await db.TblCustomerRules
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.CustomerCode == orderToEdit.CustomerCode);

            var place = await db.TblJobPlaces
                .FirstOrDefaultAsync(w => w.JobCode == jobCode);

            if (place == null)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูลสถานที่ของงาน");
            }

            await UpdateContainerPlaceAsync(place, orderToEdit, placeJob, customerCost, payment, container);
            await UpdateJobDetailAdvanceCashingAsync(jobCode, placeJob, orderToEdit.CostCode, place, orderToEdit.UpdateBy, now);

            orderToEdit.CustomerCode = customerCost.CustomerCode;
            orderToEdit.ContainerSizeName = GetContainerSizeName(orderToEdit.ContainerSizeCode, txtOther, container?.ContainerSizeName);
            orderToEdit.JobInvoice = NormalizeUpper(orderToEdit.JobInvoice);
            orderToEdit.BookingNo = NormalizeUpper(orderToEdit.BookingNo);

            db.TblJobMasters.Update(orderToEdit);

            await UpdateBinding2ShortAsync(orderToEdit.JobCode, orderToEdit.ContainerSizeCode);

            log.AddSystemLog(orderToEdit.JobCode, "Update job data.", orderToEdit.UpdateBy);

            await db.SaveChangesAsync();
            await tr.CommitAsync();

            return Success();
        }
        catch (Exception ex)
        {
            await tr.RollbackAsync();
            return Error(ex);
        }
    }

    public async Task<ResponseViewModel> RemoveOrder(
        string code,
        string job_detail_code,
        string update_by)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);
            var now = DateTime.Now;

            int count = await db.TblBindingGenerates
                .AsNoTracking()
                .CountAsync(w => w.JobCode == code);

            int removedBinding = await db.TblBindingGenerates
                .Where(w => w.JobCode == code &&
                            w.JobDetailCode == job_detail_code)
                .ExecuteDeleteAsync();

            int removeDetail = await db.TblJobDetails
                .Where(w => w.JobDetailCode == code)
                .ExecuteDeleteAsync();

            int removeJobTruck = await db.TblJobTrucks
                .Where(w => w.JobDetailCode == code)
                .ExecuteDeleteAsync();

            if (removedBinding == 0)
            {
                await tr.RollbackAsync();
                return NotFound("ไม่พบข้อมูล Order");
            }

            if (count == 1)
            {
                await db.TblJobPlaces
                    .Where(w => w.JobCode == code)
                    .ExecuteDeleteAsync();

                await db.TblJobMasters
                    .Where(w => w.JobCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsEnabled, false)
                        .SetProperty(s => s.UpdateBy, update_by)
                        .SetProperty(s => s.UpdateDate, now));
            }
            else
            {
                await db.TblJobMasters
                    .Where(w => w.JobCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.ContainerAmount, s => s.ContainerAmount - 1)
                        .SetProperty(s => s.UpdateBy, update_by)
                        .SetProperty(s => s.UpdateDate, now));
            }

            log.AddSystemLog(job_detail_code, $"Remove order. [job code : {code}]", update_by);

            await db.SaveChangesAsync();
            await tr.CommitAsync();

            return new ResponseViewModel
            {
                result = true,
                resultMessage = "จัดการข้อมูลเรียบร้อยแล้ว"
            };
        }
        catch (Exception ex)
        {
            await tr.RollbackAsync();
            return Error(ex);
        }
    }

    public async Task<decimal> GetDriverCost(string place_code)
    {
        return await GetDriverCostAsync(place_code);
    }

    /*public async Task<decimal> GetSupplierCost(string supplyer_code, string truck_type, string place_code)
    {
        return await GetSupplierCostAsync(supplyer_code, truck_type, place_code);
    }*/

    public async Task<ViewDriverListActive?> GetDriverCodeByTruck(string code)
    {
        return await db.ViewDriverListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.TruckCode == code);
    }

    private async Task<TblCustomerCost?> GetCustomerCostDataByCodeAsync(string code)
    {
        return await db.TblCustomerCosts
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.CostCode == code);
    }

    private async Task<TblPortCashing?> GetPortLiftAsync(string yardport_code, string container_kind_code)
    {
        return await db.TblPortCashings
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.YardportCode == yardport_code &&
                                      w.ContainerSizeCode == container_kind_code);
    }

    private async Task<TblYardCashing?> GetYardLiftAsync(string yardport_code)
    {
        return await db.TblYardCashings
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.YardportCode == yardport_code);
    }

    private async Task<TblYardport?> GetYardPortAsync(string yardport_code)
    {
        return await db.TblYardports
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.YardportCode == yardport_code);
    }

    private async Task<decimal> GetDriverCostAsync(string placeCode)
    {
        return await db.TblDriverRates
            .AsNoTracking()
            .Where(w => w.PlaceCode == placeCode)
            .Select(s => (decimal?)s.RateValue)
            .FirstOrDefaultAsync() ?? 0;
    }

    private async Task<decimal> GetSupplierCostAsync(string supplierCode, string placeCode)
    {
        return await db.TblSupplierRates
            .AsNoTracking()
            .Where(w => w.SupplierCode == supplierCode &&
                        w.PlaceCode == placeCode)
            .Select(s => (decimal?)s.CostRate)
            .FirstOrDefaultAsync() ?? 0;
    }

    private async Task<decimal> GetFleetCostAsync(
        string truckCode,
        string truckType,
        string placeCode)
    {

        var fleetRate = await (from fleetrate in db.TblFleetRates.AsNoTracking()
                                  join fleetsuppliertype in db.TblFleetSupplierTypes.AsNoTracking()
                                     on fleetrate.FleetSupplierTypeCode equals fleetsuppliertype.FleetSupplierTypeCode
                                  join fleetsupplier in db.TblFleetSuppliers.AsNoTracking()
                                     on fleetsuppliertype.FleetSupplierTypeCode equals fleetsupplier.FleetSuppplierTypeCode
                                  join truck in db.TblTruckDetails.AsNoTracking()
                                     on fleetsupplier.FleetSupplierCode equals truck.FleetSupplierCode
                                  where truck.TruckCode == truckCode && fleetrate.PlaceCode == placeCode
                                  select fleetrate).FirstOrDefaultAsync();

        if (fleetRate == null)
        {
            return 0;
        }

        return truckType switch
        {
            "01" => (decimal?)fleetRate.Truck4w ?? 0,
            "02" => (decimal?)fleetRate.Truck6w ?? 0,
            "03" => (decimal?)fleetRate.Truck10w ?? 0,
            _ => 0
        };
    }

    private async Task<TblJobPlace> BuildContainerPlaceAsync(
        TblJobMaster order,
        OrderPlaceModel placeJob,
        TblCustomerCost customerCost,
        TblCustomerRule? payment,
        TblContainerSize? container,
        bool isOcean = false)
    {
        var place = new TblJobPlace
        {
            StartCode = isOcean ? placeJob.txtOceanStartCode : placeJob.txtStartCode,
            MissionCode = customerCost.MissionCode,
            FinishCode = isOcean
                ? (placeJob.txtOceanFinishCode ?? "000000000")
                : (placeJob.txtFinishCode ?? "000000000"),
            JobCode = order.JobCode,
            IsStartyardport = placeJob.chkStartPortYard,
            IsFinishyardport = placeJob.chkFinishPortYard
        };

        if (order.TradeType == "IMPORT")
        {
            var startYardport = await GetYardPortAsync(place.StartCode);

            if (place.FinishCode == "000000000")
            {
                place.LiftoffValue = 0;
                place.LiftonValue = 0;
                place.YardValue = 0;
                place.PortValue = payment?.IsPort == true && placeJob.chkStartPortYard == true
                    ? startYardport?.ImportFee ?? 0
                    : 0;
            }
            else
            {
                var finishYardport = await GetYardPortAsync(place.FinishCode);
                var lift = await GetYardLiftAsync(place.FinishCode);

                place.LiftoffValue = 0;
                place.LiftonValue = payment?.IsLifton == true
                    ? IsShortContainer(container)
                        ? lift?.ShortliftoffFee ?? 0
                        : lift?.LongliftoffFee ?? 0
                    : 0;
                place.YardValue = payment?.IsYard == true ? finishYardport?.ImportFee ?? 0 : 0;
                place.PortValue = payment?.IsPort == true && placeJob.chkStartPortYard == true
                    ? startYardport?.ImportFee ?? 0
                    : 0;
            }
        }
        else
        {
            var startYardport = await GetYardPortAsync(place.StartCode);
            var finishYardport = await GetYardPortAsync(place.FinishCode);
            var lifton = await GetYardLiftAsync(place.StartCode);
            var liftoff = await GetPortLiftAsync(place.FinishCode, order.ContainerSizeCode);

            place.LiftonValue = payment?.IsLifton == true
                ? IsShortContainer(container)
                    ? lifton?.ShortliftonFee ?? 0
                    : lifton?.LongliftonFee ?? 0
                : 0;
            place.LiftoffValue = payment?.IsLiftoff == true ? liftoff?.ContainerFee ?? 0 : 0;
            place.YardValue = payment?.IsYard == true && placeJob.chkStartPortYard == true
                ? startYardport?.ImportFee ?? 0
                : 0;
            place.PortValue = payment?.IsPort == true ? finishYardport?.ExportFee ?? 0 : 0;
        }

        return place;
    }

    private async Task UpdateContainerPlaceAsync(
        TblJobPlace place,
        TblJobMaster order,
        OrderPlaceModel placeJob,
        TblCustomerCost customerCost,
        TblCustomerRule? payment,
        TblContainerSize? container)
    {
        place.StartCode = placeJob.txtStartCode;
        place.MissionCode = customerCost.MissionCode;
        place.FinishCode = placeJob.txtFinishCode ?? "000000000";
        place.JobCode = order.JobCode;

        if (order.TradeType == "IMPORT")
        {
            var startYardport = await GetYardPortAsync(place.StartCode);

            if (place.FinishCode == "000000000")
            {
                place.LiftoffValue = 0;
                place.LiftonValue = 0;
                place.YardValue = 0;
                place.PortValue = placeJob.chkStartPortYard == true ? startYardport?.ImportFee ?? 0 : 0;
            }
            else
            {
                var finishYardport = await GetYardPortAsync(place.FinishCode);
                var lift = await GetYardLiftAsync(place.FinishCode);

                place.LiftoffValue = 0;
                place.LiftonValue = payment?.IsLifton == true
                    ? IsShortContainer(container)
                        ? lift?.ShortliftoffFee ?? 0
                        : lift?.LongliftoffFee ?? 0
                    : 0;
                place.YardValue = finishYardport?.ImportFee ?? 0;
                place.PortValue = placeJob.chkStartPortYard == true ? startYardport?.ImportFee ?? 0 : 0;
            }
        }
        else
        {
            var startYardport = await GetYardPortAsync(place.StartCode);
            var finishYardport = await GetYardPortAsync(place.FinishCode);
            var lifton = await GetYardLiftAsync(place.StartCode);
            var liftoff = await GetPortLiftAsync(place.FinishCode, order.ContainerSizeCode);

            place.LiftonValue = payment?.IsLifton == true
                ? IsShortContainer(container)
                    ? lifton?.ShortliftonFee ?? 0
                    : lifton?.LongliftonFee ?? 0
                : 0;
            place.LiftoffValue = payment?.IsLiftoff == true ? liftoff?.ContainerFee ?? 0 : 0;
            place.YardValue = payment?.IsYard == true && placeJob.chkStartPortYard == true
                ? startYardport?.ImportFee ?? 0
                : 0;
            place.PortValue = payment?.IsPort == true ? finishYardport?.ExportFee ?? 0 : 0;
        }

        place.IsStartyardport = placeJob.chkStartPortYard;
        place.IsFinishyardport = placeJob.chkFinishPortYard;
    }

    private async Task UpdateJobDetailAdvanceCashingAsync(
        string jobCode,
        OrderPlaceModel placeJob,
        string costCode,
        TblJobPlace place,
        string updateBy,
        DateTime now)
    {
        var cost = await GetCustomerCostDataByCodeAsync(costCode);
        decimal costValue = cost?.CostValue ?? 0;

        var details = await db.TblJobDetails
            .Where(w => w.JobCode == jobCode)
            .ToListAsync();

        if (details.Count == 0)
        {
            return;
        }

        var detailCodes = details.Select(s => s.JobDetailCode).ToList();

        var advances = await db.TblJobadvances
            .Where(w => detailCodes.Contains(w.JobDetailCode))
            .ToListAsync();

        var cashings = await db.TblJobcashings
            .Where(w => detailCodes.Contains(w.JobDetailCode))
            .ToListAsync();

        var advanceMap = advances
            .GroupBy(g => g.JobDetailCode)
            .ToDictionary(k => k.Key, v => v.First());

        var cashingMap = cashings
            .GroupBy(g => g.JobDetailCode)
            .ToDictionary(k => k.Key, v => v.First());

        foreach (var detail in details)
        {
            detail.StartPlaceCode = placeJob.txtStartCode;
            detail.MissionPlaceCode = placeJob.txtMissionCode;
            detail.FinishPlaceCode = placeJob.txtFinishCode;
            detail.CostValue = costValue;
            detail.UpdateDate = now;
            detail.UpdateBy = updateBy;

            if (advanceMap.TryGetValue(detail.JobDetailCode, out var advance))
            {
                advance.AdvanceLifton = place.LiftonValue;
                advance.AdvanceLiftoff = place.LiftoffValue;
                advance.AdvancePort = place.PortValue;
                advance.AdvanceYard = place.YardValue;
                advance.AdvanceTotal = advance.AdvanceLifton + advance.AdvanceLiftoff + advance.AdvancePort + advance.AdvanceYard;
                advance.UpdateDate = now;
                advance.UpdateBy = updateBy;
            }
        }
    }

    private async Task UpdateBinding2ShortAsync(string jobCode, string containerSizeCode)
    {
        var bindings = await db.TblBindingGenerates
            .Where(w => w.JobCode == jobCode)
            .ToListAsync();

        foreach (var binding in bindings)
        {
            if (containerSizeCode == "06")
            {
                var round = binding.IndexCode % 2;
                binding.Is2short = round == 0;
            }
            else
            {
                binding.Is2short = false;
            }
        }
    }

    private List<TblBindingGenerate> BuildBindingGenerates(
        string jobCode,
        int containerAmount,
        string containerSizeCode,
        string acronym,
        string createBy,
        DateTime now)
    {
        var list = new List<TblBindingGenerate>();

        for (int i = 1; i <= containerAmount; i++)
        {
            var round = i % 2;

            list.Add(new TblBindingGenerate
            {
                IndexCode = i,
                Is2short = containerSizeCode == "06" && round == 0,
                JobCode = jobCode,
                JobDetailCode = _codeService.GetJobDetailCode(acronym),
                CreateDate = now,
                CreateBy = createBy,
                UpdateDate = now,
                UpdateBy = createBy
            });
        }

        return list;
    }

    private static string GetContainerSizeName(
        string containerSizeCode,
        string txtOther,
        string? containerSizeName)
    {
        if (containerSizeCode == "06")
        {
            return "20 GP";
        }

        return containerSizeCode == "99"
            ? txtOther
            : containerSizeName ?? "-";
    }

    private static bool IsShortContainer(TblContainerSize? container)
    {
        return container?.ContainerSizeGroup == "S";
    }

    private static string NormalizeUpper(string? value, string fallback = "-")
    {
        return string.IsNullOrWhiteSpace(value)
            ? fallback
            : value.ToUpper();
    }

    private static ResponseViewModel Success()
    {
        return new ResponseViewModel
        {
            result = true,
            resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว"
        };
    }

    private static ResponseViewModel NotFound(string message)
    {
        return new ResponseViewModel
        {
            result = false,
            resultMessage = message
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
