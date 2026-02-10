using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace StudentManagement
{
    /// <summary>
    /// Concrete student type for first-year students.
    /// Demonstrates:
    /// - Inheritance (FirstYearStudent : Student)
    /// - Interface implementation (IStudentData)
    /// - Method overriding (DisplayStudentDetails)
    /// - File I/O (Save/Load)
    /// </summary>
    public class FirstYearStudent : Student, IStudentData
    {
        // Initialize list to satisfy nullable warnings and avoid null refs.
        private List<FirstYearStudent> fstudents = new();

        /// <summary>
        /// Year of study (defaults to 1 for FirstYearStudent).
        /// </summary>
        public int yearOfStudy { get; set; }

        /// <summary>
        /// Work term status (e.g., "Yes", "No", "Searching", etc.).
        /// Never null because we default it to empty string.
        /// </summary>
        public string workTermStatus { get; set; } = string.Empty;

        
        /// File name for persistence.
        /// Stored alongside executable (bin\Debug\net9.0-windows) during development.
        /// </summary>
        private static readonly string StudentMasterFile =
            Path.Combine(Application.StartupPath, "StudentMaster.txt");

        
        /// Constructor uses base(...) to initialize inherited Student properties.
       
        public FirstYearStudent(string fname, string lname, int age, string program, string workTerm)
            : base(fname, lname, age, program)
        {
            yearOfStudy = 1;
            workTermStatus = workTerm ?? string.Empty;
        }

        /// <summary>
        /// Overrides the abstract method from Student.
        /// If you get "no suitable method to override", Student.cs is not compiling or is duplicated.
        /// </summary>
        public override void DisplayStudentDetails()
        {
            MessageBox.Show(
                $"Student Name: {Fname} {LName}\n" +
                $"Age: {Age}\n" +
                $"Program: {sProgram}\n" +
                $"Year of Study: {yearOfStudy}\n" +
                $"Work Term Status: {workTermStatus}"
            );
        }

        /// <summary>
        /// Load student records from file.
        /// Expected format per line (CSV):
        /// fname,lname,age,program,yearOfStudy,workTermStatus
        /// </summary>
        public List<FirstYearStudent> Load()
        {
            // Always start with a fresh list.
            fstudents = new List<FirstYearStudent>();

            try
            {
                if (!File.Exists(StudentMasterFile))
                {
                    // No file yet = no saved students (not an error).
                    return fstudents;
                }

                using StreamReader reader = new StreamReader(StudentMasterFile);

                // ReadLine() can return null at end of file, so use string?
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = line.Split(',');

                    if (parts.Length != 6)
                        continue; // skip malformed lines

                    string firstName = parts[0].Trim();
                    string lastName = parts[1].Trim();

                    if (!int.TryParse(parts[2].Trim(), out int age))
                        continue;

                    string program = parts[3].Trim();

                    int yos = 1;
                    int.TryParse(parts[4].Trim(), out yos);

                    string workStatus = parts[5].Trim();

                    var student = new FirstYearStudent(firstName, lastName, age, program, workStatus)
                    {
                        yearOfStudy = yos
                    };

                    fstudents.Add(student);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student data: {ex.Message}\n\nFile:\n{StudentMasterFile}");
            }

            return fstudents;
        }

        /// <summary>
        /// Save student records to file (overwrite mode).
        /// </summary>
        public void Save(List<FirstYearStudent> students)
        {
            fstudents = students ?? new List<FirstYearStudent>();

            try
            {
                using StreamWriter writer = new StreamWriter(StudentMasterFile, false);

                foreach (var st in fstudents)
                {
                    writer.WriteLine(
                        $"{st.Fname},{st.LName},{st.Age},{st.sProgram},{st.yearOfStudy},{st.workTermStatus}"
                    );
                }

                MessageBox.Show($"Student data saved successfully!\n\nSaved to:\n{StudentMasterFile}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}\n\nFile:\n{StudentMasterFile}");
            }
        }
    }
}