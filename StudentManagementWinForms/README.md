# StudentManagementWinForms

A minimal Windows Forms app in C# (.NET 8) that matches the layout in your screenshot:
- **New Student** form on the left with fields: First Name, Last Name, Age, Program, Web/Comm
- Buttons along the bottom: Load Student Data, Save Student Data, Add New Student
- A **ListBox** on the right showing students in the format `Last, First (Age) - Program [Web/Comm]`

## How to open
1. Unzip the archive.
2. Double-click `StudentManagementWinForms.csproj` (or open the folder in Visual Studio 2022/2025/2026).
3. Set configuration to `Debug` and press **F5**.

> Requires the .NET 8 SDK (or newer). Visual Studio will offer to install the missing workload if needed: **.NET Desktop Development**.

## Features
- Add a student with validation for required fields and numeric Age.
- Save/Load students from `students.json` stored next to the executable.
- Strongly-typed `Student` model and `BindingList<T>` for live updates of the ListBox.

## Files
- `StudentManagementWinForms.csproj` – project file targeting `net8.0-windows`.
- `Program.cs` – app entry point.
- `Form1.cs` & `Form1.Designer.cs` – UI and event handlers.
- `Student.cs` – data model.

## Notes
- You can redesign using the Visual Designer; the code-behind will continue to work.
- To persist somewhere else, change `_dataPath` in `Form1.cs`.
