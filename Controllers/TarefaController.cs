using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Tar;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController(OrganizadorContext context) : ControllerBase
    {
        private readonly OrganizadorContext _context = context;

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            Tarefa? tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return NotFound(new { Erro = "Registro não encontrado." });

            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.ToList();

            if (tarefas.Count == 0)
                return NotFound(new { Erro = "Nenhum registro encontrado." });

            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefas = _context.Tarefas.Where(x => x.Titulo == titulo).ToList();

            if (tarefas.Count == 0)
                return NotFound(new { Erro = "Nenhum registro encontrado." });

            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefas = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();

            if (tarefas.Count == 0)
                return NotFound(new { Erro = "Nenhum registro encontrado." });

            return Ok(tarefas);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefas = _context.Tarefas.Where(x => x.Status == status).ToList();

            if (tarefas.Count == 0)
                return NotFound(new { Erro = "Nenhum registro encontrado." });

            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            if (tarefa == null)
                return BadRequest(new {Erro = "O registro não foi encontrado."});

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            Tarefa? tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound(new { Erro = "Tarefa não encontrada." });

            if (tarefa == null)
                return BadRequest(new { Erro = "A tarefa informada não pode ser nula." });

            if (tarefa.Id != tarefaBanco.Id)
                return BadRequest(new { Erro = "As informações de identificação não são as mesmas."});

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia." });

            tarefaBanco.Id = tarefa.Id;
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return Ok(tarefa);
        }

        /*
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (id != tarefa.Id || tarefa == null)
                return BadRequest(new {Erro = "A tarefa informada não corresponde ao identificador fornecido para modificação."});

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Entry(tarefa).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(tarefa);
        }
        */

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound($"Registro {id} não encontrado.");

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
