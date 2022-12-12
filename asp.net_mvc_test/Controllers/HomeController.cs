using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using asp.net_mvc_test.Models;


namespace asp.net_mvc_test.Controllers;

public class HomeController : Controller
{
    private readonly MvcMovieContext _context;

    public HomeController(MvcMovieContext context)
    {
        _context = context;
    }

    //ホーム画面表示（登録されているものが全て表示）
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

        //ジャンルと抽出して映画データをそれぞれリストにして格納
        var movieGenreVM = new MovieGenreViewModel
        {
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
            Movies = await movies.ToListAsync()
        };

        Console.WriteLine(movieGenreVM);
        //結果を返す
        return View(movieGenreVM);
    }

    public async Task<IActionResult> Details(int? id)
    {
        //もしもidと一致しなかったら
        if(id==null || _context.Movie == null)
        {
            return NotFound();
        }
        //一致する要素を非同期に取得
        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    //プライバシーポリシー
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

