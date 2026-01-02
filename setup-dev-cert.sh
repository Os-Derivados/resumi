#!/usr/bin/env bash
set -e

# Usage: ./setup-dev-cert.sh <password>
if [ -z "$1" ]; then
  echo "Usage: $0 <password>"
  exit 1
fi

PASSWORD="$1"
CERT_DIR="$HOME/.aspnet/https"
CERT_NAME="aspnet-dev-cert"
PFX_FILE="$CERT_DIR/$CERT_NAME.pfx"
CRT_FILE="$CERT_DIR/$CERT_NAME.crt"

mkdir -p "$CERT_DIR"

echo "Generating dev certificate with password: $PASSWORD"
dotnet dev-certs https -ep "$PFX_FILE" -p "$PASSWORD"
dotnet dev-certs https -v -ep "$CRT_FILE"

echo "Installing certificate into Fedora trust store..."
sudo cp "$CRT_FILE" /etc/pki/ca-trust/source/anchors/
sudo update-ca-trust extract

echo "Certificate exported to:"
echo "  $PFX_FILE (for Kestrel)"
echo "  $CRT_FILE (for trust store)"
