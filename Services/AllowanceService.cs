using System.Text.Json;

namespace TMSWeb.Services;

public sealed class AllowanceService
{
    private readonly decimal _mealRate;
    private readonly decimal _fuelRate;
    private readonly decimal[] _diliTiers; // [500,700,900]

    // ------- OT_HOL settings -------
    private readonly bool _treatSundayAsHoliday = true;
    private readonly double _fullDayMinHours = 8.0;
    private readonly decimal _fullDayAmount = 550m;
    private readonly decimal _halfDayAmount = 300m;
    private readonly double _halfDayMinHours = 4.0;
    private readonly double _halfDayMaxHours = 7.99;
    private readonly bool _perHourEnabled = false;
    private readonly decimal _perHourRate = 0m;

    public AllowanceService(string configPath)
    {
        var json = File.ReadAllText(configPath);
        using var doc = JsonDocument.Parse(json, new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        });
        var root = doc.RootElement;

        // ---------- MEAL / FUEL / DILIGENCE (เหมือนเดิม) ----------
        _mealRate = root.GetProperty("allowances")[0].GetProperty("rule").GetProperty("rate").GetDecimal(); // MEAL
        _fuelRate = root.GetProperty("allowances")[1].GetProperty("rule").GetProperty("rate").GetDecimal(); // FUEL
        _diliTiers = root.GetProperty("allowances")[2].GetProperty("rule").GetProperty("tiers")
                         .EnumerateArray().Select(e => e.GetProperty("amount").GetDecimal()).ToArray();

        // ---------- OT_HOL (หาโดย code) ----------
        if (root.TryGetProperty("allowances", out var arr) && arr.ValueKind == JsonValueKind.Array)
        {
            foreach (var a in arr.EnumerateArray())
            {
                if (!a.TryGetProperty("code", out var codeEl)) continue;
                if (!string.Equals(codeEl.GetString(), "OT_HOL", StringComparison.OrdinalIgnoreCase)) continue;

                if (a.TryGetProperty("ot", out var ot) &&
                    ot.TryGetProperty("holiday", out var hol))
                {
                    _treatSundayAsHoliday = hol.TryGetProperty("treatSundayAsHoliday", out var t) ? t.GetBoolean() : _treatSundayAsHoliday;

                    if (hol.TryGetProperty("fullDay", out var full))
                    {
                        if (full.TryGetProperty("minHours", out var fMin)) _fullDayMinHours = fMin.GetDouble();
                        if (full.TryGetProperty("amount", out var fAmt)) _fullDayAmount = fAmt.GetDecimal();
                    }
                    if (hol.TryGetProperty("halfDay", out var half))
                    {
                        if (half.TryGetProperty("minHours", out var hMin)) _halfDayMinHours = hMin.GetDouble();
                        if (half.TryGetProperty("maxHours", out var hMax)) _halfDayMaxHours = hMax.GetDouble();
                        if (half.TryGetProperty("amount", out var hAmt)) _halfDayAmount = hAmt.GetDecimal();
                    }
                    if (hol.TryGetProperty("perHour", out var ph))
                    {
                        if (ph.TryGetProperty("enabled", out var en)) _perHourEnabled = en.GetBoolean();
                        if (ph.TryGetProperty("rate", out var rate)) _perHourRate = rate.GetDecimal();
                    }
                }
                break; // พบ OT_HOL แล้วพอ
            }
        }
    }

    // ---------- เดิม ----------
    public decimal Meal(int fullDayCount) => fullDayCount <= 0 ? 0 : _mealRate * fullDayCount;
    public decimal Fuel(int fullDayCount) => fullDayCount <= 0 ? 0 : _fuelRate * fullDayCount;

    // hadAbsent/hadLeave/hadLate = เดือนนี้มีเหตุเหล่านี้ไหม, streakBefore = สตรีคก่อนเดือนนี้
    public (decimal amount, int streak) Diligence(bool hadAbsent, bool hadLate, int streakBefore)
    {
        bool pass = !hadAbsent && !hadLate;
        if (!pass) return (0, 0);

        int streakThis = streakBefore + 1;
        var idx = Math.Min(streakThis, _diliTiers.Length) - 1;
        return (_diliTiers[idx], streakThis);
    }

    // ---------- ใหม่: คำนวณ OT วันหยุดแบบ “รายวัน” ----------
    // hoursWorked: ชม.ทำงานในวันนั้น
    // isSunday: เป็นวันอาทิตย์หรือไม่ (ส่งมาจาก controller/db)
    // isCompanyHoliday: เป็นวันหยุดบริษัทหรือไม่ (ธงจากตารางวันหยุด)
    public (double worked, double ot_hour, decimal ot_amount) HolidayOtOneDay(double hoursWorked)
    {

        if (hoursWorked >= _fullDayMinHours)
            return (1, hoursWorked, _fullDayAmount);

        if (hoursWorked >= _halfDayMinHours && hoursWorked <= _halfDayMaxHours)
            return (0.5, hoursWorked, _halfDayAmount);

        if (_perHourEnabled && hoursWorked > 0)
            return (1, hoursWorked, _perHourRate * (decimal)hoursWorked);

        return (0, 0, 0m);
    }
}
