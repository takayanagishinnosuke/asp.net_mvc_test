using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asp.net_mvc_test.Models
{
	public class MovieGenreViewModel
	{
			public List<Movie>? Movies { get; set; }
			public SelectList? Genres { get; set; }
			public string? MovieGenre { get; set; }
			public string? SearchString { get; set; }
    }
	
}

