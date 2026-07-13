using TMSWeb.Models.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TMSWeb.Services
{
    public class DataHub : Hub
    {
        // เอาไว้ให้ client ส่ง message กลับมาก็ได้
        public async Task SendDataToClient(string data)
        {
            await Clients.All.SendAsync("ReceiveData", data);
        }
    }

    // BackgroundService สำหรับดึงข้อมูลจาก MSSQL แล้ว broadcast ผ่าน Hub
    public class DataHubService : BackgroundService
    {
        private readonly ILogger<DataHubService> _logger;
        private readonly IHubContext<DataHub> _hub;
        private readonly string _connectionString;
        private long _lastId = 0;

        public DataHubService(
            ILogger<DataHubService> logger,
            IHubContext<DataHub> hub,
            IConfiguration config)
        {
            _logger = logger;
            _hub = hub;
            _connectionString = config.GetConnectionString("servMatang");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DataHubService started...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var conn = new SqlConnection(_connectionString);
                    await conn.OpenAsync(stoppingToken);

                    // อ่านค่า id ล่าสุด (ไม่ให้ยิงย้อนหลัง)
                    using (var cmdInit = new SqlCommand("SELECT ISNULL(MAX(id),0) FROM dbo.iclock_transaction;", conn))
                    {
                        if (_lastId == 0)
                            _lastId = Convert.ToInt64(await cmdInit.ExecuteScalarAsync(stoppingToken));
                    }

                    // ดึงแถวใหม่
                    using var cmd = new SqlCommand(@"
                        SELECT TOP (1)
                            t.id,
                            t.terminal_sn,
                            e.emp_code,
                            e.emp_firstname + ' ' + e.emp_lastname AS fullname,
                            m.area_alias,
                            t.punch_time
                        FROM iclock_transaction AS t WITH (READPAST)          -- << ใส่ตรงนี้
                        INNER JOIN personnel_employee AS e WITH (NOLOCK)      -- (ถ้าจะใส่สำหรับ lookup)
                            ON t.emp_code = e.emp_code
                        INNER JOIN iclock_terminal AS m WITH (NOLOCK)         -- (ถ้าจะใส่สำหรับ lookup)
                            ON t.terminal_sn = m.terminal_sn
                        WHERE t.id > @lastId
                        ORDER BY t.id DESC;", conn);

                    cmd.Parameters.Add(new SqlParameter("@lastId", SqlDbType.BigInt) { Value = _lastId });

                    using var rdr = await cmd.ExecuteReaderAsync(stoppingToken);
                    while (await rdr.ReadAsync(stoppingToken))
                    {
                        long id = rdr.GetFieldType(0) == typeof(int) ? rdr.GetInt32(0) : rdr.GetInt64(0);
                        string empCode = rdr["emp_code"]?.ToString() ?? "";
                        string areaAlias = rdr["area_alias"]?.ToString() ?? "";
                        string fullname = rdr["fullname"]?.ToString() ?? "";
                        string terminal = rdr["terminal_sn"]?.ToString() ?? "";
                        DateTime punchTime = rdr.GetDateTime(rdr.GetOrdinal("punch_time"));

                        var payload = new
                        {
                            emp_code = empCode,
                            fullname = fullname,
                            area_alias = areaAlias,
                            punch_date = punchTime.ToString("dd MMM"),
                            punch_time = punchTime.ToString("HH:mm"),
                            status = punchTime.Hour > 8 ? "Late" : "On time",
                            emp_img = "https://hrpulse.int.matang/biotime/" + punchTime.ToString("MMyyyy") + "/" + terminal + "/" + punchTime.ToString("yyyyMMddHHmmss") + "-" + empCode + ".jpg",
                        };

                        string json = JsonSerializer.Serialize(payload);
                        await _hub.Clients.All.SendAsync("ReceiveData", json, stoppingToken);

                        _lastId = id;
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error polling MSSQL");
                }

                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }

            _logger.LogInformation("DataHubService stopped.");
        }
    }
}
