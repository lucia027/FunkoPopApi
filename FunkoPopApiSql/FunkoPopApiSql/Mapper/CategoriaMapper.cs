using FunkoPopApiSql.Dto;
using FunkoPopApiSql.Models;

namespace FunkoPopApiSql.Mapper;

/// <summary>
/// Proporciona conversiones de categorías a DTOs.
/// </summary>
public static class CategoriaMapper {

    /// <summary>Convierte una categoría en su DTO.</summary>
    /// <param name="categoria">Categoría a convertir.</param>
    /// <returns>DTO de la categoría.</returns>
    public static CategoriaDto ToDto(this Categoria categoria) => new(categoria.Id, categoria.Nombre);
}