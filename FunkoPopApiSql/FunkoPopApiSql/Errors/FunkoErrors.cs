using FunkoPopApiSql.Errors.Common;

namespace FunkoPopApiSql.Errors;

/// <summary>
/// Representa los errores específicos de los Funkos.
/// </summary>
/// <param name="Message">Descripción del error.</param>
public abstract record FunkoError(string Message) : DomainError(Message) {

    /// <summary>
    /// Representa los errores de validación de un Funko.
    /// </summary>
    /// <param name="Errores">Errores encontrados durante la validación.</param>
    public sealed record Validation(IReadOnlyList<string> Errores)
        : FunkoError($"Errores en la validación del Funko:{Environment.NewLine}• {string.Join($"{Environment.NewLine}• ", Errores)}");

    /// <summary>
    /// Representa un Funko que no se ha encontrado.
    /// </summary>
    /// <param name="Id">Identificador del Funko.</param>
    public sealed record NotFound(int Id)
        : FunkoError($"No se ha encontrado el Funko con ID {Id}.");
}

/// <summary>
/// Proporciona métodos para crear errores de Funkos.
/// </summary>
public static class FunkoErrors {

    /// <summary>
    /// Crea un error con todos los problemas encontrados al validar un Funko.
    /// </summary>
    /// <param name="errores">Mensajes de validación.</param>
    /// <returns>Error de validación del Funko.</returns>
    public static DomainError Validation(IEnumerable<string> errores) => new FunkoError.Validation(errores.ToArray());

    /// <summary>
    /// Crea un error cuando un Funko no existe.
    /// </summary>
    /// <param name="id">Identificador del Funko.</param>
    /// <returns>Error de Funko no encontrado.</returns>
    public static DomainError NotFound(int id) => new FunkoError.NotFound(id);
}