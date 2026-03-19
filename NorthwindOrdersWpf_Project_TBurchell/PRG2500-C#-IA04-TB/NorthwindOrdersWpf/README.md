
# Northwind Orders Manager (WPF + ADO.NET)

A Windows desktop WPF application for managing **Orders** in the **Northwind** database using **ADO.NET** (`SqlConnection`, `SqlCommand`, `SqlDataAdapter`).

## Features
- DataGrid listing Orders (OrderID, Customer, Employee, OrderDate, ShipAddress)
- Add, Update, Delete orders
- Search by **OrderID**
- On grid selection, a message shows **OrderID** and **Product Names** in the order
- Customer/Employee dropdowns populated dynamically from `Customers` and `Employees`
- Immediate grid refresh after any CRUD action
- Basic validation & error handling
- Simple WPF styling

## Prerequisites
- Visual Studio 2022+
- .NET 6 SDK
- SQL Server with **Northwind** sample database installed

## Configure connection string
Edit `App.config`:
```
<add name="Northwind" connectionString="Server=.;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;" />
```
Change `Server` and `Database` as needed.

## Build & Run
1. Open the folder in Visual Studio (`NorthwindOrdersWpf.csproj`)
2. Restore NuGet packages
3. Run (F5)

## Notes
- Delete operation removes from `[Order Details]` first, then `Orders` (FK safety)
- OrderDate uses format `yyyy-MM-dd`

## Structure
```
NorthwindOrdersWpf/
  DAL/DataAccess.cs           # All ADO.NET CRUD & queries
  Models/Customer.cs          # DTOs
  Models/Employee.cs
  Models/OrderRecord.cs
  App.xaml, App.xaml.cs       # Global styles
  MainWindow.xaml(+.cs)       # UI + event handlers
  App.config                  # Connection string
  NorthwindOrdersWpf.csproj   # SDK-style WPF project (.NET 6)
```
