using AutoMapper;
using ControleFerias.Data;
using ControleFerias.DTO;
using ControleFerias.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ControleFerias.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public ColaboradorController(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //[HttpPost, Route("IncluirColaborador")]
        //#region Documentacao
        //[SwaggerOperation(
        //Summary = "Adiciona um Colaborador se os valores forem válidos",
        // Description = @"
        //  Entrada:
        //- sNome: Nome do colaborador (mínimo 3 caracteres)
        //- equipeId: ID da equipe existente

        // *Saida*
        //- Objeto Colaborador

        //-Obs:Id da equipe(Verificar no endpoint ConsultarEquipe)",
        //OperationId = "IncluirColaboradorSwagger")]
        //#endregion
        //public async Task<ActionResult> AdicionarColaborador(Colaborador colaborador)
        //{
        //    try
        //    {
        //        #region Validação
        //        if (colaborador == null)
        //        {
        //            return BadRequest("Objeto colaborador não foi enviado corretamente.");
        //        }
        //        if (string.IsNullOrWhiteSpace(colaborador.sNome) && colaborador.EquipeId == 0)
        //        {
        //            return BadRequest("sNome e equipeId precisam ser preenchidos");
        //        }
        //        if (colaborador.sNome == null || String.IsNullOrWhiteSpace(colaborador.sNome))
        //        {
        //            return BadRequest("O nome do colaborador precisa ser preenchido.");
        //        }
        //        colaborador.sNome = colaborador.sNome.Trim();
        //        if (!Regex.IsMatch(colaborador.sNome, @"^[\p{L}\s]+$"))
        //        {
        //            return BadRequest("O nome do colaborador deve conter apenas letras.");
        //        }
        //        if (colaborador.sNome.Length < 3 || colaborador.sNome.Length > 32)
        //        {
        //            return BadRequest("O nome do colaborador deve ter entre 3 e 32 caracteres.");
        //        }
        //        if (colaborador.EquipeId <= 0 || colaborador.EquipeId > 100)
        //        {
        //            return BadRequest("O ID da equipe é inválido.");
        //        }
        //        var equipe = await _context.Equipe.FindAsync(colaborador.EquipeId);
        //        if (equipe == null)
        //        {
        //            return NotFound($"Nenhuma equipe encontrada com o ID {colaborador.EquipeId}.");
        //        }
        //        #endregion

        //        _context.Colaborador.Add(colaborador);
        //        await _context.SaveChangesAsync();
        //        return Ok($"Colaborador {colaborador.sNome} adicionado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao adicionar colaborador: {ex.Message}");
        //    }
        //}

        [HttpPost, Route("IncluirColaborador")]
        #region Documentacao
        [SwaggerOperation(
        Summary = "Adiciona um Colaborador se os valores forem válidos",
         Description = @"
          Entrada:
        - sNome: Nome do colaborador (mínimo 3 caracteres)
        - equipeId: ID da equipe existente

         *Saida*
        - Objeto Colaborador
        
        -Obs:Id da equipe(Verificar no endpoint ConsultarEquipe)",
        OperationId = "IncluirColaboradorSwagger")]
        #endregion
        public async Task<ActionResult> IncluirColaborador(ColaboradorIncluirDTO ColaboradorDTO)
        {
            try
            {
                if (ColaboradorDTO == null ||
            (ColaboradorDTO.sNome == null && ColaboradorDTO.EquipeId == 0))
                {
                    return BadRequest("sNome e EquipeId precisam ser preenchidos.");
                }

                var equipe = await _context.Equipe.FindAsync(ColaboradorDTO.EquipeId);
                if (equipe == null) { return NotFound($"Nenhuma equipe encontrada com o ID {ColaboradorDTO.EquipeId}."); }

                var colaborador = _mapper.Map<Colaborador>(ColaboradorDTO);
                _context.Colaborador.Add(colaborador);
                await _context.SaveChangesAsync();

                return Ok($"Colaborador {ColaboradorDTO.sNome} adicionado com sucesso.");
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
        [HttpGet, Route("ConsultarColaboradores")]
        #region Documentacao
        [SwaggerOperation(
        Summary = "Consulta todos os colaboradores válidos",
         Description = @"
          Entrada:
        

         *Saida*
        - Objetos de todos os Colaboradores(Por ordem de Id)",

        OperationId = "ConsultarColaboradorSwagger")]
        #endregion
        public async Task<ActionResult> ConsultarColaboradores()
        {
            try
            {
                var colaboradores = await _context.Colaborador
                    .Include(e => e.Equipe)
                    .Include(e => e.Ferias)
                    .ToListAsync();

                var colaboradoresDTO = _mapper.Map<List<ColaboradorConsultarDTO>>(colaboradores);
                return Ok(colaboradoresDTO);
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao Consultar os Colaboradores!");
            }
        }
        [HttpPost, Route("ExcluirColaborador")]
        #region Documentacao
        [SwaggerOperation(
        Summary = "Exclui um Colaborador se o Id for compatível com algum Colaborador no banco",
        Description = @"
        **Entrada:**
        - `Id`: Id do colaborador (maior que 0)

         *Saida*
        - Colaborador{Nome do Colaborador} excluido com sucesso`",
        OperationId = "ExcluirolaboradorSwagger")]
        #endregion
        public async Task<ActionResult> ExcluirColaborador(int id)
        {
            try
            {
                var colaborador = await _context.Colaborador.FirstOrDefaultAsync(c => c.Id == id);
                if (colaborador == null || colaborador.Id <= 0) { return NotFound($"O colaborador com o id {id} não foi encontrado."); }
                _context.Remove(colaborador);
                await _context.SaveChangesAsync();

                return Ok($"Colaborador {colaborador.sNome} excluido com  sucesso.");
            }
            catch (Exception)
            {
                return BadRequest($"O Colaborador com o id:{id} não pôde ser encontrado.");
            }
        }
        [HttpPost, Route("AlterarColaborador")]
        #region Documentacao
        [SwaggerOperation(
        Summary = "Altera um Colaborador se os valores forem válidos",
         Description = @"
              Entrada:
            - Id: Id do colaborador (maior que 0 e existente no banco de dados)

            - sNome: Nome do colaborador (mínimo 3 caracteres)
            - equipeId: ID da equipe existente

             *Saida*
            -  colaborador {sNome} Alterado com sucesso
            -Obs:Id da equipe(Verificar no endpoint ConsultarColaboradores)",
        OperationId = "AlterarColaboradorSwagger")]
        #endregion
        public async Task<IActionResult> AlterarColaborador(int id, [FromBody] ColaboradorAlterarDTO ColaboradorDTO)
        {
            try
            {
                if (ColaboradorDTO == null) return BadRequest("Nome e equipe precisam ser preenchidos");

                var colaboradorExistente = await _context.Colaborador
                    .Include(c => c.Equipe)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (colaboradorExistente == null) return NotFound($"Nenhum colaborador encontrado com o ID {id}.");

                var equipeNova = await _context.Equipe.FindAsync(ColaboradorDTO.EquipeId);
                if (equipeNova == null) return NotFound($"Nenhuma equipe encontrada com o ID {ColaboradorDTO.EquipeId}.");

                var nomeAntigo = colaboradorExistente.sNome;
                var equipeAntigaNome = colaboradorExistente.Equipe?.sNome ?? "Desconhecida";

                colaboradorExistente.Update(ColaboradorDTO.sNome, ColaboradorDTO.EquipeId);
                await _context.SaveChangesAsync();

                bool nomeAlterado = nomeAntigo != colaboradorExistente.sNome;
                bool equipeAlterada = equipeAntigaNome != equipeNova.sNome;

                if (nomeAlterado && equipeAlterada) return Ok($"Nome e equipe do colaborador foram alterados com sucesso\no nome: {nomeAntigo} foi alterado para {colaboradorExistente.sNome}\na equipe: {equipeAntigaNome} foi alterado para {equipeNova.sNome}");
                if (nomeAlterado) return Ok($"Nome do colaborador foi alterado de {nomeAntigo} para {colaboradorExistente.sNome} com sucesso.");
                if (equipeAlterada) return Ok($"Equipe do colaborador {colaboradorExistente.sNome} foi alterada de {equipeAntigaNome} para {equipeNova.sNome} com sucesso.");

                return Ok("Nenhuma alteração foi realizada.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
        //[HttpPost, Route("AlterarColaborador")]
        //#region Documentacao
        //[SwaggerOperation(
        //Summary = "Altera um Colaborador se os valores forem válidos",
        // Description = @"
        //      Entrada:
        //    - Id: Id do colaborador (maior que 0 e existente no banco de dados)

        //    - sNome: Nome do colaborador (mínimo 3 caracteres)
        //    - equipeId: ID da equipe existente

        //     *Saida*
        //    -  colaborador {sNome} Alterado com sucesso
        //    -Obs:Id da equipe(Verificar no endpoint ConsultarColaboradores)",
        //OperationId = "AlterarColaboradorSwagger")]
        //#endregion
        //public async Task<IActionResult> AlterarColaborador(int id, [FromBody] ColaboradorAlterarDTO ColaboradorDTO)
        //{
        //    try
        //    {
        //        var colaboradorExistente = await _context.Colaborador.FindAsync(id);
        //        if (colaboradorExistente == null)
        //            return NotFound($"Nenhum colaborador encontrado com o ID {id}.");

        //        if (ColaboradorDTO == null)
        //            return BadRequest("Há campos não preenchidos corretamente.");

        //        // Busca equipe nova
        //        var equipeNova = await _context.Equipe.FindAsync(ColaboradorDTO.EquipeId);
        //        if (equipeNova == null)
        //            return NotFound($"Nenhuma equipe encontrada com o ID {ColaboradorDTO.EquipeId}.");

        //        // Busca equipe antiga
        //        var equipeAntigaObj = await _context.Equipe.FindAsync(colaboradorExistente.EquipeId);

        //        var nomeAntigo = colaboradorExistente.sNome;
        //        var equipeAntigaNome = equipeAntigaObj?.sNome ?? "Desconhecida";

        //        // Atualiza colaborador
        //        colaboradorExistente.Update(ColaboradorDTO.sNome, ColaboradorDTO.EquipeId);

        //        _context.Colaborador.Update(colaboradorExistente);
        //        await _context.SaveChangesAsync();

        //        bool nomeAlterado = nomeAntigo != colaboradorExistente.sNome;
        //        bool equipeAlterada = equipeAntigaNome != equipeNova.sNome;

        //        if (nomeAlterado && equipeAlterada) return Ok($"Nome e equipe do colaborador foram alterados com sucesso \n" + $"Nome: {nomeAntigo} → {colaboradorExistente.sNome}\n" + $"Equipe: {equipeAntigaNome} → {equipeNova.sNome}");

        //        if (nomeAlterado) return Ok($"Nome do colaborador de {nomeAntigo} para {colaboradorExistente.sNome} com sucesso.");

        //        if (equipeAlterada) return Ok($"Equipe do colaborador {colaboradorExistente.sNome} alterada para {equipeNova.sNome} com sucesso.");

        //        return Ok("Nenhuma alteração foi realizada.");

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
        //    }
    }
}

