SkyInstaller
============
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/chyyran/skyinstaller?label=download&color=brightgreen)](https://github.com/chyyran/skyinstaller/releases/latest)
[![license](https://img.shields.io/github/license/chyyran/skyinstaller)](https://github.com/chyyran/skyinstaller/blob/master/LICENSE.md)


One-click installer for the Evolution voice mod for the *Trails in the Sky* series.

![image](https://user-images.githubusercontent.com/1000503/174701235-4ff814fc-bef6-4391-9242-97ab0786206c.png)

SkyInstaller automatically downloads and installs [SoraVoice Lite](https://github.com/ZhenjianYang/SoraVoice) and the necessary voice data to the game installation.

Usage
-----

SkyInstaller only supports the English versions of the *Trails in the Sky* games published by XSEED. The Chinese or original Japanese versions are not supported.

SkyInstaller will automatically detect the game when it is installed via Steam. Simply press the 'Install to Steam version' button for the corresponding game,
confirm the path is correct, and press install. To specify the location manually, press the 'Browse' button to find the location of the game executable.

All necessary files will be automatically downloaded and installed to the game installation.

Troubleshooting
---------------

### SkyInstaller is stuck on 'Downloading voice data...'
The voice data from the Evolution releases are downloaded from the Internet Archive via torrent. Very rarely, the download may fail to connect properly. If this happens, you can retry the download
by cancelling the installation and trying again. The voice data download will resume from where it left off. Also ensure that your router allows NAT port forwarding and that SkyInstaller is allowed
through Windows Firewall.

### SkyInstaller is stuck on 'GitHub rate limit exceeded'
SkyInstaller needs to fetch a manifest file and download the latest version of the [SoraVoice](https://github.com/ZhenjianYang/SoraVoice) mod and scripts from GitHub using the GitHub API, which limits
how many times you can use it per hour. Try the install again in an hour.

### SkyInstaller is stuck downloading something other than voice data.
Required files other than the voice data are downloaded via HTTP. Check your internet connection and try again.

### What happens if I cancel the installation midway?
You can cancel midway through installing the mod. This will delete `dinput8.dll` as well as any battle voice data that may have been previously installed from the game directory.

### Can I use SkyInstaller with other mods?
SkyInstaller assumes a fresh installation from Steam, which may conflict with other mods. It is suggested that the Evolution voice mods are installed via SkyInstaller *first*, before
installing other mods manually.

### What is 'Steamworks Common Redistributables'?
SkyInstaller uses the Steam API to locate the installation path and verify ownership of the games. The Steam API needs an [AppID](https://developer.valvesoftware.com/wiki/Steam_Application_IDs) to
initialize, and SkyInstaller borrows the AppID of the [Steamworks Common Redistributables](https://steamdb.info/app/228980/) for this purpose.

### How do I uninstall the mod?
SkyInstaller does not have a dedicated uninstaller for the Evolution voice mod. Manually delete `dinput8.dll` from the install folder to uninstall the mod.

License
-------
SkyInstaller is licensed under the Mozilla Public License 2.0 (MPL2). 
