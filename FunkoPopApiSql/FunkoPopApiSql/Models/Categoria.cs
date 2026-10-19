namespace FunkoPopApiSql.Models;

/// <summary>
/// Representa una Categoria dentro del sistema.
/// </summary>
public class Categoria {
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public ICollection<FunkoPop> FunkoPops { get; set; } = [];
 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}