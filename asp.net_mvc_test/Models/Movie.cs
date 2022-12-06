using System.ComponentModel.DataAnnotations;

namespace asp.net_mvc_test.Models
{
	public class Movie
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Genre { get; set; }
		public decimal Revenue { get; set; }

		[DataType(DataType.Date)]
		public DateTime ReleaseDate { get; set; }

	}
}

