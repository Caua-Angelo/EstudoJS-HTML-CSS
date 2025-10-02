using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFerias.Models
{
    public class LogAlteracaoColaborador
    {
        public int Id { get; set; }
        public int ColaboradorId { get; set; }


        public DateTime ddataAlteracao { get; set; } = DateTime.UtcNow;
        public string sNome { get; set; } = null!;
        public int EquipeId { get; set; }


    }
}
