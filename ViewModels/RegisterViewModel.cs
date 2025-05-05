using System;
using System.ComponentModel;

namespace Project1.ViewModels
{
	public class RegisterViewModel
	{
		public RegisterViewModel()
		{
		}


        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }


        [PasswordPropertyText]
        [DataType(DataType.Password, ErrorMessage = "password is invalid 8 characters, lower and uppercase letters and 1 specific letter")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Required(ErrorMessage = "You must confirm your password")]
        public string ConfirmPassword { get; set; }

	}
}

