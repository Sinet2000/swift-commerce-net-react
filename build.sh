
#!/usr/bin/env bash
set -euo pipefail
pushd src/server >/dev/null
dotnet build SviftCommerce.sln
popd >/dev/null

