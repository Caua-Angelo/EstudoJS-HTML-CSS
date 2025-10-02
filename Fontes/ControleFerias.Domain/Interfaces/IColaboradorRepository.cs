using ControleFerias.Domain.Models;

namespace ControleFerias.Domain.Interfaces
{
    public interface IColaboradorRepository
    {
        Task<IEnumerable<Colaborador>> ConsultarColaborador();
        Task<Colaborador> IncluirColaborador(Colaborador colaborador);
        Task<Colaborador> GetbyId(int? id);
        Task<Colaborador> AlterarColaborador(Colaborador colaborador);
        Task<Colaborador> ExcluirColaborador(Colaborador colaborador);




    }
}
