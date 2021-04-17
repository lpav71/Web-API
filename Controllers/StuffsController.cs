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
    public class StuffsController : ControllerBase
    {
        private readonly WebAPIContext _context;

        public StuffsController(WebAPIContext context)
        {
            _context = context;
            if (!_context.Stuff.Any())
            {
                _context.Stuff.Add(new Stuff { FirstName = "Иван", LastName = "Иванов", CreatedDateTime = DateTime.Now, DivisionId = 1 });
                _context.Stuff.Add(new Stuff { FirstName = "Яна", LastName = "Константинова", CreatedDateTime = DateTime.Now, DivisionId = 1 });
                _context.Stuff.Add(new Stuff { FirstName = "Петр", LastName = "Васильев", CreatedDateTime = DateTime.Now, DivisionId = 2 });
                _context.Stuff.Add(new Stuff { FirstName = "Мария", LastName = "Стронцева", CreatedDateTime = DateTime.Now, DivisionId = 2 });

                _context.Division.Add(new Division { Name = "Краснодар", CreatedDateTime = DateTime.Now });
                _context.Division.Add(new Division { Name = "Москва", CreatedDateTime = DateTime.Now });

                _context.SaveChanges();
            }
        }

        // GET api/stuffs/page/1
        [Route("/api/stuffs/page/{page}")]
        [HttpGet]
        public ActionResult Get(int page = 1)
        {
            int pageSize = 2;
            var stuff = _context.Stuff.Include(x => x.Division);
            var count = stuff.Count();
            var items = stuff.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            int w = pageViewModel.TotalPages;
            IndexViewModelStuff viewModel = new IndexViewModelStuff
            {
                PageViewModel = pageViewModel,
                Stuff = items
            };
            return new OkObjectResult(viewModel);
        }

        // GET: api/Stuffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stuff>>> GetStuff()
        {
            return await _context.Stuff.ToListAsync();
        }

        // GET: api/Stuffs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Stuff>> GetStuff(int id)
        {
            var stuff = await _context.Stuff.FindAsync(id);

            if (stuff == null)
            {
                return NotFound();
            }

            return stuff;
        }

        // GET api/stuffs/lastname/Иванов
        [Route("/api/stuffs/lastname/{LastName}/page/{page}")]
        [HttpGet]
        public ActionResult LastName(string LastName, int page)
        {
            int pageSize = 2;
            var sr = _context.Stuff.Where(p => EF.Functions.Like(p.FirstName, "%" + LastName + "%"));
            // Можно и так:
            /*var sr = from c in _context.Realtors
                     where c.LastName.Contains(LastName)
                     select c;*/
            var count = sr.Count();
            var items = sr.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            int w = pageViewModel.TotalPages;
            IndexViewModelStuff viewModel = new IndexViewModelStuff
            {
                PageViewModel = pageViewModel,
                Stuff = items
            };

            return new OkObjectResult(viewModel);
        }

        // GET api/stuffs/division/Краснодар
        [Route("/api/stuffs/division/{division}")]
        [HttpGet]
        public ActionResult Division(string division)
        {
            Division divs = _context.Division.FirstOrDefault(d => d.Name == division);
            IQueryable realtor = _context.Stuff.Where(x => x.DivisionId == divs.Id).Select(x => new {
                x.FirstName,
                x.LastName,
                x.CreatedDateTime,
                x.Division.Name
            });
            return new OkObjectResult(realtor);
        }

        // PATCH: api/Stuffs/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchStuff(int id, Stuff stuff)
        {
            Stuff r = await _context.Stuff.FindAsync(id);
            if (r == null)
            {
                return BadRequest();
            }
            if (stuff.LastName != null ) r.LastName = stuff.LastName;
            if(stuff.FirstName != null) r.FirstName = stuff.FirstName;
            if (stuff.DivisionId != 0) r.DivisionId = stuff.DivisionId;
            _context.Entry(r).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StuffExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new OkObjectResult(r);
        }

        // PUT: api/Stuffs
        [HttpPut]
        public async Task<IActionResult> PutStuff(Stuff stuff)
        {
            Stuff r = await _context.Stuff.FindAsync(stuff.Id);
            if (r == null | stuff.FirstName == null | stuff.LastName == null | stuff.DivisionId == 0 )
            {
                return BadRequest();
            }
            r.LastName = stuff.LastName;
            r.FirstName = stuff.FirstName;
            r.DivisionId = stuff.DivisionId;
            _context.Entry(r).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StuffExists(stuff.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new OkObjectResult(r);
        }

        // POST: api/Stuffs
        [HttpPost]
        public async Task<ActionResult<Stuff>> PostStuff(Stuff stuff)
        {
            stuff.CreatedDateTime = DateTime.Now;
            _context.Stuff.Add(stuff);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStuff", new { id = stuff.Id }, stuff);
        }

        // DELETE: api/Stuffs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Stuff>> DeleteStuff(int id)
        {
            var stuff = await _context.Stuff.FindAsync(id);
            if (stuff == null)
            {
                return NotFound();
            }

            _context.Stuff.Remove(stuff);
            await _context.SaveChangesAsync();

            return stuff;
        }

        private bool StuffExists(int id)
        {
            return _context.Stuff.Any(e => e.Id == id);
        }
    }
}
