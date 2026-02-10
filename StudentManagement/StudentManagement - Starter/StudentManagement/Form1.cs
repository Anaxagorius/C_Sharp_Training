using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StudentManagement
{
    /// <summary>
    /// WinForms UI layer (presentation layer).
    /// Responsible for:
    /// - Accepting user input
    /// - Displaying students in the ListBox
    /// - Triggering data persistence via FirstYearStudent.Save/Load
    /// 
    /// Note: We keep business rules in Student/FirstYearStudent classes,
    /// while UI mainly coordinates and validates inputs.
    /// </summary>
    public partial class Form1 : Form
    {
        private FirstYearStudent firstYearStudent;

        public Form1()
        {
            InitializeComponent();

            // IMPORTANT:
            // If user clicks Save before Load, firstYearStudent must not be null.
            // Student.Fname cannot be empty due to validation, so use safe placeholders.
            firstYearStudent = new FirstYearStudent("System", "User", 18, "NA", "NA");
        }

        /// <summary>
        /// LOAD button click handler.
        /// Loads student list from file and displays them in the ListBox.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            List<FirstYearStudent> students = firstYearStudent.Load();

            listBox1.Items.Clear();
            foreach (var student in students)
            {
                listBox1.Items.Add(
                    $"{student.Fname},{student.LName},{student.Age},{student.sProgram},{student.yearOfStudy},{student.workTermStatus}"
                );
            }
        }

        /// <summary>
        /// SAVE button click handler.
        /// Reads ListBox rows, rebuilds student objects, and saves to file.
        /// </summary>
        private void Save_Click(object sender, EventArgs e)
        {
            // Extra guard (should not be needed because constructor initializes it,
            // but included for safety and professionalism).
            if (firstYearStudent == null)
                firstYearStudent = new FirstYearStudent("System", "User", 18, "NA", "NA");

            List<FirstYearStudent> studentsToSave = new List<FirstYearStudent>();

            foreach (var item in listBox1.Items)
            {
                string[] parts = item.ToString().Split(',');

                // Expected: fname,lname,age,program,yearOfStudy,workTermStatus
                if (parts.Length != 6)
                    continue;

                // Safer parsing: avoid crashes due to invalid data
                if (!int.TryParse(parts[2].Trim(), out int age))
                    continue;

                string fname = parts[0].Trim();
                string lname = parts[1].Trim();
                string program = parts[3].Trim();

                // yearOfStudy is stored and loaded, but constructor defaults to 1
                int yos = 1;
                int.TryParse(parts[4].Trim(), out yos);

                string workTerm = parts[5].Trim();

                var student = new FirstYearStudent(fname, lname, age, program, workTerm)
                {
                    yearOfStudy = yos
                };

                studentsToSave.Add(student);
            }

            firstYearStudent.Save(studentsToSave);
        }

        /// <summary>
        /// ADD button click handler.
        /// Takes input from textboxes, validates it, creates a student, and adds to ListBox.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate Age (avoid FormatException)
            if (!int.TryParse(txtAge.Text.Trim(), out int age))
            {
                MessageBox.Show("Please enter a valid numeric age.");
                txtAge.Focus();
                return;
            }

            string firstName = txtFname.Text.Trim();
            string lastName = txtLname.Text.Trim();
            string program = txtProgram.Text.Trim();
            string workTerm = txtWorkterm.Text.Trim();

            // Validate required fields (Fname property throws exception if empty)
            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("First name is required.");
                txtFname.Focus();
                return;
            }

            try
            {
                FirstYearStudent newStudent = new FirstYearStudent(firstName, lastName, age, program, workTerm);

                // Show the same 6-field format used in Save/Load
                listBox1.Items.Add(
                    $"{newStudent.Fname},{newStudent.LName},{newStudent.Age},{newStudent.sProgram},{newStudent.yearOfStudy},{newStudent.workTermStatus}"
                );

                // Clear fields after adding for good UX
                txtFname.Clear();
                txtLname.Clear();
                txtAge.Clear();
                txtProgram.Clear();
                txtWorkterm.Clear();
                txtFname.Focus();
            }
            catch (Exception ex)
            {
                // Handles validation exceptions (ex: invalid age range or empty name)
                MessageBox.Show($"Could not add student: {ex.Message}");
            }
        }
    }
}