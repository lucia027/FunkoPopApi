using FluentAssertions;
using FunkoPopApiSql.Errors;
using FunkoPopApiSql.Models;
using FunkoPopApiSql.Validator;

namespace FunkoPopApiSql.Test.Validator;

[TestFixture]
public class FunkoValidatorTests {

    private FunkoValidator _validator = null!;

    [SetUp]
    public void SetUp() {
        _validator = new FunkoValidator();
    }

    [Test]
    public void Validate_FunkoValido_RetornaSuccess() {
        // Arrange
        var funko = new FunkoPop {
            Nombre = "Naruto",
            Precio = 20m,
            Stock = 5,
            CategoriaId = 1
        };

        // Act
        var resultado = _validator.Validate(funko);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Should().BeSameAs(funko);
    }

    [Test]
    public void Validate_FunkoInvalido_RetornaTodosLosErrores() {
        // Arrange
        var funko = new FunkoPop {
            Nombre = " ",
            Precio = -5m,
            Stock = -1,
            CategoriaId = 0
        };

        // Act
        var resultado = _validator.Validate(funko);

        // Assert
        resultado.IsFailure.Should().BeTrue();
        resultado.Error.Should().BeOfType<FunkoError.Validation>();

        var error = (FunkoError.Validation)resultado.Error;
        error.Errores.Should().HaveCount(4);
    }

    [Test]
    public void Validate_NombreDemasiadoLargo_RetornaFailure() {
        // Arrange
        var funko = new FunkoPop {
            Nombre = new string('A', 101),
            Precio = 20m,
            Stock = 5,
            CategoriaId = 1
        };

        // Act
        var resultado = _validator.Validate(funko);

        // Assert
        resultado.IsFailure.Should().BeTrue();
        resultado.Error.Message.Should().Contain("100 caracteres");
    }

    [Test]
    public void Validate_PrecioSuperaLimite_RetornaFailure() {
        // Arrange
        var funko = new FunkoPop {
            Nombre = "Batman",
            Precio = 1000000m,
            Stock = 5,
            CategoriaId = 1
        };

        // Act
        var resultado = _validator.Validate(funko);

        // Assert
        resultado.IsFailure.Should().BeTrue();
        resultado.Error.Message.Should().Contain("precio");
    }

    [Test]
    public void Validate_StockCero_RetornaSuccess() {
        // Arrange
        var funko = new FunkoPop {
            Nombre = "Goku",
            Precio = 25m,
            Stock = 0,
            CategoriaId = 1
        };

        // Act
        var resultado = _validator.Validate(funko);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
    }
}