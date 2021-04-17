using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : ControllerBase
    {
        private readonly WebAPIContext _context;

        public DivisionsController(WebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Divisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Division>>> GetDivision()
        {
            return await _context.Division.ToListAsync();
        }

        // GET api/divisions/page/{page}
        [Route("/api/divisions/page/{page}")]
        [HttpGet]
        public ActionResult Get(int page)
        {
            int pageSize = 2;
            var division = _context.Division;
            var count = division.Count();
            var items = division.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModelDivision viewModel = new IndexViewModelDivision
            {
                PageViewModel = pageViewModel,
                Division = items
            };
            return new OkObjectResult(viewModel);
        }

        // GET: api/Divisions/5
        [Route("/api/divisions/{id}")]
        [HttpGet("{id:int}")]
        public ActionResult GetDivision(int id)
        {
            Division division = _context.Division.FirstOrDefault(x => x.Id == id);
            return new OkObjectResult(division);
        }



        // PUT: api/Divisions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDivision(int id, Division division)
        {
            Division d = await _context.Division.FindAsync(id);
            if (d == null | division.Name == null)
            {
                return BadRequest();
            }

            d.Name = division.Name;
            _context.Entry(d).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DivisionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new OkObjectResult(d);
        }

        // POST: api/Divisions
        [HttpPost]
        public async Task<ActionResult<Division>> PostDivision(Division division)
        {
            division.CreatedDateTime = DateTime.Now;
            _context.Division.Add(division);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDivision", new { id = division.Id }, division);
        }

        // DELETE: api/Divisions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Division>> DeleteDivision(int id)
        {
            var division = await _context.Division.FindAsync(id);
            if (division == null)
            {
                return NotFound();
            }

            _context.Division.Remove(division);
            await _context.SaveChangesAsync();

            return division;
        }

        private bool DivisionExists(int id)
        {
            return _context.Division.Any(e => e.Id == id);
        }
    }
}
