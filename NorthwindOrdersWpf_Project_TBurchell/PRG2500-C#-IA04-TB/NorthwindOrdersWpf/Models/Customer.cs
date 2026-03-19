namespace NorthwindOrdersWpf.Models
{
    public class Customer
    {
        public string CustomerID { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public override string ToString() => CompanyName;
    }
}
