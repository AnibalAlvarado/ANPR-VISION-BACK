# Flujo completo de trabajo

## 1. Crear red compartida (una sola vez)

```bash
docker network create backend-network
```

## 2. Levantar bases de datos

```bash
docker compose -f docker-compose.databases.yml up -d
```

## 3. Conectar bases de datos a la red del backend

```bash
docker network connect backend-network postgres_db
docker network connect backend-network mysql_db
docker network connect backend-network sqlserver_db
```

## 4. Levantar backend

```bash
cd Web
docker compose up -d
```

## 5. Crear migraciones

### Linux/macOS:
```bash
cd Entity/tools
./migrate.sh Init
```

### Windows PowerShell:
```powershell
cd Entity\tools
.\migrate.ps1 Init
```

## 6. Aplicar migraciones

### Linux/macOS:
```bash
./update.sh
```

### Windows PowerShell:
```powershell
.\update.ps1
```