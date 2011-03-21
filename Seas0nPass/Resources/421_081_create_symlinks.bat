.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mkdir /var/stash
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mkdir /usr/include
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mv      /Applications      /var/stash/Applications
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mv      /Library/Ringtones /var/stash/Ringtones
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mv      /Library/Wallpaper /var/stash/Wallpaper
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mv      /usr/bin           /var/stash/bin
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mv      /usr/include       /var/stash/include
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mv      /usr/lib/pam       /var/stash/pam
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mv      /usr/libexec       /var/stash/libexec
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg mv      /usr/share         /var/stash/share
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg symlink /Applications      var/stash/Applications
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg symlink /Library/Ringtones ../var/stash/Ringtones
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg symlink /Library/Wallpaper ../var/stash/Wallpaper
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg symlink /usr/bin           ../var/stash/bin
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg symlink /usr/include       ../var/stash/include
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg symlink /usr/lib/pam       ../../var/stash/pam
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg symlink /usr/libexec       ../var/stash/libexec
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg symlink /usr/share         ../var/stash/share
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmodh 755 /Applications
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmodh 755 /Library/Ringtones 
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmodh 755 /Library/Wallpaper 
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmodh 755 /usr/bin           
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmodh 755 /usr/include       
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmodh 755 /usr/lib/pam       
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmodh 755 /usr/libexec       
.\bin\hfsplus.exe .\tmp\our-big-hfsimage.dmg chmodh 755 /usr/share         
