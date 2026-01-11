#!/bin/bash

# 🎨 Format script for acore-godot
# Formats all project files using acore-scripts

set -e

# Get script directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

# Source acore-scripts formatting utilities
ACORE_SCRIPTS_DIR="$PROJECT_ROOT/packages/acore-scripts/src"

# Check if acore-scripts submodule exists
if [[ ! -d "$ACORE_SCRIPTS_DIR" ]]; then
  echo "❌ acore-scripts not found at $ACORE_SCRIPTS_DIR"
  echo "Please initialize the submodule:"
  echo "  git submodule update --init --recursive"
  exit 1
fi

# Source logger
# shellcheck source=/dev/null
source "$ACORE_SCRIPTS_DIR/logger.sh"

# Main formatting logic
main() {
  cd "$PROJECT_ROOT"

  acore_log_section "🎨 Formatting acore-godot"

  # Format shell scripts
  acore_log_section "🐚 Shell Scripts"
  # shellcheck source=/dev/null
  TARGET_DIR="$PROJECT_ROOT" source "$ACORE_SCRIPTS_DIR/format_sh.sh"

  # Format markdown
  acore_log_section "📝 Markdown"
  # shellcheck source=/dev/null
  TARGET_DIR="$PROJECT_ROOT" source "$ACORE_SCRIPTS_DIR/format_md.sh"

  # Format yaml
  acore_log_section "📄 YAML"
  # shellcheck source=/dev/null
  TARGET_DIR="$PROJECT_ROOT" source "$ACORE_SCRIPTS_DIR/format_yaml.sh"

  # Format json
  acore_log_section "📋 JSON"
  # shellcheck source=/dev/null
  TARGET_DIR="$PROJECT_ROOT" source "$ACORE_SCRIPTS_DIR/format_json.sh"

  # Format C#
  acore_log_section "🔷 C#"
  if command -v dotnet > /dev/null 2>&1; then
    if dotnet tool list | grep -q "csharpier"; then
      acore_log_info "Using dotnet csharpier"
      if dotnet csharpier format . 2> /dev/null; then
        acore_log_success "C# files formatted successfully!"
      else
        acore_log_warning "C# formatting completed with some issues"
      fi
    else
      acore_log_warning "dotnet csharpier tool not found"
      acore_log_info "To format C# files, install CSharpier:"
      acore_log_info "  dotnet tool install -g csharpier"
      acore_log_warning "Continuing without C# formatting..."
    fi
  else
    acore_log_warning "dotnet not found"
    acore_log_warning "Cannot format C# files without dotnet"
  fi

  acore_log_section "✨ Done"
}

# Run main
main "$@"
