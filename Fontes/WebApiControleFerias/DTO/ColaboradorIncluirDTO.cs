using ControleFerias.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace ControleFerias.DTO
{
    public class ColaboradorIncluirDTO
    {
        //[Required(ErrorMessage ="Nome Precisa ser preenchido")]
        //[MaxLength(36,ErrorMessage = "O nome do colaborador deve ter entre 3 e 32 caracteres."),]
        //[MinLength(3,ErrorMessage = "O nome do colaborador deve ter entre 3 e 32 caracteres.")]
        public string sNome { get; set; } = null!;

        //[Required(ErrorMessage ="Id de equipe precisa ser preenchido")]
        //[MinLength(1,ErrorMessage = "EquipeId deve ser maior que 0")]
        public int EquipeId { get; set; }

    }
}
