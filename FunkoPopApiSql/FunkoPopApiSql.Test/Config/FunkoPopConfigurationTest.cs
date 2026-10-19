using System.Reflection;
using FluentAssertions;
using FunkoPopApiSql.Config;
using Microsoft.Extensions.Configuration;

namespace FunkoPopApiSql.Test.Config;

/// <summary>
/// Pruebas unitarias de la configuración global de la API de Funkos.
/// </summary>
[TestFixture]
[NonParallelizable]
public class FunkoPopConfigurationTests {

    private IConfiguration? _configuracionOriginal;
    
    [SetUp]
    public void SetUp() {
        _configuracionOriginal = FunkoPopConfiguration.Config;
    }
    
    [TearDown]
    public void TearDown() {
        var configuracionActual = FunkoPopConfiguration.Config;
        AsignarConfiguracion(_configuracionOriginal);

        if (!ReferenceEquals(configuracionActual, _configuracionOriginal))
            (configuracionActual as IDisposable)?.Dispose();
    }
    
    private static void EstablecerConfiguracion(Dictionary<string, string?> valores) {
        var configuracion = new ConfigurationBuilder()
            .AddInMemoryCollection(valores)
            .Build();

        AsignarConfiguracion(configuracion);
    }
    
    private static void AsignarConfiguracion(IConfiguration? configuracion) {
        var propiedad = typeof(FunkoPopConfiguration).GetProperty(
            nameof(FunkoPopConfiguration.Config),
            BindingFlags.Public | BindingFlags.Static
        )!;

        propiedad.SetValue(null, configuracion);
    }
    
    [TestCase("SQLite")]
    [TestCase("PostgreSQL")]
    public void RepositoryName_ConValorConfigurado_RetornaValor(string nombre) {
        // Arrange
        EstablecerConfiguracion(new Dictionary<string, string?> {
            ["Repository:Name"] = nombre
        });

        // Act
        var resultado = FunkoPopConfiguration.RepositoryName;

        // Assert
        resultado.Should().Be(nombre);
    }
    
    [Test]
    public void DbConnection_ConValorConfigurado_RetornaValor() {
        // Arrange
        const string conexion = "Data Source=funkos-test.db";

        EstablecerConfiguracion(new Dictionary<string, string?> {
            ["Repository:ConnectionString"] = conexion
        });

        // Act
        var resultado = FunkoPopConfiguration.DbConnection;

        // Assert
        resultado.Should().Be(conexion);
    }
    
    [TestCase("Development", "SQLite", "Data Source=funkopop-dev.db")]
    [TestCase("Production", "PostgreSQL", "Database=funkopop_db")]
    public void Init_EntornoValido_CargaConfiguracion(string entorno, string repositorio, string conexion) {
        // Act
        FunkoPopConfiguration.Init(entorno);

        // Assert
        FunkoPopConfiguration.Config.Should().NotBeNull();
        FunkoPopConfiguration.RepositoryName.Should().Be(repositorio);
        FunkoPopConfiguration.DbConnection.Should().Contain(conexion);
    }
    
    [Test]
    public void Init_Development_CargaConfiguracionComun() {
        // Arrange
        const string entorno = "Development";

        // Act
        FunkoPopConfiguration.Init(entorno);

        // Assert
        FunkoPopConfiguration.Config["AllowedHosts"].Should().Be("*");
    }
    
    [Test]
    public void RepositoryName_SinValorConfigurado_LanzaExcepcion() {
        // Arrange
        EstablecerConfiguracion(new Dictionary<string, string?>());

        // Act
        Action accion = () => { _ = FunkoPopConfiguration.RepositoryName; };

        // Assert
        accion.Should().Throw<InvalidOperationException>().WithMessage("*Repository:Name*");
    }
    
    [Test]
    public void DbConnection_SinValorConfigurado_LanzaExcepcion() {
        // Arrange
        EstablecerConfiguracion(new Dictionary<string, string?>());

        // Act
        Action accion = () => { _ = FunkoPopConfiguration.DbConnection; };

        // Assert
        accion.Should().Throw<InvalidOperationException>().WithMessage("*Repository:ConnectionString*");
    }
    
    [Test]
    public void Init_EntornoNoValido_LanzaExcepcion() {
        // Arrange
        const string entorno = "Testing";
        var configuracionAnterior = FunkoPopConfiguration.Config;

        // Act
        Action accion = () => FunkoPopConfiguration.Init(entorno);

        // Assert
        accion.Should().Throw<ArgumentException>().WithMessage("*Entorno no soportado*");
        FunkoPopConfiguration.Config.Should().BeSameAs(configuracionAnterior);
    }
}