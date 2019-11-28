using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Connect4.Data;
using Connect4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Connect4.Controllers
{
    [Produces("application/json")]
    [Route("api/Jogo")]
    public class JogoAPIController : Controller
    {
        private UserManager<ApplicationUser> _userManager { get; set; }
        private SignInManager<ApplicationUser> _signInManager { get; set; }
        private ILogger<ManageController> _logger { get; set; }

        private ApplicationDbContext _context { get; set; }
        public JogoAPIController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          ILogger<ManageController> logger,
          ApplicationDbContext context
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;           
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "Obter")]
        [Route("Obter")]
        [Authorize]
        public Tabuleiro ObterJogo()
        {
            Tabuleiro t = null;
            try
            {
                t = new Tabuleiro();
                _context.Tabuleiros.Add(t);
                _context.SaveChanges();
            }catch(Exception e)
            {
                _logger.LogCritical(e, e.Message, null);
            }
            return t;
        }


        [HttpGet(Name = "Obter")]
        [Route("Obter/{id}")]
        [Authorize]
        public Tabuleiro ObterJogo(int id)
        {
            var jogo = _context.Jogos
                .Include(j => j.Tabuleiro)
                .Where(j => j.Id == id)
                .FirstOrDefault();

            if (jogo == null)
            {
                throw new ApplicationException("Não Existe o Jogo");
            }
            //TODO: Verificar Permissão antes.
            if (jogo.Tabuleiro != null)
            {                
                return jogo.Tabuleiro;
            }
            jogo.Tabuleiro = new Tabuleiro();
            _context.SaveChanges();
            return jogo.Tabuleiro;
        }

        [HttpPost(Name = "Vencedor")]
        [Route("Vencedor")]
        public int Vencedor(Tabuleiro t)
        {
            return t.Vencedor();
        }

        [HttpPost(Name = "Jogar")]
        [Route("Jogar")]
        //(...)/Jogar?JogoId=1&Pos=4
        public IActionResult Jogar([FromQuery] int JogoId, 
            [FromQuery]int Pos)
        {
            var jogo = _context.Jogos
                .Include(j => j.Tabuleiro)
                .Where(j => j.Id == JogoId)
                .FirstOrDefault();
            if(jogo == null)
            {
                return NotFound();
            }
            if(jogo.Tabuleiro == null)
            {
                return BadRequest();
            }
            ApplicationUser applicationUser = _userManager.GetUserAsync (User).Result;
            if ((applicationUser.JogadorId == jogo.Jogador1Id && jogo.Tabuleiro.Turno == 1) || (applicationUser.JogadorId == jogo.Jogador2Id && jogo.Tabuleiro.Turno == 2))
            {
                jogo.Tabuleiro.Jogar (jogo.Tabuleiro.Turno, Pos);
                _context.SaveChanges ();
            }
            else
            {
                throw new ApplicationException ("Não é a sua vez de jogar");
            }
            return Ok(jogo.Tabuleiro);
        }
    }
}