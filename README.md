SkyInstaller
============

1-click installer for the Evolution voice mod for the *Trails in the Sky* series.

![Screenshot 2022-06-20 140057](https://user-images.githubusercontent.com/1000503/174656943-e1f63a91-376b-4087-97f9-cda95e57b617.png)

Usage
-----

SkyInstaller only supports the Steam versions of the *Trails in the Sky* games published by XSEED.

SkyInstaller does not work with any other versions of the game except the Steam versions, including the original Japanese PC release,
the Chinese release, and the GOG or Humble Bundle releases. 

For non-Steam releases of the game, follow the instructions on the [PC Gaming Wiki](https://www.pcgamingwiki.com/wiki/The_Legend_of_Heroes:_Trails_in_the_Sky)
to manually install the voice patches.

SkyInstaller will automatically detect the game when it is installed. Simply press the 'Install Voice Patches' button for the corresponding game,
confirm the path is correct, and press install. All necessary files will be automatically downloaded and installed to the game installation.

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

### What is 'Spacewar'?
SkyInstaller uses the Steam API to locate the installation path of the game and verify ownership. The Steam API needs an [AppID](https://developer.valvesoftware.com/wiki/Steam_Application_IDs) to
initialize, and [Spacewar](https://steamdb.info/app/480/) is a commonly used testing AppID that SkyInstaller also uses. Spacewar will disappear from your library after restarting Steam.

### How do I uninstall the mod?
SkyInstaller does not have a dedicated uninstaller for the Evolution voice mod. Manually delete `dinput8.dll` from the install folder to uninstall the mod.

License
-------
SkyInstaller is licensed under the Mozilla Public License 2.0 (MPL2). 