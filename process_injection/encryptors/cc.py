# Caesar cipher implementation used for encrypting raw binary shellcode.

import sys

def cc(data, key, mode='encrypt'):
    encrypted = bytearray(len(data))
    for i in range(len(data)):
        if mode == 'encrypt':
            encrypted[i] = (data[i] + key) % 256
        elif mode == 'decrypt':
            encrypted[i] = (data[i] - key) % 256
        else:
            raise ValueError("Invalid mode. Use 'encrypt' or 'decrypt'.")

    return encrypted
    
if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python cc.py <key> <filename>")
        exit(0)

    key = sys.argv[1]
    filename = sys.argv[2]

    with open(filename, 'rb') as f:
        data = bytearray(f.read())

    encrypted = cc(data, int(key))

    with open(f"{filename}.enc", 'wb') as f:
        f.write(encrypted)

    print(f"Written {filename}.enc")