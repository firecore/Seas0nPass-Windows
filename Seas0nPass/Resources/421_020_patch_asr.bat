.\bin\hfsplus.exe .\tmp\our.dmg ls /usr/sbin
.\bin\hfsplus.exe .\tmp\our.dmg extract /usr/sbin/asr .\tmp\asr
.\bin\hfsplus.exe .\tmp\our.dmg rm /usr/sbin/asr
.\bin\hfsplus.exe .\tmp\our.dmg ls /usr/sbin
.\bin\hfsplus.exe .\tmp\our.dmg grow 16542208
.\bin\bspatch.exe .\tmp\asr .\tmp\asr.patched .\8C154\asr.patch
.\bin\hfsplus.exe .\tmp\our.dmg add .\tmp\asr.patched /usr/sbin/asr
.\bin\hfsplus.exe .\tmp\our.dmg ls /usr/sbin
.\bin\hfsplus.exe .\tmp\our.dmg chmod 755 /usr/sbin/asr 
.\bin\hfsplus.exe .\tmp\our.dmg ls /usr/sbin
