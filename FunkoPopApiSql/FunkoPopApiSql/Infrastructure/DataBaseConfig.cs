using FunkoPopApiSql.Config;
using FunkoPopApiSql.Entity;
using Microsoft.EntityFrameworkCore;

namespace FunkoPopApiSql.Infrastructure;

/// <summary>
/// Configura la base de datos SQL de la API de Funkos utilizando SQLite o PostgreSQL.
/// </summary>
public static class DataBaseConfig {

    /// <summary>
    /// Registra AppDbContext con el proveedor y la cadena de conexión
    /// establecidos en FunkoPopConfiguration.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación.</param>
    /// <returns>Colección de servicios con AppDbContext registrado.</returns>
    /// <exception cref="InvalidOperationException"> Se produce cuando el proveedor de base de datos no está soportado.</exception>
    public static IServiceCollection AddDataBaseConfig(this IServiceCollection services) {
        var nameProperty = FunkoPopConfiguration.RepositoryName;
        var connection = FunkoPopConfiguration.DbConnection;

        switch (nameProperty) {
            case "SQLite": 
                services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));
                break;
            case "PostgreSQL": 
                services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));
                break;
            default: 
                throw new InvalidOperationException($"Proveedor de base de datos no soportado: {nameProperty}");
        }
        return services;
    }
}