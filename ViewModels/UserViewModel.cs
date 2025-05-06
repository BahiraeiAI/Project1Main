using System;
namespace Project1.ViewModels
{
	public class UserViewModel
	{
		public string Id { get; set; }

		[EmailAddress(ErrorMessage = "Enter a valid Email address")]
		[Required(ErrorMessage = "you must enter a valid email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "enter your username")]
		public string UserName { get; set; }

	}
}

