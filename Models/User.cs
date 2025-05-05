using System;
namespace Project1.Models
{
	public class User:IdentityUser
	{
		public ICollection<Post>? posts { get; set; }
		
	}
}

