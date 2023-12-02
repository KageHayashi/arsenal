# XOR implementation used for encrypting raw binary shellcode.

import sys

def xor(data, key):
    encrypted = bytearray(len(data))
    for i in range(len(data)):
        encrypted[i] = data[i] ^ key

    return encrypted
    
if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python xor.py <key> <filename>")
        exit(0)

    key = sys.argv[1]
    filename = sys.argv[2]

    with open(filename, 'rb') as f:
        data = bytearray(f.read())

    encrypted = xor(data, key)

    with open(f"{filename}.enc", 'wb') as f:
        f.write(encrypted)

    print(f"Written {filename}.enc")