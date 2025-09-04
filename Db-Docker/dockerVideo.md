
docker --version          # Ver la versión instalada de Docker
docker compose version    # Ver la versión instalada de Docker Compose

docker pull mysql:9.0      # Descargar imagen de MySQL versión 9.0
docker pull postgres:17    # Descargar imagen de PostgreSQL versión 17
docker pull mcr.microsoft.com/mssql/server:2022-latest   # Descargar SQL Server 2022


docker run -d --name postgres_db \
  -e POSTGRES_DB=companydb \
  -e POSTGRES_USER=devuser \
  -e POSTGRES_PASSWORD=DevPass123! \
  -p 5432:5432 \
  postgres:17
# Levanta un contenedor PostgreSQL en segundo plano (-d), creando la base companydb en el puerto 5432

docker compose up -d           # Levanta todos los servicios definidos en docker-compose.yml
docker compose up -d mysql     # Levanta solo MySQL
docker compose up -d postgres  # Levanta solo PostgreSQL
docker compose up -d sqlserver # Levanta solo SQL Server
docker compose stop            # Detiene todos los servicios del proyecto
docker compose down            # Elimina los contenedores del proyecto (mantiene volúmenes/datos)
docker compose down -v         # Elimina contenedores + volúmenes (también los datos)



docker ps               # Lista los contenedores en ejecución
docker ps -a            # Lista todos los contenedores (incluidos apagados)
docker stop mysql_db    # Detiene un contenedor específico
docker start mysql_db   # Arranca un contenedor detenido
docker restart mysql_db # Reinicia un contenedor
docker rm mysql_db      # Elimina un contenedor detenido


docker images              # Lista todas las imágenes descargadas en tu máquina
docker rmi mysql:9.0       # Elimina la imagen de MySQL 9.0

docker logs mysql_db        # Muestra los logs del contenedor mysql_db
docker logs -f postgres_db  # Muestra logs en vivo (modo seguimiento)
docker inspect sqlserver_db # Muestra toda la configuración e info de sqlserver_db en formato JSON


docker exec -it mysql_db bash    # Abrir una shell bash dentro del contenedor MySQL
docker exec -it postgres_db psql -U devuser -d companydb   # Abrir consola SQL de PostgreSQL
docker exec -it sqlserver_db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Admin123!"  # Abrir consola SQL Server


docker pause postgres_db   # Pausa temporalmente el contenedor (congela procesos)
docker unpause postgres_db # Reanuda el contenedor pausado

