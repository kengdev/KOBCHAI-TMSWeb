using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class AgentRepository
{
    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;

    public AgentRepository(DbTMSContext context, CodeRepository codeService)
    {
        db = context;
        _codeService = codeService;
    }

    public async Task<List<AgentViewModel>> GetAgentDataList()
    {
        return await db.TblAgents
            .AsNoTracking()
            .Where(w => w.AgentCode != "1900" &&
                        w.IsEnabled == true)
            .OrderBy(o => o.AgentName)
            .Select(s => new AgentViewModel
            {
                code = s.AgentCode,
                name = s.AgentName,
                is_actived = s.IsActived
            })
            .ToListAsync();
    }

    public async Task<TblAgent?> GetAgentDataByCode(string code)
    {
        return await db.TblAgents
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.AgentCode == code);
    }

    public async Task<ResponseViewModel> AddData(TblAgent dataToAdd)
    {
        try
        {
            dataToAdd.AgentCode = _codeService.GetAgentCode();

            db.TblAgents.Add(dataToAdd);

            await db.SaveChangesAsync();

            return Success();
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public async Task<ResponseViewModel> EditAgentData(TblAgent dataToEdit)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var log = new LogRepository(db);

            db.Entry(dataToEdit).State = EntityState.Modified;

            log.AddSystemLog(dataToEdit.AgentName, "Update agent.", dataToEdit.UpdateBy);

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

    public async Task<ResponseViewModel> Remove(string code, string username)
    {
        return await UpdateAgentStatusAsync(
            code,
            username,
            isActived: false,
            isEnabled: false,
            logAction: "Remove agent."
        );
    }

    public async Task<ResponseViewModel> Active(string code, string username)
    {
        return await UpdateAgentStatusAsync(
            code,
            username,
            isActived: true,
            isEnabled: null,
            logAction: "Active agent."
        );
    }

    public async Task<ResponseViewModel> Inactive(string code, string username)
    {
        return await UpdateAgentStatusAsync(
            code,
            username,
            isActived: false,
            isEnabled: null,
            logAction: "Inactive agent."
        );
    }

    private async Task<ResponseViewModel> UpdateAgentStatusAsync(
        string code,
        string username,
        bool isActived,
        bool? isEnabled,
        string logAction)
    {
        await using var tr = await db.Database.BeginTransactionAsync();

        try
        {
            var agent = await db.TblAgents
                .AsNoTracking()
                .Where(w => w.AgentCode == code)
                .Select(s => new
                {
                    s.AgentCode,
                    s.AgentName
                })
                .FirstOrDefaultAsync();

            if (agent == null)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            DateTime now = DateTime.Now;
            int affectedRows;

            if (isEnabled.HasValue)
            {
                affectedRows = await db.TblAgents
                    .Where(w => w.AgentCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.IsEnabled, isEnabled.Value)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }
            else
            {
                affectedRows = await db.TblAgents
                    .Where(w => w.AgentCode == code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.IsActived, isActived)
                        .SetProperty(s => s.UpdateDate, now)
                        .SetProperty(s => s.UpdateBy, username));
            }

            if (affectedRows == 0)
            {
                await tr.RollbackAsync();
                return NotFound();
            }

            var log = new LogRepository(db);
            log.AddSystemLog(agent.AgentName, logAction, username);

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

    private static ResponseViewModel Success()
    {
        return new ResponseViewModel
        {
            result = true,
            resultMessage = "บันทึกข้อมูลเรียบร้อยแล้ว"
        };
    }

    private static ResponseViewModel NotFound()
    {
        return new ResponseViewModel
        {
            result = false,
            resultMessage = "ไม่พบข้อมูล Agent"
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