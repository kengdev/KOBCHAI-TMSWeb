using System;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TMSWeb.Repositories;
using TMSWeb.Models.ViewModels;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models;

namespace TMSWeb.Repositories;

public class DriverRepository
{
    private readonly DbTMSContext db;

    public DriverRepository(DbTMSContext _db)
    {
        db = _db;
    }

}