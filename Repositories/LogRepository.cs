using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;

namespace TMSWeb.Repositories;
public class LogRepository
{
    private DbTMSContext db;

    public LogRepository(DbTMSContext context)
    {
        db = context;
    }

    public void AddSystemLog(string code, string action, string username)
    {
        LogSystem log = new LogSystem();
        log.LogReferenceCode = code;
        log.LogAction = action;
        log.CreateDate = DateTime.Now;
        log.CreateBy = username;

        db.LogSystems.Add(log);
    }
}