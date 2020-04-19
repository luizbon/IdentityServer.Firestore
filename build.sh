#!/usr/bin/env bash
set -euo pipefail
rm -rf nuget
mkdir nuget

dotnet tool restore

pushd ./src/IdentityServer4.Firebase.Storage
./build.sh "$@"
popd

pushd ./src/IdentityServer4.Firestore
./build.sh "$@"
popd
