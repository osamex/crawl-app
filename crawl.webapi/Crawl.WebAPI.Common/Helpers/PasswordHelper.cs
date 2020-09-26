using System;
using System.Linq;
using Crawl.WebAPI.Common.Enums;

namespace Crawl.WebAPI.Common.Helpers
{
	public static class PasswordHelper
	{
		public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			if (password == null)
			{
				throw new ArgumentNullException(nameof(password));
			}

			if (string.IsNullOrWhiteSpace(password))
			{
				throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
			}

			using var hmac = new System.Security.Cryptography.HMACSHA512();
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}

		public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
		{
			if (password == null)
			{
				throw new ArgumentNullException(nameof(password));
			}

			if (string.IsNullOrWhiteSpace(password))
			{
				throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
			}

			if (storedHash.Length != 64)
			{
				throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(password));
			}

			if (storedSalt.Length != 128)
			{
				throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(password));
			}

			using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				for (int i = 0; i < computedHash.Length; i++)
				{
					if (computedHash[i] != storedHash[i]) return false;
				}
			}

			return true;
		}

		public static PasswordScores CheckStrength(string password)
		{
			var score = 2;

			if (password.Length < 1)
			{
				return PasswordScores.Blank;
			}

			if (password.Length < 4)
			{
				return PasswordScores.VeryWeak;
			}

			if (password.Length >= 8)
			{
				score++;
			}

			if (password.Length >= 12)
			{
				score++;
			}

			// match uppercase and lowercase letters
			if (password.Any(char.IsUpper) && password.Any(char.IsLower))
			{
				score++;
			}

			// match special char
			if (password.Any(p => !char.IsLetterOrDigit(p)))
			{
				score++;
			}

			return (PasswordScores)score;
		}
	}
}