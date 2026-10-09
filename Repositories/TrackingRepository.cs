using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class TrackingRepository
{
    private readonly DbTMSContext db;

    public TrackingRepository(DbTMSContext context)
    {
        db = context;
    }

    public async Task<ViewJobListActive?> GetOrderByCodeAsync(string code)
    {
        return await db.ViewJobListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.HashCode == code);
    }

    public async Task<ViewOrderListActive?> GetJobOrderByCodeAsync(string code)
    {
        return await db.ViewOrderListActives
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.HashCode == code);
    }

    public async Task<ResponseViewModel> UpdateSTAsync(UpdateTrackingJobModel model)
    {
        try
        {
            var data = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (data == null)
                return NotFoundResponse("ไม่พบข้อมูลงาน");

            data.TrackingStatus = "ST";

            model.status = data.TrackingStatus;
            db.TblJobTrackings.Add(CreateTracking(model));

            await db.SaveChangesAsync();

            return SuccessResponse();
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    public async Task<ResponseViewModel> UpdateWPAsync(UpdateTrackingJobModel model)
    {
        try
        {
            var data = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (data == null)
                return NotFoundResponse("ไม่พบข้อมูลงาน");

            data.TrackingStatus = "WP";
            data.ContainerNo = model.container_no.ToUpper();
            data.SealNo = model.seal_no.ToUpper();
            data.TrailerLicense = model.trailer_license;
            data.TareWeight = model.tare_weight;

            model.status = data.TrackingStatus;
            db.TblJobTrackings.Add(CreateTracking(model));

            db.TblJobImgs.AddRange(
                new TblJobImg
                {
                    JobDetailCode = model.job_detail_code,
                    JobType = "container",
                    JobDetailImg = model.container_pic
                },
                new TblJobImg
                {
                    JobDetailCode = model.job_detail_code,
                    JobType = "seal",
                    JobDetailImg = model.seal_pic
                });

            await db.SaveChangesAsync();

            return SuccessResponse();
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    public async Task<ResponseViewModel> UpdateImportARAsync(UpdateTrackingJobModel model)
    {
        try
        {
            var data = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (data == null)
                return NotFoundResponse("ไม่พบข้อมูลงาน");

            data.TrackingStatus = "AR";

            model.status = data.TrackingStatus;
            db.TblJobTrackings.Add(CreateTracking(model));

            await db.SaveChangesAsync();

            return SuccessResponse();
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    public async Task<ResponseViewModel> UpdateImportFNAsync(UpdateTrackingJobModel model)
    {
        try
        {
            var data = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (data == null)
                return NotFoundResponse("ไม่พบข้อมูลงาน");

            data.TrackingStatus = "FN";

            model.status = data.TrackingStatus;
            db.TblJobTrackings.Add(CreateTracking(model));

            await db.SaveChangesAsync();

            return SuccessResponse();
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    public async Task<ResponseViewModel> UpdateImportDOAsync(UpdateTrackingJobModel model)
    {
        try
        {
            var data = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (data == null)
                return NotFoundResponse("ไม่พบข้อมูลงาน");

            var advance = await db.TblJobadvances
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (advance == null)
                return NotFoundResponse("ไม่พบข้อมูลเงินสำรองของงาน");

            var cashing = await db.TblJobcashings
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (cashing == null)
                return NotFoundResponse("ไม่พบข้อมูลค่าใช้จ่ายของงาน");

            data.TrackingStatus = "DO";


            model.status = data.TrackingStatus;
            db.TblJobTrackings.Add(CreateTracking(model));

            await db.SaveChangesAsync();

            return SuccessResponse();
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    public async Task<ResponseViewModel> UpdateExportARAsync(UpdateTrackingJobModel model)
    {
        try
        {
            var data = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (data == null)
                return NotFoundResponse("ไม่พบข้อมูลงาน");

            var advance = await db.TblJobadvances
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (advance == null)
                return NotFoundResponse("ไม่พบข้อมูลเงินสำรองของงาน");

            var cashing = await db.TblJobcashings
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (cashing == null)
                return NotFoundResponse("ไม่พบข้อมูลค่าใช้จ่ายของงาน");

            data.TrackingStatus = "AR";

            model.status = data.TrackingStatus;
            db.TblJobTrackings.Add(CreateTracking(model));

            await db.SaveChangesAsync();

            return SuccessResponse();
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    public async Task<ResponseViewModel> UpdateExportDOAsync(UpdateTrackingJobModel model)
    {
        try
        {
            var data = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (data == null)
                return NotFoundResponse("ไม่พบข้อมูลงาน");

            var advance = await db.TblJobadvances
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (advance == null)
                return NotFoundResponse("ไม่พบข้อมูลเงินสำรองของงาน");

            var cashing = await db.TblJobcashings
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (cashing == null)
                return NotFoundResponse("ไม่พบข้อมูลค่าใช้จ่ายของงาน");

            data.TrackingStatus = "DO";

            model.status = data.TrackingStatus;
            db.TblJobTrackings.Add(CreateTracking(model));

            await db.SaveChangesAsync();

            return SuccessResponse();
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    public async Task<ResponseViewModel> UpdateFloorARAsync(UpdateTrackingJobModel model)
    {
        try
        {
            var data = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (data == null)
                return NotFoundResponse("ไม่พบข้อมูลงาน");

            data.TrackingStatus = "AR";

            model.status = data.TrackingStatus;
            db.TblJobTrackings.Add(CreateTracking(model));

            if (model.floor_images != null && model.floor_images.Any())
            {
                var images = model.floor_images.Select(item => new TblJobImg
                {
                    JobDetailCode = model.job_detail_code,
                    JobType = "floor",
                    JobDetailImg = item
                });

                db.TblJobImgs.AddRange(images);
            }

            await db.SaveChangesAsync();

            return SuccessResponse();
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    public async Task<ResponseViewModel> UpdateFloorDOAsync(UpdateTrackingJobModel model)
    {
        try
        {
            var data = await db.TblJobDetails
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (data == null)
                return NotFoundResponse("ไม่พบข้อมูลงาน");

            var advance = await db.TblJobadvances
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (advance == null)
                return NotFoundResponse("ไม่พบข้อมูลเงินสำรองของงาน");

            var cashing = await db.TblJobcashings
                .FirstOrDefaultAsync(w => w.JobDetailCode == model.job_detail_code);

            if (cashing == null)
                return NotFoundResponse("ไม่พบข้อมูลค่าใช้จ่ายของงาน");

            data.TrackingStatus = "DO";

            model.status = data.TrackingStatus;
            db.TblJobTrackings.Add(CreateTracking(model));

            await db.SaveChangesAsync();

            return SuccessResponse();
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    private static TblJobTracking CreateTracking(
        UpdateTrackingJobModel model)
    {
        return new TblJobTracking
        {
            JobDetailCode = model.job_detail_code,
            TruckCode = model.truck_code,
            DriverCode = model.driver_code,
            SupplierCode = model.supplier_code,
            TrackingStatus = model.status,
            TrackingDate = DateTime.Now,
            Lattitude = model.Lat,
            Longitude = model.Lon,
            Speed = "0"
        };
    }

    private static ResponseViewModel SuccessResponse()
    {
        return new ResponseViewModel
        {
            result = true,
            resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว"
        };
    }

    private static ResponseViewModel NotFoundResponse(string message)
    {
        return new ResponseViewModel
        {
            result = false,
            resultMessage = message
        };
    }

    private static ResponseViewModel ErrorResponse(Exception ex)
    {
        return new ResponseViewModel
        {
            result = false,
            resultMessage = "<div>เกิดข้อผิดพลาดระหว่างการทำงาน</div><div style='margin-top:-30px'> กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ</div>",
            errorException = ex
        };
    }
}
