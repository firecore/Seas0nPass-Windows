.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg extract /Applications/AppleTV.app/AppleTV .\tmp\AppleTV
.\bin\bspatch.exe .\tmp\AppleTV .\tmp\AppleTV.patched .\8F191m\AppleTV.patch
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /Applications/AppleTV.app
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mv /Applications/AppleTV.app/AppleTV /Applications/AppleTV.app/AppleTV_backup
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /Applications/AppleTV.app
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg add .\tmp\AppleTV.patched /Applications/AppleTV.app/AppleTV
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /Applications/AppleTV.app
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmod 775 /Applications/AppleTV.app/AppleTV
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /Applications/AppleTV.app