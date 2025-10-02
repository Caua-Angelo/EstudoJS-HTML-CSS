using AutoMapper;
using ControleFerias.API.DTO;
using ControleFerias.Data;
using ControleFerias.DTO;
using ControleFerias.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ControleFerias.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EquipeController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public EquipeController(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost, Route("IncluirEquipe")]
        #region Documentacao
        [SwaggerOperation(
        Summary = "Adiciona uma equipe se os valores forem válidos",
         Description = @"
          Entrada:
        - sNome: Nome da equipe (mínimo 2 caracteres)

         *Saida*
        - Equipe (nomeequipe) criada com sucesso",

        OperationId = "IncluirEquipeSwagger")]
        #endregion
        public async Task<ActionResult> IncluirEquipe(EquipeIncluirDTO equipeDTO)
        {
            try
            {
                if (equipeDTO == null){return BadRequest("O nome da equipe precisa ser preenchido.");}

                var equipe = _mapper.Map<Equipe>(equipeDTO);
                _context.Equipe.Add(equipe);
                await _context.SaveChangesAsync();

                return Ok($"Equipe {equipe.sNome} adicionada com sucesso.");
            }
            //catch para mostrar erros com o banco de dados
            //catch (DbUpdateException ex)
            //{
            //    return BadRequest(new { error = ex.InnerException?.Message });
            //}
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("ConsultarEquipe")]
        #region Documentacao
        [SwaggerOperation(
        Summary = "Consulta todas as equipes disponíveis",
         Description = @"
          Entrada:
        

         *Saida*
        - Todas as equipes disponíveis
        
        ",
        OperationId = "IncluirColaboradorSwagger")]
        #endregion
        public async Task<ActionResult> ConsultarEquipe()
        {
            try
            {
                var equipes = await _context.Equipe.ToListAsync();

                if (equipes.Count == 0)return NotFound();
                return Ok(equipes);
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao Consultar as equipes!");
            }
        }

        [HttpPost, Route("ExcluirEquipe")]
        #region Documentacao
        [SwaggerOperation(

        Summary = "Exclui uma Equipe se o Id for compatível com alguma Equipe no banco",
        Description = @"
            **Entrada:**
            - `Id`: Id da Equipe (maior que 0)

            *Saida*
            - Equipe(Nome da Equipe) excluida com sucesso.",
        OperationId = "ExcluirEquipeSwagger")]
        #endregion
        public async Task<ActionResult> ExcluirEquipe(int id)
        {
            try
            {
                var equipe = await _context.Equipe.FirstOrDefaultAsync(c => c.Id == id);
                if (equipe == null || equipe.Id <= 0)
                {
                    return NotFound($"a Equipe com o id {id} não foi encontrada.");
                }
                _context.Remove(equipe);
                await _context.SaveChangesAsync();

                return Ok($"Equipe {equipe.sNome} excluida com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest($"A Equipe com o id:{id} não foi encontrada.");
            }
        }

        [HttpPost, Route("AlterarEquipe")]
        #region Documentacao
        [SwaggerOperation(
        Summary = "Altera uma equipe se os valores forem válidos",
         Description = @"
              Entrada:
            - Id: Id da Equipe (existente no banco de dados)

            - sNome: Nome da equipe (mínimo 2 caracteres)

             *Saida*
            -  Nome da equipe foi alterada de {Nome antigo} para {Nome Novo} com sucesso.
            -Obs:Id da equipe(Verificar no endpoint ConsultarEquipe)",
        OperationId = "AlterarEquipeSwagger")]
        #endregion
        public async Task<IActionResult> AlterarEquipe(int id, [FromBody] EquipeAlterarDTO EquipeDTO)
        {
            try
            {
                if (EquipeDTO == null) return BadRequest("O Nome precisa se preenchido");

                var EquipeExistente = await _context.Equipe.FirstOrDefaultAsync(c => c.Id == id);
                if (EquipeExistente == null) return NotFound($"Nenhum Equipe encontrada com o ID {id}.");

                var nomeAntigo = EquipeExistente.sNome;

                EquipeExistente.Update(EquipeDTO.sNome);
                await _context.SaveChangesAsync();

                bool nomeAlterado = nomeAntigo != EquipeExistente.sNome;

                if (nomeAlterado) return Ok($"Nome da equipe foi alterada de {nomeAntigo} para {EquipeExistente.sNome} com sucesso.");

                return Ok("Nenhuma alteração foi realizada.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }


        //public async Task<ActionResult> CalcularQuantidadeEmFerias()
        //{
        //    try
        //    {
        //        var equipes = await _context.Equipe.ToListAsync();
        //        return Ok(equipes);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar equipes.");
        //    }
        //}
    }
}
