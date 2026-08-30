#!/bin/bash
set -e
mkdir -p /root/.ssh
chmod 700 /root/.ssh
grep -q 'github-actions-izotoff' /root/.ssh/authorized_keys 2>/dev/null || echo 'ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIFtDfwswmV1ChpG/6W3piRLtXcm5ato3AEFG8ZdOq9fm github-actions-izotoff' >> /root/.ssh/authorized_keys
chmod 600 /root/.ssh/authorized_keys
echo KEY_OK
