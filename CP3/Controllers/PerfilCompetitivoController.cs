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
public class PerfilCompetitivoController : ControllerBase
{
    private readonly ApplicationContext _context;

    public PerfilCompetitivoController(ApplicationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna todos os perfis competitivos com seus jogadores.
    /// </summary>
    [HttpGet]
    [SwaggerResponse(200, "Perfis competitivos encontrados com sucesso")]
    [SwaggerResponse(204, "Nenhum perfil competitivo encontrado")]
    public async Task<IActionResult> GetAll()
    {
        var perfis = await _context.PerfisCompetitivos
            .Include(p => p.Jogador)
            .ThenInclude(j => j!.Time)
            .ToListAsync();

        if (!perfis.Any())
            return NoContent();

        var response = perfis.Select(p => new PerfilCompetitivoResponseDto
        {
            Id = p.Id,
            KDA = p.KDA,
            WinRate = p.WinRate,
            HorasJogadas = p.HorasJogadas,
            JogadorId = p.JogadorId,
            Jogador = p.Jogador == null ? null : new JogadorResumoDto
            {
                Id = p.Jogador.Id,
                Nickname = p.Jogador.Nickname,
                Funcao = p.Jogador.Funcao,
                Idade = p.Jogador.Idade,
                Time = p.Jogador.Time == null ? null : new TimeResumoDto
                {
                    Id = p.Jogador.Time.Id,
                    Nome = p.Jogador.Time.Nome,
                    Jogo = p.Jogador.Time.Jogo,
                    Pais = p.Jogador.Time.Pais,
                    Ranking = p.Jogador.Time.Ranking
                }
            }
        });

        return Ok(response);
    }

    /// <summary>
    /// Retorna um perfil competitivo pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerResponse(200, "Perfil competitivo encontrado com sucesso")]
    [SwaggerResponse(404, "Perfil competitivo não encontrado")]
    public async Task<IActionResult> GetById(int id)
    {
        var perfil = await _context.PerfisCompetitivos
            .Include(p => p.Jogador)
            .ThenInclude(j => j!.Time)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (perfil == null)
            return NotFound();

        var response = new PerfilCompetitivoResponseDto
        {
            Id = perfil.Id,
            KDA = perfil.KDA,
            WinRate = perfil.WinRate,
            HorasJogadas = perfil.HorasJogadas,
            JogadorId = perfil.JogadorId,
            Jogador = perfil.Jogador == null ? null : new JogadorResumoDto
            {
                Id = perfil.Jogador.Id,
                Nickname = perfil.Jogador.Nickname,
                Funcao = perfil.Jogador.Funcao,
                Idade = perfil.Jogador.Idade,
                Time = perfil.Jogador.Time == null ? null : new TimeResumoDto
                {
                    Id = perfil.Jogador.Time.Id,
                    Nome = perfil.Jogador.Time.Nome,
                    Jogo = perfil.Jogador.Time.Jogo,
                    Pais = perfil.Jogador.Time.Pais,
                    Ranking = perfil.Jogador.Time.Ranking
                }
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Retorna o perfil competitivo de um jogador.
    /// </summary>
    [HttpGet("jogador/{jogadorId}")]
    [SwaggerResponse(200, "Perfil competitivo encontrado com sucesso")]
    [SwaggerResponse(404, "Perfil competitivo não encontrado para este jogador")]
    public async Task<IActionResult> GetByJogador(int jogadorId)
    {
        var perfil = await _context.PerfisCompetitivos
            .Include(p => p.Jogador)
            .ThenInclude(j => j!.Time)
            .FirstOrDefaultAsync(p => p.JogadorId == jogadorId);

        if (perfil == null)
            return NotFound();

        var response = new PerfilCompetitivoResponseDto
        {
            Id = perfil.Id,
            KDA = perfil.KDA,
            WinRate = perfil.WinRate,
            HorasJogadas = perfil.HorasJogadas,
            JogadorId = perfil.JogadorId,
            Jogador = perfil.Jogador == null ? null : new JogadorResumoDto
            {
                Id = perfil.Jogador.Id,
                Nickname = perfil.Jogador.Nickname,
                Funcao = perfil.Jogador.Funcao,
                Idade = perfil.Jogador.Idade,
                Time = perfil.Jogador.Time == null ? null : new TimeResumoDto
                {
                    Id = perfil.Jogador.Time.Id,
                    Nome = perfil.Jogador.Time.Nome,
                    Jogo = perfil.Jogador.Time.Jogo,
                    Pais = perfil.Jogador.Time.Pais,
                    Ranking = perfil.Jogador.Time.Ranking
                }
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Cadastra um perfil competitivo para um jogador.
    /// </summary>
    [HttpPost]
    [SwaggerResponse(201, "Perfil competitivo criado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou jogador já possui perfil")]
    [SwaggerResponse(404, "Jogador não encontrado")]
    public async Task<IActionResult> Create(PerfilCompetitivoCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var jogador = await _context.Jogadores.FirstOrDefaultAsync(j => j.Id == dto.JogadorId);

        if (jogador == null)
            return NotFound("Jogador não encontrado.");

        var perfilExistente = await _context.PerfisCompetitivos
            .FirstOrDefaultAsync(p => p.JogadorId == dto.JogadorId);

        if (perfilExistente != null)
            return BadRequest("Este jogador já possui perfil competitivo.");

        var perfil = new PerfilCompetitivo
        {
            KDA = dto.KDA,
            WinRate = dto.WinRate,
            HorasJogadas = dto.HorasJogadas,
            JogadorId = dto.JogadorId
        };

        _context.PerfisCompetitivos.Add(perfil);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = perfil.Id }, perfil);
    }

    /// <summary>
    /// Atualiza um perfil competitivo existente.
    /// </summary>
    [HttpPut("{id}")]
    [SwaggerResponse(200, "Perfil competitivo atualizado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(404, "Perfil competitivo ou jogador não encontrado")]
    public async Task<IActionResult> Update(int id, PerfilCompetitivoCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var perfil = await _context.PerfisCompetitivos.FindAsync(id);

        if (perfil == null)
            return NotFound("Perfil competitivo não encontrado.");

        var jogador = await _context.Jogadores.FirstOrDefaultAsync(j => j.Id == dto.JogadorId);

        if (jogador == null)
            return NotFound("Jogador não encontrado.");

        perfil.KDA = dto.KDA;
        perfil.WinRate = dto.WinRate;
        perfil.HorasJogadas = dto.HorasJogadas;
        perfil.JogadorId = dto.JogadorId;

        await _context.SaveChangesAsync();

        return Ok(perfil);
    }

    /// <summary>
    /// Remove um perfil competitivo pelo ID.
    /// </summary>
    [HttpDelete("{id}")]
    [SwaggerResponse(204, "Perfil competitivo removido com sucesso")]
    [SwaggerResponse(404, "Perfil competitivo não encontrado")]
    public async Task<IActionResult> Delete(int id)
    {
        var perfil = await _context.PerfisCompetitivos.FindAsync(id);

        if (perfil == null)
            return NotFound();

        _context.PerfisCompetitivos.Remove(perfil);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}