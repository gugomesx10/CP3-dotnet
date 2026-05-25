using CP3.Data;
using CP3.DTOs;
using CP3.Entities;
using CP3.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace CP3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JogadorController : ControllerBase
{
    private readonly ApplicationContext _context;

    public JogadorController(ApplicationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna todos os jogadores cadastrados com seus times e perfis competitivos.
    /// </summary>
    [HttpGet]
    [SwaggerResponse(200, "Jogadores encontrados com sucesso")]
    [SwaggerResponse(204, "Nenhum jogador encontrado")]
    public async Task<IActionResult> GetAll()
    {
        var jogadores = await _context.Jogadores
            .Include(j => j.Time)
            .ToListAsync();

        if (!jogadores.Any())
            return NoContent();

        var response = jogadores.Select(j => new JogadorResumoDto
        {
            Id = j.Id,
            Nickname = j.Nickname,
            Funcao = j.Funcao,
            Idade = j.Idade,
            Time = j.Time == null ? null : new TimeResumoDto
            {
                Id = j.Time.Id,
                Nome = j.Time.Nome,
                Jogo = j.Time.Jogo,
                Pais = j.Time.Pais,
                Ranking = j.Time.Ranking
            }
        });

        return Ok(response);
    }

    /// <summary>
    /// Retorna um jogador pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerResponse(200, "Jogador encontrado com sucesso")]
    [SwaggerResponse(404, "Jogador não encontrado")]
    public async Task<IActionResult> GetById(int id)
    {
        var jogador = await _context.Jogadores
            .Include(j => j.Time)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (jogador == null)
            return NotFound();

        var response = new JogadorResumoDto
        {
            Id = jogador.Id,
            Nickname = jogador.Nickname,
            Funcao = jogador.Funcao,
            Idade = jogador.Idade,
            Time = jogador.Time == null ? null : new TimeResumoDto
            {
                Id = jogador.Time.Id,
                Nome = jogador.Time.Nome,
                Jogo = jogador.Time.Jogo,
                Pais = jogador.Time.Pais,
                Ranking = jogador.Time.Ranking
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Retorna jogadores filtrados pelo ID do time.
    /// </summary>
    [HttpGet("time/{timeId}")]
    [SwaggerResponse(200, "Jogadores encontrados com sucesso")]
    [SwaggerResponse(204, "Nenhum jogador encontrado para este time")]
    public async Task<IActionResult> GetByTime(int timeId)
    {
        var jogadores = await _context.Jogadores
            .Include(j => j.Time)
            .Where(j => j.TimeId == timeId)
            .ToListAsync();

        if (!jogadores.Any())
            return NoContent();

        var response = jogadores.Select(j => new JogadorResumoDto
        {
            Id = j.Id,
            Nickname = j.Nickname,
            Funcao = j.Funcao,
            Idade = j.Idade,
            Time = j.Time == null ? null : new TimeResumoDto
            {
                Id = j.Time.Id,
                Nome = j.Time.Nome,
                Jogo = j.Time.Jogo,
                Pais = j.Time.Pais,
                Ranking = j.Time.Ranking
            }
        });

        return Ok(response);
    }

    /// <summary>
    /// Retorna jogadores filtrados pela função.
    /// </summary>
    [HttpGet("funcao/{funcao}")]
    [SwaggerResponse(200, "Jogadores encontrados com sucesso")]
    [SwaggerResponse(204, "Nenhum jogador encontrado para esta função")]
    public async Task<IActionResult> GetByFuncao(string funcao)
    {
        var jogadores = await _context.Jogadores
            .Include(j => j.Time)
            .Where(j => j.Funcao.ToLower() == funcao.ToLower())
            .ToListAsync();

        if (!jogadores.Any())
            return NoContent();

        var response = jogadores.Select(j => new JogadorResumoDto
        {
            Id = j.Id,
            Nickname = j.Nickname,
            Funcao = j.Funcao,
            Idade = j.Idade,
            Time = j.Time == null ? null : new TimeResumoDto
            {
                Id = j.Time.Id,
                Nome = j.Time.Nome,
                Jogo = j.Time.Jogo,
                Pais = j.Time.Pais,
                Ranking = j.Time.Ranking
            }
        });

        return Ok(response);
    }

    /// <summary>
    /// Cadastra um novo jogador vinculado a um time.
    /// </summary>
    [HttpPost]
    [SwaggerResponse(201, "Jogador criado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(404, "Time não encontrado")]
    public async Task<IActionResult> Create(JogadorCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var time = await _context.Times.FirstOrDefaultAsync(t => t.Id == dto.TimeId);

        if (time == null)
            return NotFound("Time não encontrado.");

        var jogador = new Jogador
        {
            Nickname = dto.Nickname,
            Funcao = dto.Funcao,
            Idade = dto.Idade,
            TimeId = dto.TimeId
        };

        _context.Jogadores.Add(jogador);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = jogador.Id }, jogador);
    }

    /// <summary>
    /// Atualiza um jogador existente.
    /// </summary>
    [HttpPut("{id}")]
    [SwaggerResponse(200, "Jogador atualizado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(404, "Jogador ou time não encontrado")]
    public async Task<IActionResult> Update(int id, JogadorCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var jogador = await _context.Jogadores.FindAsync(id);

        if (jogador == null)
            return NotFound("Jogador não encontrado.");

        var time = await _context.Times.FirstOrDefaultAsync(t => t.Id == dto.TimeId);

        if (time == null)
            return NotFound("Time não encontrado.");

        jogador.Nickname = dto.Nickname;
        jogador.Funcao = dto.Funcao;
        jogador.Idade = dto.Idade;
        jogador.TimeId = dto.TimeId;

        await _context.SaveChangesAsync();

        return Ok(jogador);
    }

    /// <summary>
    /// Remove um jogador pelo ID.
    /// </summary>
    [HttpDelete("{id}")]
    [SwaggerResponse(204, "Jogador removido com sucesso")]
    [SwaggerResponse(404, "Jogador não encontrado")]
    public async Task<IActionResult> Delete(int id)
    {
        var jogador = await _context.Jogadores.FindAsync(id);

        if (jogador == null)
            return NotFound();

        _context.Jogadores.Remove(jogador);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}