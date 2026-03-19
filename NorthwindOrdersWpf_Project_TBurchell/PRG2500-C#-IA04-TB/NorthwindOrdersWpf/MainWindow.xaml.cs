using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using NorthwindOrdersWpf.DAL;
using NorthwindOrdersWpf.Models;

namespace NorthwindOrdersWpf
{
    /// <summary>
    /// Main window for managing Northwind Orders.
    /// - Displays orders in a DataGrid
    /// - Supports Add, Update, Delete, and Search by OrderID
    /// - Shows a MessageBox with OrderID + Product names when a row is selected
    /// - Uses ADO.NET-based DataAccess for CRUD and lookups
    /// </summary>
    public partial class MainWindow : Window
    {
        // -----------------------------
        // Fields
        // -----------------------------

        /// <summary>
        /// Data access layer (ADO.NET).
        /// </summary>
        private readonly DataAccess _db = new DataAccess();

        /// <summary>
        /// Backing storage bound to the DataGrid (via DefaultView).
        /// </summary>
        private DataTable? _orders;

        /// <summary>
        /// Lookup cache for Customers (binds to CustomerCombo).
        /// </summary>
        private List<Customer>? _customers;

        /// <summary>
        /// Lookup cache for Employees (binds to EmployeeCombo).
        /// </summary>
        private List<Employee>? _employees;

        // -----------------------------
        // Construction / Initialization
        // -----------------------------

        /// <summary>
        /// Initializes the UI, loads lookup lists, and loads the initial orders grid.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LoadLookups();
            LoadOrders();
        }

        // -----------------------------
        // Data Loading (Lookups + Orders)
        // -----------------------------

