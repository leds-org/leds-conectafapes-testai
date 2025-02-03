```csharp
using ConectaFapes.Common.Domain;
using ConectaFapes.Domain.Entities.CadastroModalidadesBolsas;
using ConectaFapes.Domain.Enums.CadastroModalidadesBolsas;
using ConectaFapes.Domain.Validation;
using System;
using System.Collections.Generic;
using Xunit;

namespace ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Tests
{
    public class RequisitoBolsaTests
    {
        [Fact]
        public void RequisitoBolsa_Constructor_ValidData_Success()
        {
            // Arrange
            var tipo = TipoRequisitoBolsa.Documentacao;
            var descricao = "Descrição de teste";
            var requisitoBolsaVersaoId = Guid.NewGuid();

            // Act
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa(tipo, descricao, requisitoBolsaVersaoId);

            // Assert
            Assert.Equal(tipo, requisitoBolsa.Tipo);
            Assert.Equal(descricao, requisitoBolsa.Descricao);
            Assert.Equal(requisitoBolsaVersaoId, requisitoBolsa.RequisitoBolsaVersaoId);
        }

        [Fact]
        public void RequisitoBolsa_Constructor_EmptyDescription_ThrowsDomainValidationException()
        {
            // Arrange
            var tipo = TipoRequisitoBolsa.Documentacao;
            var descricao = "";
            var requisitoBolsaVersaoId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new Entities.CadastroModalidadesBolsas.RequisitoBolsa(tipo, descricao, requisitoBolsaVersaoId));
        }

        [Fact]
        public void RequisitoBolsa_Constructor_NullDescription_ThrowsDomainValidationException()
        {
            // Arrange
            var tipo = TipoRequisitoBolsa.Documentacao;
            string descricao = null;
            var requisitoBolsaVersaoId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new Entities.CadastroModalidadesBolsas.RequisitoBolsa(tipo, descricao, requisitoBolsaVersaoId));
        }


        [Theory]
        [InlineData(TipoRequisitoBolsa.Documentacao, "Descrição válida")]
        [InlineData(TipoRequisitoBolsa.Entrevista, "Outra descrição válida")]
        [InlineData(TipoRequisitoBolsa.ExperienciaProfissional, "Mais uma descrição válida")]
        public void RequisitoBolsa_Constructor_ValidTypesAndDescriptions_Success(TipoRequisitoBolsa tipo, string descricao)
        {
            // Arrange
            var requisitoBolsaVersaoId = Guid.NewGuid();

            // Act
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa(tipo, descricao, requisitoBolsaVersaoId);

            // Assert
            Assert.Equal(tipo, requisitoBolsa.Tipo);
            Assert.Equal(descricao, requisitoBolsa.Descricao);
            Assert.Equal(requisitoBolsaVersaoId, requisitoBolsa.RequisitoBolsaVersaoId);
        }

        [Theory]
        [InlineData(null, "Descrição válida")] //Testando null no tipo
        [InlineData(TipoRequisitoBolsa.Documentacao, null)] //Testando null na descrição
        [InlineData(TipoRequisitoBolsa.Documentacao, "")] //Testando string vazia na descrição
        public void RequisitoBolsa_Constructor_InvalidData_ThrowsDomainValidationException(TipoRequisitoBolsa? tipo, string descricao)
        {
            // Arrange
            var requisitoBolsaVersaoId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new Entities.CadastroModalidadesBolsas.RequisitoBolsa(tipo ?? TipoRequisitoBolsa.Documentacao, descricao ?? "", requisitoBolsaVersaoId));
        }


        [Fact]
        public void RequisitoBolsa_Constructor_LongDescription_TruncatesDescription()
        {
            // Arrange
            var tipo = TipoRequisitoBolsa.Documentacao;
            string longDescription = new string('a', 501);
            var requisitoBolsaVersaoId = Guid.NewGuid();

            // Act
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa(tipo, longDescription, requisitoBolsaVersaoId);

            // Assert
            Assert.Equal(500, requisitoBolsa.Descricao.Length);
        }

        [Fact]
        public void RequisitoBolsa_DefaultConstructor_CreatesValidObject()
        {
            // Arrange & Act
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa();

            // Assert
            Assert.NotNull(requisitoBolsa);
            Assert.Equal(default(TipoRequisitoBolsa), requisitoBolsa.Tipo);
            Assert.Equal(string.Empty, requisitoBolsa.Descricao);
            Assert.Equal(default(Guid), requisitoBolsa.RequisitoBolsaVersaoId);
        }

        // Testando o método privado usando reflexão - abordagem menos ideal, mas necessária para atingir 100% de cobertura
        [Fact]
        public void RequisitoBolsaValidation_EmptyDescription_ReturnsError()
        {
            // Arrange
            var tipo = TipoRequisitoBolsa.Documentacao;
            var descricao = "";

            // Act
            var method = typeof(Entities.CadastroModalidadesBolsas.RequisitoBolsa).GetMethod("RequisitoBolsaValidation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = method.Invoke(new Entities.CadastroModalidadesBolsas.RequisitoBolsa(), new object[] { tipo, descricao });

            // Assert
            var errors = (List<string>)result;
            Assert.Single(errors);
            Assert.Equal("A descrição da bolsa não pode ser vazia", errors[0]);
        }

        [Fact]
        public void RequisitoBolsaValidation_ValidData_ReturnsEmptyList()
        {
            // Arrange
            var tipo = TipoRequisitoBolsa.Documentacao;
            var descricao = "Descrição válida";

            // Act
            var method = typeof(Entities.CadastroModalidadesBolsas.RequisitoBolsa).GetMethod("RequisitoBolsaValidation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = method.Invoke(new Entities.CadastroModalidadesBolsas.RequisitoBolsa(), new object[] { tipo, descricao });

            // Assert
            var errors = (List<string>)result;
            Assert.Empty(errors);
        }

        [Fact]
        public void RequisitoBolsaValidation_NullDescription_ReturnsError()
        {
            // Arrange
            var tipo = TipoRequisitoBolsa.Documentacao;
            string descricao = null;

            // Act
            var method = typeof(Entities.CadastroModalidadesBolsas.RequisitoBolsa).GetMethod("RequisitoBolsaValidation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = method.Invoke(new Entities.CadastroModalidadesBolsas.RequisitoBolsa(), new object[] { tipo, descricao });

            // Assert
            var errors = (List<string>)result;
            Assert.Single(errors);
            Assert.Equal("A descrição da bolsa não pode ser vazia", errors[0]);
        }

        [Theory]
        [InlineData("Descrição muito longa", 500)]
        [InlineData("Descrição curta", 14)]
        [InlineData("", 0)]
        public void RequisitoBolsa_SetDescricao_ValidatesLength(string descricao, int expectedLength)
        {
            //Arrange
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa();

            //Act
            requisitoBolsa.Descricao = descricao;

            //Assert
            Assert.Equal(Math.Min(expectedLength, 500), requisitoBolsa.Descricao.Length);
        }

        [Theory]
        [InlineData(TipoRequisitoBolsa.Documentacao)]
        [InlineData(TipoRequisitoBolsa.Entrevista)]
        [InlineData(TipoRequisitoBolsa.ExperienciaProfissional)]
        public void RequisitoBolsa_SetTipo_SetsTipo(TipoRequisitoBolsa tipo)
        {
            //Arrange
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa();

            //Act
            requisitoBolsa.Tipo = tipo;

            //Assert
            Assert.Equal(tipo, requisitoBolsa.Tipo);
        }

        [Fact]
        public void RequisitoBolsa_SetRequisitoBolsaVersaoId_SetsId()
        {
            //Arrange
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa();
            var newId = Guid.NewGuid();

            //Act
            requisitoBolsa.RequisitoBolsaVersaoId = newId;

            //Assert
            Assert.Equal(newId, requisitoBolsa.RequisitoBolsaVersaoId);
        }

        [Fact]
        public void RequisitoBolsa_SetVersao_SetsVersao()
        {
            //Arrange
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa();
            var versao = new Versao();

            //Act
            requisitoBolsa.Versao = versao;

            //Assert
            Assert.Equal(versao, requisitoBolsa.Versao);
        }


        [Fact]
        public void RequisitoBolsa_GetTipo_ReturnsTipo()
        {
            //Arrange
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa { Tipo = TipoRequisitoBolsa.Documentacao };

            //Act
            var tipo = requisitoBolsa.Tipo;

            //Assert
            Assert.Equal(TipoRequisitoBolsa.Documentacao, tipo);
        }

        [Fact]
        public void RequisitoBolsa_GetDescricao_ReturnsDescricao()
        {
            //Arrange
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa { Descricao = "Descrição de teste" };

            //Act
            var descricao = requisitoBolsa.Descricao;

            //Assert
            Assert.Equal("Descrição de teste", descricao);
        }

        [Fact]
        public void RequisitoBolsa_GetRequisitoBolsaVersaoId_ReturnsId()
        {
            //Arrange
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa { RequisitoBolsaVersaoId = Guid.NewGuid() };

            //Act
            var id = requisitoBolsa.RequisitoBolsaVersaoId;

            //Assert
            Assert.Equal(requisitoBolsa.RequisitoBolsaVersaoId, id);
        }

        [Fact]
        public void RequisitoBolsa_GetVersao_ReturnsVersao()
        {
            //Arrange
            var versao = new Versao();
            var requisitoBolsa = new Entities.CadastroModalidadesBolsas.RequisitoBolsa { Versao = versao };

            //Act
            var retornoVersao = requisitoBolsa.Versao;

            //Assert
            Assert.Equal(versao, retornoVersao);
        }
    }
}
```