using CSharpFunctionalExtensions;
using FunkoPopApiSql.Errors.Common;

namespace FunkoPopApiSql.Validator.Common
    ;

/// <summary>
/// Contrato genérico para validar entidades.
/// </summary>
/// <typeparam name="T">Tipo de entidad que se desea validar.</typeparam>
public interface IValidator<T> {

    /// <summary>
    /// Valida una entidad y devuelve el resultado de la validación.
    /// </summary>
    /// <param name="entity">Entidad que se desea validar.</param>
    /// <returns>Entidad validada o error de dominio.</returns>
    Result<T, DomainError> Validate(T entity);
}