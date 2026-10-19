namespace FunkoPopApiSql.Repository.Categoria;

/// <summary>
/// Contrato del repositorio de categorías mediante Entity Framework Core.
/// Permite consultar las categorías almacenadas en la base de datos.
/// </summary>
public interface ICategoriaEfcRepository {

    /// <summary>
    /// Obtiene todas las categorías almacenadas.
    /// </summary>
    /// <returns>Colección de categorías.</returns>
    Task<IReadOnlyList<Models.Categoria>> GetAllAsync();

    /// <summary>
    /// Busca una categoría por su identificador.
    /// </summary>
    /// <param name="id">Identificador de la categoría.</param>
    /// <returns>Categoría encontrada o null si no existe.</returns>
    Task<Models.Categoria?> GetByIdAsync(int id);
}