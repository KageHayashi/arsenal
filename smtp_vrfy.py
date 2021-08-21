#!/usr/bin/python3

# The Simple Mail Transport Protocol (SMTP) supports commands
# such as VRFY and EXPN. VRFY requests the server to verify 
# if an existing email address or user exists. EXPN requests the 
# server for the membership of a mailing list

# This script tries to use the VRFY command to verify if 
# a certain user exits on a specific SMTP server

import socket
import sys

if len(sys.argv) != 3:
    print(f"Usage: {sys.argv[0]} <server> <username>")
    sys.exit(0)

server = sys.argv[1]
username = sys.argv[2]

# Create a socket and connect to server
print('[*] Connecting to server...')
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
try:    
    connect = s.connect((server, 25))
except Exception as e:
    print('[-] Connection failed. Closing...')
    print(e)
    sys.exit(0)

# Receive banner
banner = s.recv(1024)
print(banner)

# Try to VRFY username
try:
    print('[*] Sending VRFY')
    s.send(('VRFY ' + username + '\r\n').encode())
    res = s.recv(1024)
    print('[+] Received: ', res.decode('utf-8'))
except Exception as e:
    print('[-] Protocol error.')
    print(e)

# Close socket
s.close()
