using System;
namespace Project1.Models
{
	public class Image
	{
		[Key]
		public int Id { get; set;}

		public byte[]? Content { get; set; }
		public string? ContentType { get; set; }
		public string? ImageName { get; set; }

		//navigation properties
		public Post Post { get; set; }

		[ForeignKey("Post")]
		public int PostId { get; set; }
	}
}

