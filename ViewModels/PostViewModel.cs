using System;
namespace Project1.ViewModels
{
	public class PostViewModel
	{

		public int Id { get; set; }
		[Required(ErrorMessage = "Title can't be empty! ")]
		[Length(5,40,ErrorMessage = "Title range can only be 5 - 40 letters")]
		public string Title { get; set; }


        [Required(ErrorMessage = "Description can't be empty! ")]
        [Length(10, 400, ErrorMessage = "Description range can only be 10 - 400 letters")]
        public string Description { get; set; }

		[Required(ErrorMessage = "You must upload an image! ")]
		public IFormFile Image { get; set; }
	}
}

