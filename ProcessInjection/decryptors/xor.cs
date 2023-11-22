// C# XOR. Just snippets of code.

// Function
static byte[] XOR(byte[] data, byte[] key)
{
    byte[] res = new byte[data.Length];

    for (int i = 0; i < data.Length; i++)
    {
        res[i] = (byte)(data[i] ^ key[i % key.Length]);
    }

    return res;
}

// Stand-alone
byte[] buf = new byte[510] {0xfc,0x48,0x83,0xe4,0xf0,0xe8,...};
byte[] key = Encoding.UTF8.GetBytes("SecretKey");

for (int i = 0; i < buf.Length; i++)
{
	buf[i] = (byte)(buf[i] ^ key[i % key.Length]);
}