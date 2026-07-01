# Module3_C#ADO.NET

This module contains 30 C# console examples demonstrating core language features and ADO.NET usage.

Requirements
- .NET 8 SDK (recommended for C# 12 features). If you don't have C# 12 support, some features (primary constructors, `required`) may need adjustments.

Run
- Build and run a single exercise:

```bash
dotnet run --project "Upskilling/Module3_C#ADO.NET/Module3_CSharp_ADO.csproj" 1
```

- Run all exercises:

```bash
dotnet run --project "Upskilling/Module3_C#ADO.NET/Module3_CSharp_ADO.csproj" all
```

Notes
- Exercise 30 (ADO.NET CRUD) uses `Microsoft.Data.SqlClient`. Update the connection string in `Program.cs` before running.
- The project file includes the package reference; run `dotnet restore` if needed.
- `user.json`, `sample.txt`, and `log.txt` may be created by exercises.

Sample Employees SQL (create a test database and user as needed):
- `Employees` table is created automatically by Exercise 30 if it does not exist.

