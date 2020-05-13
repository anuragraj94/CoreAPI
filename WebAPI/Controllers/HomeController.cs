using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private IConfiguration _config;

        public HomeController(DoctorDBContex context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        //http://localhost:64275/api/home/login?username=abc&pass=123
        [HttpGet(template:"login", Name ="login_Get")]
         public IActionResult Login(string username,string pass)
         {
             UserModel login = new UserModel();
             login.UserName = username;
             login.Password = pass;
             IActionResult response = Unauthorized();

             var user = Authenticateuser(login);

             if (user!=null)
             {
                 var tokenStr = GenerateJSONWebToken(user);
                 response = Ok(new { tokenStr });
             }
             return response;
         }
         private UserModel Authenticateuser(UserModel login)
         {
             UserModel user = null;
             if (login.UserName=="abc"&& login.Password=="123")
             {
                 user = new UserModel { UserName = "ABC", Password = "123" };
             }
             return user;
         }
         private string GenerateJSONWebToken(UserModel userinfo)
         {
             var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
             var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

             var claims = new[]
             {
                 new Claim(JwtRegisteredClaimNames.Sub,userinfo.UserName),
                 new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
             };

             var token = new JwtSecurityToken(
                 issuer: _config["Jwt:Issuer"],
                 audience: _config["Jwt:Issuer"],
                 claims,
                 expires: DateTime.Now.AddMinutes(120),
                 signingCredentials: credentials);

             var encodedtoken = new JwtSecurityTokenHandler().WriteToken(token);
             return encodedtoken;
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
