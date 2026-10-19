using FunkoPopApiSql.Entity;
using FunkoPopApiSql.Models;
using Microsoft.EntityFrameworkCore;

namespace FunkoPopApiSql.Data.Seed;

/// <summary>
/// Se encarga de insertar las categorías iniciales de la aplicación
/// utilizando Entity Framework Core.
/// </summary>
public class CategoriaSeeder {

    private readonly AppDbContext _context;
    private readonly ILogger<CategoriaSeeder> _logger;

    /// <summary>
    /// Crea una instancia del seeder de categorías.
    /// </summary>
    /// <param name="context">Contexto de la base de datos SQL.</param>
    /// <param name="logger">Logger para registrar el proceso de inicialización.</param>
    public CategoriaSeeder(AppDbContext context, ILogger<CategoriaSeeder> logger) {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Inserta las categorías iniciales si todavía no existen
    /// categorías en la base de datos.
    /// </summary>
    /// <returns>Tarea que representa la operación de inserción.</returns>
    public async Task SeedAsync() {
        // Comprobar si las categorías ya están registradas.
        if (await _context.Categorias.AnyAsync()) {
            _logger.LogInformation("Las categorías ya existen. No es necesario insertar semillas.");
            return;
        }

        // Preparar las categorías iniciales.
        var ahora = DateTime.UtcNow;

        var categorias = new List<Categoria> {
            new() { Nombre = "Anime", CreatedAt = ahora, IsDeleted = false },
            new() { Nombre = "Videojuegos", CreatedAt = ahora, IsDeleted = false },
            new() { Nombre = "Cine", CreatedAt = ahora, IsDeleted = false }
        };

        // Insertar y guardar las categorías en la base de datos.
        await _context.Categorias.AddRangeAsync(categorias);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Se han insertado {Count} categorías iniciales.", categorias.Count);
    }
}