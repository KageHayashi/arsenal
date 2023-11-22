// C# RC4. Just snippets of code.

static void Swap(int[] array, int i, int j)
{
    int temp = array[i];
    array[i] = array[j];
    array[j] = temp;
}

// Function
static byte[] RC4(byte[] data, byte[] key)
{
    int[] S = new int[256];
    byte[] K = new byte[256];
    byte[] cipher = new byte[data.Length];

    // Key-scheduling algorithm (KSA)
    for (int i = 0; i < 256; i++)
    {
        S[i] = i;
        K[i] = key[i % key.Length];
    }

    int j = 0;
    for (int i = 0; i < 256; i++)
    {
        j = (j + S[i] + K[i]) % 256;
        Swap(S, i, j);
    }

    // Pseudo-random generation algorithm (PRGA)
    int x = 0, y = 0;
    for (int i = 0; i < data.Length; i++)
    {
        x = (x + 1) % 256;
        y = (y + S[x]) % 256;

        Swap(S, x, y);

        cipher[i] = (byte)(data[i] ^ S[(S[x] + S[y]) % 256]);
    }

    return cipher;
}