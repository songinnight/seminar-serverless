using System.IO;
using System.Text;
using System.Security.Cryptography;


public class AesCbc256
{
	/// <summary>
	/// 암호화
	/// </summary>
	/// <param name="plainText">암호화 할 문자열</param>
	/// <param name="keyBytes">키</param>
	/// <param name="ivBytes">IV 초기화 벡터</param>
	/// <returns></returns>
	public static string Encrypt(string plainText, byte[] keyBytes, byte[] ivBytes)
	{
		// --- VALIDATE ---
		if (plainText == null || plainText.Length <= 0)
			throw new System.ArgumentNullException("plainText is null or empty");

		if (keyBytes == null || keyBytes.Length != 32)
			throw new System.ArgumentNullException("Key length must be 32");
		if (ivBytes == null || ivBytes.Length != 16)
			throw new System.ArgumentNullException("IV length must be 16");


		byte[] encrypted;
		using (RijndaelManaged rijAlg = new RijndaelManaged())
		{
			//rijAlg.BlockSize = 128; // 기본값 128 (AES-CBC-256는 블록 사이즈 128비트(=16바이트) 고정)
			//rijAlg.Mode = CipherMode.CBC; // 기본값 CBC
			//rijAlg.Padding = PaddingMode.PKCS7; // 기본값 PKCS7
			rijAlg.Key = keyBytes;
			rijAlg.IV = ivBytes;


			// 암호화 객체 생성
			ICryptoTransform encryptor = rijAlg.CreateEncryptor();

			// ㅇ호과 스트림 처리
			using (MemoryStream msEncrypt = new MemoryStream())
			{
				using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
				{
					using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
					{
						swEncrypt.Write(plainText);
					}
					encrypted = msEncrypt.ToArray();
				}
			}
		}

		// 바이트 배열을 64진수 문자열로 변환
		return System.Convert.ToBase64String(encrypted);
	}

	/// <summary>
	/// 암호화 - 평문 key 사용
	/// </summary>
	/// <param name="plainText">암호화 할 문자열</param>
	/// <param name="keyString">평문 key 문자열</param>
	/// <param name="ivString">IV 초기화 벡터 문자열</param>
	/// <returns></returns>
	public static string Encrypt(string plainText, string keyString, string ivString)
	{
		if (keyString == null || keyString.Length != 32)
			throw new System.ArgumentNullException("Key length must be 32");
		if (ivString == null || ivString.Length != 16)
			throw new System.ArgumentNullException("IV length must be 16");

		byte[] key = Encoding.UTF8.GetBytes(keyString); // 평문 대시 바이트 배열을 사용하도록 변경해도 됨
		byte[] iv = Encoding.UTF8.GetBytes(ivString);
		return Encrypt(plainText, key, iv);
	}







	/// <summary>
	/// 복호화
	/// </summary>
	/// <param name="encText">암호화 된 Base64 문자열</param>
	/// <param name="keyBytes">키</param>
	/// <param name="ivBytes">IV 초기화 벡터</param>
	/// <returns></returns>
	public static string Decrypt(string encText, byte[] keyBytes, byte[] ivBytes)
	{
		// --- VALIDATE ---
		if (encText == null || encText.Length <= 0)
			throw new System.ArgumentNullException("encText is null or empty");
		if (keyBytes == null || keyBytes.Length != 32)
			throw new System.ArgumentNullException("Key length must be 32");
		if (ivBytes == null || ivBytes.Length != 16)
			throw new System.ArgumentNullException("IV length must be 16");


		// 64진수 문자열로 부터 바이트 배열로 변환
		byte[] cipherText = System.Convert.FromBase64String(encText);
		
		string plaintext = null;

		using (RijndaelManaged rijAlg = new RijndaelManaged())
		{
			//rijAlg.BlockSize = 128; // 기본값 128 (AES-CBC-256는 블록 사이즈 128비트(=16바이트) 고정)
			//rijAlg.Mode = CipherMode.CBC; // 기본값 CBC
			//rijAlg.Padding = PaddingMode.PKCS7; // 기본값 PKCS7
			rijAlg.Key = keyBytes;
			rijAlg.IV = ivBytes;

			// 복호화 객체 생성
			ICryptoTransform decryptor = rijAlg.CreateDecryptor();

			// 복호과 스트림 처리
			using (MemoryStream msDecrypt = new MemoryStream(cipherText))
			{
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					using (StreamReader srDecrypt = new StreamReader(csDecrypt))
					{
						plaintext = srDecrypt.ReadToEnd();
					}
				}
			}
		}

		return plaintext;
	}


	/// <summary>
	/// 복호화 - 평문 key 사용
	/// </summary>
	/// <param name="encText"></param>
	/// <param name="keyString"></param>
	/// <param name="ivString"></param>
	/// <returns></returns>
	public static string Decrypt(string encText, string keyString, string ivString)
	{
		if (keyString == null || keyString.Length != 32)
			throw new System.ArgumentNullException("Key length must be 32");
		if (ivString == null || ivString.Length != 16)
			throw new System.ArgumentNullException("IV length must be 16");

		byte[] key = Encoding.UTF8.GetBytes(keyString); // 평문 대시 바이트 배열을 사용하도록 변경해도 됨
		byte[] iv = Encoding.UTF8.GetBytes(ivString);
		return Decrypt(encText, key, iv);
	}
}
