# Comandos de Migración

## Linux/macOS (Bash)

```bash
./migrate.sh Init
./update.sh
```

## Windows (PowerShell)

```powershell
.\migrate.ps1 Init
.\update.ps1
```

## Dentro del contenedor backend

```bash
docker exec -it backend_web bash
cd /app/Entity/tools
./migrate.sh Init
./update.sh
```