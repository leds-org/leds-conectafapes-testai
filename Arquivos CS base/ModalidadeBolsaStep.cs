using ConectaFapes.Application.DTOs.CadastroModalidadesBolsas.Request;
using ConectaFapes.Application.DTOs.CadastroModalidadesBolsas.Response;
using ConectaFapes.Test.Shared;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Gherkin.Quick;

namespace ConectaFapes.Test.Steps
{
    [FeatureFile("../../../Features/modalidadebolsaFeature.feature")]
    [Collection(WebApplicationFactoryParameters.CollectionName)]
    public class ModalidadeBolsaStep : Xunit.Gherkin.Quick.Feature
    {
        private const string BASE_URL = "https://localhost:3000/api/modalidadebolsa/";
        private readonly WebApplicationFactory _factory;
        private readonly HttpClient _client;
        private HttpResponseMessage? _response;
        private ApiDataProvider _provider;

        public ModalidadeBolsaStep(WebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(); // new HttpClient();
            _provider = new ApiDataProvider(_client);
        }

        #region Check if API is running
        [Given("I have access to the ModalidadeBolsa API")]
        public async Task IHaveAccessAPI()
        {
            var response = await _client.GetAsync(BASE_URL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion

        #region Retrieve Request
        [When(@"I send a GET request to /modalidadebolsa/""(.+)""")]
        public async Task WhenISendAGetRequest(string modalidadeBolsaId)
        {
            _response = await _client.GetAsync(BASE_URL + modalidadeBolsaId);
        }
        #endregion

        #region Post Request
        [When(@"I send a POST request to /modalidadebolsa with the following ModalidadeBolsa details: ""(.+)"", ""(.+)""")]
        public async Task WhenISendAPostRequest(string sigla, string nome)
        {
            var modalidadeBolsa = new ModalidadeBolsaRequestDTO
            {
                Sigla = StringValidator.CheckEmptyString(sigla),
                Nome = StringValidator.CheckEmptyString(nome)
            };

            var content = new StringContent(JsonSerializer.Serialize(modalidadeBolsa), Encoding.UTF8, "application/json");
            _response = await _client.PostAsync(BASE_URL, content);
        }
        #endregion

        #region Put Request
        [When(@"I send a PUT request to /modalidadebolsa/""(.+)"" with the following ModalidadeBolsa details: ""(.+)"", ""(.+)""")]
        public async Task WhenISendAPutRequest(string modalidadeBolsaId, string sigla, string nome)
        {
            ModalidadeBolsaResponseDTO modalidadeBolsa = await _provider.GetEntityById<ModalidadeBolsaResponseDTO>("ModalidadeBolsa", modalidadeBolsaId);

            modalidadeBolsa.Sigla = StringValidator.CheckEmptyString(sigla);
            modalidadeBolsa.Nome = StringValidator.CheckEmptyString(nome);

            var content = new StringContent(JsonSerializer.Serialize(modalidadeBolsa), Encoding.UTF8, "application/json");
            _response = await _client.PutAsync(BASE_URL + modalidadeBolsaId, content);
        }
        #endregion

        #region Delete Request
        [When(@"I send a DELETE request to /modalidadebolsa/""(.+)""")]
        public async Task WhenISendADeleteRequest(string modalidadeBolsaId)
        {
            _response = await _client.DeleteAsync(BASE_URL + modalidadeBolsaId);
        }
        #endregion

        #region Active Request
        [When(@"I send a PUT request to /modalidadebolsa/""(.+)""/ativar")]
        public async Task WhenISendAPutActiveRequest(string modalidadeBolsaId)
        {
            _response = await _client.PutAsync(BASE_URL + modalidadeBolsaId + "/ativar", null);
        }
        #endregion

        #region Disable Request
        [When(@"I send a PUT request to /modalidadebolsa/""(.+)""/desativar")]
        public async Task WhenISendAPutDisableRequest(string modalidadeBolsaId)
        {
            _response = await _client.PutAsync(BASE_URL + modalidadeBolsaId + "/desativar", null);
        }
        #endregion

        #region Check API Response
        [Then(@"the API response should be: ""(.+)""")]
        public void ThenApiResponse(string StatusCode)
        {
            if (_response != null)
            {
                Assert.Equal(Convert.ToInt32(StatusCode), (int)_response.StatusCode);
            }
        }
        #endregion
    }
}
