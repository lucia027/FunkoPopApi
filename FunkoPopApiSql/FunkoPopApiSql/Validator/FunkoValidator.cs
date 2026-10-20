using CSharpFunctionalExtensions;
using FunkoPopApiSql.Errors;
using FunkoPopApiSql.Errors.Common;
using FunkoPopApiSql.Infrastructure.Interfaces;
using FunkoPopApiSql.Models;
using FunkoPopApiSql.Validator.Common;
using Serilog;
using ILogger = Serilog.ILogger;

namespace FunkoPopApiSql.Validator;

/// <summary>
/// Validador de dominio para las entidades FunkoPop.
/// </summary>
public class FunkoValidator : IValidator<FunkoPop>, ITransientService {

    /// <summary>
    /// Logger para registrar las operaciones de validación.
    /// </summary>
    private readonly ILogger _logger = Log.ForContext<FunkoValidator>();

    /// <inheritdoc cref="IValidator{T}.Validate"/>
    public Result<FunkoPop, DomainError> Validate(FunkoPop entity) {
        _logger.Debug("Intentando validar el Funko: {Nombre}", entity.Nombre);

        // Recoger todos los errores de validación encontrados.
        var errores = new List<string>();

        // Validar el nombre.
        if (string.IsNullOrWhiteSpace(entity.Nombre)) {
            errores.Add("El nombre no puede estar vacío.");
        } else if (entity.Nombre.Length > 100) {
            errores.Add("El nombre no puede superar los 100 caracteres.");
        }

        // Validar el precio.
        if (entity.Precio <= 0 || entity.Precio > 999999.99m) {
            errores.Add("El precio debe ser mayor que cero y no superar 999999.99.");
        }

        // Validar el stock.
        if (entity.Stock < 0) {
            errores.Add("El stock no puede ser negativo.");
        }

        // Validar el identificador de la categoría.
        if (entity.CategoriaId <= 0) {
            errores.Add("El identificador de la categoría debe ser positivo.");
        }

        // Devolver todos los errores encontrados.
        if (errores.Count > 0) {
            _logger.Warning("No se ha podido validar el Funko: {Nombre}", entity.Nombre);
            return Result.Failure<FunkoPop, DomainError>(FunkoErrors.Validation(errores));
        }

        _logger.Debug("Funko validado correctamente: {Nombre}", entity.Nombre);
        return Result.Success<FunkoPop, DomainError>(entity);
    }
}
