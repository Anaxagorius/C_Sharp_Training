namespace StudentManagementWinForms
{
    public class Student
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Program { get; set; } = string.Empty;
        public string WebComm { get; set; } = string.Empty; // e.g., website, GitHub, etc.

        public override string ToString()
        {
            return $"{LastName}, {FirstName} ({Age}) - {Program} [{WebComm}]";
        }
    }
}
