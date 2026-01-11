#!/bin/bash
# Wrapper script for run-script to avoid shell built-in conflicts

DOTNET_TOOLS="$HOME/.dotnet/tools"
exec "$DOTNET_TOOLS/r" "$@"
