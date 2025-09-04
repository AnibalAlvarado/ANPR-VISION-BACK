# PostgreSQL
$env:DatabaseProvider="PostgreSql"
dotnet ef database update --context ApplicationDbContext

# SQL Server
$env:DatabaseProvider="SqlServer"
dotnet ef database update --context ApplicationDbContext

# MySQL
$env:DatabaseProvider="MySql"
dotnet ef database update --context ApplicationDbContext
