using System;

namespace NorthwindOrdersWpf.Models
{
    public class OrderRecord//DTO
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; } = string.Empty;
        public int EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ShipAddress { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
    }
}
