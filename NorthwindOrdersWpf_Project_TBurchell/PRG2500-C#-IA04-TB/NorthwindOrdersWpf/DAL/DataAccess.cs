// Tom Burchell
// 2024-06-01
// Data access class for Northwind Orders WPF application. Provides methods to retrieve, insert, update, and delete orders,
// as well as to get lists of customers, employees, and product names for orders.
// PROG 2500 - Programming in C#

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NorthwindOrdersWpf.Models;

namespace NorthwindOrdersWpf.DAL
{
    public class DataAccess// Data access class for Northwind Orders WPF application
    {
        private readonly string _connString;// Connection string for database access
        public DataAccess()
        {
            _connString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;// Get connection string from app.config
        }

        private SqlConnection CreateConn() => new SqlConnection(_connString);// Helper method to create a new SqlConnection

        public DataTable GetOrders(int? orderId = null)// Retrieves orders from the database, optionally filtered by order ID. Returns a DataTable with order details.
        {
            using var conn = CreateConn();// Create a new database connection
            string sql = @"SELECT o.OrderID,
                                  o.CustomerID,
                                  o.EmployeeID,
                                  o.OrderDate,
                                  o.ShipAddress,
                                  c.CompanyName AS CustomerName,
                                  e.FirstName + ' ' + e.LastName AS EmployeeName
                           FROM Orders o
                           INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                           INNER JOIN Employees e ON o.EmployeeID = e.EmployeeID
                           " + (orderId.HasValue ? " WHERE o.OrderID = @OrderID" : "") +
                           " ORDER BY o.OrderID DESC";
            using var da = new SqlDataAdapter(sql, conn);// Create a SqlDataAdapter to execute the query and fill a DataTable
            if (orderId.HasValue)// If an order ID is provided, add it as a parameter to the query
                da.SelectCommand!.Parameters.AddWithValue("@OrderID", orderId.Value);// Fill the DataTable with the results of the query and return it
            var table = new DataTable();// Create a new DataTable to hold the results
            da.Fill(table);// Fill the DataTable with the results of the query
            return table;// Return the filled DataTable
        }

        public List<Customer> GetCustomers()// Retrieves a list of customers from the database. Returns a List of Customer objects.
        {
            using var conn = CreateConn();// Create a new database connection
            const string sql = "SELECT CustomerID, CompanyName FROM Customers ORDER BY CompanyName";// SQL query to select customer ID and company name, ordered by company name
            using var cmd = new SqlCommand(sql, conn);// Create a SqlCommand to execute the query
            conn.Open();// Open the database connection
            using var rdr = cmd.ExecuteReader();// Execute the query and get a SqlDataReader to read the results
            var list = new List<Customer>();// Create a new List to hold the Customer objects
            while (rdr.Read())// Loop through the results and create a Customer object for each row, adding it to the list
            {
                list.Add(new Customer { CustomerID = rdr.GetString(0), CompanyName = rdr.GetString(1) });// Add a new Customer object to the list with the customer ID and company name from the current row
            }
            return list;// Return the list of Customer objects
        }

        public List<Employee> GetEmployees()// Retrieves a list of employees from the database. Returns a List of Employee objects.
        {
            using var conn = CreateConn();// Create a new database connection
            const string sql = "SELECT EmployeeID, FirstName + ' ' + LastName AS FullName FROM Employees ORDER BY FullName";
            using var cmd = new SqlCommand(sql, conn);// Create a SqlCommand to execute the query
            conn.Open();// Open the database connection
            using var rdr = cmd.ExecuteReader();// Execute the query and get a SqlDataReader to read the results
            var list = new List<Employee>();// Create a new List to hold the Employee objects
            while (rdr.Read())// Loop through the results and create an Employee object for each row, adding it to the list
            {
                list.Add(new Employee { EmployeeID = rdr.GetInt32(0), FullName = rdr.GetString(1) });// Add a new Employee object to the list with the employee ID and full name from the current row
            }
            return list;// Return the list of Employee objects
        }

