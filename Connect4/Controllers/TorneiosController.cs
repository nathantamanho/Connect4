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
            var resultado = await _context.Torneio.Include (t => t.Jogadores).ToListAsync ();
            return View(resultado);
        }

        // GET: Torneios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DetalhesTorneioViewModel viewModel = new DetalhesTorneioViewModel ();
            viewModel.Torneio = await _context.Torneio.Include(t => t.Jogos).SingleOrDefaultAsync (t => t.Id == id);
            if (viewModel.Torneio == null)
            {
                return NotFound();
            }
            viewModel.JogosConcluidos = 0;
            for (int i = 0; i < viewModel.Torneio.Jogos.Count; i++)// Jogo j in viewModel.Jogos)
            {
                viewModel.Torneio.Jogos[i] = 
                    _context.Jogos
                    .Include (j => j.Jogador1)
                    .Include (j => j.Jogador2)
                    .Include (j => j.Tabuleiro)
                    .Where (j => j.TabuleiroId == viewModel.Torneio.Jogos[i].TabuleiroId)
                    .FirstOrDefault ();
                if (viewModel.Torneio.Jogos[i].Tabuleiro.Vencedor () != 0)
                {
                    viewModel.JogosConcluidos++;
                }
                else
                {
                    if (viewModel.ProximoJogo == null)
                    {
                        viewModel.ProximoJogo = viewModel.Torneio.Jogos[i];
                    }
                }
            }
            for (int i = 0; i < viewModel.Torneio.Jogadores.Count; i++)
            {
                if (viewModel.Torneio.Jogadores[i] is JogadorPessoa)
                {
                    viewModel.Torneio.Jogadores[i] = _context.JogadorPessoas
                                    .Include (j => j.Usuario)
                                    .Where (j => j.Id == viewModel.Torneio.Jogadores[i].Id)
                                    .FirstOrDefault ();
                }
            }
            if (viewModel.ProximoJogo != null)
            {
                if (viewModel.ProximoJogo.Jogador1 is JogadorPessoa)
                {
                    viewModel.ProximoJogo.Jogador1 = _context.JogadorPessoas
                                    .Include (j => j.Usuario)
                                    .Where (j => j.Id == viewModel.ProximoJogo.Jogador1Id)
                                    .FirstOrDefault ();
                }
                if (viewModel.ProximoJogo.Jogador2 is JogadorPessoa)
                {
                    viewModel.ProximoJogo.Jogador2 = _context.JogadorPessoas
                                    .Include (j => j.Usuario)
                                    .Where (j => j.Id == viewModel.ProximoJogo.Jogador2Id)
                                    .FirstOrDefault ();
                }
            }
            viewModel.Ranking = new List<Ranking> ();
            foreach (Jogador jogador in viewModel.Torneio.Jogadores)
            {
                Ranking ranking = new Ranking ();
                ranking.Jogador = jogador;
                ranking.Pontuacao = 0;
                foreach (Jogo jogo in viewModel.Torneio.Jogos.Where (j => j.Jogador1Id == jogador.Id || j.Jogador2Id == jogador.Id))
                {
                    if (jogo.Tabuleiro.Vencedor () == -1)
                    {
                        ranking.Pontuacao++;
                    }
                    else if ((jogador.Id == jogo.Jogador1Id && jogo.Tabuleiro.Vencedor () == 1) || (jogador.Id == jogo.Jogador2Id && jogo.Tabuleiro.Vencedor () == 2))
                    {
                        ranking.Pontuacao += 3;
                    }
                }
                viewModel.Ranking.Add (ranking);
            }
            viewModel.Ranking = viewModel.Ranking.OrderByDescending (r => r.Pontuacao).ToList ();
            return View(viewModel);
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
            viewModel.QuantidadeJogadores = torneio.QuantidadeJogadores;
            return View (viewModel);
        }


        [HttpPost]
        public IActionResult SelecionarJogadores (
            int id,
            [Bind (nameof (SelecionarUsuarioViewModel.JogadoresIds))] SelecionarUsuarioViewModel viewModel)
        {
            var torneio = _context.Torneio.Include (t => t.Jogos).SingleOrDefault (m => m.Id == id);
            if (torneio == null)
            {
                return NotFound ();
            }
            if (viewModel.JogadoresIds.Count == torneio.QuantidadeJogadores)
            {

                var jogadores = _context.JogadorPessoas.Where (
                    jp => viewModel.JogadoresIds.Exists (j => j == jp.Id))
                    .ToList ();
                foreach (var item in jogadores)
                {
                    torneio.Jogadores.Add (item);
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
            }
            _context.SaveChanges ();
            return RedirectToAction (nameof (Index));
            //return View (viewModel);
        }
    }
}
