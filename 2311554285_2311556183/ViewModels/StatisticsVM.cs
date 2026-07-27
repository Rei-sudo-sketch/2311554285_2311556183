namespace _2311554285_2311556183.ViewModels
{
    public class StatisticsVM
    {
        public decimal TotalRevenue { get; set; }

        public List<ProductStatisticVM> TopProducts { get; set; }
            = new();
    }

    public class ProductStatisticVM
    {
        public string ProductName { get; set; } = "";

        public int TotalSold { get; set; }
    }
}