namespace TMSWeb.Models.ViewModels
{
    public class CashflowViewModel
    {
        public string job_detail_code { get; set; }
        public DateOnly job_date { get; set; }
        public string driver{ get; set; }
        public string license { get; set; }
        public decimal cashflow { get; set; }
    }
}
