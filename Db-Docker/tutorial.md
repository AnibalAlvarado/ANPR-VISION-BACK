# Guía  Manejo de bases de datos con Docker Compose

Este `docker-compose.yml` levanta **MySQL, PostgreSQL y SQL Server** en contenedores separados. 



```bash
# Levantar las 3 bases
docker-compose up -d

# Levantar solo MySQL
docker-compose up -d mysql

# Levantar solo PostgreSQL
docker-compose up -d postgres

# Levantar solo SQL Server
docker-compose up -d sqlserver

# Levantar MySQL + PostgreSQL (dos específicas)
docker-compose up -d mysql postgres

# Ver estado de los contenedores
docker ps

# Parar un servicio (ej. PostgreSQL)
docker-compose stop postgres

# Parar todos los servicios
docker-compose stop

# Eliminar un servicio (contenedor) pero mantener los datos
docker-compose rm -f postgres

# Eliminar todos los contenedores (los volúmenes/datos persisten)
docker-compose down

# Eliminar todo (contenedores + volúmenes/datos)
docker-compose down -v

