using API.entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtlisateursController : ControllerBase
    {
        private readonly Data _context;
        public UtlisateursController(Data context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utlisateurs>>> getutlisateurs()
        {
           return await _context.utlisateurs.ToListAsync();
        }
        // pour chercher un utilsateur par son id
        [HttpGet("{id}")]
        public async Task<ActionResult<Utlisateurs>> getutlisateur(int id)
        {
          
            return await _context.utlisateurs.FindAsync(id);
        }

    }
}
