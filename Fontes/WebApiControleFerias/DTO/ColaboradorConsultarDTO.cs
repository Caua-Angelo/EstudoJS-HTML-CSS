using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFerias.DTO
{
    public class ColaboradorConsultarDTO
    {
        public int Id { get; set; }
        public string sNome { get; set; } = null!;
        public int EquipeId { get; set; }


    }
}
