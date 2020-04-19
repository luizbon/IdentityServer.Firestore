$ErrorActionPreference = "Stop";

New-Item -ItemType Directory -Force -Path ./nuget

dotnet tool restore

Push-Location ./src/IdentityServer4.Firestore.Storage
./build.ps1 $args
Pop-Location

Push-Location ./src/IdentityServer4.Firestore
./build.ps1 $args
Pop-Location
