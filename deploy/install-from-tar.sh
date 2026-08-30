#!/bin/bash
set -e
if [ ! -f /tmp/sites.tgz ]; then
  echo "NO /tmp/sites.tgz — сначала скачайте архив" >&2
  exit 1
fi
mkdir -p /tmp/sites-unpack /var/www/waldau /var/www/izotoff
tar -xzf /tmp/sites.tgz -C /tmp/sites-unpack
rsync -a /tmp/sites-unpack/waldau/ /var/www/waldau/
rsync -a /tmp/sites-unpack/izotoff/ /var/www/izotoff/
cp /tmp/sites-unpack/waldau.env /etc/waldau.env
cp /tmp/sites-unpack/izotoff.env /etc/izotoff.env
chmod 600 /etc/waldau.env /etc/izotoff.env
sed -i '/Telegram__ProxyUrl/d' /etc/izotoff.env
grep -q 'SiteSettings__BaseUrl=' /etc/izotoff.env || echo 'SiteSettings__BaseUrl=http://82.23.173.134:8080' >> /etc/izotoff.env
curl -fsSL -o /etc/systemd/system/waldau.service https://raw.githubusercontent.com/Befivee/Izotoff/main/deploy/waldau.service
curl -fsSL -o /etc/systemd/system/izotoff.service https://raw.githubusercontent.com/Befivee/Izotoff/main/deploy/izotoff.service
curl -fsSL -o /etc/nginx/sites-available/waldau https://raw.githubusercontent.com/Befivee/Izotoff/main/deploy/nginx-waldau.conf
curl -fsSL -o /etc/nginx/sites-available/izotoff https://raw.githubusercontent.com/Befivee/Izotoff/main/deploy/nginx-izotoff.conf
rm -f /etc/nginx/sites-enabled/default
ln -sfn /etc/nginx/sites-available/waldau /etc/nginx/sites-enabled/waldau
ln -sfn /etc/nginx/sites-available/izotoff /etc/nginx/sites-enabled/izotoff
systemctl daemon-reload
systemctl enable --now waldau izotoff nginx
systemctl restart waldau izotoff
nginx -t
systemctl reload nginx
echo "==== STATUS ===="
systemctl is-active waldau izotoff nginx
echo INSTALL_OK
