using System;
using System.Windows.Forms;

namespace StudentManagement
{
    /// <summary>
    /// Abstract base class representing shared attributes and behavior for all student types.
    ///
    /// Demonstrates core OOP principles:
    /// 1) Encapsulation: private fields exposed through validated properties.
    /// 2) Abstraction: abstract method forces child classes to implement details display.
    /// 3) Validation: protects object state (e.g., age range).
    /// </summary>
    public abstract class Student
    {
        // Initialize fields to satisfy Nullable Reference Types (NRT) rules.
        // This prevents "Non-nullable field must contain a non-null value" warnings.
        private string fName = string.Empty;
        private string lName = string.Empty;
        private string program = string.Empty;
        private int age;

        /// <summary>
        /// First name with validation. Must not be blank.
        /// </summary>
        public string Fname
        {
            get => fName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    fName = value;
                else
                    throw new Exception("Enter a valid first name.");
            }
        }

        /// <summary>
        /// Last name. Allows empty but not null (stored as empty string).
        /// </summary>
        public string LName
        {
            get => lName;
            set => lName = value ?? string.Empty;
        }

        /// <summary>
        /// Program name. Allows empty but not null.
        /// </summary>
        public string sProgram
        {
            get => program;
            set => program = value ?? string.Empty;
        }

        /// <summary>
        /// Student age must be between 1 and 99 inclusive.
        /// </summary>
        public int Age
        {
            get => age;
            set
            {
                if (value > 0 && value < 100)
                    age = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(Age), "Age must be between 1 and 99.");
            }
        }

        /// <summary>
        /// Base constructor: used by derived classes via : base(...)
        /// Demonstrates constructor chaining in inheritance.
        /// </summary>
        protected Student(string firstName, string lastName, int studentAge, string studentProgram)
        {
            // Use properties (not fields) so validation is applied consistently.
            Fname = firstName;
            LName = lastName;
            Age = studentAge;
            sProgram = studentProgram;
        }

        /// <summary>
        /// Non-abstract method in an abstract class (valid and common).
        /// </summary>
        public void PrintClassDetails()
        {
            MessageBox.Show("This is a non-abstract member in an abstract class.");
        }

        /// <summary>
        /// Abstract method: derived classes MUST implement it.
        /// This enables polymorphism (same call, different behavior by type).
        /// </summary>
        public abstract void DisplayStudentDetails();
    }
}