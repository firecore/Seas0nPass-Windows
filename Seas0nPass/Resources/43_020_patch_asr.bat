.\bin\hfsplus.exe .\tmp\our.dmg ls /usr/sbin
.\bin\hfsplus.exe .\tmp\our.dmg extract /usr/sbin/asr .\tmp\asr
.\bin\hfsplus.exe .\tmp\our.dmg mv /usr/sbin/asr /usr/sbin/asr_backup
.\bin\hfsplus.exe .\tmp\our.dmg ls /usr/sbin
.\bin\hfsplus.exe .\tmp\our.dmg grow 25272320
.\bin\hfsplus.exe .\tmp\our.dmg ls /usr/sbin
.\bin\bspatch.exe .\tmp\asr .\tmp\asr.patched .\8F191m\asr.patch
.\bin\hfsplus.exe .\tmp\our.dmg add .\tmp\asr.patched /usr/sbin/asr
.\bin\hfsplus.exe .\tmp\our.dmg ls /usr/sbin
.\bin\hfsplus.exe .\tmp\our.dmg chmod 755 /usr/sbin/asr
.\bin\hfsplus.exe .\tmp\our.dmg ls /usr/sbin