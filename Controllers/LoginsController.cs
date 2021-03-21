using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProvaApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly provaContext _context;

        public LoginsController(provaContext context)
        {
            _context = context;
        }

        // GET: api/logins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Login>>> GetLogins()
        {
			// Return Status200OK
            return Ok(await _context.Logins.ToListAsync());
        }

        // GET: api/logins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Login>> GetLogin(long id)
        {
            var login = await _context.Logins.FindAsync(id);

            if (login == null)
            {
                return NotFound();
            }

            return login;
        }

        // PUT: api/logins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogin(long id, Login login)
        {
            if (id != login.Id)
            {
                return BadRequest();
            }

            _context.Entry(login).State = EntityState.Modified;

            var user = _context.Logins.Find(id);
            user.Email = login.Email;
            user.Senha = login.Senha;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginExists(id))
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

		// POST: api/logins
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<string> PostLogin(Login login)
        {
            var user = _context.Logins.Where(user => user.Email.Equals(login.Email) && user.Senha.Equals(login.Senha)).FirstOrDefault();
            var message = "";
            if (user == null) 
            {
                message = "Login inválido";
			}
			else 
            {
                message = "Login válido";
            }
            return Ok(new LoginResult(message)); ;
        }

        //POST: api/logins/register
        [HttpPost("register", Name="Login Register")]
        public async Task<ActionResult<Login>> CreateLogin(Login login)
		{
            var userAlreadyExist = 
                _context.Logins.Where(user => user.Email.Equals(login.Email)).FirstOrDefault() == null? false : true;
			if (userAlreadyExist)
			{
                return Ok("User already exist in database");
			}
			else
			{
                _context.Logins.Add(login);
                await _context.SaveChangesAsync();
                return Ok(login);
			}
        }

        // DELETE: api/logins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogin(long id)
        {
            var login = await _context.Logins.FindAsync(id);
            if (login == null)
            {
                return NotFound();
            }

            _context.Logins.Remove(login);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoginExists(long id)
        {
            return _context.Logins.Any(e => e.Id == id);
        }
    }
}
