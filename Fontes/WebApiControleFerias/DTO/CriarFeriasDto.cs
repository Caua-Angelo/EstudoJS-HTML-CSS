namespace ControleFerias.DTO
{
    public class CriarFeriasDto
    {
        public DateTime dDataInicio { get; set; }
        public int sDias { get; set; }
        public DateTime dDataFinal { get; set; }
        public string? sComentario { get; set; }
        public int colaboradorId { get; set; }

    }
}
