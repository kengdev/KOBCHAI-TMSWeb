using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class CustomerRepository
{
    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;

    public CustomerRepository(DbTMSContext context, CodeRepository codeService)
    {
        db = context;
        _codeService = codeService;
    }

    public async Task<TblCustomer?> GetCustomerByCode(string customer_code)
    {
        return await db.TblCustomers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.CustomerCode == customer_code);
    }

    public async Task<List<CustomerViewModel>> GetCustomerDataList()
    {
        return await db.TblCustomers
            .AsNoTracking()
            .Where(w => w.IsEnabled == true)
            .Select(s => new CustomerViewModel
            {
                customer_code = s.CustomerCode,
                customer_name_en = s.CustomerNameEn,
                customer_acronym = s.CustomerAcronym,
                is_actived = s.IsActived
            })
            .ToListAsync();
    }

    public async Task<CustomerViewModel?> GetCustomerDataByCode(string code)
    {
        return await db.TblCustomers
            .AsNoTracking()
            .Where(w => w.CustomerCode == code)
            .Select(s => new CustomerViewModel
            {
                customer_code = s.CustomerCode,
                customer_name_th = s.CustomerNameTh,
                customer_address = s.CustomerAddress,
                customer_tax = s.CustomerVatNo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TblCustomerRule?> GetRuleByCode(string code)
    {
        return await db.TblCustomerRules
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.CustomerCode == code);
    }

    public async Task<ResponseViewModel> AddCustomer(TblCustomer customerToAdd, TblCustomerRule paymentToAdd)
    {
        try
        {
            paymentToAdd.CustomerCode = customerToAdd.CustomerCode;
            customerToAdd.TblCustomerRule = paymentToAdd;

            db.TblCustomers.Add(customerToAdd);

            await db.SaveChangesAsync();

            return Success();
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public async Task<ResponseViewModel> EditCustomer(TblCustomer customerToEdit, TblCustomerRule ruleToEdit)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            string customerCode = customerToEdit.CustomerCode;

            await db.TblCustomerAddresses
                .Where(w => w.CustomerCode == customerCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.TransportName, customerToEdit.CustomerNameTh)
                    .SetProperty(s => s.TransportAddress, customerToEdit.CustomerAddress)
                    .SetProperty(s => s.AdvanceName, customerToEdit.CustomerNameTh)
                    .SetProperty(s => s.AdvanceAddress, customerToEdit.CustomerAddress));

            db.Entry(customerToEdit).State = EntityState.Modified;

            ruleToEdit.IsTransportOther = true;
            db.Entry(ruleToEdit).State = EntityState.Modified;

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

    public async Task<ResponseViewModel> RemoveCustomer(string customer_code, string user)
    {
        return await UpdateCustomerStatusAsync(
            customer_code,
            user,
            isActived: false,
            isEnabled: false
        );
    }

    public async Task<ResponseViewModel> ActiveCustomer(string customer_code, string user)
    {
        return await UpdateCustomerStatusAsync(
            customer_code,
            user,
            isActived: true,
            isEnabled: null
        );
    }

    public async Task<ResponseViewModel> InactiveCustomer(string customer_code, string user)
    {
        return await UpdateCustomerStatusAsync(
            customer_code,
            user,
            isActived: false,
            isEnabled: null
        );
    }

    public async Task<bool> IsVatNoDuplicate(string vat_no)
    {
        return await db.TblCustomers
            .AsNoTracking()
            .AnyAsync(w => w.CustomerVatNo == vat_no &&
                           w.IsEnabled == true);
    }

    public async Task<bool> IsAcronymDuplicate(string name)
    {
        return await db.TblCustomers
            .AsNoTracking()
            .AnyAsync(w => w.CustomerAcronym == name &&
                           w.IsEnabled == true);
    }

    public async Task<string> GetCode(string type)
    {
        return await _codeService.GetCustomerCode(type);
    }

    private async Task<ResponseViewModel> UpdateCustomerStatusAsync(
        string customerCode,
        string username,
        bool isActived,
        bool? isEnabled)
    {
        try
        {
            DateTime now = DateTime.Now;

            int affectedRows;

            if (isEnabled.HasValue)
            {
                affectedRows = await db.TblCustomers
                    .Where(w => w.CustomerCode == customerCode)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.IsEnabled, isEnabled.Value)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }
            else
            {
                affectedRows = await db.TblCustomers
                    .Where(w => w.CustomerCode == customerCode)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }

            if (affectedRows == 0)
            {
                return new ResponseViewModel
                {
                    result = false,
                    resultMessage = "ไม่พบข้อมูลลูกค้า"
                };
            }

            return Success();
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    private static ResponseViewModel Success()
    {
        return new ResponseViewModel
        {
            result = true,
            resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว"
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