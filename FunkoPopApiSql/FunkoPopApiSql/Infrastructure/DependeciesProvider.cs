using FunkoPopApiSql.Infrastructure.Interfaces;

namespace FunkoPopApiSql.Infrastructure;

/// <summary>
/// Proveedor de dependencias de la API de Funkos.
/// Registra automáticamente las clases mediante Scrutor.
/// </summary>
public static class DependenciesProvider {

    /// <summary>
    /// Registra las dependencias según su ciclo de vida
    /// utilizando las interfaces marcadoras.
    /// </summary>
    /// <param name="services">Colección de servicios de ASP.NET Core.</param>
    /// <returns>Colección de servicios con las dependencias registradas.</returns>
    public static IServiceCollection AddDependencies(this IServiceCollection services) {
        services.Scan(scan => scan
            .FromAssemblyOf<Program>()
            .AddClasses(c => c.AssignableTo<ITransientService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime()
            .AddClasses(c => c.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(c => c.AssignableTo<ISingletonService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
        return services;
    }
}