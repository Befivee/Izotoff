#!/bin/bash
set -e
chmod 755 /root
mkdir -p /root/.ssh
chmod 700 /root/.ssh
touch /root/.ssh/authorized_keys
grep -q 'github-deploy' /root/.ssh/authorized_keys || echo 'ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIJgq4WYkNXdQH8G9WAh18g69btDVEx8NWow+QJORZ8LG github-deploy' >> /root/.ssh/authorized_keys
grep -q 'github-actions-izotoff' /root/.ssh/authorized_keys || echo 'ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIFtDfwswmV1ChpG/6W3piRLtXcm5ato3AEFG8ZdOq9fm github-actions-izotoff' >> /root/.ssh/authorized_keys
chmod 600 /root/.ssh/authorized_keys
chown -R root:root /root/.ssh
cat >/etc/ssh/sshd_config.d/99-root-key.conf <<'EOF'
PermitRootLogin prohibit-password
PubkeyAuthentication yes
AuthorizedKeysFile .ssh/authorized_keys
PasswordAuthentication yes
UseDNS no
EOF
sshd -t
systemctl restart ssh
echo "==== keys ===="
wc -l /root/.ssh/authorized_keys
echo "==== sshd ===="
sshd -T | grep -E 'permitrootlogin|pubkeyauthentication|authorizedkeysfile|passwordauthentication'
echo "==== listen ===="
ss -tlnp | grep ssh || true
echo SSH_FIX_OK
