namespace NorthwindOrdersWpf.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public override string ToString() => FullName;
    }
}