        /// <summary>
        /// Loads Customers and Employees from the database and binds to the
        /// respective ComboBoxes.
        /// </summary>
        private void LoadLookups()
        {
            try
            {
                _customers = _db.GetCustomers();
                _employees = _db.GetEmployees();

                // Bind to ComboBoxes (DisplayMemberPath/SelectedValuePath are set in XAML)
                CustomerCombo.ItemsSource = _customers;
                EmployeeCombo.ItemsSource = _employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load lookups: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Loads orders into a DataTable and assigns its DefaultView to the DataGrid.
        /// Optionally filters to a specific OrderID when provided.
        /// </summary>
        /// <param name="orderId">Optional OrderID filter (null to load all).</param>
        private void LoadOrders(int? orderId = null)
        {
            try
            {
                _orders = _db.GetOrders(orderId);
                OrdersGrid.ItemsSource = _orders.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load orders: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // -----------------------------
        // Validation
        // -----------------------------

        /// <summary>
        /// Validates input fields prior to Insert/Update.
        /// Ensures a customer/employee is selected, date is in yyyy-MM-dd,
        /// and ShipAddress is not empty.
        /// </summary>
        /// <param name="customerId">Resolved CustomerID (output).</param>
        /// <param name="employeeId">Resolved EmployeeID (output).</param>
        /// <param name="orderDate">Resolved OrderDate (output).</param>
        /// <param name="shipAddress">Resolved ShipAddress (output).</param>
        /// <returns>True if inputs are valid, otherwise false.</returns>
        private bool ValidateInputs(
            out string customerId,
            out int employeeId,
            out DateTime orderDate,
            out string shipAddress)
        {
            customerId = string.Empty;
            employeeId = 0;
            orderDate = DateTime.MinValue;
            shipAddress = string.Empty;

            // Customer selection is required; SelectedValue carries the CustomerID (string).
            if (CustomerCombo.SelectedValue == null)
            {
                MessageBox.Show(
                    "Please select a Customer.",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }
            customerId = CustomerCombo.SelectedValue.ToString()!;

            // Employee selection is required; SelectedValue carries the EmployeeID (int).
            if (EmployeeCombo.SelectedValue == null ||
                !int.TryParse(EmployeeCombo.SelectedValue.ToString(), out employeeId))
            {
                MessageBox.Show(
                    "Please select an Employee.",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            // OrderDate must be in ISO yyyy-MM-dd (keeps parsing consistent and database-friendly).
            if (string.IsNullOrWhiteSpace(OrderDateBox.Text) ||
                !DateTime.TryParseExact(
                    OrderDateBox.Text.Trim(),
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out orderDate))
            {
                MessageBox.Show(
                    "Please enter OrderDate in yyyy-MM-dd format.",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            // Ship address cannot be empty for this example.
            shipAddress = ShipAddressBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(shipAddress))
            {
                MessageBox.Show(
                    "Please enter ShipAddress.",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        // -----------------------------
        // CRUD Handlers
        // -----------------------------

        /// <summary>
        /// Adds a new Order using values from the form.
        /// On success, reloads the grid and clears the form.
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(
                out var customerId,
                out var employeeId,
                out var orderDate,
                out var shipAddress))
            {
                return;
            }

            try
            {
                int newId = _db.InsertOrder(customerId, employeeId, orderDate, shipAddress);

                // Refresh grid after insert so the new row is visible immediately.
                LoadOrders();

                MessageBox.Show(
                    $"Order {newId} added.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Insert failed: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Updates the currently selected Order (by OrderID) with form values.
        /// Lightweight optimistic approach: relies on primary key and simple update.
        /// </summary>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(OrderIdBox.Text, out int orderId))
            {
                MessageBox.Show(
                    "Select an order to update.",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!ValidateInputs(
                out var customerId,
                out var employeeId,
                out var orderDate,
                out var shipAddress))
            {
                return;
            }

            try
            {
                _db.UpdateOrder(orderId, customerId, employeeId, orderDate, shipAddress);

                // Reload just the updated row (filtered), or reload all if you prefer.
                LoadOrders(orderId);

                MessageBox.Show(
                    $"Order {orderId} updated.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Update failed: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Deletes the selected order (and its child order details).
        /// Shows a confirmation prompt before proceeding.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(OrderIdBox.Text, out int orderId))
            {
                MessageBox.Show(
                    "Select an order to delete.",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Confirm destructive action. Note: DAL deletes [Order Details] first (FK-safety).
            if (MessageBox.Show(
                    $"Delete order {orderId}? This also deletes its order details.",
                    "Confirm",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                _db.DeleteOrder(orderId);

                // Refresh grid after delete and clear the editor.
                LoadOrders();
                ClearForm();

                MessageBox.Show(
                    $"Order {orderId} deleted.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Delete failed: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // -----------------------------
        // Search / Refresh / Clear
        // -----------------------------

        /// <summary>
        /// Searches by OrderID. If it parses to an int, loads the single matching record;
        /// otherwise shows a validation warning.
        /// </summary>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SearchOrderIdBox.Text, out int orderId))
            {
                LoadOrders(orderId);
            }
            else
            {
                MessageBox.Show(
                    "Enter a valid numeric OrderID.",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Reloads all orders (clears any OrderID filter).
        /// </summary>
        private void RefreshButton_Click(object sender, RoutedEventArgs e) => LoadOrders();

        /// <summary>
        /// Clears all editor fields and unselects the DataGrid row.
        /// </summary>
        private void ClearButton_Click(object sender, RoutedEventArgs e) => ClearForm();

        /// <summary>
        /// Utility: clears the form fields and unselects the DataGrid to avoid stale context.
        /// </summary>
        private void ClearForm()
        {
            OrderIdBox.Text = string.Empty;
            CustomerCombo.SelectedIndex = -1;
            EmployeeCombo.SelectedIndex = -1;
            OrderDateBox.Text = string.Empty;
            ShipAddressBox.Text = string.Empty;
            OrdersGrid.UnselectAll();
        }

        // -----------------------------
        // Selection Handling
        // -----------------------------

        /// <summary>
        /// Handles DataGrid selection changes:
        /// - Pushes the selected row's values into the editor fields
        /// - Displays a MessageBox with the OrderID and associated Product names
        /// </summary>
        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // DataGrid is bound to a DataView; items are DataRowView.
            if (OrdersGrid.SelectedItem is not DataRowView row)
                return;

            // Push selected row values into the form (keeps UI context aligned with selection).
            OrderIdBox.Text = row["OrderID"].ToString();
            CustomerCombo.SelectedValue = row["CustomerID"].ToString();
            EmployeeCombo.SelectedValue = Convert.ToInt32(row["EmployeeID"]);
            OrderDateBox.Text = row["OrderDate"] == DBNull.Value
                ? string.Empty
                : Convert.ToDateTime(row["OrderDate"]).ToString("yyyy-MM-dd");
            ShipAddressBox.Text = row["ShipAddress"].ToString();

            // On selection: fetch and show product names for the order.
            if (!int.TryParse(OrderIdBox.Text, out int orderId))
                return;

            try
            {
                var products = _db.GetProductNamesForOrder(orderId);
                string prodList = products.Count == 0 ? "(no products)" : string.Join(", ", products);

                MessageBox.Show(
                    $"OrderID: {orderId}\nProducts: {prodList}",
                    "Order Details",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load products: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}