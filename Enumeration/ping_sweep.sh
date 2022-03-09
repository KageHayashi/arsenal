#!/bin/bash

if [ "$1" == "" ]
then
	echo "Usage: ./ping_sweep.sh [network]"
	echo "example: ./ping_sweep.sh 10.11.1"
else
	for ip in `seq 1 254`; do
		ping -c 1 $1.$ip | grep "64 bytes" | cut -d " " -f 4 | tr -d ":" &
	done
fi
