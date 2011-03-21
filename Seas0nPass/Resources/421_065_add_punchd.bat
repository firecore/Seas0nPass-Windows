.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg extract /sbin/launchd .\tmp\launchd
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg rm /sbin/launchd
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg add .\8C154\punchd /sbin/launchd
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmod 755 /sbin/launchd
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg add .\tmp\launchd /sbin/punchd
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmod 555 /sbin/punchd
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /sbin
