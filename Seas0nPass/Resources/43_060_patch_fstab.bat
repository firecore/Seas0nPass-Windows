.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg extract /etc/fstab .\tmp\fstab
.\bin\bspatch.exe .\tmp\fstab .\tmp\fstab.patched .\8F191m\fstab.patch
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /etc
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mv /etc/fstab etc/fstab_backup
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /etc
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg add .\tmp\fstab.patched /etc/fstab
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /etc
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmod 644 /etc/fstab
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg ls /etc