using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularTourOfHeroesApiDemo.Models;

namespace AngularTourOfHeroesApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly AngularTourOfHeroesContext _context;

        public HeroesController(AngularTourOfHeroesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Hero>>> GetHeroes()
        {
            var data = await _context.Hero.ToListAsync();
            return data;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hero>> GetHeroById(int id)
        {
            var hero = await _context.Hero.FindAsync(id);

            if (hero == null)
            {
                return NotFound();
            }

            return hero;
        }

        [HttpPost]
        public async Task<ActionResult<Hero>> AddHero(Hero newHero)
        {
            var lastHero = await _context.Hero.OrderByDescending(d => d.Id).FirstOrDefaultAsync();
            if (lastHero == null)
            {
                newHero.Id = 1;
            }
            else
            {
                newHero.Id = lastHero.Id + 1;
            }

            await _context.Hero.AddAsync(newHero);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetHeroById), new {id = newHero.Id},newHero);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateHero(int id,Hero hero)
        {
            if (id != hero.Id)
            {
                return BadRequest();
            }
            
            var originalHero = await _context.Hero.FindAsync(hero.Id);
            if (originalHero == null)
            {
                return NotFound();
            }

            originalHero.Name = hero.Name;
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
