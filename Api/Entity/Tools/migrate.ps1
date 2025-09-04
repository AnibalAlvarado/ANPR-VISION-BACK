param([string]$Name)

# PostgreSQL
$env:DatabaseProvider="PostgreSql"
dotnet ef migrations add "${Name}_Postgres" --context ApplicationDbContext --output-dir Migrations/Postgres

# SQL Server
$env:DatabaseProvider="SqlServer"
dotnet ef migrations add "${Name}_SqlServer" --context ApplicationDbContext --output-dir Migrations/SqlServer

# MySQL
$env:DatabaseProvider="MySql"
dotnet ef migrations add "${Name}_MySql" --context ApplicationDbContext --output-dir Migrations/MySql
