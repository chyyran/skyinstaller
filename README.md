SkyInstaller
============
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/chyyran/skyinstaller?label=download&color=brightgreen)](https://github.com/chyyran/skyinstaller/releases/latest)
[![license](https://img.shields.io/github/license/chyyran/skyinstaller)](https://github.com/chyyran/skyinstaller/blob/master/LICENSE.md)


One-click installer for Japanese voice acting with the evolution voice mod for the *Trails in the Sky* series.

![slice1](https://user-images.githubusercontent.com/1000503/174936993-612bc03a-1b80-42db-b233-2cd81fd689df.png)


SkyInstaller automatically downloads and installs [SoraVoice Lite](https://github.com/ZhenjianYang/SoraVoice) and the necessary voice data for Japanese voice acting.

Supported Releases
------------------
Only the English XSEED release is supported.

### Steam
SkyInstaller will automatically detect installed Steam versions of the games.
* [The Legend of Heroes: Trails in the Sky](https://store.steampowered.com/app/251150/)
* [The Legend of Heroes: Trails in the Sky SC](https://store.steampowered.com/app/251290/)
* [The Legend of Heroes: Trails in the Sky the 3rd](https://store.steampowered.com/app/436670/)

### GOG
The GOG releases require browsing to the game installation folder.
* [The Legend of Heroes: Trails in the Sky](https://www.gog.com/en/game/the_legend_of_heroes_trails_in_the_sky)
* [The Legend of Heroes: Trails in the Sky SC](https://www.gog.com/en/game/legend_of_heroes_trails_in_the_sky_sc_the)
* [The Legend of Heroes: Trails in the Sky the 3rd](https://www.gog.com/en/game/legend_of_heroes_trails_in_the_sky_the_3rd_the)

Usage
-----

SkyInstaller only supports the English versions of the *Trails in the Sky* games published by XSEED. The Chinese or original Japanese versions are not supported.

SkyInstaller will automatically detect the game when it is installed via Steam. Simply press the 'Install to Steam version' button for the corresponding game,
confirm the path is correct, and press install. To specify the location manually, press the 'Browse' button to find the location of the game executable.

All necessary files will be automatically downloaded and installed to the game installation.

Troubleshooting
---------------
### SkyInstaller does not start
The single-file download should work on the majority of systems but sometimes an aggressive antivirus might delete a necessary DLL library. Try the [non self-extracting ZIP file](https://github.com/chyyran/skyinstaller/releases/latest/download/SkyInstaller.zip) and see if it might work. Windows 7 is not supported but may work.

### SkyInstaller does not start on Linux
SkyInstaller needs `fontconfig` and X11, ensure you have support for this on your distribution. SteamOS is the only Linux distribution that I can guarantee support on.

### SkyInstaller is stuck on 'Waiting for Steam'
If Steam is installed, SkyInstaller needs the Steam API to find the installation location of the game. Ensure that Steam is running and that you are logged in to Steam. Alternatively, you can browse to the install location manually.

### What happens if I cancel the installation midway?
You can cancel midway through installing the mod. This will delete `dinput8.dll` as well as any battle voice data that may have been previously installed from the game directory.

### Why does Steam say I'm playing 'Steamworks Common Redistributables'?
SkyInstaller uses the Steam API to locate the installation path and verify ownership of the games. The Steam API needs an [AppID](https://developer.valvesoftware.com/wiki/Steam_Application_IDs) to
initialize, and SkyInstaller borrows the AppID of the [Steamworks Common Redistributables](https://steamdb.info/app/228980/) for this purpose. 

### Can I use SkyInstaller with other mods?
SkyInstaller assumes a fresh installation and may conflict with other mods. It is suggested that the Evolution voice mods are installed via SkyInstaller *first*, before installing other mods manually.

### SkyInstaller is stuck on 'Downloading...'
Sometimes the download may stall on a slow internet connection. Check your internet connection and try again.

### How do I uninstall the mod?
SkyInstaller does not have a dedicated uninstaller for the Evolution voice mod. Manually delete `dinput8.dll` from the install folder to uninstall the mod.
 
License
-------
SkyInstaller is licensed under the Mozilla Public License 2.0 (MPL2). 

Code Signing
------------
SkyInstaller releases are signed with free code-signing services provided by [SignPath.io](https://about.signpath.io/), certificate by [SignPath Foundation](https://signpath.org/). @chyyran is the sole approver.
