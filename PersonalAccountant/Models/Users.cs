using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalAccountant.Models
{
	public class Users
	{
		[Key]
		public int UserId { get; set; }
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }

		public SafeUsers GetSafeUser()
		{
			SafeUsers safeUser = new SafeUsers
			{
				UserId = this.UserId,
				Username = this.Username
			};

			return safeUser;
		}
	}

	public class SafeUsers
	{
		public int UserId { get; set; }
		public string Username { get; set; }
	}
}