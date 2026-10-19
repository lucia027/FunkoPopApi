using Microsoft.Extensions.Configuration;

namespace FunkoPopApiSql.Config;

/// <summary>
/// Gestiona la configuración global de la API de Funkos a partir de los archivos appsettings.
/// </summary>
public static class FunkoPopConfiguration {

    /// <summary>
    /// Configuración cargada de los archivos JSON.
    /// </summary>
    public static IConfiguration Config { get; private set; } = null!;

    /// <summary>
    /// Inicializa la configuración según el entorno de ejecución.
    /// </summary>
    /// <param name="environment">Nombre del entorno de ejecución.</param>
    /// <exception cref="ArgumentException">El entorno no está soportado.</exception>
    public static void Init(string environment) {
        var file = environment switch {
            "Development" => "appsettings.Development.json",
            "Production" => "appsettings.Production.json",
            _ => throw new ArgumentException($"Entorno no soportado: {environment}")
        };

        // Cargar la configuración común y la específica del entorno.
        Config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(file, optional: false, reloadOnChange: true)
            .Build();
    }

    /// <summary>
    /// Nombre del proveedor de base de datos configurado.
    /// </summary>
    public static string RepositoryName => Config["Repository:Name"] ?? throw new InvalidOperationException("Falta Repository:Name.");

    /// <summary>
    /// Cadena de conexión de la base de datos configurada.
    /// </summary>
    public static string DbConnection => Config["Repository:ConnectionString"] ?? throw new InvalidOperationException("Falta Repository:ConnectionString.");
}