        public int InsertOrder(string customerId, int employeeId, DateTime orderDate, string shipAddress)// Inserts a new order into the database with the provided details. Returns the ID of the newly inserted order.
        {
            using var conn = CreateConn();// Create a new database connection
            conn.Open();//Open the database connection
            using var tx = conn.BeginTransaction();// Begin a new transaction to ensure that the insert operation is atomic
            try// Try to execute the insert operation
            {
                const string sql = @"INSERT INTO Orders (CustomerID, EmployeeID, OrderDate, ShipAddress)
                                      VALUES (@CustomerID, @EmployeeID, @OrderDate, @ShipAddress);";// SQL query to insert a new order with parameters for customer ID, employee ID, order date, and ship address
                using var cmd = new SqlCommand(sql, conn, tx);// Create a SqlCommand to execute the insert query within the transaction
                cmd.Parameters.AddWithValue("@CustomerID", customerId);// Add parameters to the SqlCommand for the customer ID, employee ID, order date, and ship address
                cmd.Parameters.AddWithValue("@EmployeeID", employeeId);// Add parameter for employee ID
                cmd.Parameters.AddWithValue("@OrderDate", orderDate);// Add parameter for order date
                cmd.Parameters.AddWithValue("@ShipAddress", shipAddress);// Add parameter for ship address
                cmd.ExecuteNonQuery();// Execute the insert command to add the new order to the database
                using var idCmd = new SqlCommand("SELECT CAST(SCOPE_IDENTITY() AS int)", conn, tx);// Create a new SqlCommand to retrieve the ID of the newly inserted order using SCOPE_IDENTITY()
                int newId = (int)idCmd.ExecuteScalar()!;// Execute the command to get the new order ID and store it in a variable
                tx.Commit();// Commit the transaction to finalize the insert operation
                return newId;// Return the ID of the newly inserted order
            }
            catch// If an error occurs during the insert operation, roll back the transaction to maintain data integrity and rethrow the exception
            {
                tx.Rollback();// Roll back the transaction to undo any changes made during the insert operation
                throw;// Rethrow the exception to be handled by the calling code
            }
        }

        public void UpdateOrder(int orderId, string customerId, int employeeId, DateTime orderDate, string shipAddress)// Updates an existing order in the database with the provided details based on the order ID.
        {
            using var conn = CreateConn(); // Create a new database connection
            const string sql = @"UPDATE Orders
                                  SET CustomerID=@CustomerID, EmployeeID=@EmployeeID, OrderDate=@OrderDate, ShipAddress=@ShipAddress
                                  WHERE OrderID=@OrderID";// SQL query to update an existing order with parameters for customer ID, employee ID, order date, ship address, and order ID
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CustomerID", customerId);// Add parameters to the SqlCommand for the customer ID, employee ID, order date, ship address, and order ID
            cmd.Parameters.AddWithValue("@EmployeeID", employeeId);// Add parameter for employee ID
            cmd.Parameters.AddWithValue("@OrderDate", orderDate);// Add parameter for order date
            cmd.Parameters.AddWithValue("@ShipAddress", shipAddress);// Add parameter for ship address
            cmd.Parameters.AddWithValue("@OrderID", orderId);// Add parameter for order ID
            conn.Open();//Open the database connection
            cmd.ExecuteNonQuery();// Execute the update command to modify the existing order in the database
        }

        public void DeleteOrder(int orderId)// Deletes an order from the database based on the provided order ID. Also deletes related order details to maintain referential integrity.
        {
            using var conn = CreateConn();// Create a new database connection
            conn.Open();//Open the database connection
            using var tx = conn.BeginTransaction();// Begin a new transaction to ensure that the delete operation is atomic, especially since it involves deleting from multiple tables to maintain referential integrity
            try// Try to execute the delete operation
            {
                using (var cmdDetails = new SqlCommand("DELETE FROM [Order Details] WHERE OrderID=@OrderID", conn, tx))// Create a SqlCommand to delete related order details for the specified order ID within the transaction
                {
                    cmdDetails.Parameters.AddWithValue("@OrderID", orderId);// Add parameter for order ID to the command that deletes order details
                    cmdDetails.ExecuteNonQuery();// Execute the command to delete the related order details from the database
                }
                using (var cmd = new SqlCommand("DELETE FROM Orders WHERE OrderID=@OrderID", conn, tx))// Create a SqlCommand to delete the order itself based on the order ID within the transaction
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);// Add parameter for order ID to the command that deletes the order
                    cmd.ExecuteNonQuery();// Execute the command to delete the order from the database
                }
                tx.Commit();// Commit the transaction to finalize the delete operation, ensuring that both the order and its related details are removed from the database
            }
            catch// If an error occurs during the delete operation, roll back the transaction to maintain data integrity and rethrow the exception
            {
                tx.Rollback();// Roll back the transaction to undo any changes made during the delete operation
                throw;// Rethrow the exception to be handled by the calling code
            }
        }

        public List<string> GetProductNamesForOrder(int orderId)// Retrieves a list of product names associated with a specific order ID. Returns a List of product names.
        {
            using var conn = CreateConn();// Create a new database connection
            const string sql = @"SELECT p.ProductName
                                 FROM [Order Details] od
                                 INNER JOIN Products p ON od.ProductID = p.ProductID
                                 WHERE od.OrderID=@OrderID
                                 ORDER BY p.ProductName";
            using var cmd = new SqlCommand(sql, conn);// Create a SqlCommand to execute the query that retrieves product names for the specified order ID
            cmd.Parameters.AddWithValue("@OrderID", orderId);// Add parameter for order ID to the command 
            conn.Open();//Open the database connection
            using var rdr = cmd.ExecuteReader();// Execute the query and get a SqlDataReader to read the results
            var names = new List<string>();// Create a new List to hold the product names
            while (rdr.Read()) names.Add(rdr.GetString(0));// Loop through the results and add each product name to the list
            return names;// Return the list of product names associated with the specified order ID
        }
    }
}
