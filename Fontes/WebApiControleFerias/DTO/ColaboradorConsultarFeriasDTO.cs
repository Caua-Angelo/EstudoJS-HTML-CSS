using ControleFerias.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFerias.DTO
{
    public class ColaboradorConsultarFeriasDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório")]
        [Column(TypeName = "text")]
        public string sNome { get; set; } = null!;
        public int EquipeId { get; set; }

        public List<FeriasFormatadaDTO> Ferias { get; set; } = new List<FeriasFormatadaDTO>();



    }
}
