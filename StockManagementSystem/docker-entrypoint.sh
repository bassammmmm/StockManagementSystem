#!/usr/bin/env bash
set -e

# Run the application (migrations run inside the app's startup code)
exec dotnet StockManagementSystem.dll
