using FluentAssertions;
using FunkoPopApiSql.Mapper;
using FunkoPopApiSql.Models;

namespace FunkoPopApiSql.Test.Mapper;

[TestFixture]
public class CategoriaMapperTest {

    [Test]
    public void ToDto_CategoriaValida_ConvierteIdYNombre() {
        // Arrange
        var categoria = new Categoria { Id = 2, Nombre = "Anime" };

        // Act
        var resultado = categoria.ToDto();

        // Assert
        resultado.Id.Should().Be(2);
        resultado.Nombre.Should().Be("Anime");
    }
}