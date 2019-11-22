using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Connect4.Data;
using Connect4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect4.Controllers
{
    /// <summary>
    /// Controlador para o ModelJogos.
    /// Opção para entrar no Lobby, criar jogos e excluir Jogos.
    /// </summary>
    public class JogoController : Controller
    {
        private ApplicationDbContext _context { get; set; }

        private UserManager<ApplicationUser> _userManager { get; set; }

        private SignInManager<ApplicationUser> _signInManager { get; set; }


        public JogoController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            ApplicationDbContext dbContext)
        {
            this._context = dbContext;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tabuleiro(int id)
        {
            var jogo = _context.Jogos
                            .Include(j => j.Jogador1)
                            .Include(j => j.Jogador2)
                            .Include(j => j.Tabuleiro)
                            .Where(j => j.Id == id)
                            .Select(j => j)
                            .FirstOrDefault();

            if (jogo == null)
            {
                return NotFound();
            }
            if (jogo.Tabuleiro == null)
            {
                jogo.Tabuleiro = new Tabuleiro();
                _context.SaveChanges();
            }
            return View(jogo);
        }

        [Authorize]
        public IActionResult Lobby(int id)
        {
            var jogo = _context.Jogos
                .Include(j => j.Jogador1)
                .Include(j => j.Jogador2)
                .Where(j => j.Id == id)
                .Select(j => j)
                .FirstOrDefault();

            if (jogo == null)
            {
                return NotFound();
            }

            if (jogo.Jogador1 is JogadorPessoa)
            {
                jogo.Jogador1 = _context.JogadorPessoas
                                .Include(j => j.Usuario)
                                .Where(j => j.Id == jogo.Jogador1Id)
                                .FirstOrDefault();
            }
            if (jogo.Jogador2 is JogadorPessoa)
            {
                jogo.Jogador2 = _context.JogadorPessoas
                                .Include(j => j.Usuario)
                                .Where(j => j.Id == jogo.Jogador2Id)
                                .FirstOrDefault();
            }
            int? jogadorId =
            _userManager.GetUserAsync(User).Result.JogadorId;
            if(!(jogadorId == jogo.Jogador1Id || 
                jogadorId == jogo.Jogador2Id)){
                return Forbid();
            }
            return View(jogo);
        }

        /// <summary>
        /// Ação para criar o jogo.
        /// Irá verificar se existe algum jogo disponível sem 
        /// todos os jogadores. Caso não exista irá criar um
        /// novo jogo.
        /// </summary>
        /// <returns>Redireciona o usuário para o Lobby.</returns>
        public IActionResult CriarJogo()
        {
            Jogo jogo;
            int? jogadorId =
                _userManager.GetUserAsync(User).Result.JogadorId;
            if(jogadorId == null)
            {
                throw new ApplicationException("O usuário atual não é um jogador.");
            }
            var jogadorAtual = _context.JogadorPessoas.Find(jogadorId);
            if (jogadorAtual == null || jogadorAtual.Id == 0)
            {
                return NotFound();
            }
            //Verificar se existe jogo com um jogador
            jogo = (from item in _context.Jogos.Include(j=> j.Jogador1)
                                               .Include(j => j.Jogador2)
                    where (item.Jogador1 == null ||
                         item.Jogador2 == null) &&
                         (item.Jogador1 != jogadorAtual &&
                         item.Jogador2!= jogadorAtual)
                    select item).FirstOrDefault();
            if (jogo != null) {
                if (jogo.Jogador1 == null)
                {
                    jogo.Jogador1 = jogadorAtual;
                }else if(jogo.Jogador2 == null)
                {
                    jogo.Jogador2 = jogadorAtual;
                }
            }
            //Caso contrário
            else {
                jogo = new Jogo();
                jogo.Jogador1 = jogadorAtual;
                _context.Add(jogo);
            }
            _context.SaveChanges();
            //Redirecionar para Lobby
            return RedirectToAction(nameof(Lobby), 
                new { id = jogo.Id });
        }
    }
}