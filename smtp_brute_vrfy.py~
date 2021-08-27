#!/usr/bin/python3

# Like single verify but uses a list of usernames
# instead of a single one.

import sys
import socket
import os.path

if len(sys.argv) != 3:
    print(f"Usage: {sys.argv[0]} <server> <file>")
    sys.exit(0)

server = sys.argv[1]
filename = sys.argv[2]

# Checks if file exists
if not os.path.isfile(filename):
    print(f"File \"{filename}\" doesn't exist")
    sys.exit(0)

# Create socket and connect to server
def connect(server: str, port) -> 'Connection':
    '''Tries to connect to server and returns connection object'''
    print('[*] Connecting to server...')
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.settimeout(10)
    try:
        connect = s.connect((server, 25))

        # Receive banner
        banner = s.recv(1024)
        print(banner.decode('utf-8'))

        return connect
    except Exception as e:
        print('[-] Connection failed. Closing...')
        print(e)
        sys.exit(0)

def verify_user(username: str, con: 'Connection'):
    '''Send VRFY <username> command to server connection'''
    try:
        s.send(('VRFY ' + username + '\r\n').encode())
        res = s.recv(1024)
        print('[+] Received:', res.decode('utf-8'))
    except Exception as e:
        print('[-] Protocol error.')
        print(e)

connection = connect(server, 25)

# Loop through all names in file
with open(sys.argv[2], 'r') as f:
    for username in f.readlines():
        username = username.strip()
        verify_user(username, connection)

# Close socket
s.close()
