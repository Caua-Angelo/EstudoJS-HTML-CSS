using ControleFerias.Domain.Models;

namespace ControleFerias.Domain.Interfaces
{
    public interface IEquipeRepository
    {
        Task<IEnumerable<Equipe>> ConsultarEquipe();
        Task<Equipe> IncluirEquipe(Equipe equipe);
        Task<Equipe> GetbyId(int? id);
        Task<Equipe> AlterarEquipe(Equipe equipe);
        Task<Equipe> ExcluirEquipe(Equipe equipe);


    }
}
