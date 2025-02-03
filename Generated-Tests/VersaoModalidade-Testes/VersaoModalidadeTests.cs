```csharp
using ConectaFapes.Domain.Entities.CadastroModalidadesBolsas;
using ConectaFapes.Domain.Enums.CadastroModalidadesBolsas;
using System;
using System.Collections.Generic;
using Xunit;

namespace ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Tests
{
    public class VersaoModalidadeTests
    {
        [Fact]
        public void VersaoModalidade_Constructor_ValidData_Success()
        {
            // Arrange
            var reducaoPorVinculo = 0.5m;
            var descricao = "Descrição de teste";
            var dataInicioVigencia = DateTimeOffset.Now;
            var versaoModalidadeResolucaoId = Guid.NewGuid();
            var modalidadeBolsaId = Guid.NewGuid();

            // Act
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade(reducaoPorVinculo, descricao, dataInicioVigencia, versaoModalidadeResolucaoId, modalidadeBolsaId);

            // Assert
            Assert.Equal(reducaoPorVinculo, versaoModalidade.ReducaoPorVinculo);
            Assert.Equal(descricao, versaoModalidade.Descricao);
            Assert.Equal(dataInicioVigencia, versaoModalidade.DataInicioVigencia);
            Assert.Equal(versaoModalidadeResolucaoId, versaoModalidade.VersaoModalidadeResolucaoId);
            Assert.Equal(modalidadeBolsaId, versaoModalidade.VersaoModalidadeModalidadeBolsaId);
        }

        [Fact]
        public void VersaoModalidade_Constructor_InvalidReducaoPorVinculo_ThrowsException()
        {
            // Arrange
            var reducaoPorVinculo = 1.5m;
            var descricao = "Descrição de teste";
            var dataInicioVigencia = DateTimeOffset.Now;
            var versaoModalidadeResolucaoId = Guid.NewGuid();
            var modalidadeBolsaId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade(reducaoPorVinculo, descricao, dataInicioVigencia, versaoModalidadeResolucaoId, modalidadeBolsaId));
        }

        [Fact]
        public void VersaoModalidade_Constructor_EmptyDescricao_ThrowsException()
        {
            // Arrange
            var reducaoPorVinculo = 0.5m;
            var descricao = "";
            var dataInicioVigencia = DateTimeOffset.Now;
            var versaoModalidadeResolucaoId = Guid.NewGuid();
            var modalidadeBolsaId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade(reducaoPorVinculo, descricao, dataInicioVigencia, versaoModalidadeResolucaoId, modalidadeBolsaId));
        }

        [Fact]
        public void VersaoModalidade_Constructor_EmptyResolucaoId_ThrowsException()
        {
            // Arrange
            var reducaoPorVinculo = 0.5m;
            var descricao = "Descrição de teste";
            var dataInicioVigencia = DateTimeOffset.Now;
            var versaoModalidadeResolucaoId = Guid.Empty;
            var modalidadeBolsaId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade(reducaoPorVinculo, descricao, dataInicioVigencia, versaoModalidadeResolucaoId, modalidadeBolsaId));
        }

        [Fact]
        public void VersaoModalidade_Constructor_EmptyModalidadeBolsaId_ThrowsException()
        {
            // Arrange
            var reducaoPorVinculo = 0.5m;
            var descricao = "Descrição de teste";
            var dataInicioVigencia = DateTimeOffset.Now;
            var versaoModalidadeResolucaoId = Guid.NewGuid();
            var modalidadeBolsaId = Guid.Empty;

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade(reducaoPorVinculo, descricao, dataInicioVigencia, versaoModalidadeResolucaoId, modalidadeBolsaId));
        }


        [Fact]
        public void VerificarDataValida_DataPosterior_ReturnsTrue()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();
            versaoModalidade.DataInicioVigencia = new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero);
            var versaoAnteriorDataInicioVigencia = new DateTimeOffset(2023, 12, 31, 0, 0, 0, TimeSpan.Zero);

            // Act
            var result = versaoModalidade.VerificarDataValida(versaoAnteriorDataInicioVigencia);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerificarDataValida_DataAnterior_ReturnsFalse()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();
            versaoModalidade.DataInicioVigencia = new DateTimeOffset(2023, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var versaoAnteriorDataInicioVigencia = new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero);

            // Act
            var result = versaoModalidade.VerificarDataValida(versaoAnteriorDataInicioVigencia);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData("")]
        public void VerificarReducaoPorVinculo_NullOrEmpty_SetsReducaoPorVinculoTo1(decimal? value)
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();

            // Act
            versaoModalidade.VerificarReducaoPorVinculo(value);

            // Assert
            Assert.Equal(1m, versaoModalidade.ReducaoPorVinculo);
        }

        [Fact]
        public void VerificarReducaoPorVinculo_ValidValue_KeepsReducaoPorVinculo()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();
            decimal value = 0.7m;

            // Act
            versaoModalidade.VerificarReducaoPorVinculo(value);

            // Assert
            Assert.Equal(value, versaoModalidade.ReducaoPorVinculo);
        }

        [Fact]
        public void VerificarSePossuiNivel_NivelExistente_ReturnsTrue()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();
            var nivel = Guid.NewGuid();
            versaoModalidade.VersaoNiveis.Add(new VersaoNivel { VersaoNivelNivelBolsaId = nivel });

            // Act
            var result = versaoModalidade.VerificarSePossuiNivel(nivel);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerificarSePossuiNivel_NivelInexistente_ReturnsFalse()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();
            var nivel = Guid.NewGuid();

            // Act
            var result = versaoModalidade.VerificarSePossuiNivel(nivel);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GerarSigla_SiglaEData_GeraSiglaCorreta()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();
            var sigla = "TESTE";
            var dataInicioVigencia = new DateTimeOffset(2023, 10, 26, 0, 0, 0, TimeSpan.Zero);
            versaoModalidade.DataInicioVigencia = dataInicioVigencia;

            // Act
            versaoModalidade.GerarSigla(sigla);

            // Assert
            Assert.Equal("TESTE-2023", versaoModalidade.Sigla);
        }

        [Fact]
        public void VersaoEmEdicao_EstadoEmEdicao_ReturnsTrue()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade { Estado = EstadoVersaoModalidade.EM_EDICAO };

            // Act
            var result = versaoModalidade.VersaoEmEdicao();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VersaoEmEdicao_EstadoNaoEmEdicao_ReturnsFalse()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade { Estado = EstadoVersaoModalidade.ATIVA };

            // Act
            var result = versaoModalidade.VersaoEmEdicao();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EncerrarVigenciaVersaoModalidade_DataValida_SetsDataFimVigencia()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();
            var dataVigenciaNovaVersao = DateTimeOffset.Now.AddDays(1);

            // Act
            versaoModalidade.EncerrarVigenciaVersaoModalidade(dataVigenciaNovaVersao);

            // Assert
            Assert.Equal(dataVigenciaNovaVersao, versaoModalidade.DataFimVigencia);
        }

        [Fact]
        public void AlterarEstado_NovoEstado_AlteraEstado()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();
            var novoEstado = EstadoVersaoModalidade.ATIVA;

            // Act
            versaoModalidade.AlterarEstado(novoEstado);

            // Assert
            Assert.Equal(novoEstado, versaoModalidade.Estado);
        }


        //Testes para métodos privados -  Acesso indireto via métodos públicos

        [Fact]
        public void VersaoModalidadeValidation_ValidData_ReturnsEmptyList()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();
            var reducaoPorVinculo = 0.5m;
            var descricao = "Descrição de teste";
            var versaoModalidadeResolucaoId = Guid.NewGuid();
            var modalidadeBolsaId = Guid.NewGuid();

            // Act
            var errors = ReflectionHelper.CallPrivateMethod<List<string>>(versaoModalidade, "VersaoModalidadeVadidation", reducaoPorVinculo, descricao, versaoModalidadeResolucaoId, modalidadeBolsaId);

            // Assert
            Assert.Empty(errors);
        }

        [Fact]
        public void VersaoModalidadeValidation_InvalidData_ReturnsErrors()
        {
            // Arrange
            var versaoModalidade = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.VersaoModalidade();
            var reducaoPorVinculo = 1.5m;
            var descricao = "";
            var versaoModalidadeResolucaoId = Guid.Empty;
            var modalidadeBolsaId = Guid.Empty;

            // Act
            var errors = ReflectionHelper.CallPrivateMethod<List<string>>(versaoModalidade, "VersaoModalidadeVadidation", reducaoPorVinculo, descricao, versaoModalidadeResolucaoId, modalidadeBolsaId);

            // Assert
            Assert.NotEmpty(errors);
            Assert.Contains("A redução por vinculo não pode ser acima de 100%", errors);
            Assert.Contains("o descricao da resolução não pode ser vazia", errors);
            Assert.Contains("A versão da modalidade deve estar associada a uma resolução", errors);
            Assert.Contains("A versão da modalidade deve estar associada a uma modalidade de bolsa", errors);
        }

        //Helper class for accessing private methods via reflection.
        public static class ReflectionHelper
        {
            public static T CallPrivateMethod<T>(object obj, string methodName, params object[] parameters)
            {
                var method = obj.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (T)method.Invoke(obj, parameters);
            }
        }
    }
}
```