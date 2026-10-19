using FluentAssertions;
using FunkoPopApiSql.Models;

namespace FunkoPopApiSql.Test.Models;

/// <summary>
/// Pruebas unitarias de los valores y propiedades de navegación del modelo <see cref="FunkoPop"/>.
/// No comprueban reglas de validación ni comportamiento de Entity Framework Core.
/// </summary>
[TestFixture]
public class FunkoPopTests {
    /// <summary>
    /// Comprueba que un Funko recién creado conserva los valores predeterminados
    /// definidos en el modelo, sin asignar fechas ni categorías automáticamente.
    /// </summary>
    [Test]
    public void Constructor_SinAsignarPropiedades_UsaValoresPredeterminados() {
        // Arrange: no se necesitan datos previos.

        // Act
        var funko = new FunkoPop();

        // Assert
        funko.Id.Should().Be(0);
        funko.Nombre.Should().BeEmpty();
        funko.Precio.Should().Be(0m);
        funko.Stock.Should().Be(0);
        funko.CreatedAt.Should().Be(DateTime.MinValue);
        funko.UpdatedAt.Should().BeNull();
        funko.DeletedAt.Should().BeNull();
        funko.IsDeleted.Should().BeFalse();
        funko.CategoriaId.Should().Be(0);
        funko.Categoria.Should().BeNull();
    }

    /// <summary>
    /// Comprueba que se pueden inicializar los datos básicos y la fecha de creación
    /// de un Funko mediante las propiedades públicas del modelo.
    /// </summary>
    [Test]
    public void Constructor_ConDatosBasicos_AsignaPropiedades() {
        // Arrange
        var fechaCreacion = new DateTime(2026, 1, 10, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var funko = new FunkoPop
        {
            Id = 7,
            Nombre = "Naruto",
            Precio = 24.95m,
            Stock = 10,
            CreatedAt = fechaCreacion
        };

        // Assert
        funko.Id.Should().Be(7);
        funko.Nombre.Should().Be("Naruto");
        funko.Precio.Should().Be(24.95m);
        funko.Stock.Should().Be(10);
        funko.CreatedAt.Should().Be(fechaCreacion);
        funko.IsDeleted.Should().BeFalse();
    }

    /// <summary>
    /// Verifica que las propiedades editables de un Funko admiten cambios.
    /// </summary>
    [Test]
    public void ActualizarDatos_CambiandoNombrePrecioYStock_ConservaLosNuevosValores() {
        // Arrange
        var funko = new FunkoPop { Nombre = "Naruto", Precio = 20m, Stock = 5 };

        // Act
        funko.Nombre = "Naruto Shippuden";
        funko.Precio = 27.50m;
        funko.Stock = 3;

        // Assert
        funko.Nombre.Should().Be("Naruto Shippuden");
        funko.Precio.Should().Be(27.50m);
        funko.Stock.Should().Be(3);
    }

    /// <summary>
    /// Comprueba que se pueden asignar tanto la clave foránea como la referencia
    /// a la categoría correspondiente.
    /// </summary>
    [Test]
    public void AsociarCategoria_ConCategoriaExistente_AsignaIdYReferencia() {
        // Arrange
        var categoria = new Categoria { Id = 2, Nombre = "Anime" };
        
        // Act
        var funko = new FunkoPop {
            CategoriaId = categoria.Id,
            Categoria = categoria
        };

        // Assert
        funko.CategoriaId.Should().Be(2);
        funko.Categoria.Should().BeSameAs(categoria);
    }

    /// <summary>
    /// Comprueba que el modelo permite cambiar explícitamente la categoría
    /// y su clave foránea por otra categoría.
    /// </summary>
    [Test]
    public void CambiarCategoria_AsignandoOtraCategoria_ActualizaIdYReferencia() {
        // Arrange
        var original = new Categoria { Id = 1, Nombre = "Cine" };
        var nueva = new Categoria { Id = 3, Nombre = "Videojuegos" };
        var funko = new FunkoPop { CategoriaId = original.Id, Categoria = original };

        // Act
        funko.CategoriaId = nueva.Id;
        funko.Categoria = nueva;

        // Assert
        funko.CategoriaId.Should().Be(3);
        funko.Categoria.Should().BeSameAs(nueva);
    }

    /// <summary>
    /// Verifica que la fecha de modificación, inicialmente nula, puede recibir
    /// una fecha concreta.
    /// </summary>
    [Test]
    public void ActualizarFechaModificacion_ConFechaValida_AlmacenaLaFecha() {
        // Arrange
        var funko = new FunkoPop();
        var fechaActualizacion = new DateTime(2026, 1, 11, 9, 0, 0, DateTimeKind.Utc);

        // Act
        funko.UpdatedAt = fechaActualizacion;

        // Assert
        funko.UpdatedAt.Should().Be(fechaActualizacion);
    }

    /// <summary>
    /// Comprueba que el modelo permite representar un borrado lógico mediante
    /// IsDeleted y DeletedAt, asignando ambos valores de forma explícita.
    /// </summary>
    [Test]
    public void MarcarBorradoLogico_AsignandoIndicadorYFecha_ConservaAmbosValores() {
        // Arrange
        var funko = new FunkoPop { Nombre = "Naruto" };
        var fechaBorrado = new DateTime(2026, 1, 12, 18, 0, 0, DateTimeKind.Utc);

        // Act
        funko.IsDeleted = true;
        funko.DeletedAt = fechaBorrado;

        // Assert
        funko.IsDeleted.Should().BeTrue();
        funko.DeletedAt.Should().Be(fechaBorrado);
    }

    /// <summary>
    /// Comprueba que el modelo permite restaurar explícitamente el estado
    /// del borrado lógico, sin asumir reglas automáticas de negocio.
    /// </summary>
    [Test]
    public void RestaurarFunko_LimpiandoIndicadorYFecha_QuedaActivo() {
        // Arrange
        var funko = new FunkoPop
        {
            IsDeleted = true,
            DeletedAt = new DateTime(2026, 1, 12, 18, 0, 0, DateTimeKind.Utc)
        };

        // Act
        funko.IsDeleted = false;
        funko.DeletedAt = null;

        // Assert
        funko.IsDeleted.Should().BeFalse();
        funko.DeletedAt.Should().BeNull();
    }
}