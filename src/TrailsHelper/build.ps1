dotnet publish -c "Publish" -r "win-x64"
dotnet publish -c "Publish" -r "linux-x64"
dotnet publish -c "PublishZip" -r "win-x64"

New-Item -Path . -Name "out" -ItemType "directory" -Force
Copy-Item -Path ".\bin\Publish\net9.0\win-x64\publish\SkyInstaller.exe" -Destination ".\out\SkyInstaller.exe"
Copy-Item -Path ".\bin\Publish\net9.0\linux-x64\publish\SkyInstaller" -Destination ".\out\skyinstaller-linux-x64"
Compress-Archive -Update -Path ".\bin\PublishZip\net9.0\win-x64\publish\*" -Destination ".\out\SkyInstaller.zip"