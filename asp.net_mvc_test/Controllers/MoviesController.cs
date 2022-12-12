using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using asp.net_mvc_test.Models;

namespace asp.net_mvc_test.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            //Movieテーブルから全てのジャンルを取得するクエリ
            var genreQuery = _context.Movie
                .OrderBy(m => m.Genre)
                .Select(m => m.Genre);

            //Movieテーブルから全てのデータを取得するクエリ
            var movies = _context.Movie.Select(m => m);

            //タイトル検索処理
            if (!string.IsNullOrEmpty(searchString))
            {
                //タイトルに検索文字列が含まれるデータを抽出するクエリ
                movies = movies.Where(s => s.Title!.Contains(searchString));
            }

            //ジャンル検索処理
            if (!string.IsNullOrEmpty(movieGenre))
            {
                //選択したジャンルが一致するデータを抽出するクエリ
                movies = movies.Where(x => x.Genre == movieGenre);

            }

            //ジャンルと抽出した映画データをそれぞれリストにしてプロパティに格納する
            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()

            };

            //結果をビューに返す
            return View(movieGenreVM);

        }
            
            

        // GET: Movies/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,Revenue,ReleaseDate,Reating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //idやMovieテーブルがnullの場合はNotoFound
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }
            //Movieテーブルからidが一致している映画データを取得してビューに返す
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Edit画面でセーブrをクリックした際に実行される
        [HttpPost] 
        [ValidateAntiForgeryToken] //CSRF対策(Formタグヘルパーとセットで使用する）
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,Revenue,ReleaseDate,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }
            //モデル検証が成功した場合の処理
            if (ModelState.IsValid)
            {
                try
                {
                    //送信されたデータをもとにMovieテーブルのデータを更新
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                //同時に同じデータが変更された（競合が発生した）場合の例外処理
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //Indexページにリダイレクトする
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
