```csharp
using ConectaFapes.Common.Domain;
using ConectaFapes.Domain.Entities.CadastroModalidadesBolsas;
using ConectaFapes.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Tests
{
    public class NivelBolsaTests
    {
        [Fact]
        public void NivelBolsa_Constructor_ValidSigla_CreatesNivelBolsa()
        {
            // Arrange
            string validSigla = "IC";

            // Act
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa(validSigla);

            // Assert
            Assert.Equal(validSigla.ToUpper(), nivelBolsa.Sigla);
        }

        [Fact]
        public void NivelBolsa_Constructor_EmptySigla_ThrowsDomainValidationException()
        {
            // Arrange
            string invalidSigla = "";

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa(invalidSigla));
        }

        [Fact]
        public void NivelBolsa_Constructor_LongSigla_ThrowsDomainValidationException()
        {
            // Arrange
            string invalidSigla = "1234567890123456"; // 16 characters

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa(invalidSigla));
        }

        [Theory]
        [InlineData("IC", false)]
        [InlineData("Mestrado", false)]
        [InlineData("Doutorado", false)]
        [InlineData("IniciaçãoCientífica", false)]
        [InlineData("123456789012345", true)] //15 characters
        public void NivelBolsaValidation_VariousSiglas_ReturnsExpectedErrors(string sigla, bool expectedResult)
        {
            //Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();

            //Act
            var errors = nivelBolsa.NivelBolsaValidation(sigla);

            //Assert
            Assert.Equal(expectedResult, errors.Count > 0);
        }


        [Fact]
        public void NivelBolsaValidation_NullSigla_ReturnsError()
        {
            // Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();
            string sigla = null;

            // Act
            var errors = nivelBolsa.NivelBolsaValidation(sigla);

            // Assert
            Assert.Single(errors);
            Assert.Equal("A sigla do nivel da bolsa não pode ser vazia", errors[0]);
        }

        [Fact]
        public void UnicaVersaoNivel_EmptyVersaoNiveis_ReturnsTrue()
        {
            // Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();

            // Act
            bool result = nivelBolsa.UnicaVersaoNivel();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UnicaVersaoNivel_OneVersaoNivel_ReturnsTrue()
        {
            // Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();
            nivelBolsa.VersaoNiveis.Add(new VersaoNivel());

            // Act
            bool result = nivelBolsa.UnicaVersaoNivel();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UnicaVersaoNivel_MultipleVersaoNiveis_ReturnsFalse()
        {
            // Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();
            nivelBolsa.VersaoNiveis.Add(new VersaoNivel());
            nivelBolsa.VersaoNiveis.Add(new VersaoNivel());

            // Act
            bool result = nivelBolsa.UnicaVersaoNivel();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void NivelBolsa_Sigla_Setter_SetsSiglaToUpper()
        {
            // Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();

            // Act
            nivelBolsa.Sigla = "ic";

            // Assert
            Assert.Equal("IC", nivelBolsa.Sigla);
        }

        [Fact]
        public void NivelBolsa_Sigla_Setter_HandlesNull()
        {
            // Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();

            // Act
            nivelBolsa.Sigla = null;

            // Assert
            Assert.Equal(String.Empty, nivelBolsa.Sigla);
        }


        // Testes para o método privado NivelBolsaValidation usando reflexão.

        [Fact]
        public void NivelBolsaValidation_PrivateMethod_EmptySigla_ReturnsError()
        {
            // Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();
            MethodInfo method = typeof(ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa).GetMethod("NivelBolsaValidation", BindingFlags.NonPublic | BindingFlags.Instance);
            string sigla = "";

            // Act
            var result = method.Invoke(nivelBolsa, new object[] { sigla }) as List<string>;

            // Assert
            Assert.Single(result);
            Assert.Equal("A sigla do nivel da bolsa não pode ser vazia", result[0]);
        }

        [Fact]
        public void NivelBolsaValidation_PrivateMethod_LongSigla_ReturnsError()
        {
            // Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();
            MethodInfo method = typeof(ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa).GetMethod("NivelBolsaValidation", BindingFlags.NonPublic | BindingFlags.Instance);
            string sigla = "12345678901234567";

            // Act
            var result = method.Invoke(nivelBolsa, new object[] { sigla }) as List<string>;

            // Assert
            Assert.Single(result);
            Assert.Equal("A sigla do nivel da bolsa não pode ser maior que 15 caracteres", result[0]);

        }

        [Fact]
        public void NivelBolsaValidation_PrivateMethod_ValidSigla_ReturnsEmptyList()
        {
            // Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();
            MethodInfo method = typeof(ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa).GetMethod("NivelBolsaValidation", BindingFlags.NonPublic | BindingFlags.Instance);
            string sigla = "IC";

            // Act
            var result = method.Invoke(nivelBolsa, new object[] { sigla }) as List<string>;

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void NivelBolsaValidation_PrivateMethod_NullSigla_ReturnsError()
        {
            // Arrange
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();
            MethodInfo method = typeof(ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa).GetMethod("NivelBolsaValidation", BindingFlags.NonPublic | BindingFlags.Instance);
            string sigla = null;

            // Act
            var result = method.Invoke(nivelBolsa, new object[] { sigla }) as List<string>;

            // Assert
            Assert.Single(result);
            Assert.Equal("A sigla do nivel da bolsa não pode ser vazia", result[0]);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NivelBolsa_Constructor_InvalidSigla_ThrowsException(string sigla)
        {
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa(sigla));
        }

        [Theory]
        [InlineData("ABC")]
        [InlineData("123")]
        [InlineData("ABC123")]
        public void NivelBolsa_Constructor_ValidSigla_CreatesObject(string sigla)
        {
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa(sigla);
            Assert.Equal(sigla.ToUpper(), nivelBolsa.Sigla);
        }

        [Fact]
        public void NivelBolsa_DefaultConstructor_CreatesObjectWithEmptySigla()
        {
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();
            Assert.Equal(string.Empty, nivelBolsa.Sigla);
        }

        [Fact]
        public void NivelBolsa_VersaoNiveis_Default_IsEmpty()
        {
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();
            Assert.Empty(nivelBolsa.VersaoNiveis);
        }

        [Fact]
        public void NivelBolsa_VersaoNiveis_CanAddItems()
        {
            var nivelBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.NivelBolsa();
            nivelBolsa.VersaoNiveis.Add(new VersaoNivel());
            Assert.Single(nivelBolsa.VersaoNiveis);
        }


    }
}
```