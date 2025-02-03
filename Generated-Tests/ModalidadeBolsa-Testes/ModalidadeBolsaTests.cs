```csharp
using ConectaFapes.Common.Domain;
using ConectaFapes.Domain.Entities.CadastroModalidadesBolsas;
using ConectaFapes.Domain.Enums.CadastroModalidadesBolsas;
using ConectaFapes.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Tests
{
    public class ModalidadeBolsaTests
    {
        [Fact]
        public void ModalidadeBolsa_Constructor_ValidData_Success()
        {
            // Arrange
            string sigla = "BOLSA1";
            string nome = "Bolsa de Estudos 1";

            // Act
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa(sigla, nome);

            // Assert
            Assert.Equal(sigla.ToUpper(), modalidadeBolsa.Sigla);
            Assert.Equal(nome, modalidadeBolsa.Nome);
        }

        [Fact]
        public void ModalidadeBolsa_Constructor_InvalidSigla_ThrowsDomainValidationException()
        {
            // Arrange
            string sigla = "";
            string nome = "Bolsa de Estudos 1";

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa(sigla, nome));
        }

        [Fact]
        public void ModalidadeBolsa_Constructor_InvalidNome_ThrowsDomainValidationException()
        {
            // Arrange
            string sigla = "BOLSA1";
            string nome = "";

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa(sigla, nome));
        }

        [Fact]
        public void ModalidadeBolsa_ModalidadeInativa_NoVersoes_ReturnsFalse()
        {
            //Arrange
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();

            //Act
            var result = modalidadeBolsa.ModalidadeInativa();

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ModalidadeBolsa_ModalidadeInativa_AllInativas_ReturnsTrue()
        {
            //Arrange
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade { Estado = EstadoVersaoModalidade.INATIVA });
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade { Estado = EstadoVersaoModalidade.INATIVA });

            //Act
            var result = modalidadeBolsa.ModalidadeInativa();

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ModalidadeBolsa_ModalidadeInativa_MixedStates_ReturnsFalse()
        {
            //Arrange
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade { Estado = EstadoVersaoModalidade.INATIVA });
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade { Estado = EstadoVersaoModalidade.ATIVA });

            //Act
            var result = modalidadeBolsa.ModalidadeInativa();

            //Assert
            Assert.False(result);
        }


        [Fact]
        public void ModalidadeBolsa_PossuiVersaoModalidades_Empty_ReturnsFalse()
        {
            // Arrange
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();

            // Act
            bool result = modalidadeBolsa.PossuiVersaoModalidades();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ModalidadeBolsa_PossuiVersaoModalidades_NotEmpty_ReturnsTrue()
        {
            // Arrange
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade());

            // Act
            bool result = modalidadeBolsa.PossuiVersaoModalidades();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ModalidadeBolsa_UnicaVersaoModalidade_Empty_ReturnsFalse()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            Assert.False(modalidadeBolsa.UnicaVersaoModalidade());
        }

        [Fact]
        public void ModalidadeBolsa_UnicaVersaoModalidade_OneVersion_ReturnsTrue()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade());
            Assert.True(modalidadeBolsa.UnicaVersaoModalidade());
        }

        [Fact]
        public void ModalidadeBolsa_UnicaVersaoModalidade_MultipleVersions_ReturnsFalse()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade());
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade());
            Assert.False(modalidadeBolsa.UnicaVersaoModalidade());
        }

        [Fact]
        public void ModalidadeBolsa_VerificarVersaoModalidadeAtiva_AtivaExists_ReturnsTrue()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade { Estado = EstadoVersaoModalidade.ATIVA });
            Assert.True(modalidadeBolsa.VerificarVersaoModalidadeAtiva());
        }

        [Fact]
        public void ModalidadeBolsa_VerificarVersaoModalidadeAtiva_NoAtiva_ReturnsFalse()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            Assert.False(modalidadeBolsa.VerificarVersaoModalidadeAtiva());
        }

        [Fact]
        public void ModalidadeBolsa_VerificarVersaoModalidadeEmEdicao_EmEdicaoExists_ReturnsTrue()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade { Estado = EstadoVersaoModalidade.EM_EDICAO });
            Assert.True(modalidadeBolsa.VerificarVersaoModalidadeEmEdicao());
        }

        [Fact]
        public void ModalidadeBolsa_VerificarVersaoModalidadeEmEdicao_NoEmEdicao_ReturnsFalse()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            Assert.False(modalidadeBolsa.VerificarVersaoModalidadeEmEdicao());
        }

        [Fact]
        public void ModalidadeBolsa_AdicionarVersaoModalidade_AddsVersion()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            var versao = new VersaoModalidade();
            modalidadeBolsa.AdicionarVersaoModalidade(versao);
            Assert.Single(modalidadeBolsa.VersaoModalidadesBolsas);
        }

        [Fact]
        public void ModalidadeBolsa_AtivarVersaoModalidade_OneEmEdicao_Activates()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade { Estado = EstadoVersaoModalidade.EM_EDICAO });
            modalidadeBolsa.AtivarVersaoModalidade();
            Assert.Single(modalidadeBolsa.VersaoModalidadesBolsas.Where(v => v.Estado == EstadoVersaoModalidade.ATIVA));
        }

        [Fact]
        public void ModalidadeBolsa_DesativarVersaoModalidade_OneAtiva_Deactivates()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            modalidadeBolsa.VersaoModalidadesBolsas.Add(new VersaoModalidade { Estado = EstadoVersaoModalidade.ATIVA });
            modalidadeBolsa.DesativarVersaoModalidade();
            Assert.Single(modalidadeBolsa.VersaoModalidadesBolsas.Where(v => v.Estado == EstadoVersaoModalidade.INATIVA));
        }

        [Fact]
        public void ModalidadeBolsa_PegarModalidadeAtiva_AtivaExists_ReturnsAtiva()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            var versaoAtiva = new VersaoModalidade { Estado = EstadoVersaoModalidade.ATIVA };
            modalidadeBolsa.VersaoModalidadesBolsas.Add(versaoAtiva);
            Assert.Same(versaoAtiva, modalidadeBolsa.PegarModalidadeAtiva());
        }

        [Fact]
        public void ModalidadeBolsa_PegarModalidadeAtiva_NoAtiva_ThrowsException()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            Assert.Throws<InvalidOperationException>(() => modalidadeBolsa.PegarModalidadeAtiva());
        }


        //Testes para métodos privados -  Acesso indireto via métodos públicos

        [Fact]
        public void ModalidadeBolsaValidation_EmptySigla_ReturnsError()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            var errors =  ReflectionHelper.CallPrivateMethod<List<string>>(modalidadeBolsa, "ModalidadeBolsaValidation", "", "Nome");
            Assert.Contains("A sigla da resolução não pode ser vazia", errors);
        }

        [Fact]
        public void ModalidadeBolsaValidation_EmptyNome_ReturnsError()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            var errors = ReflectionHelper.CallPrivateMethod<List<string>>(modalidadeBolsa, "ModalidadeBolsaValidation", "Sigla", "");
            Assert.Contains("o nome da resolução não pode ser vazia", errors);
        }

        [Fact]
        public void ModalidadeBolsaValidation_ValidData_ReturnsEmptyList()
        {
            var modalidadeBolsa = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.ModalidadeBolsa();
            var errors = ReflectionHelper.CallPrivateMethod<List<string>>(modalidadeBolsa, "ModalidadeBolsaValidation", "Sigla", "Nome");
            Assert.Empty(errors);
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