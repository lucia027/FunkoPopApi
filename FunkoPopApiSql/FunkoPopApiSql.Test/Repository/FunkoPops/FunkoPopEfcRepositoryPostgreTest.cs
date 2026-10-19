using FluentAssertions;
using FunkoPopApiSql.Entity;
using FunkoPopApiSql.Models;
using FunkoPopApiSql.Repository.FunkoPops;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace FunkoPopApiSql.Test.Integration.Repositories;

[TestFixture]
[Category("Integration")]
[Category("Postgre")]
[NonParallelizable]
public class FunkoPopEfcRepositoryPostgreTest {

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .WithDatabase("funkopop_tests")
        .WithUsername("test")
        .WithPassword("test")
        .Build();

    private AppDbContext _context = null!;
    private FunkoPopEfcRepository _repository = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp() {
        // Arrancar PostgreSQL real antes de los tests de esta clase.
        await _container.StartAsync();
    }

    [SetUp]
    public async Task SetUp() {
        // Utilizar el mismo AppDbContext, pero con el proveedor Npgsql.
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_container.GetConnectionString()).Options;
        _context = new AppDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        // Limpiar la base de datos antes de cada test.
        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"FunkoPops\", \"Categorias\" RESTART IDENTITY CASCADE;");
        _context.ChangeTracker.Clear();

        _repository = new FunkoPopEfcRepository(_context);
    }

    [TearDown]
    public async Task TearDown() {
        await _context.DisposeAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown() {
        await _container.DisposeAsync();
    }

    private async Task<Categoria> CrearCategoriaAsync(string nombre = "Anime") {
        var categoria = new Categoria {
            Nombre = nombre,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        return categoria;
    }

    private async Task<FunkoPop> CrearFunkoAsync(int categoriaId, string nombre = "Naruto", bool eliminado = false) {
        var funko = new FunkoPop {
            Nombre = nombre,
            Precio = 20m,
            Stock = 5,
            CategoriaId = categoriaId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = eliminado,
            DeletedAt = eliminado ? DateTime.UtcNow : null
        };

        _context.FunkoPops.Add(funko);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        return funko;
    }

    [Test]
    public async Task GetAllAsync_SinFunkos_RetornaListaVacia() {
        // Act
        var resultado = await _repository.GetAllAsync();

        // Assert
        resultado.Should().BeEmpty();
    }

    [Test]
    public async Task GetAllAsync_ConFunkos_RetornaSoloActivosYConCategoria() {
        // Arrange
        var categoria = await CrearCategoriaAsync();
        await CrearFunkoAsync(categoria.Id, "Naruto");
        await CrearFunkoAsync(categoria.Id, "Vegeta", eliminado: true);

        // Act
        var resultado = await _repository.GetAllAsync();

        // Assert
        resultado.Should().ContainSingle();
        resultado[0].Nombre.Should().Be("Naruto");
        resultado[0].Categoria.Nombre.Should().Be("Anime");
    }

    [Test]
    public async Task GetByIdAsync_Existe_RetornaFunkoConCategoria() {
        // Arrange
        var categoria = await CrearCategoriaAsync();
        var creado = await CrearFunkoAsync(categoria.Id, "Batman");

        // Act
        var resultado = await _repository.GetByIdAsync(creado.Id);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Nombre.Should().Be("Batman");
        resultado.CategoriaId.Should().Be(categoria.Id);
        resultado.Categoria.Nombre.Should().Be("Anime");
    }

    [Test]
    public async Task GetByIdAsync_NoExiste_RetornaNull() {
        // Act
        var resultado = await _repository.GetByIdAsync(999);

        // Assert
        resultado.Should().BeNull();
    }

    [Test]
    public async Task GetByIdAsync_EstaBorrado_RetornaNull() {
        // Arrange
        var categoria = await CrearCategoriaAsync();
        var creado = await CrearFunkoAsync(categoria.Id, eliminado: true);

        // Act
        var resultado = await _repository.GetByIdAsync(creado.Id);

        // Assert
        resultado.Should().BeNull();
    }

    [Test]
    public async Task AddAsync_FunkoValido_GuardaEnBaseDeDatos() {
        // Arrange
        var categoria = await CrearCategoriaAsync();
        var funko = new FunkoPop {
            Nombre = "Goku",
            Precio = 29.99m,
            Stock = 10,
            CategoriaId = categoria.Id
        };

        // Act
        var resultado = await _repository.AddAsync(funko);

        // Assert
        resultado.Id.Should().BeGreaterThan(0);
        resultado.CreatedAt.Should().BeAfter(DateTime.MinValue);
        resultado.IsDeleted.Should().BeFalse();
        resultado.UpdatedAt.Should().BeNull();
        resultado.DeletedAt.Should().BeNull();

        _context.ChangeTracker.Clear();

        var guardado = await _context.FunkoPops
            .AsNoTracking()
            .SingleAsync(f => f.Id == resultado.Id);

        guardado.Nombre.Should().Be("Goku");
        guardado.Precio.Should().Be(29.99m);
        guardado.Stock.Should().Be(10);
        guardado.CategoriaId.Should().Be(categoria.Id);
    }

    [Test]
    public async Task UpdateAsync_Existe_ActualizaDatosYCategoria() {
        // Arrange
        var categoriaOriginal = await CrearCategoriaAsync();
        var categoriaNueva = await CrearCategoriaAsync("Videojuegos");
        var creado = await CrearFunkoAsync(categoriaOriginal.Id, "Naruto");

        var fechaOriginal = await _context.FunkoPops
            .AsNoTracking()
            .Where(f => f.Id == creado.Id)
            .Select(f => f.CreatedAt)
            .SingleAsync();

        var cambios = new FunkoPop {
            Id = creado.Id,
            Nombre = "Naruto Shippuden",
            Precio = 35.50m,
            Stock = 8,
            CategoriaId = categoriaNueva.Id
        };

        // Act
        var resultado = await _repository.UpdateAsync(cambios);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.UpdatedAt.Should().NotBeNull();

        _context.ChangeTracker.Clear();

        var guardado = await _repository.GetByIdAsync(creado.Id);

        guardado.Should().NotBeNull();
        guardado!.Nombre.Should().Be("Naruto Shippuden");
        guardado.Precio.Should().Be(35.50m);
        guardado.Stock.Should().Be(8);
        guardado.CategoriaId.Should().Be(categoriaNueva.Id);
        guardado.Categoria.Nombre.Should().Be("Videojuegos");
        guardado.CreatedAt.Should().Be(fechaOriginal);
        guardado.UpdatedAt.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateAsync_NoExiste_RetornaNull() {
        // Arrange
        var categoria = await CrearCategoriaAsync();
        var cambios = new FunkoPop {
            Id = 999,
            Nombre = "No existe",
            Precio = 10m,
            Stock = 1,
            CategoriaId = categoria.Id
        };

        // Act
        var resultado = await _repository.UpdateAsync(cambios);

        // Assert
        resultado.Should().BeNull();
    }

    [Test]
    public async Task UpdateAsync_EstaBorrado_RetornaNull() {
        // Arrange
        var categoria = await CrearCategoriaAsync();
        var creado = await CrearFunkoAsync(categoria.Id, eliminado: true);

        var cambios = new FunkoPop {
            Id = creado.Id,
            Nombre = "No actualizar",
            Precio = 15m,
            Stock = 2,
            CategoriaId = categoria.Id
        };

        // Act
        var resultado = await _repository.UpdateAsync(cambios);

        // Assert
        resultado.Should().BeNull();
    }

    [Test]
    public async Task DeleteAsync_Existe_MarcaBorradoLogicoSinEliminarFila() {
        // Arrange
        var categoria = await CrearCategoriaAsync();
        var creado = await CrearFunkoAsync(categoria.Id, "Vegeta");

        // Act
        var resultado = await _repository.DeleteAsync(creado.Id);

        // Assert
        resultado.Should().BeTrue();

        _context.ChangeTracker.Clear();

        (await _repository.GetByIdAsync(creado.Id)).Should().BeNull();

        var borrado = await _context.FunkoPops
            .IgnoreQueryFilters()
            .AsNoTracking()
            .SingleOrDefaultAsync(f => f.Id == creado.Id);

        borrado.Should().NotBeNull();
        borrado!.IsDeleted.Should().BeTrue();
        borrado.DeletedAt.Should().NotBeNull();
    }

    [Test]
    public async Task DeleteAsync_NoExiste_RetornaFalse() {
        // Act
        var resultado = await _repository.DeleteAsync(999);

        // Assert
        resultado.Should().BeFalse();
    }

    [Test]
    public async Task DeleteAsync_YaEstaBorrado_RetornaFalse() {
        // Arrange
        var categoria = await CrearCategoriaAsync();
        var creado = await CrearFunkoAsync(categoria.Id, eliminado: true);

        // Act
        var resultado = await _repository.DeleteAsync(creado.Id);

        // Assert
        resultado.Should().BeFalse();
    }
}