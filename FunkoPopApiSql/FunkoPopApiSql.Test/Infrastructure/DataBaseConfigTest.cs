using System.Reflection;
using FluentAssertions;
using FunkoPopApiSql.Config;
using FunkoPopApiSql.Entity;
using FunkoPopApiSql.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunkoPopApiSql.Test.Infrastructure;

[TestFixture]
[NonParallelizable]
public class DataBaseConfigTests {

    private IConfiguration? _configuracionOriginal;
    private IConfigurationRoot? _configuracionPrueba;

    [SetUp]
    public void SetUp() {
        _configuracionOriginal = FunkoPopConfiguration.Config;
    }

    [TearDown]
    public void TearDown() {
        AsignarConfiguracion(_configuracionOriginal);
        (_configuracionPrueba as IDisposable)?.Dispose();
    }

    private void EstablecerConfiguracion(string nombre, string conexion) {
        _configuracionPrueba = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> {
                ["Repository:Name"] = nombre,
                ["Repository:ConnectionString"] = conexion
            })
            .Build();

        AsignarConfiguracion(_configuracionPrueba);
    }

    private static void AsignarConfiguracion(IConfiguration? configuracion) {
        var propiedad = typeof(FunkoPopConfiguration).GetProperty(
            nameof(FunkoPopConfiguration.Config),
            BindingFlags.Public | BindingFlags.Static
        )!;

        propiedad.SetValue(null, configuracion);
    }

    [TestCase("SQLite", "Data Source=:memory:", "Microsoft.EntityFrameworkCore.Sqlite")]
    [TestCase("PostgreSQL", "Host=localhost;Database=test;Username=postgres;Password=test", "Npgsql.EntityFrameworkCore.PostgreSQL")]
    public void AddDataBaseConfig_ProveedorValido_RegistraContexto(string nombre, string conexion, string proveedorEsperado) {
        // Arrange
        EstablecerConfiguracion(nombre, conexion);
        var services = new ServiceCollection();

        // Act
        var resultado = services.AddDataBaseConfig();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Assert
        resultado.Should().BeSameAs(services);
        services.Should().Contain(d =>
            d.ServiceType == typeof(AppDbContext) &&
            d.Lifetime == ServiceLifetime.Scoped);
        context.Database.ProviderName.Should().Be(proveedorEsperado);
    }

    [Test]
    public void AddDataBaseConfig_ProveedorNoValido_LanzaExcepcion() {
        // Arrange
        EstablecerConfiguracion("MySQL", "conexion-prueba");
        var services = new ServiceCollection();

        // Act
        Action accion = () => services.AddDataBaseConfig();

        // Assert
        accion.Should().Throw<InvalidOperationException>().WithMessage("*Proveedor de base de datos no soportado*");
    }
}
