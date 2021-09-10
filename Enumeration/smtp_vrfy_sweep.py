#!/usr/bin/python3

# Sweeps hosts with SMTP servers open
# and tries VRFY command on them.

import socket, sys, os.path

if len(sys.argv) != 2:
    print(f"Usage: {sys.argv[0]} <file>")
    sys.exit(0)

filename = sys.argv[1]
if not os.path.isfile(filename):
    print(f'[-] File "{filename}" is not a file.')
    sys.exit(0)

print('[*] Starting VRFY Sweep...')

def verify(host, port):
    print(f'[*] Verifying on {host}...')

    # Create socket and connect to server
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.settimeout(20)
        con = s.connect((host, 25))
        s.settimeout(None)
    except Exception as e:
        print('[-] Connection failed.')
        print(e)
        s.close()
        return

    # Receive banner
    banner = s.recv(1024)
    print('[*] Banner received:', banner.decode().strip())

    # Send VRFY command and receive response
    try:
        print('[*] Sending VRFY...')
        s.send(('VRFY root \r\n').encode())
        s.settimeout(20)
        res = s.recv(1024)
        print('[*] Received:', res.decode().strip())
    except Exception as e:
        print('[-] Protocol error.')
        print(e)
        s.close()
        return

    # Close
    s.close()

# For each host in file, try to verify
with open(filename, 'r') as f:
    for host in f.readlines():
        host = host.strip()
        print()
        verify(host, 25)
