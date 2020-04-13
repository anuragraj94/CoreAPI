using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Added this 
    [EnableCors("MyPolicy")]
    public class HomeController : ControllerBase
    {
        private readonly DoctorDBContex _context;

        public HomeController(DoctorDBContex context)
        {
            _context = context;
        }

        // GET: api/Home
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandidateModel>>> GetCandidateModels()
        {
            return await _context.CandidateModels.ToListAsync();
        }

        // GET: api/Home/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CandidateModel>> GetCandidateModel(int id)
        {
            var candidateModel = await _context.CandidateModels.FindAsync(id);

            if (candidateModel == null)
            {
                return NotFound();
            }

            return candidateModel;
        }

        // PUT: api/Home/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCandidateModel(int id, CandidateModel candidateModel)
        {
            candidateModel.ID = id;

            _context.Entry(candidateModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidateModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Home
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CandidateModel>> PostCandidateModel(CandidateModel candidateModel)
        {
            _context.CandidateModels.Add(candidateModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCandidateModel", new { id = candidateModel.ID }, candidateModel);
        }

        // DELETE: api/Home/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CandidateModel>> DeleteCandidateModel(int id)
        {
            var candidateModel = await _context.CandidateModels.FindAsync(id);
            if (candidateModel == null)
            {
                return NotFound();
            }

            _context.CandidateModels.Remove(candidateModel);
            await _context.SaveChangesAsync();

            return candidateModel;
        }

        private bool CandidateModelExists(int id)
        {
            return _context.CandidateModels.Any(e => e.ID == id);
        }
    }
}
