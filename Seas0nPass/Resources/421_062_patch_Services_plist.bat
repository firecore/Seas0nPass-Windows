.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg extract /System/Library/Lockdown/Services.plist .\tmp\Services.plist
.\bin\bspatch.exe .\tmp\Services.plist .\tmp\Services.plist.patched .\8C154\Services.patch
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /System/Library/Lockdown
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg rm /System/Library/Lockdown/Services.plist
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /System/Library/Lockdown
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg add .\tmp\Services.plist.patched /System/Library/Lockdown/Services.plist
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /System/Library/Lockdown
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmod 644 /System/Library/Lockdown/Services.plist
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /System/Library/Lockdown