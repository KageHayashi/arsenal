# Converts a raw binary shellcode file into C# formatted byte array

import sys

cs_buf = "byte[] buf = new byte[{buf_len}] {{{buf_str}}};"

if __name__ == '__main__':
    if len(sys.argv) != 2:
        print("Usage: python bin_to_cs.py <filename>")
        exit(0)

    filename = sys.argv[1]

    with open(filename, 'rb') as f:
        data = bytearray(f.read())

    hex_array = [f'0x{byte:02X}' for byte in data]
    print(cs_buf.format(buf_len=len(hex_array), buf_str=','.join(hex_array)))