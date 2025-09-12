
#!/usr/bin/env bash
set -euo pipefail
pushd src/server >/dev/null
dotnet run --project Svift.Gateway/Svift.Gateway.csproj &
dotnet run --project Svift.Bff/Svift.Bff.csproj &
popd >/dev/null
pushd src/web/svift-web >/dev/null
npm run dev

