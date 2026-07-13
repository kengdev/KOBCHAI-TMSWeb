using System.Globalization;
using System.Text.Json;

public sealed class SsfService
{
    private readonly SsfSettings _settings;

    public SsfService(string configPath)
    {
        var json = File.ReadAllText(configPath);
        _settings = SsfSettings.FromJson(json);
        Normalize(ref _settings);
    }

    public SsfService(SsfSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        Normalize(ref _settings);
    }

    public SsfSettings Settings => _settings;

    /// <summary>คำนวณยอดประกันสังคม "รอบนี้" จากฐานรายเดือน (คิดทั้งเดือนแล้วแบ่งตามรอบ)</summary>
    public decimal AmountThisRun(decimal monthlyBaseWage, int runIndex)
    {
        var s = _settings;
        var clampedBase = Clamp(monthlyBaseWage, s.MinBase, s.MaxBase);
        var monthly = Round2(clampedBase * s.Rate);
        return SplitMonthlyToRun(monthly, runIndex, s.RunsPerMonth);
    }

    /// <summary>ถ้าคิดยอดทั้งเดือนมาแล้ว (after clamp * rate) ให้แบ่งเป็นยอดรอบนี้</summary>
    public decimal AmountThisRunFromMonthly(decimal monthlySsf, int runIndex)
        => SplitMonthlyToRun(Round2(monthlySsf), runIndex, _settings.RunsPerMonth);

    // ---------- Static (เผื่อเรียกแบบไม่ผูกไฟล์) ----------
    public static decimal AmountThisRun(
        decimal monthlyBaseWage, int runIndex,
        int runsPerMonth = 2, decimal rate = 0.05m,
        decimal minBase = 1650m, decimal maxBase = 15000m)
    {
        var monthly = Round2(Clamp(monthlyBaseWage, minBase, maxBase) * rate);
        return SplitMonthlyToRun(monthly, runIndex, runsPerMonth);
    }

    public static decimal AmountThisRunFromConfigJson(decimal monthlyBaseWage, int runIndex, string payrollConfigJson)
    {
        var s = SsfSettings.FromJson(payrollConfigJson);
        Normalize(ref s);
        var monthly = Round2(Clamp(monthlyBaseWage, s.MinBase, s.MaxBase) * s.Rate);
        return SplitMonthlyToRun(monthly, runIndex, s.RunsPerMonth);
    }

    // ---------- Helpers ----------
    private static void Normalize(ref SsfSettings s)
    {
        if (s.RunsPerMonth < 1) s = s with { RunsPerMonth = 1 };
        if (s.MinBase > s.MaxBase) s = s with { MinBase = s.MaxBase, MaxBase = s.MinBase };
        if (s.Rate < 0) s = s with { Rate = Math.Abs(s.Rate) };
    }

    private static decimal Round2(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
    private static decimal Clamp(decimal v, decimal min, decimal max) => Math.Min(Math.Max(v, min), max);

    private static decimal SplitMonthlyToRun(decimal monthly, int runIndex, int runsPerMonth)
    {
        if (runsPerMonth <= 1) return Round2(monthly);

        // clamp runIndex ให้ปลอดภัย
        if (runIndex < 1) runIndex = 1;
        if (runIndex > runsPerMonth) runIndex = runsPerMonth;

        if (runsPerMonth == 2)
        {
            var r1 = Round2(monthly / 2m);
            var r2 = Round2(monthly - r1);              // รวมแล้วเท่ากับทั้งเดือนเสมอ
            return runIndex == 1 ? r1 : r2;
        }

        var equal = Round2(monthly / runsPerMonth);
        if (runIndex < runsPerMonth) return equal;
        return Round2(monthly - (equal * (runsPerMonth - 1))); // รอบสุดท้ายรับเศษ
    }

    // ---------- Settings Loader ----------
    public record SsfSettings(decimal Rate, decimal MinBase, decimal MaxBase, int RunsPerMonth)
    {
        public static SsfSettings FromJson(string json)
        {
            using var doc = JsonDocument.Parse(json, new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            });
            var root = doc.RootElement;

            decimal rate = 0.05m, minBase = 1650m, maxBase = 15000m;
            int runs = 2;

            if (root.TryGetProperty("deductions", out var deds) && deds.ValueKind == JsonValueKind.Array)
            {
                foreach (var d in deds.EnumerateArray())
                {
                    if (!d.TryGetProperty("code", out var codeEl)) continue;
                    if (!"SSF".Equals(codeEl.GetString(), StringComparison.OrdinalIgnoreCase)) continue;

                    if (d.TryGetProperty("rule", out var rule))
                    {
                        if (TryGetDecimal(rule, "rate", out var r)) rate = r;

                        if (rule.TryGetProperty("base", out var b))
                        {
                            if (TryGetDecimal(b, "min", out var mn)) minBase = mn;
                            if (TryGetDecimal(b, "max", out var mx)) maxBase = mx;
                        }

                        if (rule.TryGetProperty("cycle", out var cyc))
                        {
                            if (TryGetInt(cyc, "runsPerMonth", out var rpm)) runs = Math.Max(1, rpm);
                        }
                    }
                    break; // เจอ SSF แล้วออกเลย
                }
            }

            return new SsfSettings(rate, minBase, maxBase, runs);
        }

        private static bool TryGetDecimal(JsonElement obj, string name, out decimal value)
        {
            value = 0m;
            if (!obj.TryGetProperty(name, out var el)) return false;

            if (el.ValueKind == JsonValueKind.Number) return el.TryGetDecimal(out value);
            if (el.ValueKind == JsonValueKind.String)
                return decimal.TryParse(el.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out value);

            return false;
        }

        private static bool TryGetInt(JsonElement obj, string name, out int value)
        {
            value = 0;
            if (!obj.TryGetProperty(name, out var el)) return false;

            if (el.ValueKind == JsonValueKind.Number) return el.TryGetInt32(out value);
            if (el.ValueKind == JsonValueKind.String)
                return int.TryParse(el.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value);

            return false;
        }
    }
}
