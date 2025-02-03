```csharp
using ConectaFapes.Common.Domain;
using ConectaFapes.Domain.Entities.CadastroModalidadesBolsas;
using ConectaFapes.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

namespace ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Tests
{
    public class ResolucaoTests
    {
        [Fact]
        public void Resolucao_Constructor_ValidData_ShouldCreateResolucao()
        {
            // Arrange
            var numero = 123;
            var data = DateTimeOffset.Now;
            var ementa = "Ementa da Resolução";
            var link = "http://example.com";
            var numRastreioEdocs = "12345";

            // Act
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(numero, data, ementa, link, numRastreioEdocs);

            // Assert
            Assert.Equal(numero, resolucao.Numero);
            Assert.Equal(data, resolucao.Data);
            Assert.Equal(ementa, resolucao.Ementa);
            Assert.Equal(link, resolucao.Link);
            Assert.Equal(numRastreioEdocs, resolucao.NumRastreioEdocs);
        }

        [Fact]
        public void Resolucao_Constructor_InvalidNumero_ShouldThrowDomainValidationException()
        {
            // Arrange
            var numero = -1;
            var data = DateTimeOffset.Now;
            var ementa = "Ementa da Resolução";
            var link = "http://example.com";
            var numRastreioEdocs = "12345";

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(numero, data, ementa, link, numRastreioEdocs));
        }

        [Fact]
        public void Resolucao_Constructor_InvalidEmenta_ShouldThrowDomainValidationException()
        {
            // Arrange
            var numero = 123;
            var data = DateTimeOffset.Now;
            var ementa = "";
            var link = "http://example.com";
            var numRastreioEdocs = "12345";

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(numero, data, ementa, link, numRastreioEdocs));
        }

        [Fact]
        public void Resolucao_Constructor_InvalidLink_ShouldThrowDomainValidationException()
        {
            // Arrange
            var numero = 123;
            var data = DateTimeOffset.Now;
            var ementa = "Ementa da Resolução";
            var link = "";
            var numRastreioEdocs = "12345";

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(numero, data, ementa, link, numRastreioEdocs));
        }

        [Fact]
        public void Resolucao_Constructor_InvalidNumRastreioEdocs_ShouldThrowDomainValidationException()
        {
            // Arrange
            var numero = 123;
            var data = DateTimeOffset.Now;
            var ementa = "Ementa da Resolução";
            var link = "http://example.com";
            var numRastreioEdocs = "";

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(numero, data, ementa, link, numRastreioEdocs));
        }


        [Fact]
        public void Resolucao_PossuiModalidades_EmptyVersaoModalidadesBolsas_ShouldReturnFalse()
        {
            // Arrange
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao();

            // Act
            var result = resolucao.PossuiModalidades();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Resolucao_PossuiModalidades_NullVersaoModalidadesBolsas_ShouldReturnFalse()
        {
            // Arrange
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao();
            //Reflection to set private property to null for testing purposes.
            typeof(ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao).GetField("_versaoModalidadesBolsas", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(resolucao, null);

            // Act
            var result = resolucao.PossuiModalidades();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Resolucao_PossuiModalidades_NotEmptyVersaoModalidadesBolsas_ShouldReturnTrue()
        {
            // Arrange
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao();
            resolucao.AdicionarVersaoModalidade(new VersaoModalidade());

            // Act
            var result = resolucao.PossuiModalidades();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Resolucao_AdicionarVersaoModalidade_ShouldAddVersaoModalidade()
        {
            // Arrange
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao();
            var versaoModalidade = new VersaoModalidade();

            // Act
            resolucao.AdicionarVersaoModalidade(versaoModalidade);

            // Assert
            Assert.Single(resolucao.VersaoModalidadesBolsas);
            Assert.Contains(versaoModalidade, resolucao.VersaoModalidadesBolsas);
        }

        [Theory]
        [InlineData(123, "Ementa", "Link", "Rastreio")]
        [InlineData(456, "Outra Ementa", "Outro Link", "Outro Rastreio")]
        public void Resolucao_ResolucaoValidation_ValidData_ShouldReturnEmptyList(int numero, string ementa, string link, string numRastreioEdocs)
        {
            // Arrange
            var data = DateTimeOffset.Now;
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao();

            // Act
            var result = (List<string>)typeof(ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao).GetMethod("ResolucaoValidation", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(resolucao, new object[] { numero, data, ementa, link, numRastreioEdocs });

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(-1, "Ementa", "Link", "Rastreio")]
        [InlineData(123, "", "Link", "Rastreio")]
        [InlineData(123, "Ementa", "", "Rastreio")]
        [InlineData(123, "Ementa", "Link", "")]
        public void Resolucao_ResolucaoValidation_InvalidData_ShouldReturnErrors(int numero, string ementa, string link, string numRastreioEdocs)
        {
            // Arrange
            var data = DateTimeOffset.Now;
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao();

            // Act
            var result = (List<string>)typeof(ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao).GetMethod("ResolucaoValidation", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(resolucao, new object[] { numero, data, ementa, link, numRastreioEdocs });

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Resolucao_DefaultConstructor_ShouldCreateValidResolucao()
        {
            //Arrange
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao();

            //Assert
            Assert.NotNull(resolucao);
            Assert.Empty(resolucao.VersaoModalidadesBolsas);
            Assert.Equal(String.Empty, resolucao.Ementa);
            Assert.Equal(String.Empty, resolucao.Link);
            Assert.Equal(String.Empty, resolucao.NumRastreioEdocs);
        }


        [Fact]
        public void Resolucao_Properties_ShouldSetValuesCorrectly()
        {
            //Arrange
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao();
            var numero = 1;
            var data = DateTimeOffset.Now;
            var ementa = "Ementa de Teste";
            var link = "http://example.com";
            var numRastreio = "12345";

            //Act
            resolucao.Numero = numero;
            resolucao.Data = data;
            resolucao.Ementa = ementa;
            resolucao.Link = link;
            resolucao.NumRastreioEdocs = numRastreio;

            //Assert
            Assert.Equal(numero, resolucao.Numero);
            Assert.Equal(data, resolucao.Data);
            Assert.Equal(ementa, resolucao.Ementa);
            Assert.Equal(link, resolucao.Link);
            Assert.Equal(numRastreio, resolucao.NumRastreioEdocs);
        }

        [Fact]
        public void Resolucao_Ementa_MaxLength_ShouldNotThrowException()
        {
            //Arrange
            var longEmenta = new string('a', 500);
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(1, DateTimeOffset.Now, longEmenta, "http://example.com", "12345");

            //Assert - No exception thrown means it passed the MaxLength validation
            Assert.Equal(longEmenta, resolucao.Ementa);
        }

        [Fact]
        public void Resolucao_Ementa_MaxLengthExceeded_ShouldThrowValidationException()
        {
            //Arrange
            var longEmenta = new string('a', 501);

            //Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(1, DateTimeOffset.Now, longEmenta, "http://example.com", "12345"));
        }

        [Fact]
        public void Resolucao_NullEmenta_ShouldSetToEmptyString()
        {
            //Arrange
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(1, DateTimeOffset.Now, null, "http://example.com", "12345");

            //Assert
            Assert.Equal(String.Empty, resolucao.Ementa);
        }

        [Fact]
        public void Resolucao_NullLink_ShouldSetToEmptyString()
        {
            //Arrange
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(1, DateTimeOffset.Now, "Ementa", null, "12345");

            //Assert
            Assert.Equal(String.Empty, resolucao.Link);
        }

        [Fact]
        public void Resolucao_NullNumRastreio_ShouldSetToEmptyString()
        {
            //Arrange
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(1, DateTimeOffset.Now, "Ementa", "http://example.com", null);

            //Assert
            Assert.Equal(String.Empty, resolucao.NumRastreioEdocs);
        }

        [Fact]
        public void Resolucao_ZeroNumero_ShouldThrowValidationException()
        {
            //Arrange
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(0, DateTimeOffset.Now, "Ementa", "http://example.com", "12345"));
        }

        [Fact]
        public void Resolucao_LargeNumero_ShouldNotThrowException()
        {
            //Arrange
            var resolucao = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao(int.MaxValue, DateTimeOffset.Now, "Ementa", "http://example.com", "12345");

            //Assert - No exception thrown means it passed the validation
            Assert.Equal(int.MaxValue, resolucao.Numero);
        }

    }

    public class VersaoModalidade { }
}
```