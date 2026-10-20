using FunkoPopApiSql.Dto;
using FunkoPopApiSql.Models;

namespace FunkoPopApiSql.Mapper;

/// <summary>
/// Proporciona conversiones entre los modelos FunkoPop y sus DTOs.
/// </summary>
public static class FunkoMapper {

    /// <summary>Convierte un Funko en su DTO de respuesta.</summary>
    /// <param name="funko">Entidad a convertir.</param>
    /// <returns>DTO con los datos del Funko.</returns>
    public static FunkoDto ToDto(this FunkoPop funko) => new(
        funko.Id,
        funko.Nombre,
        funko.Precio,
        funko.Stock,
        funko.CategoriaId,
        funko.Categoria?.Nombre ?? string.Empty,
        funko.IsDeleted,
        funko.CreatedAt,
        funko.UpdatedAt
    );

    /// <summary>Convierte los datos de creación en un Funko.</summary>
    /// <param name="dto">DTO de creación.</param>
    /// <returns>Entidad preparada para crear.</returns>
    public static FunkoPop ToModel(this CreateFunkoDto dto) => new() {
        Nombre = dto.Nombre,
        Precio = dto.Precio,
        Stock = dto.Stock,
        CategoriaId = dto.CategoriaId
    };

    /// <summary>Convierte los datos de actualización en un Funko.</summary>
    /// <param name="dto">DTO de actualización.</param>
    /// <param name="id">Identificador del Funko existente.</param>
    /// <returns>Entidad preparada para actualizar.</returns>
    public static FunkoPop ToModel(this UpdateFunkoDto dto, int id) => new() {
        Id = id,
        Nombre = dto.Nombre,
        Precio = dto.Precio,
        Stock = dto.Stock,
        CategoriaId = dto.CategoriaId
    };
}