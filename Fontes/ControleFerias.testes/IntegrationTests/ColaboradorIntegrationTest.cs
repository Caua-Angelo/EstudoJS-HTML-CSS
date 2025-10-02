using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ControleFerias.testes.IntegrationTests
{
    public class ColaboradorIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly HttpClient _client;

        public ColaboradorIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "POST/IncluirColaborador_NomenuloEquipeIdInválido_404")]
        public async Task PostColaborador_NomeNuloEEquipeIdInvalido_DeveRetornarBadRequest()
        {
            var dto = new { sNome = "", EquipeId = 0 };

            var response = await _client.PostAsJsonAsync("Colaborador/IncluirColaborador", dto);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact(DisplayName = "POST/Incluircolaborador_NomeVazioNulo_400")]
        public async Task CriarColaborador_NomeVazio_DeveRetornarBadRequest()
        {
            var dto = new { sNome = "", EquipeId = 2 };

            var response = await _client.PostAsJsonAsync("Colaborador/IncluirColaborador", dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact(DisplayName = "POST/Incluircolaborador_NomeCurto_400 ")]
        public async Task CriarColaborador_NomeCurto_DeveRetornarBadRequest()
        {
            var dto = new { sNome = "ab", EquipeId = 2 };

            var response = await _client.PostAsJsonAsync("Colaborador/IncluirColaborador", dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}