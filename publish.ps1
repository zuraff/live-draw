dotnet publish LiveDraw.sln --self-contained true --runtime:win-x64 -c:Release


New-Item -ItemType Directory -Force -Path ".\bin"

Copy-Item ".\LiveDraw\bin\Release\net6.0-windows\win-x64" -Destination ".\bin" -Recurse -Force
Copy-Item ".\LiveDraw.Client\bin\Release\net6.0-windows\win-x64" -Destination ".\bin" -Recurse -Force

Push-Location ".\bin\win-x64"
    Copy-Item "LiveDraw.Client.exe" "color_next.exe" -Force
    Copy-Item "LiveDraw.Client.exe" "color_previous.exe" -Force
    Copy-Item "LiveDraw.Client.exe" "engage_toggle.exe" -Force
    Copy-Item "LiveDraw.Client.exe" "clear.exe" -Force
Pop-Location