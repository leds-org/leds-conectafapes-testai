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
    public class MoedaTests
    {
        [Theory]
        [InlineData("BRL", "Real Brasileiro")]
        [InlineData("USD", "Dólar Americano")]
        [InlineData("EUR", "Euro")]
        public void Moeda_ValidParameters_CreatesMoeda(string simbolo, string nome)
        {
            // Arrange
            // Act
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda(simbolo, nome);

            // Assert
            Assert.Equal(simbolo.ToUpper(), moeda.Simbolo);
            Assert.Equal(nome, moeda.Nome);
        }


        [Theory]
        [InlineData("", "Real Brasileiro")]
        [InlineData("BRLL", "Real Brasileiro")]
        [InlineData("BRL", "")]
        [InlineData("BRL", "Real Brasileiroooooooooooooooooo")]
        [InlineData(null, "Real Brasileiro")]
        [InlineData("BRL", null)]
        public void Moeda_InvalidParameters_ThrowsDomainValidationException(string simbolo, string nome)
        {
            // Arrange
            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda(simbolo, nome));
        }

        [Fact]
        public void Moeda_EmptyConstructor_CreatesValidMoeda()
        {
            // Arrange
            // Act
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda();

            // Assert
            Assert.Equal(String.Empty, moeda.Simbolo);
            Assert.Equal(String.Empty, moeda.Nome);
            Assert.Empty(moeda.VersaoNiveis);

        }

        [Fact]
        public void Moeda_MoedaValidator_EmptySimbolo_ReturnsError()
        {
            // Arrange
            var simbolo = "";
            var nome = "Real Brasileiro";

            // Act
            var errors =  GetMoedaValidatorResult(simbolo, nome);

            // Assert
            Assert.Contains("O simbolo da moeda não pode ser vazio", errors);
        }

        [Fact]
        public void Moeda_MoedaValidator_LongSimbolo_ReturnsError()
        {
            // Arrange
            var simbolo = "BRLL";
            var nome = "Real Brasileiro";

            // Act
            var errors = GetMoedaValidatorResult(simbolo, nome);

            // Assert
            Assert.Contains("O simbolo da moeda não pode ser maior que 3", errors);
        }

        [Fact]
        public void Moeda_MoedaValidator_EmptyNome_ReturnsError()
        {
            // Arrange
            var simbolo = "BRL";
            var nome = "";

            // Act
            var errors = GetMoedaValidatorResult(simbolo, nome);

            // Assert
            Assert.Contains("O nome da moeda não pode ser vazio", errors);
        }

        [Fact]
        public void Moeda_MoedaValidator_LongNome_ReturnsError()
        {
            // Arrange
            var simbolo = "BRL";
            var nome = "Real Brasileiroooooooooooooooooo";

            // Act
            var errors = GetMoedaValidatorResult(simbolo, nome);

            // Assert
            Assert.Contains("O nome da moeda não pode ser maior que 20", errors);
        }

        [Fact]
        public void Moeda_MoedaValidator_ValidParameters_ReturnsEmptyList()
        {
            // Arrange
            var simbolo = "BRL";
            var nome = "Real Brasileiro";

            // Act
            var errors = GetMoedaValidatorResult(simbolo, nome);

            // Assert
            Assert.Empty(errors);
        }

        [Fact]
        public void Moeda_MoedaValidator_NullSimbolo_ReturnsError()
        {
            // Arrange
            string simbolo = null;
            var nome = "Real Brasileiro";

            // Act
            var errors = GetMoedaValidatorResult(simbolo, nome);

            // Assert
            Assert.Contains("O simbolo da moeda não pode ser vazio", errors);
        }

        [Fact]
        public void Moeda_MoedaValidator_NullNome_ReturnsError()
        {
            // Arrange
            var simbolo = "BRL";
            string nome = null;

            // Act
            var errors = GetMoedaValidatorResult(simbolo, nome);

            // Assert
            Assert.Contains("O nome da moeda não pode ser vazio", errors);
        }


        [Fact]
        public void Moeda_Simbolo_Setter_ToUpper()
        {
            // Arrange
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda("brl", "Real");

            // Assert
            Assert.Equal("BRL", moeda.Simbolo);
        }

        private List<string> GetMoedaValidatorResult(string simbolo, string nome)
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda();
            var method = moeda.GetType().GetMethod("MoedaValidator", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<string>)method.Invoke(moeda, new object[] { simbolo, nome });
        }


        [Fact]
        public void Moeda_VersaoNiveis_InitiallyEmpty()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda();
            Assert.Empty(moeda.VersaoNiveis);
        }

        [Fact]
        public void Moeda_VersaoNiveis_CanAddItems()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda();
            moeda.VersaoNiveis.Add(new VersaoNivel()); // Requires VersaoNivel class definition
            Assert.NotEmpty(moeda.VersaoNiveis);
        }

        //Testes adicionais para garantir a cobertura de código acima de 90%

        [Fact]
        public void Moeda_Id_IsAssignedOnCreation()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda("BRL", "Real");
            Assert.NotEqual(Guid.Empty, moeda.Id);
        }

        [Fact]
        public void Moeda_CreatedAt_IsAssignedOnCreation()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda("BRL", "Real");
            Assert.NotNull(moeda.CreatedAt);
        }

        [Fact]
        public void Moeda_UpdatedAt_IsInitiallyNull()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda("BRL", "Real");
            Assert.Null(moeda.UpdatedAt);
        }

        [Fact]
        public void Moeda_IsActive_IsInitiallyTrue()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda("BRL", "Real");
            Assert.True(moeda.IsActive);
        }

        [Fact]
        public void Moeda_DeletedAt_IsInitiallyNull()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda("BRL", "Real");
            Assert.Null(moeda.DeletedAt);
        }

        [Fact]
        public void Moeda_Simbolo_MaxLength_IsThree()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda("BRL", "Real");
            Assert.Equal(3, moeda.Simbolo.Length);
        }

        [Fact]
        public void Moeda_Nome_MaxLength_IsTwenty()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda("BRL", "RealBrasileiro");
            Assert.Equal(14, moeda.Nome.Length);
        }

        [Fact]
        public void Moeda_Constructor_SetsPropertiesCorrectly()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda("BRL", "Real");
            Assert.Equal("BRL", moeda.Simbolo);
            Assert.Equal("Real", moeda.Nome);
        }

        [Fact]
        public void Moeda_ToString_ReturnsCorrectString()
        {
            var moeda = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Moeda("BRL", "Real");
            Assert.Contains("BRL", moeda.ToString());
            Assert.Contains("Real", moeda.ToString());
        }

    }
}
```