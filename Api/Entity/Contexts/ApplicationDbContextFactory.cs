using Entity.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Entity.Contexts
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // 🔹 Define el proveedor que quieras usar (ajusta según la migración que necesites correr)
            var provider = Environment.GetEnvironmentVariable("DatabaseProvider") ?? "PostgreSql";

            switch (provider)
            {
                case "PostgreSql":
                    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=anprvision;Username=devuser;Password=DevPass123!");
                    break;

                case "SqlServer":
                    optionsBuilder.UseSqlServer("Server=localhost,1433;Database=anprvision;User Id=sa;Password=Admin123!");
                    break;

                case "MySql":
                    optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=anprvision;User=devuser;Password=DevPass123!");
                    break;

                default:
                    throw new InvalidOperationException($"Proveedor {provider} no soportado.");
            }

            // ⚡ Pasa null porque tu DbContext espera IConfiguration pero en design-time no se usa
            return new ApplicationDbContext(optionsBuilder.Options, null!);
        }
    }
}
