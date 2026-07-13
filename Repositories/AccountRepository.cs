using System.Text;
using System.Security.Cryptography;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;
using System.Transactions;

namespace TMSWeb.Repositories;
public class AccountRepository
{
    private DbTMSContext db;

    public AccountRepository(DbTMSContext context)
    {
        db = context;
    }

    public TblAccount GetTblAccount(string username)
    {
        var data = db.TblAccounts.Where(w => w.AccUsername == username).FirstOrDefault();
        return data;
    }

    public ProfileViewModel GetAccountByUser(string username)
    {
        var data = (from a in db.TblAccounts
                    join d in db.TblGroups on a.GroupCode equals d.GroupCode
                    where a.AccUsername == username
                    select new ProfileViewModel
                    {
                        username = username,
                        is_active = a.IsActived,
                        password = a.AccPassword,
                        group_code = a.GroupCode,
                        group_name = d.GroupName,
                    }).FirstOrDefault();
        return data;
    }

    public ProfileViewModel GetAccountByLineId(string id)
    {
        var data = (from a in db.TblAccounts
                    join d in db.TblGroups on a.GroupCode equals d.GroupCode
                    where a.LineId == id && a.IsActived == true
                    select new ProfileViewModel
                    {
                        username = a.AccUsername,
                        is_active = a.IsEnabled,
                        password = a.AccPassword,
                        group_code = a.GroupCode,
                        group_name = d.GroupName,
                    }).FirstOrDefault();
        return data;
    }

    public List<ProfileViewModel> GetAccountList()
    {
        var data = (from a in db.TblAccounts
                    join d in db.TblGroups on a.GroupCode equals d.GroupCode
                    where a.IsEnabled == true && a.AccUsername != "admin"
                    select new ProfileViewModel
                    {
                        username = a.AccUsername,
                        is_active = a.IsActived,
                        group_code = a.GroupCode,
                        group_name = d.GroupName
                    }).ToList();
        return data;
    }

    public ProfileViewModel GetCustomerByUser(string username)
    {
        var data = (from a in db.SeCustomerAccounts
                    where a.CusUsername == username
                    select new ProfileViewModel
                    {
                        usercode = a.CustomerCode
                    }).FirstOrDefault();
        return data;
    }

    public ResponseViewModel Add(TblAccount model)
    {
        var response = new ResponseViewModel();
        try
        {
            model.AccPassword = this.SaltHash(model.AccPassword);
            db.TblAccounts.Add(model);
            db.SaveChanges();
            response.result = true;
            response.resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
        }
        catch (Exception ex )
        {
            response.result = false;
            response.resultMessage = ex.Message;
        }

        return response;
    }

    public ResponseViewModel Edit(TblAccount model)
    {
        var response = new ResponseViewModel();
        try
        {
            var data = db.TblAccounts.FirstOrDefault(w => w.AccUsername == model.AccUsername);
            data.AccFullname = model.AccFullname;
            data.LineId = model.LineId;
            data.GroupCode = model.GroupCode;
            data.UpdateDate = model.UpdateDate;
            data.UpdateBy = model.UpdateBy;

            db.SaveChanges();
            response.result = true;
            response.resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
        }
        catch (Exception ex)
        {
            response.result = false;
            response.resultMessage = ex.Message;
        }

        return response;
    }

    public ResponseViewModel ChangePassword(string username, string password, string update_by)
    {
        var response = new ResponseViewModel();
        try
        {
            var data = db.TblAccounts.FirstOrDefault(w => w.AccUsername == username);
            data.AccPassword = this.SaltHash(password);
            data.UpdateDate = DateTime.Now;
            data.UpdateBy = update_by;

            db.SaveChanges();
            response.result = true;
            response.resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
        }
        catch (Exception ex)
        {
            response.result = false;
            response.resultMessage = ex.Message;
        }

        return response;
    }

    public ResponseViewModel Remove(string code, string user)
    {
        ResponseViewModel response = new ResponseViewModel();
        using (TransactionScope tr = new TransactionScope())
        {
            try
            {
                var item = db.TblAccounts.Find(code);
                item.IsActived = false;
                item.IsEnabled = false;
                item.UpdateDate = DateTime.Now;
                item.UpdateBy = user;

                db.SaveChanges();
                tr.Complete();
                response.result = true;
                response.resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
            }
            catch (Exception ex)
            {
                tr.Dispose();
                db.Dispose();
                response.result = false;
                response.resultMessage = "<div>เกิดข้อผิดพลาดระหว่างการทำงาน</div><div style='margin-top:-30px'> กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ</div>";
                response.errorException = ex;
            }
        }

        return response;
    }

    public ResponseViewModel Active(string code, string user)
    {
        ResponseViewModel response = new ResponseViewModel();
        using (TransactionScope tr = new TransactionScope())
        {
            try
            {
                var item = db.TblAccounts.Find(code);
                item.IsActived = true;
                item.UpdateDate = DateTime.Now;
                item.UpdateBy = user;

                db.SaveChanges();
                tr.Complete();
                response.result = true;
                response.resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
            }
            catch (Exception ex)
            {
                tr.Dispose();
                db.Dispose();
                response.result = false;
                response.resultMessage = "<div>เกิดข้อผิดพลาดระหว่างการทำงาน</div><div style='margin-top:-30px'> กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ</div>";
                response.errorException = ex;
            }
        }

        return response;
    }

    public ResponseViewModel Inactive(string code, string user)
    {
        ResponseViewModel response = new ResponseViewModel();
        using (TransactionScope tr = new TransactionScope())
        {
            try
            {
                var item = db.TblAccounts.Find(code);
                item.IsActived = false;
                item.UpdateDate = DateTime.Now;
                item.UpdateBy = user;

                db.SaveChanges();
                tr.Complete();
                response.result = true;
                response.resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว";
            }
            catch (Exception ex)
            {
                tr.Dispose();
                db.Dispose();
                response.result = false;
                response.resultMessage = "<div>เกิดข้อผิดพลาดระหว่างการทำงาน</div><div style='margin-top:-30px'> กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ</div>";
                response.errorException = ex;
            }
        }

        return response;
    }

    private string SaltHash(string input)
    {
        Rfc2898DeriveBytes PBKDF2_hash = new Rfc2898DeriveBytes(input, Encoding.ASCII.GetBytes("rm4fSDh0sofK"), 20000);
        HMACSHA256 HMACSHA256_hash = new HMACSHA256();
        //System.Text.Encoding.UTF8.GetString(bytes);
        string hPassword = Convert.ToBase64String(PBKDF2_hash.GetBytes(32));

        return hPassword;
    }
}