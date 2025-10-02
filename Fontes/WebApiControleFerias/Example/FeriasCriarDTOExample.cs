using ControleFerias.DTO;
using Swashbuckle.AspNetCore.Filters;

public class FeriasCriarDTOExample : IExamplesProvider<FeriasCriarDTO>
{
    public FeriasCriarDTO GetExamples()
    {
        return new FeriasCriarDTO
        {
            dDataInicio = new DateTime(2025, 9, 5),
            sDias = 15,
            dDataFinal = new DateTime(2025, 9, 20),
            sComentario = "Reprovado por :",
            colaboradorId = 158
        };
    }
}
