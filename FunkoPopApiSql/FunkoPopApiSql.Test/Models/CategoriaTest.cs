using FluentAssertions;
using FunkoPopApiSql.Models;

namespace FunkoPopApiSql.Test.Models;

/// <summary>
/// Pruebas unitarias de los valores y la colección de navegación del modelo
/// <see cref="Categoria"/>, sin depender de una base de datos.
/// </summary>
[TestFixture]
public class CategoriaTests {
    /// <summary>
    /// Comprueba que una categoría recién creada tiene los valores
    /// predeterminados y una colección de Funkos inicializada y vacía.
    /// </summary>
    [Test]
    public void Constructor_SinAsignarPropiedades_UsaValoresPredeterminados() {
        // Act
        var categoria = new Categoria();

        // Assert
        categoria.Id.Should().Be(0);
        categoria.Nombre.Should().BeEmpty();
        categoria.FunkoPops.Should().NotBeNull();
        categoria.FunkoPops.Should().BeEmpty();
        categoria.CreatedAt.Should().Be(DateTime.MinValue);
        categoria.UpdatedAt.Should().BeNull();
        categoria.DeletedAt.Should().BeNull();
        categoria.IsDeleted.Should().BeFalse();
    }

    /// <summary>
    /// Verifica que es posible crear una categoría asignando su identificador,
    /// nombre y fecha de creación.
    /// </summary>
    [Test]
    public void Constructor_ConDatosBasicos_AsignaPropiedades() {
        // Arrange
        var fechaCreacion = new DateTime(2026, 1, 10, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var categoria = new Categoria { Id = 4, Nombre = "Anime", CreatedAt = fechaCreacion };

        // Assert
        categoria.Id.Should().Be(4);
        categoria.Nombre.Should().Be("Anime");
        categoria.CreatedAt.Should().Be(fechaCreacion);
        categoria.IsDeleted.Should().BeFalse();
    }

    /// <summary>
    /// Comprueba que el nombre de una categoría puede cambiarse.
    /// </summary>
    [Test]
    public void CambiarNombre_ConNuevoNombre_ActualizaLaPropiedad() {
        // Arrange
        var categoria = new Categoria { Nombre = "Cine" };

        // Act
        categoria.Nombre = "Series";

        // Assert
        categoria.Nombre.Should().Be("Series");
    }

    /// <summary>
    /// Comprueba que la fecha opcional de actualización puede almacenarse.
    /// </summary>
    [Test]
    public void ActualizarFechaModificacion_ConFechaValida_AlmacenaLaFecha() {
        // Arrange
        var categoria = new Categoria();
        var fechaActualizacion = new DateTime(2026, 1, 11, 9, 0, 0, DateTimeKind.Utc);

        // Act
        categoria.UpdatedAt = fechaActualizacion;

        // Assert
        categoria.UpdatedAt.Should().Be(fechaActualizacion);
    }

    /// <summary>
    /// Comprueba que se puede añadir un Funko a la colección de navegación
    /// /// de una categoría sin utilizar Entity Framework Core.
    /// </summary>
    [Test]
    public void AgregarFunko_ConFunkoRelacionado_QuedaEnLaColeccion() {
        // Arrange
        var categoria = new Categoria { Id = 1, Nombre = "Anime" };
        var funko = new FunkoPop { Nombre = "Naruto", CategoriaId = categoria.Id, Categoria = categoria };

        // Act
        categoria.FunkoPops.Add(funko);

        // Assert
        categoria.FunkoPops.Should().ContainSingle();
        categoria.FunkoPops.Should().Contain(funko);
        funko.Categoria.Should().BeSameAs(categoria);
    }

    /// <summary>
    /// Comprueba que una misma categoría puede contener varios Funkos,
    /// representando en memoria el lado «muchos» de la relación 1:N.
    /// </summary>
    [Test]
    public void AgregarDosFunkos_EnLaMismaCategoria_ContieneAmbos() {
        // Arrange
        var categoria = new Categoria { Id = 1, Nombre = "Anime" };
        var naruto = new FunkoPop { Id = 10, Nombre = "Naruto", CategoriaId = categoria.Id, Categoria = categoria };
        var sasuke = new FunkoPop { Id = 11, Nombre = "Sasuke", CategoriaId = categoria.Id, Categoria = categoria };

        // Act
        categoria.FunkoPops.Add(naruto);
        categoria.FunkoPops.Add(sasuke);

        // Assert
        categoria.FunkoPops.Should().HaveCount(2);
        categoria.FunkoPops.Should().Contain(naruto);
        categoria.FunkoPops.Should().Contain(sasuke);
        naruto.CategoriaId.Should().Be(categoria.Id);
        sasuke.CategoriaId.Should().Be(categoria.Id);
    }

    /// <summary>
    /// Verifica que es posible retirar un Funko de la colección de navegación.
    /// No comprueba operaciones de borrado en la base de datos.
    /// </summary>
    [Test]
    public void QuitarFunko_DeLaColeccion_DejaDeContenerlo() {
        // Arrange
        var categoria = new Categoria { Id = 1, Nombre = "Anime" };
        var naruto = new FunkoPop { Id = 10, Nombre = "Naruto" };
        var sasuke = new FunkoPop { Id = 11, Nombre = "Sasuke" };
        categoria.FunkoPops.Add(naruto);
        categoria.FunkoPops.Add(sasuke);

        // Act
        var eliminado = categoria.FunkoPops.Remove(naruto);

        // Assert
        eliminado.Should().BeTrue();
        categoria.FunkoPops.Should().ContainSingle();
        categoria.FunkoPops.Should().NotContain(naruto);
        categoria.FunkoPops.Should().Contain(sasuke);
    }

    /// <summary>
    /// Comprueba que se pueden almacenar explícitamente tanto el indicador
    /// como la fecha de borrado lógico de una categoría.
    /// </summary>
    [Test]
    public void MarcarBorradoLogico_AsignandoIndicadorYFecha_ConservaAmbosValores() {
        // Arrange
        var categoria = new Categoria { Nombre = "Anime" };
        var fechaBorrado = new DateTime(2026, 1, 12, 18, 0, 0, DateTimeKind.Utc);

        // Act
        categoria.IsDeleted = true;
        categoria.DeletedAt = fechaBorrado;

        // Assert
        categoria.IsDeleted.Should().BeTrue();
        categoria.DeletedAt.Should().Be(fechaBorrado);
    }

    /// <summary>
    /// Comprueba que el modelo permite restaurar de forma explícita
    /// una categoría marcada como eliminada.
    /// </summary>
    [Test]
    public void RestaurarCategoria_LimpiandoIndicadorYFecha_QuedaActiva() {
        // Arrange
        var categoria = new Categoria { IsDeleted = true, DeletedAt = new DateTime(2026, 1, 12, 18, 0, 0, DateTimeKind.Utc) };

        // Act
        categoria.IsDeleted = false;
        categoria.DeletedAt = null;

        // Assert
        categoria.IsDeleted.Should().BeFalse();
        categoria.DeletedAt.Should().BeNull();
    }
}
