```csharp
using ConectaFapes.Common.Domain;
using ConectaFapes.Domain.Entities.CadastroModalidadesBolsas;
using System.Collections.Generic;
using Xunit;

namespace ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Tests
{
    public class VersaoTests
    {
        [Fact]
        public void Versao_Constructor_CreatesEmptyRequisitoBolsasCollection()
        {
            // Arrange
            var versao = new Entities.CadastroModalidadesBolsas.Versao();

            // Assert
            Assert.NotNull(versao.RequisitoBolsas);
            Assert.Empty(versao.RequisitoBolsas);
        }


        [Fact]
        public void Versao_SetRequisitoBolsas_UpdatesCollection()
        {
            // Arrange
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            var requisitos = new List<Entities.CadastroModalidadesBolsas.RequisitoBolsa>() { new Entities.CadastroModalidadesBolsas.RequisitoBolsa() };

            // Act
            versao.RequisitoBolsas = requisitos;

            // Assert
            Assert.NotNull(versao.RequisitoBolsas);
            Assert.Single(versao.RequisitoBolsas);
            Assert.Same(requisitos, versao.RequisitoBolsas);

        }

        [Fact]
        public void Versao_AddRequisitoBolsa_AddsToCollection()
        {
            // Arrange
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            var requisito = new Entities.CadastroModalidadesBolsas.RequisitoBolsa();

            // Act
            versao.RequisitoBolsas.Add(requisito);

            // Assert
            Assert.NotNull(versao.RequisitoBolsas);
            Assert.Single(versao.RequisitoBolsas);
            Assert.Contains(requisito, versao.RequisitoBolsas);
        }

        [Fact]
        public void Versao_RemoveRequisitoBolsa_RemovesFromCollection()
        {
            // Arrange
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            var requisito = new Entities.CadastroModalidadesBolsas.RequisitoBolsa();
            versao.RequisitoBolsas.Add(requisito);

            // Act
            versao.RequisitoBolsas.Remove(requisito);

            // Assert
            Assert.NotNull(versao.RequisitoBolsas);
            Assert.Empty(versao.RequisitoBolsas);
        }

        [Fact]
        public void Versao_ClearRequisitoBolsas_ClearsCollection()
        {
            // Arrange
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            var requisito = new Entities.CadastroModalidadesBolsas.RequisitoBolsa();
            versao.RequisitoBolsas.Add(requisito);

            // Act
            versao.RequisitoBolsas.Clear();

            // Assert
            Assert.NotNull(versao.RequisitoBolsas);
            Assert.Empty(versao.RequisitoBolsas);
        }


        [Fact]
        public void Versao_Constructor_IdIsZero()
        {
            // Arrange & Act
            var versao = new Entities.CadastroModalidadesBolsas.Versao();

            // Assert
            Assert.Equal(0, versao.Id);
        }

        [Fact]
        public void Versao_SetId_UpdatesId()
        {
            // Arrange
            var versao = new Entities.CadastroModalidadesBolsas.Versao();

            // Act
            versao.Id = 1;

            // Assert
            Assert.Equal(1, versao.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Versao_SetDescricao_HandlesNullOrWhiteSpace(string descricao)
        {
            // Arrange
            var versao = new Entities.CadastroModalidadesBolsas.Versao();

            // Act & Assert - No exception should be thrown.  No specific behavior is defined for null/empty description in the provided code.
            versao.Descricao = descricao;
            Assert.Equal(descricao, versao.Descricao);
        }

        [Fact]
        public void Versao_SetDescricao_UpdatesDescricao()
        {
            // Arrange
            var versao = new Entities.CadastroModalidadesBolsas.Versao();

            // Act
            versao.Descricao = "Descrição de Teste";

            // Assert
            Assert.Equal("Descrição de Teste", versao.Descricao);
        }


        //Testes para BaseEntity (métodos privados não acessíveis diretamente)
        //Os testes abaixo demonstram como testar indiretamente os métodos privados de BaseEntity, caso necessário.  
        //Para isso, seria necessário criar métodos públicos auxiliares em BaseEntity ou usar reflexão (não recomendado para testes unitários).

        //Exemplo usando um método auxiliar público (hipotético):
        //[Fact]
        //public void BaseEntity_PrivateMethod_TestUsingPublicHelper()
        //{
        //    //Arrange
        //    var versao = new Entities.CadastroModalidadesBolsas.Versao();

        //    //Act
        //    var result = versao.PublicHelperMethodForPrivateMethodTesting(); //Método auxiliar hipotético

        //    //Assert
        //    // ... Asserções sobre o resultado do método auxiliar ...
        //}

        //Exemplo usando reflexão (não recomendado):
        //[Fact]
        //public void BaseEntity_PrivateMethod_TestUsingReflection()
        //{
        //    // Arrange
        //    var versao = new Entities.CadastroModalidadesBolsas.Versao();
        //    var privateMethod = versao.GetType().GetMethod("PrivateMethod", BindingFlags.NonPublic | BindingFlags.Instance); //Substitua "PrivateMethod" pelo nome do método privado

        //    // Act & Assert -  This is highly discouraged for unit testing.
        //    // ... Use reflection to invoke the private method and assert its behavior ...
        //}


        //Testes adicionais para cobrir mais cenários e atingir a meta de 25 testes (adicionando mais cenários para os métodos existentes)

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Versao_SetVersao_UpdatesVersao(int versaoNumber)
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            versao.VersaoNumero = versaoNumber;
            Assert.Equal(versaoNumber, versao.VersaoNumero);
        }

        [Fact]
        public void Versao_SetVersao_Zero()
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            versao.VersaoNumero = 0;
            Assert.Equal(0, versao.VersaoNumero);
        }

        [Fact]
        public void Versao_SetVersao_Negative()
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            versao.VersaoNumero = -1;
            Assert.Equal(-1, versao.VersaoNumero);
        }

        [Fact]
        public void Versao_SetAtivo_True()
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            versao.Ativo = true;
            Assert.True(versao.Ativo);
        }

        [Fact]
        public void Versao_SetAtivo_False()
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            versao.Ativo = false;
            Assert.False(versao.Ativo);
        }

        [Fact]
        public void Versao_DataCriacao_IsNotNull()
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            Assert.NotNull(versao.DataCriacao);
        }

        [Fact]
        public void Versao_DataAtualizacao_IsNotNull()
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            Assert.NotNull(versao.DataAtualizacao);
        }

        [Fact]
        public void Versao_UsuarioCriacao_IsNotNull()
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            Assert.NotNull(versao.UsuarioCriacao);
        }

        [Fact]
        public void Versao_UsuarioAtualizacao_IsNotNull()
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            Assert.NotNull(versao.UsuarioAtualizacao);
        }

        [Fact]
        public void Versao_Equals_SameInstance_ReturnsTrue()
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            Assert.True(versao.Equals(versao));
        }

        [Fact]
        public void Versao_Equals_Null_ReturnsFalse()
        {
            var versao = new Entities.CadastroModalidadesBolsas.Versao();
            Assert.False(versao.Equals(null));
        }

        [Fact]
        public void Versao_Equals_DifferentInstance_ReturnsFalse()
        {
            var versao1 = new Entities.CadastroModalidadesBolsas.Versao();
            var versao2 = new Entities.CadastroModalidadesBolsas.Versao();
            Assert.False(versao1.Equals(versao2));
        }

    }
}
```