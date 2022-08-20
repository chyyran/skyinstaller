dotnet publish --configuration Publish
dotnet publish --configuration PublishZip
New-Item -Path . -Name "out" -ItemType "directory" -Force
Copy-Item -Path ".\bin\Publish\net7.0-windows\win10-x64\publish\SkyInstaller.exe" -Destination ".\out\SkyInstaller.exe"
Compress-Archive -Update -Path ".\bin\PublishZip\net7.0-windows\win10-x64\publish\*" -Destination ".\out\SkyInstaller.zip"