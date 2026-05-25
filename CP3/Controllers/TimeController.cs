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
public class TimeController : ControllerBase
{
    private readonly ApplicationContext _context;

    public TimeController(ApplicationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna todos os times cadastrados com seus jogadores.
    /// </summary>
    [HttpGet]
    [SwaggerResponse(200, "Times encontrados com sucesso")]
    [SwaggerResponse(204, "Nenhum time encontrado")]
    public async Task<IActionResult> GetAll()
    {
        var times = await _context.Times.ToListAsync();

        if (!times.Any())
            return NoContent();

        var response = times.Select(t => new TimeResumoDto
        {
            Id = t.Id,
            Nome = t.Nome,
            Jogo = t.Jogo,
            Pais = t.Pais,
            Ranking = t.Ranking
        });

        return Ok(response);
    }

    /// <summary>
    /// Retorna um time pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerResponse(200, "Time encontrado com sucesso")]
    [SwaggerResponse(404, "Time não encontrado")]
    public async Task<IActionResult> GetById(int id)
    {
        var time = await _context.Times
            .FirstOrDefaultAsync(t => t.Id == id);

        if (time == null)
            return NotFound();

        var response = new TimeResumoDto
        {
            Id = time.Id,
            Nome = time.Nome,
            Jogo = time.Jogo,
            Pais = time.Pais,
            Ranking = time.Ranking
        };

        return Ok(response);
    }

    /// <summary>
    /// Retorna times filtrados pelo jogo.
    /// </summary>
    [HttpGet("jogo/{jogo}")]
    [SwaggerResponse(200, "Times encontrados com sucesso")]
    [SwaggerResponse(204, "Nenhum time encontrado para este jogo")]
    public async Task<IActionResult> GetByJogo(string jogo)
    {
        var times = await _context.Times
            .Where(t => t.Jogo.ToLower() == jogo.ToLower())
            .ToListAsync();

        if (!times.Any())
            return NoContent();

        var response = times.Select(t => new TimeResumoDto
        {
            Id = t.Id,
            Nome = t.Nome,
            Jogo = t.Jogo,
            Pais = t.Pais,
            Ranking = t.Ranking
        });

        return Ok(response);
    }

    /// <summary>
    /// Cadastra um novo time.
    /// </summary>
    [HttpPost]
    [SwaggerResponse(201, "Time criado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    public async Task<IActionResult> Create(TimeCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var time = new Time
        {
            Nome = dto.Nome,
            Jogo = dto.Jogo,
            Pais = dto.Pais,
            Ranking = dto.Ranking
        };

        _context.Times.Add(time);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = time.Id }, time);
    }

    /// <summary>
    /// Atualiza um time existente.
    /// </summary>
    [HttpPut("{id}")]
    [SwaggerResponse(200, "Time atualizado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(404, "Time não encontrado")]
    public async Task<IActionResult> Update(int id, TimeCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var time = await _context.Times.FindAsync(id);

        if (time == null)
            return NotFound();

        time.Nome = dto.Nome;
        time.Jogo = dto.Jogo;
        time.Pais = dto.Pais;
        time.Ranking = dto.Ranking;

        await _context.SaveChangesAsync();

        return Ok(time);
    }

    /// <summary>
    /// Remove um time pelo ID.
    /// </summary>
    [HttpDelete("{id}")]
    [SwaggerResponse(204, "Time removido com sucesso")]
    [SwaggerResponse(404, "Time não encontrado")]
    public async Task<IActionResult> Delete(int id)
    {
        var time = await _context.Times.FindAsync(id);

        if (time == null)
            return NotFound();

        _context.Times.Remove(time);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}