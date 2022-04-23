#!/bin/bash

# A simple bash script to check multiple md5sums.

# Requires a "checksums.txt" file in the current directory
# with the following format for each line:
# <file_name> <md5sum>

# Example "checksums.txt"
# DC-1.zip D052D37F7C819A2B5488FE2BFF4571D8
# DC-2.zip F66A5E3AA422A20A526DD4D1018F599B
# DC-3.zip 3DD0C0B4E96D593FBEADEC1EFC6B50C8
# ...

echo "[*] Checking md5sums (might take a while)...."
all_correct=1

while read i; do
	arr=($i)
	file_name=${arr[0]}
	file=$PWD/$file_name
	correct_md5sum=${arr[1]}
	md5sum=($(md5sum $file))
	if [[ ${md5sum^^} == ${correct_md5sum^^} ]]; then
		echo "[+] $file_name is correct."
	else
		echo "[-] $file_name is corrupted."
		echo "    > $md5sum =/= $correct_md5sum"
		all_correct=0
	fi
	
done < checksums.txt

if [[ $all_correct -eq 1 ]]; then
	echo "[+] Done! All files correct."
else
	echo "[-] Done... but some files may be corrupted."
fi
