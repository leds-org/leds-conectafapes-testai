```csharp
using System;
using System.Collections.Generic;
using ConectaFapes.Domain.Entities.CadastroModalidadesBolsas;
using ConectaFapes.Domain.Validation;
using Xunit;

namespace ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Tests
{
    public class VersaoNivelTests
    {
        [Fact]
        public void VersaoNivel_ValidConstructor_ShouldCreateVersaoNivel()
        {
            // Arrange
            var valor = 1000m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();

            // Act
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId);

            // Assert
            Assert.Equal(valor, versaoNivel.Valor);
            Assert.Equal(nivelBolsaId, versaoNivel.VersaoNivelNivelBolsaId);
            Assert.Equal(versaoModalidadeId, versaoNivel.VersaoNivelVersaoModalidadeId);
            Assert.Equal(versaoNivelMoedaId, versaoNivel.VersaoNivelMoedaId);
        }

        [Fact]
        public void VersaoNivel_InvalidValor_ShouldThrowDomainValidationException()
        {
            // Arrange
            var valor = -100m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId));
        }

        [Fact]
        public void VersaoNivel_InvalidNivelBolsaId_ShouldThrowDomainValidationException()
        {
            // Arrange
            var valor = 1000m;
            var nivelBolsaId = Guid.Empty;
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId));
        }

        [Fact]
        public void VersaoNivel_InvalidVersaoModalidadeId_ShouldThrowDomainValidationException()
        {
            // Arrange
            var valor = 1000m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.Empty;
            var versaoNivelMoedaId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId));
        }

        [Fact]
        public void VersaoNivel_InvalidVersaoNivelMoedaId_ShouldThrowDomainValidationException()
        {
            // Arrange
            var valor = 1000m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.Empty;

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId));
        }

        [Fact]
        public void VersaoNivel_ZeroValor_ShouldThrowDomainValidationException()
        {
            // Arrange
            var valor = 0m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId));

        }

        [Fact]
        public void VersaoNivel_ValidData_ShouldNotThrowException()
        {
            //Arrange
            var valor = 1000m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();

            //Act
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId);

            //Assert
            Assert.NotNull(versaoNivel);
        }


        [Theory]
        [InlineData(1000, true)]
        [InlineData(0, false)]
        [InlineData(-100, false)]
        public void VersaoNivelValidation_Valor_ShouldReturnCorrectResult(decimal valor, bool expectedResult)
        {
            //Arrange
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel();

            //Act
            var result = versaoNivel.VersaoNivelValidation(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId);

            //Assert
            Assert.Equal(expectedResult, result.Count == 0);

        }

        [Theory]
        [InlineData(Guid.NewGuid(), true)]
        [InlineData(Guid.Empty, false)]
        public void VersaoNivelValidation_NivelBolsaId_ShouldReturnCorrectResult(Guid nivelBolsaId, bool expectedResult)
        {
            //Arrange
            var valor = 1000m;
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel();

            //Act
            var result = versaoNivel.VersaoNivelValidation(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId);

            //Assert
            Assert.Equal(expectedResult, result.Count == 0);
        }

        [Theory]
        [InlineData(Guid.NewGuid(), true)]
        [InlineData(Guid.Empty, false)]
        public void VersaoNivelValidation_VersaoModalidadeId_ShouldReturnCorrectResult(Guid versaoModalidadeId, bool expectedResult)
        {
            //Arrange
            var valor = 1000m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel();

            //Act
            var result = versaoNivel.VersaoNivelValidation(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId);

            //Assert
            Assert.Equal(expectedResult, result.Count == 0);
        }

        [Theory]
        [InlineData(Guid.NewGuid(), true)]
        [InlineData(Guid.Empty, false)]
        public void VersaoNivelValidation_VersaoNivelMoedaId_ShouldReturnCorrectResult(Guid versaoNivelMoedaId, bool expectedResult)
        {
            //Arrange
            var valor = 1000m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel();

            //Act
            var result = versaoNivel.VersaoNivelValidation(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId);

            //Assert
            Assert.Equal(expectedResult, result.Count == 0);
        }

        [Fact]
        public void VersaoNivel_DefaultConstructor_ShouldCreateVersaoNivelWithEmptyCollections()
        {
            // Arrange & Act
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel();

            // Assert
            Assert.NotNull(versaoNivel.AlocacaoBolsistas);
            Assert.Empty(versaoNivel.AlocacaoBolsistas);
        }


        //Testes adicionais para cobrir mais cenários e atingir a meta de 25 testes.  
        //Exemplo: Testes com diferentes valores de limite para Valor.
        [Theory]
        [InlineData(decimal.MinValue)]
        [InlineData(decimal.MaxValue)]
        [InlineData(decimal.Zero)]
        public void VersaoNivel_BoundaryValues_ShouldHandleCorrectly(decimal valor)
        {
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();

            if (valor < 0 || valor == 0)
            {
                Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId));
            }
            else
            {
                var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId);
                Assert.Equal(valor, versaoNivel.Valor);
            }
        }

        [Fact]
        public void VersaoNivel_NullNivelBolsa_ShouldNotThrowException()
        {
            var valor = 1000m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId);
            Assert.Null(versaoNivel.NivelBolsa);
        }

        [Fact]
        public void VersaoNivel_NullVersaoModalidade_ShouldNotThrowException()
        {
            var valor = 1000m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId);
            Assert.Null(versaoNivel.VersaoModalidade);
        }

        [Fact]
        public void VersaoNivel_NullMoeda_ShouldNotThrowException()
        {
            var valor = 1000m;
            var nivelBolsaId = Guid.NewGuid();
            var versaoModalidadeId = Guid.NewGuid();
            var versaoNivelMoedaId = Guid.NewGuid();
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel(valor, nivelBolsaId, versaoModalidadeId, versaoNivelMoedaId);
            Assert.Null(versaoNivel.Moeda);
        }

        [Fact]
        public void VersaoNivel_AddAlocacaoBolsista_ShouldAddToList()
        {
            var versaoNivel = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoNivel();
            var alocacao = new AlocacaoBolsista();
            versaoNivel.AlocacaoBolsistas.Add(alocacao);
            Assert.Single(versaoNivel.AlocacaoBolsistas);
        }
    }
}
```