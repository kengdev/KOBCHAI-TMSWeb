using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Repositories;

public class BudgetRepository
{
    private readonly DbTMSContext db;
    private readonly CodeRepository _codeService;

    public BudgetRepository(DbTMSContext context)
    {
        db = context;
    }

    public async Task<AdvanceFund> GetAdvanceFundData()
    {
        var data = await db.AdvanceFunds.AsNoTracking()
                            .Include(i => i.AdvanceFundTransactions.OrderByDescending(ot => ot.TransactionId))
                            .Include(i => i.AdvanceRequests)
                            .FirstOrDefaultAsync();

        return data;
    }
}