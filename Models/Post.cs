using System;
namespace Project1.Models
{
	public class Post
	{
		[Key]
		public int Id { get; set; }

		public string Title { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }

		
		public string Text { get; set; }

		public DateTime PublishedDateTime { get; set; } = DateTime.UtcNow;

        //navigation property
        public User User { get; set; }
        public Image Image { get; set; }
    }
}

