#!/bin/bash
set -e
mkdir -p /root/.ssh /var/www/waldau /var/www/izotoff
chmod 700 /root/.ssh
grep -q 'github-deploy' /root/.ssh/authorized_keys 2>/dev/null || echo 'ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIJgq4WYkNXdQH8G9WAh18g69btDVEx8NWow+QJORZ8LG github-deploy' >> /root/.ssh/authorized_keys
chmod 600 /root/.ssh/authorized_keys
export DEBIAN_FRONTEND=noninteractive
apt-get update
apt-get install -y nginx curl rsync ufw certbot python3-certbot-nginx aspnetcore-runtime-10.0
apt-get clean
ufw allow OpenSSH
ufw allow 22/tcp
ufw allow 2222/tcp
ufw allow 80/tcp
ufw allow 443/tcp
ufw allow 8080/tcp
ufw --force enable
journalctl --vacuum-size=80M
echo STEP2_OK
hostname
dotnet --list-runtimes
systemctl is-active nginx || true
