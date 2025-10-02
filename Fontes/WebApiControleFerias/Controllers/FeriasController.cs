using ControleFerias.Data;
using ControleFerias.DTO;
using ControleFerias.Enums;
using ControleFerias.Models;
using ControleFerias.Validation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace ControleFerias.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FeriasController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public FeriasController(ApplicationDBContext context)
        {
            _context = context;
        }

        [EnableCors]
        [HttpPost, Route("IncluirFerias")]
        #region Documentacao
        [SwaggerOperation(
        Summary = "Adiciona uma Férias se os valores forem válidos",
         Description = @"
          Entrada:
        - dDataInicio: data inicial das férias (Maior que hoje)
        - sDias: dias das férias (maior que 0)
        - sDataFinal: Data Final das férias(maior que a data inicial)
        - sComentario: Comentário opcional
        - colaboradorId: Id do colaborador a ser vinculado

         *Saida*
        - Objeto Ferias 
        
       
        - Observação: Padrão de datas dia/mês/ano ",


        OperationId = "IncluirColaboradorSwagger")]
        #endregion
        public async Task<ActionResult> IncluirFerias([FromBody] FeriasCriarDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest(ModelState);

                //var ferias = new Ferias
                //{
                //    dDataInicio = dto.dDataInicio,
                //    sDias = dto.sDias,
                //    dDataFinal = dto.dDataFinal,
                //    sComentario = dto.sComentario
                //};
                var ferias = new Ferias
                (
                    dto.dDataInicio,
                    dto.sDias,
                    dto.dDataFinal,
                    StatusFerias.Pendente,
                    dto.sComentario,
                    dto.colaboradorId,
                    _context
                    );

                //var ferias = new Ferias
                //{
                //    dDataInicio = dto.dDataInicio,
                //    dDataFinal = dto.dDataFinal,
                //    sComentario = dto.sComentario,
                //    sDias = CalcularDias(dto.dDataInicio, dto.dDataFinal) // calcula automático
                //};

                var colaborador = await _context.Colaborador.FindAsync(dto.colaboradorId);
                if (colaborador == null)
                    return NotFound($"Colaborador com ID {dto.colaboradorId} não encontrado.");

                // Cria a associação com o colaborador
                var colaboradorFerias = new ColaboradorFerias
                {
                    ColaboradorId = dto.colaboradorId,
                    Ferias = ferias
                };

                //if (dto.dDataInicio > dto.dDataFinal)return BadRequest("A data inicial não pode ser maior que a data final.");
                //if (dto.dDataInicio <= DateTime.Today)return BadRequest("A data inicial não pode ser igual à data de hoje.");

                ferias.ColaboradorFerias = new List<ColaboradorFerias> { colaboradorFerias };

                _context.Ferias.Add(ferias);
                await _context.SaveChangesAsync();

                //return Created("ConsultarFerias", ferias);
                return Ok("Ferias criadas com sucesso!");
            }
            catch (DomainExceptionValidation ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { error = ex.InnerException?.Message });
            }
        }
        [HttpGet, Route("ConsultarFerias")]
        public async Task<ActionResult> ConsultarFerias()
        {
            try
            {
                var ferias = await _context.Colaborador
                    .Where(c => c.ColaboradorFerias.Any()) // filtra quem tem férias
                    .Select(c => new ColaboradorConsultarFeriasDTO
                    {
                        Id = c.Id,
                        sNome = c.sNome,
                        EquipeId = c.EquipeId,
                        Ferias = c.ColaboradorFerias
                            .Select(f => new FeriasFormatadaDTO
                            {
                                Id = f.Ferias!.Id,
                                dDataInicio = f.Ferias!.dDataInicio,
                                dDataFinal = f.Ferias!.dDataFinal,
                                sDias = f.Ferias!.sDias
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return Ok(ferias);
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao consultar as férias!");
            }
        }
        [HttpPost, Route("ExcluirFerias")]
        #region Documentacao
        [SwaggerOperation(
        Summary = "Exclui uma Ferias se o Id for compatível com alguma Ferias no banco",
        Description = @"
        **Entrada:**
        - `Id`: Id das Ferias (maior que 0)

         *Saida*
        - Ferias{Id da Ferias} excluida com sucesso`",
        OperationId = "ExcluirFeriasSwagger")]
        #endregion
        public async Task<ActionResult> ExcluirFerias(int id)
        {
            try
            {
                var ferias = await _context.Ferias.FirstOrDefaultAsync(c => c.Id == id);
                if (ferias == null || ferias.Id <= 0)
                {
                    return NotFound($"O colaborador com o id {id} não foi encontrado.");
                }
                _context.Remove(ferias);
                await _context.SaveChangesAsync();

                return Ok($"Ferias {ferias.Id} excluida com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest($"O Colaborador com o id:{id} não pôde ser encontrado.");
            }
        }

        public static int CalcularDias(DateTime dataInicio, DateTime dataFinal, bool incluirDataFinal = true)
        {
            if (dataFinal < dataInicio)
                throw new ArgumentException("A data final não pode ser menor que a data inicial.");

            var dias = (dataFinal - dataInicio).Days;
            return incluirDataFinal ? dias + 1 : dias;
        }

    }
}