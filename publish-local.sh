#!/usr/bin/env bash

set -euo pipefail

FEED_ENV="LOCAL_NUGET_FEED"
ALPHA_BUILD=false

while [[ $# -gt 0 ]]; do
    case "$1" in
        -a|--alpha)
            ALPHA_BUILD=true
            shift
            ;;
        *)
            echo "Error: unknown argument '$1'." >&2
            echo "Usage: $0 [--alpha]" >&2
            exit 1
            ;;
    esac
done

if [[ -z "${LOCAL_NUGET_FEED:-}" ]]; then
    echo "Error: environment variable '$FEED_ENV' is not set." >&2
    echo "Set it to the path of your local NuGet feed, for example:" >&2
    echo "  export $FEED_ENV=\"\$HOME/.nuget/local\"" >&2
    exit 1
fi

if [[ ! -d "$LOCAL_NUGET_FEED" ]]; then
    echo "Error: local NuGet feed directory does not exist:" >&2
    echo "  $LOCAL_NUGET_FEED" >&2
    exit 1
fi

PACKAGE_DIR="./artifacts/local-packages"
PACK_ARGS=()

if [[ "$ALPHA_BUILD" == true ]]; then
    BASE_VERSION="$(
        dotnet msbuild ./src/Kasane2D/Kasane2D.csproj \
            -nologo \
            -getProperty:Version |
        tr -d '\r' |
        tail -n 1
    )"

    TIMESTAMP="$(date '+%Y-%m-%d-%H-%M-%S')"
    PACKAGE_VERSION="${BASE_VERSION}-alpha-${TIMESTAMP}"

    PACK_ARGS+=("-p:PackageVersion=$PACKAGE_VERSION")

    echo "Alpha build version:"
    echo "  $PACKAGE_VERSION"
fi

echo "Cleaning package output..."
rm -rf "$PACKAGE_DIR"
mkdir -p "$PACKAGE_DIR"

echo "Building NuGet packages..."
dotnet pack ./src/Kasane2D.slnx \
    --configuration Release \
    --output "$PACKAGE_DIR" \
    "${PACK_ARGS[@]}"

echo "Publishing packages to local feed:"
echo "  $LOCAL_NUGET_FEED"

for package in "$PACKAGE_DIR"/*.nupkg; do
    echo "  -> $(basename "$package")"

    dotnet nuget push "$package" \
        --source "$LOCAL_NUGET_FEED"
done

echo
echo "Done"