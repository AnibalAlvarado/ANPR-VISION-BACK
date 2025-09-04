#!/bin/bash
NAME=$1

# PostgreSQL
DatabaseProvider=PostgreSql dotnet ef migrations add ${NAME}_Postgres \
  --context ApplicationDbContext \
  --project ../Entity.csproj \
  --startup-project ../../Web/Web.csproj \
  --output-dir Migrations/Postgres

# SQL Server
DatabaseProvider=SqlServer dotnet ef migrations add ${NAME}_SqlServer \
  --context ApplicationDbContext \
  --project ../Entity.csproj \
  --startup-project ../../Web/Web.csproj \
  --output-dir Migrations/SqlServer

# MySQL
DatabaseProvider=MySql dotnet ef migrations add ${NAME}_MySql \
  --context ApplicationDbContext \
  --project ../Entity.csproj \
  --startup-project ../../Web/Web.csproj \
  --output-dir Migrations/MySql
