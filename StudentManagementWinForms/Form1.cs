using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace StudentManagementWinForms
{
    public partial class Form1 : Form
    {
        private BindingList<Student> _students = new BindingList<Student>();
        private readonly string _dataPath = Path.Combine(AppContext.BaseDirectory, "students.json");

        public Form1()
        {
            InitializeComponent();
            lbStudents.DataSource = _students;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(!ValidateInputs(out var age))
                return;

            var s = new Student
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Age = age,
                Program = txtProgram.Text.Trim(),
                WebComm = txtWebComm.Text.Trim()
            };
            _students.Add(s);
            ClearInputs();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(_dataPath, JsonSerializer.Serialize(_students, options));
                MessageBox.Show($"Saved {_students.Count} student(s) to {_dataPath}.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(_dataPath))
                {
                    MessageBox.Show("No saved data found yet.", "Load", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var json = File.ReadAllText(_dataPath);
                var list = JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
                _students.Clear();
                foreach (var s in list) _students.Add(s);
                MessageBox.Show($"Loaded {_students.Count} student(s).", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs(out int age)
        {
            age = 0;
            if (string.IsNullOrWhiteSpace(txtFirstName.Text)) { MessageBox.Show("First Name is required."); return false; }
            if (string.IsNullOrWhiteSpace(txtLastName.Text)) { MessageBox.Show("Last Name is required."); return false; }
            if (!int.TryParse(txtAge.Text, out age) || age < 0) { MessageBox.Show("Age must be a non-negative whole number."); return false; }
            if (string.IsNullOrWhiteSpace(txtProgram.Text)) { MessageBox.Show("Program is required."); return false; }
            return true;
        }

        private void ClearInputs()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAge.Clear();
            txtProgram.Clear();
            txtWebComm.Clear();
            txtFirstName.Focus();
        }
    }
}
