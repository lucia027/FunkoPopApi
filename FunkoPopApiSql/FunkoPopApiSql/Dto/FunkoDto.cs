namespace FunkoPopApiSql.Dto;

/// <summary>
/// DTO que representa los datos de un Funko devueltos por la API.
/// </summary>
public record FunkoDto(
    int Id,
    string Nombre,
    decimal Precio,
    int Stock,
    int CategoriaId,
    string CategoriaNombre,
    bool IsDeleted,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);