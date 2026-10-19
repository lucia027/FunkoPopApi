using FunkoPopApiSql.Models;
using Microsoft.EntityFrameworkCore;
 
 namespace FunkoPopApiSql.Entity;
 
 /// <summary>
 /// Contexto de Entity Framework Core para gestionar la persistencia
 /// de Funkos y Categorias en PostgreSQL o SQLite.
 /// </summary>
 public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
     
     /// <summary>
     /// Conjunto de entidades FunkoPop almacenadas en la base de datos.
     /// </summary>
     public DbSet<FunkoPop> FunkoPops { get; set; }
     
     /// <summary>
     /// Conjunto de entidades Categoria almacenadas en la base de datos.
     /// </summary>
     public DbSet<Categoria> Categorias { get; set; }
 
     
     /// <summary>
     /// Configura las tablas, claves, propiedades, relaciones,
     /// índices y filtros globales mediante Fluent API.
     /// </summary>
     protected override void OnModelCreating(ModelBuilder modelBuilder) {
         modelBuilder.Entity<FunkoPop>(entity => {
             entity.ToTable("FunkoPops");
             entity.HasKey(f => f.Id);
             entity.Property(f => f.Id).ValueGeneratedOnAdd();
             entity.Property(f => f.Nombre).IsRequired().HasMaxLength(100);
             entity.Property(f => f.Precio).IsRequired().HasPrecision(10, 2);
             entity.Property(f => f.Stock).IsRequired();
             entity.Property(f => f.IsDeleted).HasDefaultValue(false);
             entity.Property(f => f.CategoriaId).IsRequired();
             entity.HasOne(f => f.Categoria)
                 .WithMany(c => c.FunkoPops)
                 .HasForeignKey(f => f.CategoriaId)
                 .OnDelete(DeleteBehavior.Restrict);
             
             entity.HasQueryFilter(f => !f.IsDeleted);
         });
 
         modelBuilder.Entity<Categoria>(entity => {
             entity.ToTable("Categorias");
             entity.HasKey(c => c.Id);
             entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
             entity.Property(c => c.IsDeleted).HasDefaultValue(false);
         });
     }
 }