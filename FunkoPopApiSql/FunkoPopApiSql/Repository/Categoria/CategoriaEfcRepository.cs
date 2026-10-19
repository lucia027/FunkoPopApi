using FunkoPopApiSql.Entity;
using FunkoPopApiSql.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FunkoPopApiSql.Repository.Categoria;

/// <inheritdoc cref="ICategoriaEfcRepository"/>
public class CategoriaEfcRepository(AppDbContext context) : ICategoriaEfcRepository, IScopedService {

    /// <inheritdoc cref="ICategoriaEfcRepository.GetAllAsync"/>
    public async Task<IReadOnlyList<Models.Categoria>> GetAllAsync() {
        return await context.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc cref="ICategoriaEfcRepository.GetByIdAsync"/>
    public async Task<Models.Categoria?> GetByIdAsync(int id) {
        return await context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}