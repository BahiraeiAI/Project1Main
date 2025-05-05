using System;
using System.ComponentModel;

namespace Project1.ViewModels
{
	public class LoginViewModel
	{
		public LoginViewModel()
		{
		}


        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }



        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
	}
}

