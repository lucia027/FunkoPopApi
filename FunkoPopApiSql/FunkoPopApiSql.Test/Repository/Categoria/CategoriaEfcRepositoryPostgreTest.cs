using FluentAssertions;
using FunkoPopApiSql.Entity;
using FunkoPopApiSql.Repository.Categoria;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace FunkoPopApiSql.Test.Repository.Categoria;

[TestFixture]
[Category("Integration")]
[Category("Postgre")]
[NonParallelizable]
public class CategoriaEfcRepositoryPostgreTest {

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .WithDatabase("funkopop_tests")
        .WithUsername("test")
        .WithPassword("test")
        .Build();

    private AppDbContext _context = null!;
    private CategoriaEfcRepository _repository = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp() {
        // Iniciar PostgreSQL antes de ejecutar los tests.
        await _container.StartAsync();
    }

    [SetUp]
    public async Task SetUp() {
        // Configurar Entity Framework Core con PostgreSQL.
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_container.GetConnectionString()).Options;

        _context = new AppDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        // Vaciar las tablas antes de cada prueba.
        await _context.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE \"FunkoPops\", \"Categorias\" RESTART IDENTITY CASCADE;");

        _context.ChangeTracker.Clear();

        _repository = new CategoriaEfcRepository(_context);
    }

    [TearDown]
    public async Task TearDown() {
        await _context.DisposeAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown() {
        await _container.DisposeAsync();
    }

    [Test]
    public async Task GetAllAsync_SinCategorias_RetornaListaVacia() {
        // Act
        var resultado = await _repository.GetAllAsync();

        // Assert
        resultado.Should().BeEmpty();
    }

    [Test]
    public async Task GetAllAsync_ConCategorias_RetornaOrdenadasPorNombre() {
        // Arrange
        _context.Categorias.AddRange(
            new FunkoPopApiSql.Models.Categoria { Nombre = "Videojuegos", CreatedAt = DateTime.UtcNow },
            new FunkoPopApiSql.Models.Categoria { Nombre = "Anime", CreatedAt = DateTime.UtcNow }
        );

        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        var resultado = await _repository.GetAllAsync();

        // Assert
        resultado.Should().HaveCount(2);
        resultado.Select(c => c.Nombre).Should().Equal("Anime", "Videojuegos");
    }

    [Test]
    public async Task GetByIdAsync_Existe_RetornaCategoria() {
        // Arrange
        var categoria = new FunkoPopApiSql.Models.Categoria {
            Nombre = "Anime",
            CreatedAt = DateTime.UtcNow
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        var resultado = await _repository.GetByIdAsync(categoria.Id);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(categoria.Id);
        resultado.Nombre.Should().Be("Anime");
    }

    [Test]
    public async Task GetByIdAsync_NoExiste_RetornaNull() {
        // Act
        var resultado = await _repository.GetByIdAsync(999);

        // Assert
        resultado.Should().BeNull();
    }
}