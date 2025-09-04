#!/bin/bash
# ./update.sh
# Aplica las migraciones en PostgreSQL, SQL Server y MySQL

# PostgreSQL
echo "🔹 Aplicando migraciones en PostgreSQL..."
DatabaseProvider=PostgreSql \
ConnectionStrings__PostgreSql="Host=localhost;Port=5432;Database=anprvision;Username=devuser;Password=DevPass123!" \
dotnet ef database update \
  --project ../Entity.csproj \
  --context ApplicationDbContext

# SQL Server
echo "🔹 Aplicando migraciones en SQL Server..."
DatabaseProvider=SqlServer \
ConnectionStrings__SqlServer="Server=localhost,1433;Database=anprvision;User Id=sa;Password=Admin123!" \
dotnet ef database update \
  --project ../Entity.csproj \
  --context ApplicationDbContext

# MySQL
echo "🔹 Aplicando migraciones en MySQL..."
DatabaseProvider=MySql \
ConnectionStrings__MySql="Server=localhost;Port=3306;Database=anprvision;User=devuser;Password=DevPass123!" \
dotnet ef database update \
  --project ../Entity.csproj \
  --context ApplicationDbContext
