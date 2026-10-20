using FunkoPopApiSql.Errors.Common;

namespace FunkoPopApiSql.Errors;

/// <summary>
/// Representa los errores específicos de las categorías.
/// </summary>
/// <param name="Message">Descripción del error.</param>
public abstract record CategoriaError(string Message) : DomainError(Message) {

    /// <summary>
    /// Representa una categoría que no se ha encontrado.
    /// </summary>
    /// <param name="Id">Identificador de la categoría.</param>
    public sealed record NotFound(int Id)
        : CategoriaError($"No se ha encontrado la categoría con ID {Id}.");
}

/// <summary>
/// Proporciona métodos para crear errores de categorías.
/// </summary>
public static class CategoriaErrors {

    /// <summary>
    /// Crea un error de categoría no encontrada.
    /// </summary>
    /// <param name="id">Identificador de la categoría.</param>
    /// <returns>Error de categoría no encontrada.</returns>
    public static DomainError NotFound(int id) => new CategoriaError.NotFound(id);
}