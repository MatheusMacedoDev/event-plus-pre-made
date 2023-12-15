using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webapi.event_.Domains;
using webapi.event_.Repositories;

namespace webapi.event_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ComentariosEventoController : ControllerBase
    {
        private readonly ComentariosEventoRepository _comentariosEventoRepository;

        private readonly ContentModeratorClient _contentModeratorClient;

        public ComentariosEventoController(ContentModeratorClient contentModeratorClient)
        {
            _contentModeratorClient = contentModeratorClient;
            _comentariosEventoRepository = new ComentariosEventoRepository();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_comentariosEventoRepository.Listar());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPorIdUsuario/{userId}/{eventId}")]
        public IActionResult GetByUserId(Guid userId, Guid eventId)
        {
            try
            {
                return Ok(_comentariosEventoRepository.BuscarPorIdUsuario(userId, eventId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetForCommomByEvent/{eventId}")]
        public IActionResult GetForCommomByEvent(Guid eventId)
        {
            try
            {
                return Ok(_comentariosEventoRepository.ListarComum(eventId));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetForAdminByEvent/{eventId}")]
        public IActionResult GetForAdminByEvent(Guid eventId)
        {
            try
            {
                return Ok(_comentariosEventoRepository.ListarAdmin(eventId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(ComentariosEvento comentario)
        {
            try
            {
                _comentariosEventoRepository.Cadastrar(comentario);

                return StatusCode(201);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostComentarioAI")]
        public async Task<IActionResult> PostAI(ComentariosEvento comentario)
        {
            try
            {
                if (comentario.Descricao.IsNullOrEmpty())
                {
                    return BadRequest("A descrição do comentário não pode ser nula ou vazia!");
                }

                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(comentario.Descricao));

                var moderationResult = await _contentModeratorClient.TextModeration
                    .ScreenTextAsync("text/plain", stream, "por", false, false, null, true);

                comentario.Exibe = moderationResult.Terms == null;

                _comentariosEventoRepository.Cadastrar(comentario);

                return StatusCode(201, comentario);

            } 
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _comentariosEventoRepository.Deletar(id);

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
