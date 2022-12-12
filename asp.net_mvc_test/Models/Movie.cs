using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace asp.net_mvc_test.Models
{
	public class Movie
	{
		public int Id { get; set; }

		[DisplayName("タイトル")]
        [StringLength(60, MinimumLength = 3, ErrorMessage ="3文字以上、60文字以内で入力してください")]
        [Required(ErrorMessage ="入力必須です")]
        public string? Title { get; set; }

        [DisplayName("ジャンル")]
        [StringLength(30)]
        [Required(ErrorMessage = "入力必須です")]
        public string? Genre { get; set; }

        [DisplayName("興行収入")]
        [Range(1,10000)]
        [Required(ErrorMessage = "入力必須です")]
        public decimal Revenue { get; set; }

        [DisplayName("公開日")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "入力必須です")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("レイティング")]
        [StringLength(5)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [Required(ErrorMessage = "入力必須です")]
        public string? Reating { get; set; }

    }
}

