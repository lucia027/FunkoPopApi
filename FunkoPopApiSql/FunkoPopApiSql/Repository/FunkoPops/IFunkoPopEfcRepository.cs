using FunkoPopApiSql.Models;

namespace FunkoPopApiSql.Repository.FunkoPops;

/// <summary>
/// Contrato del repositorio de Funkos para la persistencia
/// mediante Entity Framework Core.
/// </summary>
public interface IFunkoPopEfcRepository {

    /// <summary>
    /// Obtiene todos los Funkos no eliminados lógicamente.
    /// </summary>
    /// <returns>Colección de Funkos.</returns>
    Task<IReadOnlyList<FunkoPop>> GetAllAsync();

    /// <summary>
    /// Busca un Funko por su identificador.
    /// </summary>
    /// <param name="id">Identificador del Funko.</param>
    /// <returns>Funko encontrado o null si no existe.</returns>
    Task<FunkoPop?> GetByIdAsync(int id);

    /// <summary>
    /// Añade un nuevo Funko a la base de datos.
    /// </summary>
    /// <param name="funko">Funko que se desea crear.</param>
    /// <returns>Funko creado con su identificador generado.</returns>
    Task<FunkoPop> AddAsync(FunkoPop funko);

    /// <summary>
    /// Actualiza los datos de un Funko existente.
    /// </summary>
    /// <param name="funko">Funko con los datos actualizados.</param>
    /// <returns>Funko actualizado o null si no existe.</returns>
    Task<FunkoPop?> UpdateAsync(FunkoPop funko);

    /// <summary>
    /// Elimina lógicamente un Funko mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador del Funko.</param>
    /// <returns>True si se eliminó; false si no existe.</returns>
    Task<bool> DeleteAsync(int id);
}