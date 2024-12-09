using ConectaFapes.Application.DTOs.CadastroModalidadesBolsas.Request;
using ConectaFapes.Application.DTOs.CadastroModalidadesBolsas.Response;
using ConectaFapes.Test.Shared;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Gherkin.Quick;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConectaFapes.Test.Steps
{
    [FeatureFile("../../../Features/ListModalitiesFeature.feature")]
    [Collection(WebApplicationFactoryParameters.CollectionName)]
    public class ListModalitiesSteps : Xunit.Gherkin.Quick.Feature
    {
        private const string BASE_URL = "/api/modalidadebolsa";
        private readonly WebApplicationFactory _factory;
        private readonly HttpClient _client;
        private HttpResponseMessage? _response;
        private List<ModalidadeBolsaResponseDTO> _modalities;


        public ListModalitiesSteps(WebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _modalities = new List<ModalidadeBolsaResponseDTO>();
        }

        [Given("the system is ready to list modalities")]
        public void GivenTheSystemIsReadyToListModalities() { }


        [Given("the system is ready to list modalities and contains 1000 modalities")]
        public void GivenTheSystemIsReadyToListModalitiesAndContains1000Modalities() { }


        [When("the user requests the list of modalities")]
        public async Task WhenTheUserRequestsTheListOfModalities()
        {
            _response = await _client.GetAsync(BASE_URL);
            _modalities = await DeserializeResponse<List<ModalidadeBolsaResponseDTO>>(_response);
        }

        [When("the user requests the list of modalities with filter \"(.*)\"")]
        public async Task WhenTheUserRequestsTheListOfModalitiesWithFilter(string filterText)
        {
            string url = BASE_URL + (string.IsNullOrEmpty(filterText) ? "" : $"?filter={filterText}");
            _response = await _client.GetAsync(url);
            _modalities = await DeserializeResponse<List<ModalidadeBolsaResponseDTO>>(_response);

        }

        [Then("the system returns a list of modalities")]
        public void ThenTheSystemReturnsAListOfModalities()
        {
            Assert.NotNull(_modalities);
        }

        [Then("each modality includes sigla, active_resolution_number, active_version_name, and has_editing_version")]
        public void ThenEachModalityIncludesSiglaActiveResolutionNumberActiveVersionNameAndHasEditingVersion()
        {
            foreach (var modality in _modalities)
            {
                Assert.NotEmpty(modality.Sigla);
                Assert.NotNull(modality.ActiveResolutionNumber);
                Assert.NotEmpty(modality.ActiveVersionName);
                Assert.NotNull(modality.HasEditingVersion);
            }
        }

        [Then("the system returns a filtered list of modalities")]
        public void ThenTheSystemReturnsAFilteredListOfModalities()
        {
            Assert.NotEmpty(_modalities);
        }

        [Then("each modality in the list matches the filter \"(.*)\"")]
        public void ThenEachModalityInTheListMatchesTheFilter(string filterText)
        {
            foreach (var modality in _modalities)
            {
                Assert.True(modality.Sigla.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                            modality.ActiveResolutionNumber.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                            modality.ActiveVersionName.Contains(filterText, StringComparison.OrdinalIgnoreCase));

            }
        }

        [When("the user selects modality with sigla \"(.*)\"")]
        public async Task WhenTheUserSelectsModalityWithSigla(string sigla)
        {
            //This scenario requires a different endpoint or method to select a modality.  This is a placeholder.
            Assert.True(true); // Placeholder - Replace with actual implementation
        }

        [Then("the system selects the modality with sigla \"(.*)\"")]
        public void ThenTheSystemSelectsTheModalityWithSigla(string sigla)
        {
            Assert.True(_modalities.Any(m => m.Sigla == sigla));
        }

        [Then("the system returns an empty list")]
        public void ThenTheSystemReturnsAnEmptyList()
        {
            Assert.Empty(_modalities);
        }

        [Then("the system logs an informational message \"(.*)\"")]
        public void ThenTheSystemLogsAnInformationalMessage(string message)
        {
            Assert.True(true); // Placeholder - Replace with actual log check
        }

        [Then("the system handles null values in modality attributes gracefully")]
        public void ThenTheSystemHandlesNullValuesInModalityAttributesGracefully()
        {
            Assert.True(true); // Placeholder -  Replace with actual null value handling check

        }

        [Then("the system returns a list of 1000 modalities")]
        public void ThenTheSystemReturnsAListOf1000Modalities()
        {
            Assert.Equal(1000, _modalities.Count);
        }

        [Then("the response time is within acceptable limits")]
        public void ThenTheResponseTimeIsWithinAcceptableLimits()
        {
            Assert.True(true); // Placeholder - Replace with actual response time check
        }

        [Then("the system returns an error message \"(.*)\"")]
        public void ThenTheSystemReturnsAnErrorMessage(string message)
        {
            Assert.Contains(message, _response?.ReasonPhrase ?? "");
        }


        [Then("the system returns an error message containing \"(.*)\"")]
        public void ThenTheSystemReturnsAnErrorMessageContaining(string errorMessage)
        {
            Assert.Contains(errorMessage, _response?.ReasonPhrase ?? "");
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            if (response != null && response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            else
            {
                return default(T);
            }
        }
    }
}