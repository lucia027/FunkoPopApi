namespace FunkoPopApiSql.Models;

/// <summary>
/// Representa un FunkoPop dentro del sistema.
/// </summary>
public class FunkoPop {
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
}