
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MemberRegistrationMVP.Models;
using MemberRegistrationMVP.Presenters;
using MemberRegistrationMVP.Services;
using MemberRegistrationMVP.Views;

namespace MemberRegistrationMVP
{
    /// <summary>
    /// WPF View (MainWindow) implementing MVP.
    /// IMPORTANT: This project must be a WPF project (UseWPF=true).
    /// If you paste this into WinForms, you will get errors like MessageBoxButton not found.
    /// </summary>
    public partial class MainWindow : Window, IMemberView
    {
        private MemberPresenter _presenter;

        public MainWindow()
        {
            InitializeComponent();

            dpMemberSince.SelectedDate = DateTime.Today;

            // Wire control events to View events
            btnRegister.Click += delegate { if (RegisterClicked != null) RegisterClicked(this, EventArgs.Empty); };
            btnUpdate.Click += delegate { if (UpdateClicked != null) UpdateClicked(this, EventArgs.Empty); };
            btnDelete.Click += delegate { if (DeleteClicked != null) DeleteClicked(this, EventArgs.Empty); };
            btnSave.Click += delegate { if (SaveClicked != null) SaveClicked(this, EventArgs.Empty); };
            btnLoad.Click += delegate { if (LoadClicked != null) LoadClicked(this, EventArgs.Empty); };
            lstMembers.SelectionChanged += delegate { if (SelectionChanged != null) SelectionChanged(this, EventArgs.Empty); };

            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MemberMaster.txt");
            _presenter = new MemberPresenter(this, new FileMemberRepository(filePath));
        }

        // ---- IMemberView events ----
        public event EventHandler RegisterClicked;
        public event EventHandler UpdateClicked;
        public event EventHandler DeleteClicked;
        public event EventHandler SaveClicked;
        public event EventHandler LoadClicked;
        public event EventHandler SelectionChanged;

        // ---- IMemberView properties ----
        public string FirstNameText
        {
            get { return txtFirstName.Text; }
            set { txtFirstName.Text = value; }
        }

        public string LastNameText
        {
            get { return txtLastName.Text; }
            set { txtLastName.Text = value; }
        }

        public string PostalCodeText
        {
            get { return txtPostalCode.Text; }
            set { txtPostalCode.Text = value; }
        }

        public string GenderValue
        {
            get { return (rbFemale.IsChecked == true) ? "Female" : "Male"; }
            set
            {
                if (string.Equals(value, "Female", StringComparison.OrdinalIgnoreCase))
                {
                    rbFemale.IsChecked = true;
                    rbMale.IsChecked = false;
                }
                else
                {
                    rbMale.IsChecked = true;
                    rbFemale.IsChecked = false;
                }
            }
        }

        public string MemberTypeValue
        {
            get
            {
                ComboBoxItem item = cmbMemberType.SelectedItem as ComboBoxItem;
                if (item != null && item.Content != null)
                    return item.Content.ToString();

                return "Standard";
            }
            set
            {
                ComboBoxItem match = null;
                foreach (ComboBoxItem i in cmbMemberType.Items.OfType<ComboBoxItem>())
                {
                    if (i.Content != null && string.Equals(i.Content.ToString(), value, StringComparison.OrdinalIgnoreCase))
                    {
                        match = i;
                        break;
                    }
                }

                if (match != null)
                    cmbMemberType.SelectedItem = match;
                else
                    cmbMemberType.SelectedIndex = 0;
            }
        }

        public DateTime MemberSinceValue
        {
            get { return dpMemberSince.SelectedDate.HasValue ? dpMemberSince.SelectedDate.Value : DateTime.Today; }
            set { dpMemberSince.SelectedDate = value; }
        }

        public int SelectedIndex
        {
            get { return lstMembers.SelectedIndex; }
        }

        // ---- IMemberView methods ----
        public void SetReadOnly(bool readOnly)
        {
            txtFirstName.IsReadOnly = readOnly;
            txtLastName.IsReadOnly = readOnly;
            txtPostalCode.IsReadOnly = readOnly;

            rbMale.IsEnabled = !readOnly;
            rbFemale.IsEnabled = !readOnly;
            cmbMemberType.IsEnabled = !readOnly;
            dpMemberSince.IsEnabled = !readOnly;
        }

        public void ShowUpdateDelete(bool visible)
        {
            btnUpdate.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            btnDelete.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void DisplayMembers(List<Member> members)
        {
            lstMembers.ItemsSource = null;
            lstMembers.ItemsSource = members;
        }

        public void ClearForm()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtPostalCode.Clear();
            rbMale.IsChecked = true;
            cmbMemberType.SelectedIndex = 0;
            dpMemberSince.SelectedDate = DateTime.Today;
            lstMembers.SelectedIndex = -1;
        }

        public bool Confirm(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void ShowInfo(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
