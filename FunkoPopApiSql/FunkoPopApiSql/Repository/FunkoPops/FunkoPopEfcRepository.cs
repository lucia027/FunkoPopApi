using FunkoPopApiSql.Entity;
using FunkoPopApiSql.Infrastructure.Interfaces;
using FunkoPopApiSql.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ILogger = Serilog.ILogger;

namespace FunkoPopApiSql.Repository.FunkoPops;

/// <inheritdoc cref="IFunkoPopEfcRepository"/>
public class FunkoPopEfcRepository(AppDbContext context) : IFunkoPopEfcRepository, IScopedService {

    /// <summary>
    /// Contexto de Entity Framework Core.
    /// </summary>
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Logger de Serilog para las operaciones del repositorio.
    /// </summary>
    private readonly ILogger _logger = Log.ForContext<FunkoPopEfcRepository>();

    /// <inheritdoc cref="IFunkoPopEfcRepository.GetAllAsync"/>
    public async Task<IReadOnlyList<FunkoPop>> GetAllAsync() {
        _logger.Debug("Obteniendo todos los Funkos.");

        return await _context.FunkoPops
            .AsNoTracking()
            .Include(f => f.Categoria)
            .OrderBy(f => f.Id)
            .ToListAsync();
    }

    /// <inheritdoc cref="IFunkoPopEfcRepository.GetByIdAsync"/>
    public async Task<FunkoPop?> GetByIdAsync(int id) {
        var funko = await _context.FunkoPops
            .AsNoTracking()
            .Include(f => f.Categoria)
            .FirstOrDefaultAsync(f => f.Id == id);

        _logger.Debug(funko is null ? "No se ha encontrado el Funko con ID {Id}." : "Se ha encontrado el Funko con ID {Id}.", id);
        return funko;
    }

    /// <inheritdoc cref="IFunkoPopEfcRepository.AddAsync"/>
    public async Task<FunkoPop> AddAsync(FunkoPop funko) {
        funko.CreatedAt = DateTime.UtcNow;
        funko.UpdatedAt = null;
        funko.DeletedAt = null;
        funko.IsDeleted = false;

        await _context.FunkoPops.AddAsync(funko);
        await _context.SaveChangesAsync();

        // Cargar la categoría asociada al Funko creado.
        await _context.Entry(funko).Reference(f => f.Categoria).LoadAsync();

        _logger.Information("Funko creado correctamente con ID {Id}.", funko.Id);
        return funko;
    }

    /// <inheritdoc cref="IFunkoPopEfcRepository.UpdateAsync"/>
    public async Task<FunkoPop?> UpdateAsync(FunkoPop funko) {
        var existente = await _context.FunkoPops.FirstOrDefaultAsync(f => f.Id == funko.Id);

        if (existente is null) {
            _logger.Debug("No se puede actualizar el Funko con ID {Id}: no existe.", funko.Id);
            return null;
        }

        existente.Nombre = funko.Nombre;
        existente.Precio = funko.Precio;
        existente.Stock = funko.Stock;
        existente.CategoriaId = funko.CategoriaId;
        existente.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Cargar la categoría correspondiente tras la actualización.
        await _context.Entry(existente).Reference(f => f.Categoria).LoadAsync();

        _logger.Information("Funko actualizado correctamente con ID {Id}.", existente.Id);
        return existente;
    }

    /// <inheritdoc cref="IFunkoPopEfcRepository.DeleteAsync"/>
    public async Task<bool> DeleteAsync(int id) {
        var funko = await _context.FunkoPops.FirstOrDefaultAsync(f => f.Id == id);

        if (funko is null) {
            _logger.Debug("No se puede eliminar el Funko con ID {Id}: no existe.", id);
            return false;
        }

        funko.IsDeleted = true;
        funko.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.Information("Funko eliminado lógicamente con ID {Id}.", id);
        return true;
    }
}
