using FluentAssertions;
using FunkoPopApiSql.Data.Seed;
using FunkoPopApiSql.Entity;
using FunkoPopApiSql.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FunkoPopApiSql.Test.Data.Seed;

[TestFixture]
public class CategoriaSeederTest {

    private SqliteConnection _connection = null!;
    private AppDbContext _context = null!;
    private CategoriaSeeder _seeder = null!;

    [SetUp]
    public async Task SetUp() {
        // Crear una base SQLite en memoria para cada test.
        _connection = new SqliteConnection("Data Source=:memory:");
        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        // Crear el seeder con un logger que no escribe mensajes.
        _seeder = new CategoriaSeeder(_context, NullLogger<CategoriaSeeder>.Instance);
    }

    [TearDown]
    public async Task TearDown() {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

    [Test]
    public async Task SeedAsync_SinCategorias_InsertaTresCategorias() {
        // Act
        await _seeder.SeedAsync();

        // Assert
        var categorias = await _context.Categorias.AsNoTracking().ToListAsync();

        categorias.Should().HaveCount(3);
        categorias.Select(c => c.Nombre).Should().BeEquivalentTo("Anime", "Videojuegos", "Cine");
        categorias.Should().OnlyContain(c => c.Id > 0 && !c.IsDeleted);
        categorias.Should().OnlyContain(c => c.CreatedAt > DateTime.MinValue);
    }

    [Test]
    public async Task SeedAsync_EjecutadoDosVeces_NoDuplicaCategorias() {
        // Arrange
        await _seeder.SeedAsync();

        // Act
        await _seeder.SeedAsync();

        // Assert
        var categorias = await _context.Categorias.AsNoTracking().ToListAsync();

        categorias.Should().HaveCount(3);
        categorias.Select(c => c.Nombre).Should().OnlyHaveUniqueItems();
    }

    [Test]
    public async Task SeedAsync_ConCategoriasExistentes_NoInsertaNuevas() {
        // Arrange
        var categoria = new Categoria {
            Nombre = "Coleccionismo",
            CreatedAt = DateTime.UtcNow
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        await _seeder.SeedAsync();

        // Assert
        var categorias = await _context.Categorias.AsNoTracking().ToListAsync();

        categorias.Should().ContainSingle();
        categorias[0].Nombre.Should().Be("Coleccionismo");
    }
}