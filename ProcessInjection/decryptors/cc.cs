// C# Caesar Cipher decryptor. Just snippets of code.

// Function
static byte[] DecryptCaesar(byte[] encrypted, int key)
{
    byte[] decrypted = new byte[encrypted.Length];

    for (int i = 0; i < encrypted.Length; i++)
    {
        decrypted[i] = (byte)(((uint)encrypted[i] - key) & 0xFF);
    }

    return decryptedData;
}

// Stand-alone
byte[] buf = new byte[510] {0xF7,0x43,0x7E,0xDF...};
int key = 5;
for (int i = 0; i < buf.Length; i++)
{
	buf[i] = (byte)(((uint)buf[i] - key) & 0xFF);
}