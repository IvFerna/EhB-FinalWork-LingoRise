#!/bin/bash
set -e

TOOLS_DIR="$(pwd)/tools/piper"
mkdir -p "$TOOLS_DIR/models"

# Download Piper binary (Linux x64)
if [ ! -f "$TOOLS_DIR/piper" ]; then
  echo "Downloading Piper..."
  curl -L https://github.com/rhasspy/piper/releases/download/2023.11.14-2/piper_linux_x86_64.tar.gz \
    -o /tmp/piper.tar.gz
  tar -xzf /tmp/piper.tar.gz -C "$TOOLS_DIR" --strip-components=1
  chmod +x "$TOOLS_DIR/piper"
  echo "Piper downloaded."
fi

# Download Spanish model
MODEL="$TOOLS_DIR/models/es_ES-sharvard-medium.onnx"
if [ ! -f "$MODEL" ]; then
  echo "Downloading model..."
  curl -L "https://huggingface.co/rhasspy/piper-voices/resolve/main/es/es_ES/sharvard/medium/es_ES-sharvard-medium.onnx" \
    -o "$MODEL"
  curl -L "https://huggingface.co/rhasspy/piper-voices/resolve/main/es/es_ES/sharvard/medium/es_ES-sharvard-medium.onnx.json" \
    -o "$MODEL.json"
  echo "Model downloaded."
fi