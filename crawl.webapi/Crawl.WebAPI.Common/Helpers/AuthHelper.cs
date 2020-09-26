using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Crawl.WebAPI.Common.Contract.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Crawl.WebAPI.Common.Helpers
{
	public static class AuthHelper
	{
		private static string GetTokenFromHeader(HttpRequest request)
		{
			return StringValues.IsNullOrEmpty(request.Headers["Authorization"]) ? string.Empty : request.Headers["Authorization"].ToString().Replace("Bearer ", "");
		}

		public static string GetTokenFromHttpContext(HttpContext context)
		{
			return context?.Request == null ? string.Empty : GetTokenFromHeader(context.Request);
		}

		public static Guid GetCurrentUserAppKey(HttpContext context)
		{
			var token = GetTokenFromHttpContext(context);
			if (!string.IsNullOrEmpty(token))
			{
				var jwtData = DecodeBarerToken(token);
				if (jwtData != null)
				{
					return jwtData.UserAppKey;
				}
			}

			return Guid.Empty;
		}

		public static JwtData DecodeBarerToken(string token)
		{
			try
			{
				var handler = new JwtSecurityTokenHandler();
				if (handler.ReadToken(token) is JwtSecurityToken securityToken)
				{
					var createDateString = securityToken.Claims.First(claim => claim.Type == "CreateDate").Value.ToString(CultureInfo.CurrentCulture);
					var expirationDateString = securityToken.Claims.First(claim => claim.Type == "ExpirationDate").Value.ToString(CultureInfo.CurrentCulture);
					var userAppKeyString = securityToken.Claims.First(claim => claim.Type == "UserAppKey").Value.ToString();

					var userAppKey = new Guid(userAppKeyString);
					var createDate = Convert.ToDateTime(createDateString);
					var expirationDate = Convert.ToDateTime(expirationDateString);

					return new JwtData
					{
						UserAppKey = userAppKey,
						CreateDate = createDate,
						ExpirationDate = expirationDate
					};
				}
			}
			catch (Exception e)
			{
				Log.Error(e, "Error when decode barer token");
			}

			return null;
		}

		public static AuthResponseData CreateAuthResponseData(Guid userAppKey, string userLogin, string systemAuthSecretKey, JwtData payloadData)

		{
			if (userAppKey != Guid.Empty && !string.IsNullOrEmpty(userLogin)
					&& !string.IsNullOrEmpty(systemAuthSecretKey)
					&& systemAuthSecretKey.Length < 20)
			{
				return null;
			}

			// validate dates
			if (payloadData.CreateDate >= payloadData.ExpirationDate)
			{
				throw new InvalidDataException("CreateDate >= ExpirationDate");
			}

			if (DateTime.UtcNow >= payloadData.ExpirationDate)
			{
				throw new InvalidDataException("DateTime.UtcNow >= ExpirationDate");
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim("UserAppKey", payloadData.UserAppKey.ToString()),
					new Claim("CreateDate", payloadData.CreateDate.ToString(CultureInfo.CurrentCulture)),
					new Claim("ExpirationDate", payloadData.ExpirationDate.ToString(CultureInfo.CurrentCulture)),
				}),
				Expires = payloadData.ExpirationDate,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(systemAuthSecretKey ?? string.Empty)), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
			var barerToken = tokenHandler.WriteToken(token);

			return new AuthResponseData
			{
				UserAppKey = userAppKey,
				UserEmail = userLogin,
				BarerToken = barerToken
			};
		}
	}
}