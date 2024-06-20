#!/bin/bash
set -e
ARCH=$(uname -m)
KUBECTL_VERSION="v1.23.0" # Kies de versie die past bij je cluster
case $ARCH in
    x86_64) KUBECTL_ARCH="amd64" ;;
    aarch64) KUBECTL_ARCH="arm64" ;;
    *) echo "Unsupported architecture: $ARCH"; exit 1 ;;
esac
curl -LO "https://storage.googleapis.com/kubernetes-release/release/${KUBECTL_VERSION}/bin/linux/${KUBECTL_ARCH}/kubectl"
chmod +x kubectl
mv kubectl /usr/local/bin/
