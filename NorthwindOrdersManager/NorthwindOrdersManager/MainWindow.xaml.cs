using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NorthwindOrdersManager
{
    public partial class MainWindow : Window
    {
        // ---------------------------------------------------------------------------
        // Connection string – update server/instance name to match your environment.
        // Common alternatives:
        //   "Server=.;Database=Northwind;Integrated Security=True"
        //   "Server=localhost;Database=Northwind;Integrated Security=True"
        // ---------------------------------------------------------------------------
        private const string ConnectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True";

        // Tracks whether a SelectionChanged event was triggered programmatically
        // (e.g., when refreshing the grid) so we can suppress the product MessageBox.
        private bool _suppressSelectionMessage = false;

        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();

            // Populate dropdowns and load all orders when the window opens.
            LoadCustomers();
            LoadEmployees();
            LoadOrders();
        }

        // -----------------------------------------------------------------------
        // Data-loading helpers
        // -----------------------------------------------------------------------

        /// <summary>
        /// Populates the Customer ComboBox from the Customers table.
        /// </summary>
        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    const string sql =
                        "SELECT CustomerID, CompanyName " +
                        "FROM Customers " +
                        "ORDER BY CompanyName";

                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // ItemsSource is a DataView; DisplayMemberPath/SelectedValuePath
                    // are set in XAML (CompanyName / CustomerID).
                    cbCustomer.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading customers", ex);
            }
        }

        /// <summary>
        /// Populates the Employee ComboBox from the Employees table.
        /// </summary>
        private void LoadEmployees()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    const string sql =
                        "SELECT EmployeeID, FirstName + ' ' + LastName AS FullName " +
                        "FROM Employees " +
                        "ORDER BY LastName";

                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // DisplayMemberPath/SelectedValuePath set in XAML (FullName / EmployeeID).
                    cbEmployee.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading employees", ex);
            }
        }

        /// <summary>
        /// Loads orders into the DataGrid, optionally filtered to a single OrderID.
        /// Joins Customers and Employees so the grid shows friendly names.
        /// </summary>
        /// <param name="filterOrderID">
        /// When provided, only the order with this ID is displayed.
        /// </param>
        private void LoadOrders(int? filterOrderID = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Build the query; filter clause is added only when searching.
                    StringBuilder sb = new StringBuilder();
                    sb.Append(
                        "SELECT o.OrderID, c.CompanyName, " +
                        "       e.FirstName + ' ' + e.LastName AS EmployeeName, " +
                        "       o.OrderDate, o.ShipAddress " +
                        "FROM Orders o " +
                        "JOIN  Customers  c ON o.CustomerID  = c.CustomerID " +
                        "LEFT JOIN Employees e ON o.EmployeeID = e.EmployeeID");

                    if (filterOrderID.HasValue)
                        sb.Append(" WHERE o.OrderID = @OrderID");

                    sb.Append(" ORDER BY o.OrderID DESC");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                    if (filterOrderID.HasValue)
                        cmd.Parameters.AddWithValue("@OrderID", filterOrderID.Value);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Suppress the automatic product popup while the grid re-binds.
                    _suppressSelectionMessage = true;
                    dgOrders.ItemsSource = dt.DefaultView;
                    _suppressSelectionMessage = false;
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading orders", ex);
            }
        }

        // -----------------------------------------------------------------------
        // Button event handlers
        // -----------------------------------------------------------------------

        /// <summary>
        /// Inserts a new order into the Orders table.
        /// </summary>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out string customerID, out int employeeID,
                                out DateTime orderDate, out string shipAddress))
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    const string sql =
                        "INSERT INTO Orders (CustomerID, EmployeeID, OrderDate, ShipAddress) " +
                        "VALUES (@CustomerID, @EmployeeID, @OrderDate, @ShipAddress)";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@CustomerID",  customerID);
                    cmd.Parameters.AddWithValue("@EmployeeID",  employeeID);
                    cmd.Parameters.AddWithValue("@OrderDate",   orderDate);
                    cmd.Parameters.AddWithValue("@ShipAddress", shipAddress);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Order added successfully.", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                LoadOrders();
            }
            catch (Exception ex)
            {
                ShowError("Error adding order", ex);
            }
        }

        /// <summary>
        /// Updates the currently selected order in the Orders table.
        /// </summary>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtOrderID.Text, out int orderID))
            {
                MessageBox.Show("Please select an order from the grid to update.",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidateInputs(out string customerID, out int employeeID,
                                out DateTime orderDate, out string shipAddress))
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    const string sql =
                        "UPDATE Orders " +
                        "SET CustomerID  = @CustomerID, " +
                        "    EmployeeID  = @EmployeeID, " +
                        "    OrderDate   = @OrderDate, " +
                        "    ShipAddress = @ShipAddress " +
                        "WHERE OrderID = @OrderID";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@CustomerID",  customerID);
                    cmd.Parameters.AddWithValue("@EmployeeID",  employeeID);
                    cmd.Parameters.AddWithValue("@OrderDate",   orderDate);
                    cmd.Parameters.AddWithValue("@ShipAddress", shipAddress);
                    cmd.Parameters.AddWithValue("@OrderID",     orderID);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        MessageBox.Show("No order was updated. The record may no longer exist.",
                                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                MessageBox.Show("Order updated successfully.", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                LoadOrders();
            }
            catch (Exception ex)
            {
                ShowError("Error updating order", ex);
            }
        }

        /// <summary>
        /// Deletes the currently selected order after user confirmation.
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtOrderID.Text, out int orderID))
            {
                MessageBox.Show("Please select an order from the grid to delete.",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ask the user to confirm before deleting.
            MessageBoxResult confirm = MessageBox.Show(
                $"Are you sure you want to delete Order #{orderID}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    const string sql = "DELETE FROM Orders WHERE OrderID = @OrderID";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@OrderID", orderID);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Order deleted successfully.", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                LoadOrders();
            }
            catch (Exception ex)
            {
                ShowError("Error deleting order", ex);
            }
        }

        /// <summary>
        /// Clears the search box and reloads all orders in the DataGrid.
        /// </summary>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearchOrderID.Clear();
            ClearForm();
            LoadOrders();
        }

        /// <summary>
        /// Filters the DataGrid to display only the order matching the entered ID.
        /// </summary>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtSearchOrderID.Text.Trim(), out int orderID))
            {
                MessageBox.Show("Please enter a valid numeric Order ID to search.",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LoadOrders(orderID);
        }

        /// <summary>
        /// Clears the search filter and reloads all orders.
        /// </summary>
        private void BtnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearchOrderID.Clear();
            LoadOrders();
        }

        // -----------------------------------------------------------------------
        // DataGrid selection
        // -----------------------------------------------------------------------

        /// <summary>
        /// When a row is selected in the DataGrid:
        ///   1. Populates the form fields with the selected order's data.
        ///   2. Shows a MessageBox listing the OrderID and all product names
        ///      associated with that order (from Order Details / Products tables).
        /// </summary>
        private void DgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ignore programmatic selection changes (e.g., during grid refresh).
            if (_suppressSelectionMessage)
                return;

            if (dgOrders.SelectedItem is not DataRowView row)
                return;

            // Populate the form fields from the selected grid row.
            txtOrderID.Text   = row["OrderID"].ToString();
            txtOrderDate.Text = row["OrderDate"] == DBNull.Value
                ? string.Empty
                : Convert.ToDateTime(row["OrderDate"]).ToString("yyyy-MM-dd");
            txtShipAddress.Text = row["ShipAddress"]?.ToString() ?? string.Empty;

            // Retrieve CustomerID and EmployeeID from the database and set the dropdowns.
            if (int.TryParse(txtOrderID.Text, out int orderID))
            {
                SelectComboValues(orderID);

                // Show a popup listing the products in this order (assignment requirement).
                ShowOrderProducts(orderID);
            }
        }

        // -----------------------------------------------------------------------
        // Private helpers
        // -----------------------------------------------------------------------

        /// <summary>
        /// Looks up CustomerID and EmployeeID for the given order and selects the
        /// matching items in the Customer and Employee ComboBoxes.
        /// </summary>
        private void SelectComboValues(int orderID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    const string sql =
                        "SELECT CustomerID, EmployeeID FROM Orders WHERE OrderID = @OrderID";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@OrderID", orderID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Set the customer dropdown.
                            cbCustomer.SelectedValue = reader["CustomerID"].ToString();

                            // Set the employee dropdown (may be NULL in the database).
                            cbEmployee.SelectedValue = reader["EmployeeID"] == DBNull.Value
                                ? null
                                : (object)Convert.ToInt32(reader["EmployeeID"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Error retrieving order details", ex);
            }
        }

        /// <summary>
        /// Queries the Order Details and Products tables to retrieve all product names
        /// for the specified order, then displays them in a MessageBox.
        /// </summary>
        private void ShowOrderProducts(int orderID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    const string sql =
                        "SELECT p.ProductName " +
                        "FROM [Order Details] od " +
                        "JOIN Products p ON od.ProductID = p.ProductID " +
                        "WHERE od.OrderID = @OrderID " +
                        "ORDER BY p.ProductName";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@OrderID", orderID);

                    StringBuilder products = new StringBuilder();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            products.AppendLine($"  • {reader["ProductName"]}");
                    }

                    string message = products.Length > 0
                        ? $"Order ID: {orderID}\n\nProducts:\n{products}"
                        : $"Order ID: {orderID}\n\n(No products found for this order.)";

                    MessageBox.Show(message, "Order Products",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading order products", ex);
            }
        }

        /// <summary>
        /// Validates all required input fields. Returns <c>true</c> when all inputs
        /// are valid; otherwise shows a user-friendly warning and returns <c>false</c>.
        /// </summary>
        private bool ValidateInputs(
            out string   customerID,
            out int      employeeID,
            out DateTime orderDate,
            out string   shipAddress)
        {
            customerID  = string.Empty;
            employeeID  = 0;
            orderDate   = DateTime.MinValue;
            shipAddress = string.Empty;

            // Customer selection is required.
            if (cbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer.",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Employee selection is required.
            if (cbEmployee.SelectedValue == null)
            {
                MessageBox.Show("Please select an employee.",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Order Date must be a parseable date.
            if (!DateTime.TryParse(txtOrderDate.Text.Trim(), out orderDate))
            {
                MessageBox.Show("Please enter a valid date (e.g. 2024-01-31).",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Ship Address must not be empty.
            if (string.IsNullOrWhiteSpace(txtShipAddress.Text))
            {
                MessageBox.Show("Please enter a shipping address.",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // All validations passed – extract typed values.
            customerID  = cbCustomer.SelectedValue.ToString()!;
            employeeID  = Convert.ToInt32(cbEmployee.SelectedValue);
            shipAddress = txtShipAddress.Text.Trim();
            return true;
        }

        /// <summary>
        /// Resets all form input fields and clears the ComboBox selections.
        /// </summary>
        private void ClearForm()
        {
            txtOrderID.Clear();
            cbCustomer.SelectedIndex  = -1;
            cbEmployee.SelectedIndex  = -1;
            txtOrderDate.Clear();
            txtShipAddress.Clear();
        }

        /// <summary>
        /// Displays an error MessageBox with the exception message.
        /// </summary>
        private static void ShowError(string context, Exception ex)
        {
            MessageBox.Show($"{context}:\n\n{ex.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

