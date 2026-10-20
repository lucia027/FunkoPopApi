using System.ComponentModel.DataAnnotations;

namespace FunkoPopApiSql.Dto;

/// <summary>
/// DTO que contiene los datos necesarios para actualizar un Funko.
/// </summary>
public record UpdateFunkoDto {

    /// <summary>Nuevo nombre del Funko.</summary>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string Nombre { get; init; } = string.Empty;

    /// <summary>Nuevo precio del Funko.</summary>
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, 999999.99, ErrorMessage = "El precio debe estar entre 0.01 y 999999.99.")]
    public decimal Precio { get; init; }

    /// <summary>Nuevo stock disponible del Funko.</summary>
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; init; }

    /// <summary>Identificador de la categoría del Funko.</summary>
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "El identificador de la categoría debe ser positivo.")]
    public int CategoriaId { get; init; }
}