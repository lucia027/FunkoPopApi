using FluentAssertions;
using FunkoPopApiSql.Entity;
using FunkoPopApiSql.Repository.Categoria;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FunkoPopApiSql.Test.Repository.Categoria;

[TestFixture]
[Category("Integration")]
[Category("Sqlite")]
[NonParallelizable]
public class CategoriaEfcRepositorySqliteTests {

    private SqliteConnection _connection = null!;
    private AppDbContext _context = null!;
    private CategoriaEfcRepository _repository = null!;

    [SetUp]
    public async Task SetUp() {
        // Crear una base de datos SQLite temporal en memoria.
        _connection = new SqliteConnection("Data Source=:memory:");
        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection).Options;

        _context = new AppDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        // Mantener la base de datos limpia entre los tests.
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"FunkoPops\";");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"Categorias\";");

        _context.ChangeTracker.Clear();

        _repository = new CategoriaEfcRepository(_context);
    }

    [TearDown]
    public async Task TearDown() {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
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