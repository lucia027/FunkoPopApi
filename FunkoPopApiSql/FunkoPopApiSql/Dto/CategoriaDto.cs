namespace FunkoPopApiSql.Dto;

/// <summary>
/// DTO con la información pública de una categoría.
/// </summary>
public record CategoriaDto(
    int Id, 
    string Nombre
);