using System;
using System.Security.Cryptography;
using System.Text;

namespace Crawl.WebAPI.Common.Helpers
{
	public class CryptographyHelper
	{
		public static class TripleDES
		{
			public static string Encoder(string input, string key)
			{
				try
				{
					var toEncryptArray = Encoding.UTF8.GetBytes(input);
					var keyArray = Encoding.UTF8.GetBytes(key);

					var tdes = new TripleDESCryptoServiceProvider
					{
						Key = keyArray,
						Mode = CipherMode.ECB,
						Padding = PaddingMode.PKCS7
					};

					var cTransform = tdes.CreateEncryptor();
					var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
					tdes.Clear();
					return Convert.ToBase64String(resultArray, 0, resultArray.Length);
				}
				catch (Exception e)
				{
					throw new Exception(e.Message);
				}
			}

			public static string Decoder(string input, string key)
			{
				try
				{
					if (string.IsNullOrEmpty(input)) return string.Empty;
					var toEncryptArray = Convert.FromBase64String(input);
					var keyArray = Encoding.UTF8.GetBytes(key);

					var tdes = new TripleDESCryptoServiceProvider
					{
						Key = keyArray,
						Mode = CipherMode.ECB,
						Padding = PaddingMode.PKCS7
					};

					var cTransform = tdes.CreateDecryptor();
					var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
					tdes.Clear();
					return Encoding.UTF8.GetString(resultArray);

				}
				catch (Exception e)
				{
					throw new Exception(e.Message);
				}
			}
		}
	}
}