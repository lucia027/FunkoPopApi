namespace FunkoPopApiSql.Errors.Common;

/// <summary>
/// Clase base para representar los errores de dominio de la aplicación.
/// </summary>
/// <param name="Message">Mensaje que describe el error.</param>
public abstract record DomainError(string Message);