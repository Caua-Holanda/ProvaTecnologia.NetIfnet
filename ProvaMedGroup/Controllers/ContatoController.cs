using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProvaMedGroup.DomainModel.Entities;
using ProvaMedGroup.DomainModel.Interfaces.Services;
using ProvaMedGroup.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProvaMedGroup.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoService _contatosService;
        private readonly IMapper _mapper;

        public ContatoController(IContatoService contatosService, IMapper mapper)
        {
            _contatosService = contatosService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ContatoViewModel>> ListarContato()
        {
            return _mapper.Map<IEnumerable<ContatoViewModel>>(await _contatosService.ListarContatos());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ContatoViewModel>> ListarContatoId(Guid id)
        {
            var contato = await _contatosService.ListarContatoId(id);
            if (contato == null) return NotFound();

            return Ok(_mapper.Map<ContatoViewModel>(contato));
        }

        [HttpPost]
        public async Task<ActionResult<ContatoViewModel>> AdicionarContato([FromBody] ContatoViewModel contatoViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var contato = _mapper.Map<Contato>(contatoViewModel);
            var resultado = await _contatosService.AdicionarContato(contato);

            return Ok(_mapper.Map<ContatoViewModel>(resultado));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ContatoViewModel>> AtualizarContato(Guid id, [FromBody] ContatoViewModel contatoViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var contatoExistente = await _contatosService.ListarContatoId(id);
            if (contatoExistente == null) return NotFound();

            contatoExistente.Atualizar(contatoViewModel.PrimeiroNome, contatoViewModel.Sobrenome, contatoViewModel.DataNascimento, contatoViewModel.Sexo);
            var resultado = await _contatosService.AtualizarContato(contatoExistente);

            return Ok(_mapper.Map<ContatoViewModel>(resultado));
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<ContatoViewModel>> AtualizarContatoAtivo(Guid id)
        {
            var contato = await _contatosService.ListarContatoId(id);
            if (contato == null) return NotFound();
            
            contato.AlternarStatusAtivo();
            var resultado = await _contatosService.AtualizarContatoAtivo(contato);

            return Ok(_mapper.Map<ContatoViewModel>(resultado));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<bool>> DeletarContato(Guid id)
        {
            return Ok(await _contatosService.DeletarContato(id));
        }
    }
}
