using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Connect4.Data;
using Connect4.Models;
using Microsoft.AspNetCore.Authorization;

namespace Connect4.Controllers
{
    public class TorneiosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TorneiosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Torneios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Torneio.ToListAsync());
        }

        // GET: Torneios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var torneio = await _context.Torneio
                .SingleOrDefaultAsync(m => m.Id == id);
            if (torneio == null)
            {
                return NotFound();
            }

            return View(torneio);
        }

        [Authorize]
        // GET: Torneios/Create
        public IActionResult Create()
        {
            JogadorPessoa[] jp = _context.JogadorPessoas.Include (j => j.Usuario).ToArray ();
            ViewBag.Jogadores = new MultiSelectList (jp, "Id", "Nome");
            return View();
        }

        // POST: Torneios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,NomeTorneio,QuantidadeJogadores,Inicio")] Torneio torneio)
        {
            if (ModelState.IsValid)
            {
                torneio.Dono = User.Identity.Name;
                _context.Add (torneio);
                torneio.Jogadores = new List<Jogador> ();
                torneio.Jogos = new List<Jogo> ();
                //
                // CRIANDO JOGADORES
                //
                // NÃO SEI COMO LINKAR JOGADORES JÁ CADASTRADOS...PERGUNTAR PARA O PROFESSOR
                List<Jogador> jogadores = new List<Jogador> ();
                for (int i = 1; i <= torneio.QuantidadeJogadores; i++)
                {
                    JogadorPessoa jogadorPessoa = new JogadorPessoa ();
                    jogadores.Add (jogadorPessoa);
                    torneio.Jogadores.Add (jogadorPessoa);
                    _context.JogadorPessoas.Add (jogadorPessoa);
                }
                _context.SaveChanges ();
                //
                // CRIANDO JOGOS
                //
                for (int i = 1; i <= 2; i++)
                {
                    List<Jogo> jogos = new List<Jogo> ();
                    for (int j1 = 0; j1 < jogadores.Count - 1; j1++)
                    {
                        for (int j2 = j1 + 1; j2 < jogadores.Count; j2++)
                        {
                            Jogo jogo = new Jogo ()
                            {
                                Jogador1 = jogadores[j1],
                                Jogador2 = jogadores[j2],
                                Tabuleiro = new Tabuleiro ()
                            };
                            jogos.Add (jogo);
                        }
                    }
                    Random random = new Random ();
                    while (jogos.Count > 0)
                    {
                        int valorAleatorio = random.Next (jogos.Count);
                        torneio.Jogos.Add (jogos[valorAleatorio]);
                        jogos.RemoveAt (valorAleatorio);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(torneio);
        }

        // GET: Torneios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var torneio = await _context.Torneio.SingleOrDefaultAsync(m => m.Id == id);
            if (torneio == null)
            {
                return NotFound();
            }
            return View(torneio);
        }

        // POST: Torneios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeTorneio,QuantidadeJogadores,Inicio")] Torneio torneio)
        {
            if (id != torneio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(torneio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TorneioExists(torneio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(torneio);
        }

        // GET: Torneios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var torneio = await _context.Torneio
                .SingleOrDefaultAsync(m => m.Id == id);
            if (torneio == null)
            {
                return NotFound();
            }
            if (User.Identity.Name != torneio.Dono)
            {
                return Forbid();
            }
            return View(torneio);
        }

        // POST: Torneios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var torneio = await _context.Torneio.SingleOrDefaultAsync(m => m.Id == id);
            if (User.Identity.Name != torneio.Dono)
            {
                return Forbid();
            }
            _context.Torneio.Remove(torneio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TorneioExists(int id)
        {
            return _context.Torneio.Any(e => e.Id == id);
        }


        public IActionResult SelecionarJogadores (int id)
        {
            var torneio = _context.Torneio.Include (t => t.Jogadores)
                .SingleOrDefault (m => m.Id == id);
            if (torneio == null)
            {
                return NotFound ();
            }

            SelecionarUsuarioViewModel viewModel = new SelecionarUsuarioViewModel ();

            List<int> jogadores = new List<int> ();
            if (torneio.Jogadores != null)
            {
                jogadores = torneio.Jogadores.Select (j => j.Id).ToList ();
            }
            ViewBag.Jogadores =
                new SelectList (_context.JogadorPessoas.Include (j => j.Usuario).ToList (),
                nameof (JogadorPessoa.Id),
                nameof (JogadorPessoa.Nome),
                jogadores
                );
            viewModel.JogadoresIds = jogadores;
            return View (viewModel);
        }


        [HttpPost]
        public IActionResult SelecionarJogadores (
            int id,
            [Bind (nameof (SelecionarUsuarioViewModel.JogadoresIds))] SelecionarUsuarioViewModel viewModel)
        {
            var torneio = _context.Torneio.SingleOrDefault (m => m.Id == id);
            if (torneio == null)
            {
                return NotFound ();
            }

            var jogadores = _context.JogadorPessoas.Where (
                jp => viewModel.JogadoresIds.Exists (j => j == jp.Id))
                .ToList ();
            foreach (var item in jogadores)
            {
                torneio.Jogadores.Add (item);
            }
            _context.SaveChanges ();
            return View (viewModel);
        }
    }
}
