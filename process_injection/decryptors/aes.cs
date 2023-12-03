// C# AES decryptor. Just snippets of code.


static string AESKey = "aaaaaaaaaaaaaaaa"; // Random 16 byte string
static string AESIV = "aaaaaaaaaaaaaaaa"; // Random 16 byte string

// Function
static byte[] Decrypt(byte[] ciphertext, string AESKey, string AESIV)
{
    byte[] key = Encoding.UTF8.GetBytes(AESKey);
    byte[] IV = Encoding.UTF8.GetBytes(AESIV);

    using (Aes aesAlg = Aes.Create())
    {
        aesAlg.Key = key;
        aesAlg.IV = IV;
        aesAlg.Padding = PaddingMode.None;

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using (MemoryStream memoryStream = new MemoryStream(ciphertext))
        {
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(ciphertext, 0, ciphertext.Length);
                return memoryStream.ToArray();
            }
        }
    }
}