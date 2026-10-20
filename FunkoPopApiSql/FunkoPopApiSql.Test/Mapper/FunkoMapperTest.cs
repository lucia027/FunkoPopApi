using FluentAssertions;
using FunkoPopApiSql.Dto;
using FunkoPopApiSql.Mapper;
using FunkoPopApiSql.Models;

namespace FunkoPopApiSql.Test.Mapper;

[TestFixture]
public class FunkoMapperTest {

    [Test]
    public void ToDto_FunkoConCategoria_ConvierteDatos() {
        // Arrange
        var fecha = DateTime.UtcNow;
        var actualizado = fecha.AddMinutes(1);
        var funko = new FunkoPop {
            Id = 1, Nombre = "Naruto", Precio = 24.99m, Stock = 5,
            CategoriaId = 2, Categoria = new Categoria { Id = 2, Nombre = "Anime" },
            CreatedAt = fecha, UpdatedAt = actualizado
        };

        // Act
        var resultado = funko.ToDto();

        // Assert
        resultado.Id.Should().Be(1);
        resultado.Nombre.Should().Be("Naruto");
        resultado.Precio.Should().Be(24.99m);
        resultado.Stock.Should().Be(5);
        resultado.CategoriaId.Should().Be(2);
        resultado.CategoriaNombre.Should().Be("Anime");
        resultado.IsDeleted.Should().BeFalse();
        resultado.CreatedAt.Should().Be(fecha);
        resultado.UpdatedAt.Should().Be(actualizado);
    }

    [Test]
    public void ToDto_CategoriaNoCargada_DevuelveNombreVacio() {
        // Arrange
        var funko = new FunkoPop { Id = 1, Nombre = "Goku", Categoria = null! };

        // Act
        var resultado = funko.ToDto();

        // Assert
        resultado.CategoriaNombre.Should().BeEmpty();
    }

    [Test]
    public void ToModel_CreateFunkoDto_ConvierteDatos() {
        // Arrange
        var dto = new CreateFunkoDto { Nombre = "Batman", Precio = 20m, Stock = 10, CategoriaId = 1 };

        // Act
        var resultado = dto.ToModel();

        // Assert
        resultado.Id.Should().Be(0);
        resultado.Nombre.Should().Be("Batman");
        resultado.Precio.Should().Be(20m);
        resultado.Stock.Should().Be(10);
        resultado.CategoriaId.Should().Be(1);
        resultado.IsDeleted.Should().BeFalse();
    }

    [Test]
    public void ToModel_UpdateFunkoDto_AsignaIdYDatos() {
        // Arrange
        var dto = new UpdateFunkoDto { Nombre = "Batman nuevo", Precio = 30m, Stock = 3, CategoriaId = 2 };

        // Act
        var resultado = dto.ToModel(10);

        // Assert
        resultado.Id.Should().Be(10);
        resultado.Nombre.Should().Be("Batman nuevo");
        resultado.Precio.Should().Be(30m);
        resultado.Stock.Should().Be(3);
        resultado.CategoriaId.Should().Be(2);
    }
}