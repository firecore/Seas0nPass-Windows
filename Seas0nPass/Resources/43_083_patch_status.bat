.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /private/var/lib/dpkg
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg extract /private/var/lib/dpkg/status .\tmp\status
.\bin\bspatch.exe .\tmp\status .\tmp\status.patched .\8F191m\status.patch
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg add .\tmp\status.patched /private/var/lib/dpkg/status.patched
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /private/var/lib/dpkg